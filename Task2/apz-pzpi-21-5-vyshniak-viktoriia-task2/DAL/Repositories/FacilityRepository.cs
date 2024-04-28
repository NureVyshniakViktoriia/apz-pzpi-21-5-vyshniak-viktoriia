using AutoMapper;
using Common.Resources;
using DAL.Contracts;
using DAL.DbContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class FacilityRepository : IFacilityRepository
{
    private readonly DbContextBase _dbContext;
    private readonly Lazy<IMapper> _mapper;

    private readonly DbSet<Facility> _facilities;

    public FacilityRepository(
        DbContextBase dbContext,
        Lazy<IMapper> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

        _facilities = dbContext.Facilities;
    }

    public void Apply(Facility facility)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var dbFacility = _facilities.FirstOrDefault(f => f.FacilityId == facility.FacilityId);
            var isForEdit = dbFacility != null;

            dbFacility = _mapper.Value.Map(facility, dbFacility);
            if (!isForEdit)
            {
                _facilities.Add(facility);
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

    public void Delete(int facilityId)
    {
        var facility = _facilities.FirstOrDefault(f => f.FacilityId == facilityId)
            ?? throw new ArgumentException(Resources.Get("FACILITY_NOT_FOUND"));

        _facilities.Remove(facility);
        _dbContext.Commit();
    }

    public IQueryable<Facility> GetAll()
    {
        return _facilities.Include(f => f.Institutions).AsQueryable();
    }

    public Facility GetById(int facilityId)
    {
        var facility = _facilities.FirstOrDefault(f => f.FacilityId == facilityId)
           ?? new Facility();

        return facility;
    }
}
