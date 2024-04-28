using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.Institution;
using Common.Extensions;
using Common.Resources;
using DAL.Infrastructure.Models.Filters;
using DAL.UnitOfWork;
using Domain.Models;

namespace BLL.Services;

public record WeightedRatingGlobalCompounds(double AvgRatingForSet, int MinCountRatingForSet);

public record WeightedRatingInstitutionCompounds(double AvgRatingForInstitution, int CountRatingForInstitution);

public class InstitutionService : IInstitutionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public InstitutionService(
        IUnitOfWork unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void Apply(CreateUpdateInstitutionModel institutionModel)
    {
        var institution = _mapper.Value.Map<Institution>(institutionModel);
        _unitOfWork.Institutions.Value.Apply(institution);
    }

    public void Delete(int institutionId)
    {
        _unitOfWork.Institutions.Value.Delete(institutionId);
    }

    public IEnumerable<InstitutionListItem> GetAll(InstitutionFilter institutionFilter, int userId)
    {
        var institutions = _unitOfWork.Institutions.Value.GetAll(institutionFilter).ToList();
        var institutionModels = MapInstitutionsToModels(institutions, userId);

        if (institutionFilter.SortByRatingAscending)
            return institutionModels.OrderBy(i => i.Rating.Mark).ToList();
        else
            return institutionModels.OrderByDescending(i => i.Rating.Mark).ToList();
    }

    public IEnumerable<InstitutionListItem> GetByOwnerId(int ownerId)
    {
        var institutions = _unitOfWork.Institutions.Value.GetAll().Where(i => i.OwnerId == ownerId).ToList();
        return MapInstitutionsToModels(institutions, ownerId);
    }

    public InstitutionModel GetById(int institutionId, int userId)
    {
        var institution = _unitOfWork.Institutions.Value.GetById(institutionId);
        var institutionModel = _mapper.Value.Map<InstitutionModel>(institution);

        if (institutionModel.InstitutionId > 0)
        {
            var avgRating = institution.Ratings?.Any() == true ? institution.Ratings.Average(r => r.Mark) : 0;
            institutionModel.Logo = GetLogoBase64(institution.LogoBytes);
            institutionModel.Rating = new RatingModel
            {
                InstitutionId = institutionId,
                Mark = Math.Round(avgRating, 2),
                IsSetByCurrentUser = institution.Ratings.Any(r => r.UserId == userId)
            };
        }

        return institutionModel;
    }

    public void SetRating(int institutionId, int userId, int mark)
    {
        _unitOfWork.Institutions.Value.SetRating(institutionId, userId, mark);
    }

    public string UploadLogo(byte[] logoBytes, int institutionId)
    {
        _unitOfWork.Institutions.Value.UploadLogo(logoBytes, institutionId);
        return Convert.ToBase64String(logoBytes);
    }

    public void AddFacilityToInstitution(int facilityId, int institutionId)
    {
        _unitOfWork.Institutions.Value.AddFacilityToInstitution(facilityId, institutionId);
    }

    public void RemoveFacilityFromInstitution(int facilityId, int institutionId)
    {
        _unitOfWork.Institutions.Value.RemoveFacilityFromInstitution(facilityId, institutionId);
    }

    public void SetRFIDReaderIp(int rfidSettingsId, string ipAddress)
    {
        _unitOfWork.Institutions.Value.SetRFIDReaderIp(rfidSettingsId, ipAddress);
    }

    public RFIDSettingsModel GetRFIDSettingsByInstitutionId(int institutionId)
    {
        var rfidSettings = _unitOfWork.Institutions.Value.GetRFIDSettingsByInstitutionId(institutionId);
        var rfidSettingsModel = _mapper.Value.Map<RFIDSettingsModel>(rfidSettings);

        return rfidSettingsModel;
    }

    #region Helpers 

    private IEnumerable<InstitutionListItem> MapInstitutionsToModels(IEnumerable<Institution> institutions, int userId)
    {
        var institutionModels = new List<InstitutionListItem>();
        if (institutions == null || !institutions.Any())
            return institutionModels;

        var globalWeightedRatingCompounds = GetWeightedRatingCompounds(institutions);

        institutions.ToList().ForEach(i =>
        {
            var avgRating = i.Ratings?.Any() == true ? i.Ratings.Average(r => r.Mark) : 0;
            var ratingCount = i.Ratings?.Count() ?? 0;

            var institutionModel = new InstitutionListItem
            {
                InstitutionId = i.InstitutionId,
                Name = i.Name,
                InstitutionType = i.InstitutionType,
                Rating = new RatingModel
                {
                    InstitutionId = i.InstitutionId,
                    Mark = Math.Round(avgRating, 2),
                    IsSetByCurrentUser = i.Ratings?.Any(r => r.UserId == userId) ?? false
                },
                Logo = GetLogoBase64(i.LogoBytes),
                Region = i.Region
            };

            var institutionWeightedRatingCompounds = new WeightedRatingInstitutionCompounds(avgRating, ratingCount);
            institutionModel.WeightedRating = GetWeightedRatingForInstitution(institutionWeightedRatingCompounds, globalWeightedRatingCompounds);

            institutionModels.Add(institutionModel);
        });


        return institutionModels;
    }

    private WeightedRatingGlobalCompounds GetWeightedRatingCompounds(IEnumerable<Institution> institutions)
    {
        var avgRatingForSet = institutions?.Average(i => i.Ratings?.Any() == true ? i.Ratings.Average(r => r.Mark) : 0) ?? 0;
        var minAmountForset = institutions?.Where(i => i.Ratings?.Count() > 0).Min(i => i.Ratings?.Count() ?? 0) ?? 0;

        return new WeightedRatingGlobalCompounds(avgRatingForSet, minAmountForset);
    }

    private double GetWeightedRatingForInstitution(WeightedRatingInstitutionCompounds institutionCompounds, WeightedRatingGlobalCompounds globalCompounds)
    {
        double countRatingForInstitution = institutionCompounds.CountRatingForInstitution;
        double avgRatingForInstitution = institutionCompounds.AvgRatingForInstitution;
        double minCountRatingForSet = globalCompounds.MinCountRatingForSet;
        double avgRatingForSet = globalCompounds.AvgRatingForSet;

        if (countRatingForInstitution == 0 && minCountRatingForSet == 0)
            return default;

        double weightedRatingForInstitutionPart = (double)(countRatingForInstitution * avgRatingForInstitution) / (double)(countRatingForInstitution + minCountRatingForSet);
        double weightedRatingForSetPart = (double)(minCountRatingForSet * avgRatingForSet) / (double)(countRatingForInstitution + minCountRatingForSet);

        return Math.Round(weightedRatingForInstitutionPart + weightedRatingForSetPart, 2);
    }

    private string GetLogoBase64(byte[] logoBytes)
    {
        if (logoBytes != null && logoBytes.Any())
            return Convert.ToBase64String(logoBytes);

        return null;
    }

    #endregion
}
