using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Infrastructure.Models.DeckedBuilder;

namespace Infrastructure.Contracts {
    public interface IDeckedBuilderLoader {
        ValueTask<DeckedBuilderLatest> GetLatest();
        ValueTask<Stream> DownloadDatabase(Uri url);
    }
}
