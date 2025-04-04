﻿using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }


        public async Task<ImageUpload> Upload(ImageUpload imageUpload)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{imageUpload.FileName}{imageUpload.FileExtension}");

            // Upload Image to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await imageUpload.File.CopyToAsync(stream);

            // https://localhost:1234/images/image.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{imageUpload.FileName}{imageUpload.FileExtension}";

            imageUpload.FilePath = urlFilePath;


            // Add Image to the Images table
            await dbContext.Images.AddAsync(imageUpload);
            await dbContext.SaveChangesAsync();

            return imageUpload;
        }
    }
}
