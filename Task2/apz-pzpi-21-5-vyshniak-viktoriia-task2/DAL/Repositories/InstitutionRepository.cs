using AutoMapper;
using Common.Resources;
using DAL.Contracts;
using DAL.DbContexts;
using DAL.Infrastructure.Models.Filters;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class InstitutionRepository : IInstitutionRepository
{
    private readonly DbContextBase _dbContext;
    private readonly Lazy<IMapper> _mapper;

    private readonly DbSet<Institution> _institutions;
    private readonly DbSet<User> _users;
    private readonly DbSet<Rating> _ratings;
    private readonly DbSet<Facility> _facilities;
    private readonly DbSet<RFIDSettings> _rfIDSettings;

    public InstitutionRepository(
        DbContextBase dbContext,
        Lazy<IMapper> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

        _institutions = dbContext.Institutions;
        _users = dbContext.Users;
        _ratings = dbContext.Ratings;
        _facilities = dbContext.Facilities;
        _rfIDSettings = dbContext.RFIDSettings;
    }

    public void Apply(Institution institution)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var dbInstitution = _institutions.FirstOrDefault(i => i.InstitutionId == institution.InstitutionId);
            var isForEdit = dbInstitution != null;

            dbInstitution = _mapper.Value.Map(institution, dbInstitution);
            if (!isForEdit)
            {
                _institutions.Add(dbInstitution);
                _dbContext.Commit();

                _rfIDSettings.Add(new RFIDSettings
                {
                    InstitutionId = dbInstitution.InstitutionId,
                });
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

    public void Delete(int institutionId)
    {
        var institution = _institutions.FirstOrDefault(i => i.InstitutionId == institutionId)
           ?? throw new ArgumentException(Resources.Get("INSTITUTION_NOT_FOUND"));

        _institutions.Remove(institution);
        _dbContext.Commit();
    }

    public IQueryable<Institution> GetAll(InstitutionFilter institutionFilter)
    {
        return institutionFilter
            .Filter(_institutions)
            .Include(i => i.Ratings);
    }

    public IQueryable<Institution> GetAll()
    {
        return _institutions
            .Include(i => i.Ratings)
            .AsQueryable();
    }

    public Institution GetById(int institutionId)
    {
        var institution = _institutions
            .Include(i => i.Facilities)
            .Include(i => i.Ratings)
            .FirstOrDefault(i => i.InstitutionId == institutionId)
                ?? new Institution();

        return institution;
    }

    public IQueryable<Rating> GetRatingsByInstitutionId(int institutionId)
    {
        return _ratings.Where(r => r.InstitutionId == institutionId);
    }

    public void SetRating(int institutionId, int userId, int mark)
    {
        var institution = _institutions.FirstOrDefault(i => i.InstitutionId == institutionId)
            ?? throw new ArgumentException(Resources.Get("INSTITUTION_NOT_FOUND"));

        var user = _users.FirstOrDefault(u => u.UserId == userId)
            ?? throw new ArgumentException(Resources.Get("INVALID_USER_ID"));

        var rating = _ratings.FirstOrDefault(r => 
            r.InstitutionId == institutionId
                && r.UserId == userId
        );

        if (rating == null)
        {
            rating = new Rating
            {
                InstitutionId = institutionId,
                UserId = userId,
            };

            _ratings.Add(rating);
        }

        rating.Mark = (byte)mark;
        _dbContext.Commit();
    }

    public void UploadLogo(byte[] logoBytes, int institutionId)
    {
        var institution = _institutions.FirstOrDefault(i => i.InstitutionId == institutionId)
           ?? throw new ArgumentException(Resources.Get("INSTITUTION_NOT_FOUND"));

        institution.LogoBytes = logoBytes;
        _dbContext.Commit();
    }

    public void AddFacilityToInstitution(int facilityId, int institutionId)
    {
        var insitution = _institutions.Include(i => i.Facilities).FirstOrDefault(i => i.InstitutionId == institutionId)
            ?? throw new ArgumentException(Resources.Get("INSTITUTION_NOT_FOUND"));

        var facility = _facilities.FirstOrDefault(f => f.FacilityId == facilityId)
            ?? throw new ArgumentException(Resources.Get("FACILITY_NOT_FOUND"));

        insitution.Facilities.Add(facility);
        _dbContext.Commit();
    }

    public void RemoveFacilityFromInstitution(int facilityId, int institutionId)
    {
        var insitution = _institutions.Include(i => i.Facilities).FirstOrDefault(i => i.InstitutionId == institutionId)
           ?? throw new ArgumentException(Resources.Get("INSTITUTION_NOT_FOUND"));

        var facility = _facilities.FirstOrDefault(f => f.FacilityId == facilityId)
            ?? throw new ArgumentException(Resources.Get("FACILITY_NOT_FOUND"));

        insitution.Facilities.Remove(facility);
        _dbContext.Commit();
    }

    public void SetRFIDReaderIp(int rfidSettingsId, string ipAddress)
    {
        var rfidSettings = _rfIDSettings.FirstOrDefault(rs => rs.InstitutionId == rfidSettingsId)
            ?? throw new ArgumentException();

        rfidSettings.RFIDReaderIpAddress = ipAddress;
        _dbContext.Commit();
    }

    public RFIDSettings GetRFIDSettingsByInstitutionId(int institutionId)
    {
        var institution = _institutions
            .Include(r => r.RFIDSettings)
            .FirstOrDefault(i => i.InstitutionId == institutionId)
                ?? throw new ArgumentException(Resources.Get("INSTITUTION_NOT_FOUND"));

        return institution.RFIDSettings;
    }
}
