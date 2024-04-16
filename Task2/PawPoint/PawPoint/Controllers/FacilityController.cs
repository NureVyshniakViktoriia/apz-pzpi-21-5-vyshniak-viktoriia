using BLL.Contracts;
using BLL.Infrastructure.Models.Facility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Area("facility")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class FacilityController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilityController(IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    [HttpPost("apply")]
    public ActionResult Apply([FromBody] CreateUpdateFacilityModel facilityModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _facilityService.Apply(facilityModel);

        return Ok();
    }

    [HttpDelete("delete")]
    public ActionResult Delete([FromQuery] int facilityId)
    {
        _facilityService.Delete(facilityId);

        return Ok();
    }

    [HttpGet("get-facility-by-id")]
    public ActionResult GetById([FromQuery] int facilityId)
    {
        var facility = _facilityService.GetById(facilityId);

        return Ok(facility);
    }

    [HttpGet("get-all")]
    public ActionResult GetAll()
    {
        var facilities = _facilityService.GetAll();

        return Ok(facilities);
    }

    [HttpGet("get-all-by-institution-id")]
    public ActionResult GetAllByInstitutionId([FromQuery] int institutionId)
    {
        var facilities = _facilityService.GetAllByInstitutionId(institutionId);

        return Ok(facilities);
    }
}
