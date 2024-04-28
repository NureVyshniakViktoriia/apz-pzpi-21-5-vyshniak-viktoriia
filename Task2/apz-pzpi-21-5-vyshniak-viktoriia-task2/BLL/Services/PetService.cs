using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.Pet;
using DAL.UnitOfWork;
using Domain.Models;

namespace BLL.Services;
public class PetService : IPetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public PetService(
        IUnitOfWork unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void Apply(CreateUpdatePetModel petModel)
    {
        var pet = _mapper.Value.Map<Pet>(petModel);
        _unitOfWork.Pets.Value.Apply(pet);
    }

    public void Delete(Guid petId)
    {
        _unitOfWork.Pets.Value.Delete(petId);
    }

    public IEnumerable<PetListItem> GetAll()
    {
        var pets = _unitOfWork.Pets.Value.GetAll();
        var petModels = _mapper.Value.Map<List<PetListItem>>(pets);

        return petModels;
    }

    public IEnumerable<PetListItem> GetAllByOwnerId(int ownerId)
    {
        var pets = _unitOfWork.Pets.Value.GetAllByOwnerId(ownerId);
        var petModels = _mapper.Value.Map<List<PetListItem>>(pets);

        return petModels;
    }

    public ArduinoSettingsModel GetArduinoSettingsByPetId(Guid petId)
    {
        var settings = _unitOfWork.Pets.Value.GetArduinoSettingsByPetId(petId);
        var settingsModel = _mapper.Value.Map<ArduinoSettingsModel>(settings);

        return settingsModel;
    }

    public PetModel GetById(Guid petId)
    {
        var pet = _unitOfWork.Pets.Value.GetById(petId);
        var petModel = _mapper.Value.Map<PetModel>(pet);

        return petModel;
    }

    public PetModel GetByRFID(string petRFID)
    {
        var pet = _unitOfWork.Pets.Value.GetByRFID(petRFID);
        var petModel = _mapper.Value.Map<PetModel>(pet);

        return petModel;
    }
}
