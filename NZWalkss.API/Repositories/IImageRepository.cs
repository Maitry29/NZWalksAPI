using NZWalks.API.Models.Domain;
namespace NZWalks.API.Repositories
{
    public interface IImageRepository
    {
        Task<ImageUpload> Upload(ImageUpload imageUpload);
    }
}
