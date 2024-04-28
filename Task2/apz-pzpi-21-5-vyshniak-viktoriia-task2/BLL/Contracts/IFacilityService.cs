using BLL.Infrastructure.Models.Facility;

namespace BLL.Contracts;
public interface IFacilityService
{
    void Delete(int facilityId);

    void Apply(CreateUpdateFacilityModel facilityModel);

    FacilityModel GetById(int facilityId);

    IEnumerable<FacilityListItem> GetAll(int? institutionId);

    IEnumerable<FacilityListItem> GetAllByInstitutionId(int institutionId);
}
