using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Mapping;
using AutoMapper;
using Infrastructure.Mapping;
using Infrastructure.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Infrastructure.Data.Parsers {
    public class FileParserContext : IMapFrom<IBrowserFile> {
        /// <summary>
        /// Gets the name of the file as specified by the browser.
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Gets the last modified date as specified by the browser.
        /// </summary>
        public DateTimeOffset LastModified { get; init; }

        /// <summary>
        /// Gets the size of the file in bytes as specified by the browser.
        /// </summary>        
        public long Size { get; init; }
        
        /// <summary>
        /// Gets the MIME type of the file as specified by the browser.
        /// </summary>
        public string ContentType { get; init;  }
        
        /// <summary>
        /// Represents the file content
        /// </summary>
        public byte[] Content { get; init;  }

        public IEnumerable<CardFromFile> Cards = new List<CardFromFile>();

        public static async Task<FileParserContext> FromIBrowserFile(IBrowserFile browserFile) {
            var reader = new MemoryStream();
            await browserFile.OpenReadStream().CopyToAsync(reader);

            return new FileParserContext {
                Size = browserFile.Size,
                Name = browserFile.Name,
                LastModified = browserFile.LastModified,
                ContentType = browserFile.ContentType,
                Content = reader.GetBuffer()
            };
        }
    }
}
