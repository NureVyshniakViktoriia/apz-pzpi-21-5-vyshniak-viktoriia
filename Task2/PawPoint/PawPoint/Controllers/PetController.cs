using BLL.Contracts;
using BLL.Infrastructure.Models.Pet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers;

[Area("pet")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = "User")]
public class PetController : ControllerBase
{
    private readonly IPetService _petService;

    public PetController(IPetService petService)
    {
        _petService = petService;
    }

    [HttpPost("apply")]
    public ActionResult Apply([FromBody] CreateUpdatePetModel petModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _petService.Apply(petModel);

        return Ok();
    }

    [HttpDelete("delete")]
    public ActionResult Delete([FromQuery] Guid petId)
    {
        _petService.Delete(petId);

        return Ok();
    }

    [HttpGet("get-all-by-owner-id")]
    public ActionResult GetAll([FromQuery] int ownerId)
    {
        var pets = _petService.GetAllByOwnerId(ownerId);

        return Ok(pets);
    }

    [HttpGet("get-pet-by-id")]
    public ActionResult GetById([FromQuery] Guid petId)
    {
        var pet = _petService.GetById(petId);

        return Ok(pet);
    }
}
