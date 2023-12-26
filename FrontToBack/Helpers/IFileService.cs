namespace FrontToBack.Helpers
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file);
        bool IsImage(IFormFile file);
        bool CheckSize(IFormFile file, int maxSize);
        void Delete(string path);
    }
}
