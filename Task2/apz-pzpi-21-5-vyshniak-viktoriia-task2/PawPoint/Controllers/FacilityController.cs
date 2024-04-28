using BLL.Contracts;
using BLL.Infrastructure.Models.Facility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Area("facility")]
[Route("api/[area]")]
[ApiController]
public class FacilityController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilityController(IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    [HttpPost("apply")]
    [Authorize(Roles = "Admin")]
    public ActionResult Apply([FromBody] CreateUpdateFacilityModel facilityModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _facilityService.Apply(facilityModel);

        return Ok();
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete([FromQuery] int facilityId)
    {
        _facilityService.Delete(facilityId);

        return Ok();
    }

    [HttpGet("get-facility-by-id")]
    [Authorize(Roles = "Admin")]
    public ActionResult GetById([FromQuery] int facilityId)
    {
        var facility = _facilityService.GetById(facilityId);

        return Ok(facility);
    }

    [HttpGet("get-all")]
    [Authorize]
    public ActionResult GetAll(int? institutionId)
    {
        var facilities = _facilityService.GetAll(institutionId);

        return Ok(facilities);
    }

    [HttpGet("get-all-by-institution-id")]
    [Authorize]
    public ActionResult GetAllByInstitutionId([FromQuery] int institutionId)
    {
        var facilities = _facilityService.GetAllByInstitutionId(institutionId);

        return Ok(facilities);
    }
}
