using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Facility;
using DAL.UnitOfWork;
using Domain.Models;

namespace BLL.Services;
public class FacilityService : IFacilityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public FacilityService(
        IUnitOfWork unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void Apply(CreateUpdateFacilityModel facilityModel)
    {
        var facility = _mapper.Value.Map<Facility>(facilityModel);
        _unitOfWork.Facilities.Value.Apply(facility);
    }

    public void Delete(int facilityId)
    {
        _unitOfWork.Facilities.Value.Delete(facilityId);
    }

    public IEnumerable<FacilityListItem> GetAll(int? institutionId)
    {
        var facilities = _unitOfWork.Facilities.Value.GetAll();
        var facilityModels = _mapper.Value.Map<List<FacilityListItem>>(facilities);

        if (institutionId is not null)
        {
            facilityModels.ToList().ForEach(facilityModel =>
            {
                var isForInstitution = facilities
                    .Any(f => f.FacilityId == facilityModel.FacilityId
                        && f.Institutions.Any(i => i.InstitutionId == institutionId)
                );

                facilityModel.IsForInstitution = isForInstitution;
            });
        }

        return facilityModels;
    }

    public IEnumerable<FacilityListItem> GetAllByInstitutionId(int institutionId)
    {
        var facilities = _unitOfWork.Facilities.Value
            .GetAll()
            .Where(f => f.Institutions.Any(i => i.InstitutionId == institutionId));

        var facilityModels = _mapper.Value.Map<List<FacilityListItem>>(facilities);

        return facilityModels;
    }

    public FacilityModel GetById(int facilityId)
    {
        var facility = _unitOfWork.Facilities.Value.GetById(facilityId);
        var facilityModel = _mapper.Value.Map<FacilityModel>(facility);

        return facilityModel;
    }
}
