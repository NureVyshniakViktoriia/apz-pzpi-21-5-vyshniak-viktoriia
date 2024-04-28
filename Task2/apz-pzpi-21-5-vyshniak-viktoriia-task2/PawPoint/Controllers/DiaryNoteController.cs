using BLL.Contracts;
using BLL.Infrastructure.Models.DiaryNote;
using Common.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Controllers;
[Area("diary-note")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = "User")]
public class DiaryNoteController : ControllerBase
{
    private readonly IDiaryNoteService _diaryNoteService;

    public DiaryNoteController(IDiaryNoteService diaryNoteService)
    {
        _diaryNoteService = diaryNoteService;
    }

    [HttpPost("apply")]
    public ActionResult Apply([FromBody] CreateUpdateDiaryNoteModel diaryNoteModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _diaryNoteService.Apply(diaryNoteModel);
        return Ok();
    }

    [HttpDelete("delete")]
    public ActionResult Delete([FromQuery] Guid diaryNoteId)
    {
        _diaryNoteService.Delete(diaryNoteId);

        return Ok();
    }

    [HttpGet("get-all-for-pet")]
    public ActionResult GetAll([FromQuery] Guid petId)
    {
        var diaryNotes = _diaryNoteService.GetAllByPetId(petId);

        return Ok(diaryNotes);
    }

    [HttpGet("get-note-by-id")]
    public ActionResult GetById([FromQuery] Guid diaryNoteId)
    {
        var diaryNote = _diaryNoteService.GetById(diaryNoteId);

        return Ok(diaryNote);
    }

    [HttpPost("upload-document")]
    public async Task<ActionResult> UploadDocument([FromForm] UploadDocumentFileModel uploadModel)
    {
        if (uploadModel.File == null || uploadModel.File.Length == 0)
            return BadRequest(Resources.Get("EMPTY_FILE"));

        var fileBytes = await this.GetFileBytes(uploadModel.File);
        _diaryNoteService.UploadDocument(fileBytes, uploadModel.DiaryNoteId);

        return Ok();
    }

    [HttpGet("download-document")]
    [Authorize]
    public IActionResult DownloadDocument([FromQuery] Guid diaryNoteId)
    {
        var documentModel = _diaryNoteService.GetDocument(diaryNoteId);

        return File(documentModel.Document, "application/pdf", $"Document \"{documentModel.Title}\".pdf");
    }
}
