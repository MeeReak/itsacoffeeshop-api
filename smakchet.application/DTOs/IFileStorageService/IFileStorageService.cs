namespace smakchet.application.DTOs.IFileStorageService
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken);
        Task<Stream> GetFileAsync(string fileName, string folder, CancellationToken cancellationToken);
        Task DeleteFileAsync(string fileName, string folder, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllFilesAsync(string folder, CancellationToken cancellationToken);
    }
}
