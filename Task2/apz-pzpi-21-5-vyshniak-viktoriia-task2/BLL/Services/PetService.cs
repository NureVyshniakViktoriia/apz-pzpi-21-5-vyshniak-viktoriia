using AutoMapper;
using BLL.Contracts;
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

    public IEnumerable<PetListItem> GetAllByOwnerId(int ownerId)
    {
        var pets = _unitOfWork.Pets.Value.GetAllByOwnerId(ownerId);
        var petModels = _mapper.Value.Map<List<PetListItem>>(pets);

        return petModels;
    }

    public PetModel GetById(Guid petId)
    {
        var pet = _unitOfWork.Pets.Value.GetById(petId);
        var petModel = _mapper.Value.Map<PetModel>(pet);

        return petModel;
    }
}
