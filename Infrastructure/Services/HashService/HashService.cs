using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.HashService;

public class HashService(ILogger<HashService> logger) : IHashService
{

    public string ConvertToHash(string rawData)
    {
        try
        {
            logger.LogInformation("Starting method {ConvertToHash} in time {DateTime}", "ConvertToHash",
                DateTimeOffset.UtcNow);
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }

            logger.LogInformation("Finished method {ConvertToHash} in time {DateTime}", "ConvertToHash",
                DateTimeOffset.UtcNow);
            return builder.ToString();
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return string.Empty;
        }
    }

}