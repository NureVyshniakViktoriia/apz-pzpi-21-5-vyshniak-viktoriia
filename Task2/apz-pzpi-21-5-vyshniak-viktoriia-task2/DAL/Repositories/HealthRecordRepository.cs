using DAL.Contracts;
using DAL.DbContexts;
using DAL.Infrastructure.Models.Filters;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class HealthRecordRepository : IHealthRecordRepository
{
    private readonly DbContextBase _dbContext;
    private readonly DbSet<HealthRecord> _healthRecords;

    public HealthRecordRepository(DbContextBase dbContext)
    {
        _dbContext = dbContext;
        _healthRecords = dbContext.HealthRecords;
    }

    public void Create(HealthRecord record)
    {
         record.CreatedOnUtc = DateTime.UtcNow;
         _healthRecords.Add(record);
         _dbContext.Commit();
    }

    public IQueryable<HealthRecord> GetAll(HealthRecordFilter healthRecordFilter)
    {
        return healthRecordFilter.Filter(_healthRecords);
    }
}
