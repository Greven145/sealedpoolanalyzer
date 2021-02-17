using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IInfrastructure;
using Application.Settings;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence;

namespace Infrastructure.Data {
    public partial class DataLoader {
        private readonly AnalyzerContext _context;
        private readonly ILogger<DataLoader> _logger;
        private readonly IMapper _mapper;
        private readonly ISetLoader _setLoader;
        private readonly IEnumerable<Guid> _setsToLoadSettings;

        public DataLoader(ISetLoader setLoader, AnalyzerContext context, IOptionsSnapshot<SetsToLoad> setSnapshot,
            IMapper mapper, ILogger<DataLoader> logger) {
            _setLoader = setLoader ?? throw new ArgumentNullException(nameof(setLoader));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _setsToLoadSettings = setSnapshot?.Value?.Ids ?? throw new ArgumentNullException(nameof(setSnapshot));
        }

        public async Task EnsureSetDataIsUpToDate(CancellationToken cancellationToken) {
            foreach (var setToLoad in _setsToLoadSettings) {
                var set = await _context.Sets
                    .Include(s => s.MagicCards)
                    .ThenInclude(c => c.Review)
                    .FirstOrDefaultAsync(s => s.Id == setToLoad, cancellationToken)
                    .ConfigureAwait(false);

                if (set is null) {
                    _logger.LogInformation($"Loading data for set {setToLoad}");
                    var setData = await _setLoader.GetSetFromId(setToLoad).ConfigureAwait(false);
                    setData.DateLoaded = DateTimeOffset.Now;
                    
                    await _context.Sets.AddAsync(setData, cancellationToken).ConfigureAwait(false);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation($"Loaded and saved {setData.Name} data");
                } else if ((DateTime.Now - set.DateLoaded).TotalDays > 7) {
                    _logger.LogInformation($"Refreshing data for set {setToLoad}");
                    var setData = await _setLoader.GetSetFromId(setToLoad).ConfigureAwait(false);
                    _context.Remove(setData);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    await _context.Sets.AddAsync(setData, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation($"Loaded and saved {setData.Name} data");
                }
            }
        }

        public async Task EnsureReviewDataIsUpToDate(CancellationToken cancellationToken) {
            if (!await _context.Sets.AnyAsync(cancellationToken).ConfigureAwait(false)) {
                _logger.LogError(
                    $"Unable to get set data. Ensure {nameof(EnsureSetDataIsUpToDate)} is called before {nameof(EnsureReviewDataIsUpToDate)}");
            }

            var sets = _context.Sets
                .Where(s => s.MagicCards.Any(c => c.Review == null))
                .Include(s => s.MagicCards)
                .ThenInclude(c => c.Review);

            foreach (var set in sets) {
                var jsonString = await File
                    .ReadAllTextAsync(
                        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            $"Data\\{set.Name}.Review.json"),
                        cancellationToken).ConfigureAwait(false);
                var reviewData = JsonSerializer.Deserialize<IEnumerable<Review>>(jsonString);

                if (reviewData is null) {
                    _logger.LogError($"Unable to load and parse review data for {set.Name}");
                    continue;
                }

                foreach (var card in set.MagicCards.Where(c => c.Review is null)) {
                    var review = reviewData.FirstOrDefault(r => r.Name == card.Name);
                    if (review is null) {
                        _logger.LogError($"Unable to find review in {set.Name} that matches {card.Name}");
                        continue;
                    }

                    card.Review = _mapper.Map<MagicCardReview>(review);
                }

                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation($"Loaded review data for {set.Name}");
            }
        }
    }
}
