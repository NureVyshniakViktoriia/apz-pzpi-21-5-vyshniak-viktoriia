using BLL.Infrastructure.Models.HealthRecord;
using DAL.Infrastructure.Models.Filters;

namespace BLL.Contracts;
public interface IHealthRecordService
{
    void CreateHealthRecord(HealthRecordModel recordModel);

    IEnumerable<HealthRecordModel> GetAllHealthRecords(HealthRecordFilter healthRecordFilter);

    IEnumerable<HealthRecordStatisticsModel> GetHealthRecordStatistics(HealthRecordFilter healthRecordFilter);
}
