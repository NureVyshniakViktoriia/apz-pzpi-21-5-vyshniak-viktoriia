using Microsoft.AspNetCore.Http;

namespace WebAPI.Infrastructure.Models;
public class UploadInstitutionLogoFileModel
{
    public int InstitutionId { get; set; }

    public IFormFile File { get; set; }
}
