using Microsoft.AspNetCore.Http;
using System;

namespace WebAPI.Infrastructure.Models;
public class UploadDocumentFileModel
{
    public Guid DiaryNoteId { get; set; }

    public IFormFile File { get; set; }
}
