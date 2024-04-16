using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Infrastructure.Extensions;
public static class ControllerExtensions
{
    public static async Task<byte[]> GetFileBytes(this ControllerBase controller, IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileData = memoryStream.ToArray();

        return fileData;
    }

    public static int GetCurrentUserId(this ControllerBase controller)
    {
        var userIdString = controller.HttpContext.User.FindFirst("id")
            ?.Value ?? "-1";

        var userId = int.Parse(userIdString);

        return userId;

    }
}
