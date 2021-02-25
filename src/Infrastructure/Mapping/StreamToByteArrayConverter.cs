using System.IO;
using AutoMapper;

namespace Infrastructure.Mapping {
    public class StreamToByteArrayConverter : IValueConverter<Stream,byte[]> {
        public byte[] Convert(Stream sourceMember, ResolutionContext context) {
            var mem = new MemoryStream();
            sourceMember.CopyToAsync(mem).GetAwaiter().GetResult();
            return mem.ToArray();
        }
    }
}
