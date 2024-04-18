using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers;
[Area("arduino")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = "Admin, SysAdmin")]
public class ArduinoController : ControllerBase
{
    [HttpGet("get-pet-temperature-data")]
    public ActionResult GetPetTemperatureData([FromQuery] Guid petId)
    {
        return Ok(38.5);
    }

    [HttpGet("get-pet-location-data")]
    public ActionResult GetPetLocationData([FromQuery] Guid petId)
    {
        var address = new AddressData
        {
            Country = "Ukraine",
            City = "Kyiv",
            Address = "khreshchatyk st., 29"
        };

        return Ok(address);
    }
}
