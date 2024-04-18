using AutoMapper;
using Common.Resources;
using DAL.Contracts;
using DAL.DbContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class PetRepository : IPetRepository
{
    private readonly DbContextBase _dbContext;
    private readonly Lazy<IMapper> _mapper;

    private readonly DbSet<Pet> _pets;

    public PetRepository(
        DbContextBase dbContext,
        Lazy<IMapper> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

        _pets = dbContext.Pets;
    }

    public void Apply(Pet pet)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var dbPet = _pets.FirstOrDefault(p => p.PetId == pet.PetId);
            var isForEdit = dbPet != null;

            dbPet = _mapper.Value.Map(pet, dbPet);
            if (!isForEdit)
            {
                _pets.Add(pet);
                _dbContext.Commit();
            }

            _dbContext.Commit();
            scope.Commit();
        }
        catch (Exception ex)
        {
            scope.Rollback();
            throw new Exception(ex.Message);
        }
    }

    public void Delete(Guid petId)
    {
        var pet = _pets.FirstOrDefault(p => p.PetId == petId)
            ?? throw new ArgumentException(Resources.Get("PET_NOT_FOUND"));

        _pets.Remove(pet);
        _dbContext.Commit();
    }

    public IQueryable<Pet> GetAllByOwnerId(int ownerId)
    {
        return _pets.Where(p => p.OwnerId == ownerId);
    }

    public Pet GetById(Guid petId)
    {
        var pet = _pets
            .Include(p => p.HealthRecords)
            .Include(p => p.DiaryNotes)
            .FirstOrDefault(p => p.PetId == petId)
                ?? new Pet();

        return pet;
    }
}
