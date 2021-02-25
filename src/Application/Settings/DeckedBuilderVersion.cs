using System.IO;
using System.Linq;
using System.Reflection;

namespace Application.Settings {
    public class DeckedBuilderVersion {
        private int? _latestVersionFromBroker = null;

        public int LatestVersion {
            get {
                if (_latestVersionFromBroker.HasValue) {
                    return _latestVersionFromBroker.Value;
                }

                var localPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Directory.GetDirectories(Path.Combine(localPath, "Data\\"))
                    .Where(d => int.TryParse(d,out _))
                    .Select(int.Parse)
                    .OrderByDescending(d => d)
                    .First();
            }
            set => _latestVersionFromBroker = value;
        }
    }
}
