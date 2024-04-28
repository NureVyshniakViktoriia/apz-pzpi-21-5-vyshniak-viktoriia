using BLL.Contracts;
using BLL.Infrastructure.Models.Institution;
using Common.Resources;
using DAL.Infrastructure.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Controllers;
[Area("institution")]
[Route("api/[area]")]
[ApiController]
[Authorize]
public class InstitutionController : ControllerBase
{
    private readonly IInstitutionService _institutionService;

    public InstitutionController(IInstitutionService institutionService)
    {
        _institutionService = institutionService;
    }

    [HttpPost("apply")]
    [Authorize(Roles = "Admin")]
    public ActionResult Apply([FromBody] CreateUpdateInstitutionModel institutionModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _institutionService.Apply(institutionModel);

        return Ok();
    }

    [HttpDelete("delete")]
    public ActionResult Delete([FromQuery] int institutionId)
    {
        _institutionService.Delete(institutionId);

        return Ok();
    }

    [HttpPost("upload-logo")]
    public async Task<ActionResult> UploadLogo([FromForm] UploadInstitutionLogoFileModel uploadModel)
    {
        if (uploadModel.File == null || uploadModel.File.Length == 0)
            return BadRequest(Resources.Get("EMPTY_FILE"));

        var fileBytes = await this.GetFileBytes(uploadModel.File);
        var logoBase64String = _institutionService.UploadLogo(fileBytes, uploadModel.InstitutionId);
        var image = new ImageData { Data = logoBase64String };

        return Ok(image);
    }

    [HttpGet("get-institution-by-id")]
    [Authorize]
    public ActionResult GetById([FromQuery] int institutionId)
    {
        var currentUserId = this.GetCurrentUserId();
        var institution = _institutionService.GetById(institutionId, currentUserId);

        return Ok(institution);
    }

    [HttpGet("list")]
    [Authorize]
    public ActionResult List([FromQuery] InstitutionFilter institutionFilter)
    {
        var currentUserId = this.GetCurrentUserId();
        var institutions = _institutionService.GetAll(institutionFilter, currentUserId);

        return Ok(institutions);
    }

    [HttpGet("get-by-owner-id")]
    public ActionResult GetByOwnerId([FromQuery] int ownerId)
    {
        var institutions = _institutionService.GetByOwnerId(ownerId);

        return Ok(institutions);
    }

    [HttpPost("set-rating")]
    [Authorize]
    public ActionResult SetRating([FromBody] SetRatingModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _institutionService.SetRating(model.InstitutionId, model.UserId, model.Mark);

        return Ok();
    }

    [HttpPost("add-facility-institution")]
    public ActionResult AddFacilityToInstitution([FromBody] AddFacilityToInstitutionModel facilityToInstitutionModel)
    {
        _institutionService.AddFacilityToInstitution(facilityToInstitutionModel.FacilityId, facilityToInstitutionModel.InstitutionId);

        return Ok();
    }

    [HttpPost("remove-facility-institution")]
    public ActionResult RemoveFacilityFromInstitution([FromBody] RemoveFacilityFromInstitution removeFacilityFromInstitution)
    {
        _institutionService.RemoveFacilityFromInstitution(removeFacilityFromInstitution.FacilityId, removeFacilityFromInstitution.InstitutionId);

        return Ok();
    }

    [HttpGet("get-rfid-settings-by-id")]
    [Authorize(Roles = "Admin")]
    public ActionResult GetRFIDSettingsById([FromQuery] int institutionId)
    {
        var rfidSettingsModel = _institutionService.GetRFIDSettingsByInstitutionId(institutionId);

        return Ok(rfidSettingsModel);
    }
}
