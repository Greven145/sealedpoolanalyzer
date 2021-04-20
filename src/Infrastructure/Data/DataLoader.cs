using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IInfrastructure;
using Application.Exceptions;
using Application.Settings;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure.Models.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Contracts;
using Persistence.Models;

namespace Infrastructure.Data {
    public class DataLoader {
        private static readonly string _localPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private readonly AnalyzerContext _context;
        private readonly IDeckedBuilderLoader _deckedBuilderLoader;
        private readonly DeckedBuilderVersion _deckedBuilderVersion;
        private readonly IDeckedBuilderRepository _deckedBuilderRepository;
        private readonly IDb2ScryfallRepository _db2ScryfallRepository;
        private readonly ILogger<DataLoader> _logger;
        private readonly IMapper _mapper;
        private readonly ISetLoader _setLoader;
        private readonly IEnumerable<Guid> _setsToLoadSettings;

        public DataLoader(ISetLoader setLoader, IDeckedBuilderLoader deckedBuilderLoader, AnalyzerContext context,
            IOptionsSnapshot<SetsToLoad> setSnapshot, DeckedBuilderVersion deckedBuilderVersion,
            IDeckedBuilderRepository deckedBuilderRepository, IDb2ScryfallRepository db2ScryfallRepository,
            IMapper mapper, ILogger<DataLoader> logger) {
            _setLoader = setLoader ?? throw new ArgumentNullException(nameof(setLoader));
            _deckedBuilderLoader = deckedBuilderLoader ?? throw new ArgumentNullException(nameof(deckedBuilderLoader));
            _context = context ?? throw new ArgumentNullException(nameof(context)); //TODO: Use the repositories
            _deckedBuilderVersion =
                deckedBuilderVersion ?? throw new ArgumentNullException(nameof(deckedBuilderVersion));
            _deckedBuilderRepository = deckedBuilderRepository ?? throw new ArgumentNullException(nameof(deckedBuilderRepository));
            _db2ScryfallRepository = db2ScryfallRepository ?? throw new ArgumentNullException(nameof(db2ScryfallRepository));
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
                }
                else if ((DateTime.Now - set.DateLoaded).TotalDays > 7) {
                    _logger.LogInformation($"Refreshing data for set {setToLoad}");
                    _context.Remove(set);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    var setData = await _setLoader.GetSetFromId(setToLoad).ConfigureAwait(false);
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
                var safeSetName = new string(set.Name.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
                var fileToLoad = Path.Combine(_localPath, $"Data\\{safeSetName}.Review.json");
                var jsonString = await File.ReadAllTextAsync(fileToLoad, cancellationToken).ConfigureAwait(false);
                var reviewData = JsonSerializer.Deserialize<IEnumerable<Review>>(jsonString);

                if (reviewData is null) {
                    _logger.LogError($"Unable to load and parse review data for {set.Name} ({fileToLoad})");
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

        public async Task EnsureDeckedBuilderDataIsUpToDate(CancellationToken cancellationToken) {
            var latest = await _deckedBuilderLoader.GetLatest();
            _deckedBuilderVersion.LatestVersion = latest.Version;

            var versionPath = Path.Combine(_localPath, $"Data\\{latest.Version}");
            var dbZipPath = Path.Combine(versionPath, $"{versionPath}\\db.zip");
            const string dbFileName = "cards.sqlite";

            if (!File.Exists(dbZipPath)) {
                if (!Directory.Exists(versionPath)) {
                    Directory.CreateDirectory(versionPath);
                }

                await using var zipStream = await _deckedBuilderLoader.DownloadDatabase(new Uri(latest.Source));
                await using var outputFileStream = new FileStream(dbZipPath, FileMode.Create);
                await zipStream.CopyToAsync(outputFileStream, cancellationToken);
            }

            await using var stream = new FileStream(dbZipPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                FileOptions.SequentialScan);
            var calculatedHashArray = await MD5.Create().ComputeHashAsync(stream, cancellationToken);
            var calculatedHash = BitConverter.ToString(calculatedHashArray).ToLowerInvariant()
                .Replace("-", string.Empty);

            if (calculatedHash != latest.Md5) {
                throw new CommunicationException("Decked builder zip DB doesn't match the expected hash");
            }

            if (!File.Exists(Path.Combine(versionPath, dbFileName))) {
                using var archive = ZipFile.OpenRead(dbZipPath);
                var db = archive.Entries.First(e => e.FullName == $"dbdir-{latest.Version}/{dbFileName}");

                db.ExtractToFile(Path.Combine(versionPath, dbFileName));
            }
        }

        public async Task EnsureDeckedBuilderToScryfallRelationshipsUpToDate(CancellationToken cancellationToken) {
            var sets = await _context.Sets
                .Include(s => s.MagicCards)
                .ThenInclude(c => c.Review)
                .Where(s => _setsToLoadSettings.Contains(s.Id))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var set in sets) {
                if ((await _db2ScryfallRepository.CountOfSetAsync(set.Name,cancellationToken)) != 0) {
                    continue;
                }
                
                var cards = await _deckedBuilderRepository.GetCardsByName(set.Name);
                await _db2ScryfallRepository.AddRangeAsync(
                    cards
                    .Where(c => Guid.TryParse(c.Scryfallid, out _))
                    .Select(c => new DeckedBuilderToScryfallRelationship
                        {Id = c.Mvid, Scryfallid = Guid.Parse(c.Scryfallid)}), cancellationToken);
            }
            
        }
    }
}
