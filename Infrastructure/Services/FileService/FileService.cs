using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.FileService;

public class FileService(IWebHostEnvironment hostEnvironment, ILogger<FileService> logger) : IFileService
{

    public async Task<string> CreateFile(IFormFile file)
    {
        try
        {
            var fileName =
                string.Format($"{Guid.NewGuid() + Path.GetExtension(file.FileName)}");
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "Images", fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            logger.LogInformation("Successed, method CreateFile, time: {Data}", DateTime.Now);
            return fileName;


        }
        catch (Exception e)
        {
            logger.LogError("Error in method CreateFile, time : {Data}\nError : {Message}", DateTime.Now, e.Message);
            return e.Message;
        }
    }

    public bool DeleteFile(string file)
    {
        try
        {
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "books", file);
            File.Delete(fullPath);
            logger.LogInformation("Successed, method DeleteFile, time: {Data}", DateTime.Now);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError("Error in method DeleteFile, time : {Data}\nError : {Message}", DateTime.Now, e.Message);
            return false;
        }
    }
}