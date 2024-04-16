using DAL.Infrastructure.Models.Filters;
using Domain.Models;

namespace DAL.Contracts;
public interface IHealthRecordRepository
{
    void Create(HealthRecord record);

    IQueryable<HealthRecord> GetAll(HealthRecordFilter healthRecordFilter);
}
