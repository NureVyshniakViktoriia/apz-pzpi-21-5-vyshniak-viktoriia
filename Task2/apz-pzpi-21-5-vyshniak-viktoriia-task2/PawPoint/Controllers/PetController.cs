using BLL.Contracts;
using BLL.Infrastructure.Models.Pet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers;
[Area("pet")]
[Route("api/[area]")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly IPetService _petService;

    public PetController(IPetService petService)
    {
        _petService = petService;
    }

    [HttpPost("apply")]
    [Authorize(Roles = "User")]
    public ActionResult Apply([FromBody] CreateUpdatePetModel petModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _petService.Apply(petModel);

        return Ok();
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "User")]
    public ActionResult Delete([FromQuery] Guid petId)
    {
        _petService.Delete(petId);

        return Ok();
    }

    [HttpGet("get-all-by-owner-id")]
    [Authorize(Roles = "User")]
    public ActionResult GetAllByOwner([FromQuery] int ownerId)
    {
        var pets = _petService.GetAllByOwnerId(ownerId);

        return Ok(pets);
    }

    [HttpGet("get-all")]
    [Authorize(Roles = "SysAdmin")]
    public ActionResult GetAll()
    {
        var pets = _petService.GetAll();

        return Ok(pets);
    }

    [HttpGet("get-pet-by-id")]
    [Authorize(Roles = "User")]
    public ActionResult GetById([FromQuery] Guid petId)
    {
        var pet = _petService.GetById(petId);

        return Ok(pet);
    }

    [HttpGet("get-arduino-settings-by-pet-id")]
    [Authorize(Roles = "SysAdmin")]
    public ActionResult GetArduinoSettingsByPetId([FromQuery] Guid petId)
    {
        var pet = _petService.GetArduinoSettingsByPetId(petId);

        return Ok(pet);
    }
}
