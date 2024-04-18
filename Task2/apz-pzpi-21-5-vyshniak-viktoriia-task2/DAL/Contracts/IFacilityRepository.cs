using Domain.Models;

namespace DAL.Contracts;
public interface IFacilityRepository
{
    void Delete(int facilityId);

    void Apply(Facility facility);

    Facility GetById(int facilityId);

    IQueryable<Facility> GetAll();
}
