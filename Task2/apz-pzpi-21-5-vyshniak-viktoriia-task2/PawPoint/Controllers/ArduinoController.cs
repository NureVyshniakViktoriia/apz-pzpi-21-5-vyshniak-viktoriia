using BLL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Controllers;
[Area("arduino")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = "Admin, SysAdmin")]
public class ArduinoController : ControllerBase
{
    private readonly IArduinoSettingsService _arduinoSettingsService;

    public ArduinoController(IArduinoSettingsService arduinoSettingsService)
    {
        _arduinoSettingsService = arduinoSettingsService;
    }

    [HttpGet("get-pet-temperature-data")]
    public async Task<ActionResult> GetPetTemperatureData([FromQuery] Guid petId)
    {
        var temperature = await _arduinoSettingsService.GetPetCurrentTemperature(petId);

        return Ok(temperature);
    }

    [HttpGet("get-average-temperature")]
    public async Task<ActionResult> GetAveragePetTemperatureData([FromQuery] Guid petId)
    {
        var temperature = await _arduinoSettingsService.GetPetAverageTemperature(petId);

        return Ok(temperature);
    }

    [HttpGet("get-pet-location-data")]
    public async Task<ActionResult> GetPetLocationData([FromQuery] Guid petId)
    {
        var locationData = await _arduinoSettingsService.GetPetCurrentLocation(petId);

        return Ok(locationData);
    }

    [HttpPost("configure-pet-device")]
    public ActionResult ConfigurePetDevice([FromBody] ConfigurePetDeviceModel petDeviceModel)
    {
        var ipAddress = _arduinoSettingsService.ConfigurePetDevice(
            petDeviceModel.WifiSettings,
            petDeviceModel.ArduinoSettingsId);

        return Ok(ipAddress);
    }

    [HttpPost("configure-rfid-reader")]
    public ActionResult ConfigureRFIDReader([FromBody] ConfigureRFIDReaderModel rFIDReaderModel)
    {
        var ipAddress = _arduinoSettingsService.ConfigureRFIDReader(
            rFIDReaderModel.WifiSettings,
            rFIDReaderModel.RFIDSettingsId);

        return Ok(ipAddress);
    }
}
