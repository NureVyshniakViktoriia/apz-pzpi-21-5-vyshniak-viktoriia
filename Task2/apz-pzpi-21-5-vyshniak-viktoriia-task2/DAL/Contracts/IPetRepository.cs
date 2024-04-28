using Domain.Models;

namespace DAL.Contracts;
public interface IPetRepository
{
    void Delete(Guid petId);

    void Apply(Pet pet);

    Pet GetById(Guid petId);

    Pet GetByRFID(string petRFID);

    IQueryable<Pet> GetAllByOwnerId(int ownerId);

    IQueryable<Pet> GetAll();

    ArduinoSettings GetArduinoSettingsByPetId(Guid petId);
}
