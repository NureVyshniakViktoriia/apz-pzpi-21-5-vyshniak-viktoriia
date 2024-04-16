using Domain.Models;

namespace DAL.Contracts;
public interface IPetRepository
{
    void Delete(Guid petId);

    void Apply(Pet pet);

    Pet GetById(Guid petId);

    IQueryable<Pet> GetAllByOwnerId(int ownerId);
}
