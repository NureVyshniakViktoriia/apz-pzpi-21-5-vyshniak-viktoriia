using BLL.Infrastructure.Models.Pet;

namespace BLL.Contracts;
public interface IPetService
{
    void Delete(Guid petId);

    void Apply(CreateUpdatePetModel petModel);

    PetModel GetById(Guid petId);

    IEnumerable<PetListItem> GetAllByOwnerId(int ownerId);
}
