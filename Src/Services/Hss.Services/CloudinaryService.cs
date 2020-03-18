namespace Hss.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            using var reader = new MemoryStream();
            await file.CopyToAsync(reader);
            var fileBites = reader.ToArray();

            using var stream = new MemoryStream(fileBites);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
            };
            var result = await this.cloudinary.UploadAsync(uploadParams);

            return result.SecureUri.AbsoluteUri;
        }
    }
}
