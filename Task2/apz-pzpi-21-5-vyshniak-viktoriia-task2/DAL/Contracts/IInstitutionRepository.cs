using DAL.Infrastructure.Models.Filters;
using Domain.Models;

namespace DAL.Contracts;
public interface IInstitutionRepository
{
    void Delete(int institutionId);

    void Apply(Institution institution);

    Institution GetById(int institutionId);

    void UploadLogo(byte[] logoBytes, int institutionId);

    IQueryable<Institution> GetAll(InstitutionFilter insitutionFilter);

    IQueryable<Institution> GetAll();

    void SetRating(int institutionId, int userId, int mark);

    IQueryable<Rating> GetRatingsByInstitutionId(int institutionId);

    void AddFacilityToInstitution(int facilityId, int institutionId);

    void RemoveFacilityFromInstitution(int facilityId, int institutionId);

    void SetRFIDReaderIp(int rfidSettingsId, string ipAddress);

    RFIDSettings GetRFIDSettingsByInstitutionId(int institutionId);
}
