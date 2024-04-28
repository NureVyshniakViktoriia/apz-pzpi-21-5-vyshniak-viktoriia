using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.Institution;
using DAL.Infrastructure.Models.Filters;
using Domain.Models;

namespace BLL.Contracts;
public interface IInstitutionService
{
    void Delete(int institutionId);

    void Apply(CreateUpdateInstitutionModel institutionModel);

    InstitutionModel GetById(int institutionId, int userId);

    string UploadLogo(byte[] logoBytes, int institutionId);

    IEnumerable<InstitutionListItem> GetAll(InstitutionFilter institutionFilter, int userId);

    IEnumerable<InstitutionListItem> GetByOwnerId(int ownerId);

    void SetRating(int institutionId, int userId, int mark);

    void AddFacilityToInstitution(int facilityId, int institutionId);

    void RemoveFacilityFromInstitution(int facilityId, int institutionId);

    void SetRFIDReaderIp(int rfidSettingsId, string ipAddress);

    RFIDSettingsModel GetRFIDSettingsByInstitutionId(int institutionId);
}
