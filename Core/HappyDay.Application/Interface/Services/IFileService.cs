using Microsoft.AspNetCore.Http;

namespace HappyDay.Application.Interface.Services;

public interface IFileService
{
    Task<List<string>> SaveFilesAsync(List<IFormFile> files, string targetFolder);
    Task<string> SaveFileAsync(IFormFile file, string folder);
}