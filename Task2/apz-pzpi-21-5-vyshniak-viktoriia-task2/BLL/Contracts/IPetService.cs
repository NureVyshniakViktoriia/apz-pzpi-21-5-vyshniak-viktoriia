using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.Pet;

namespace BLL.Contracts;
public interface IPetService
{
    void Delete(Guid petId);

    void Apply(CreateUpdatePetModel petModel);

    PetModel GetById(Guid petId);

    PetModel GetByRFID(string petRFID);

    IEnumerable<PetListItem> GetAllByOwnerId(int ownerId);

    IEnumerable<PetListItem> GetAll();

    ArduinoSettingsModel GetArduinoSettingsByPetId(Guid petId);
}
