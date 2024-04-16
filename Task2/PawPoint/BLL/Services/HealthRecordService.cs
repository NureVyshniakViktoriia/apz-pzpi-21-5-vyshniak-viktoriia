using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.HealthRecord;
using DAL.Infrastructure.Models.Filters;
using DAL.UnitOfWork;
using Domain.Models;

namespace BLL.Services;
public class HealthRecordService : IHealthRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public HealthRecordService(
        IUnitOfWork unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void CreateHealthRecord(HealthRecordModel recordModel)
    {
        var record = _mapper.Value.Map<HealthRecord>(recordModel);
        _unitOfWork.HealthRecords.Value.Create(record);
    }

    public IEnumerable<HealthRecordModel> GetAllHealthRecords(HealthRecordFilter healthRecordFilter)
    {
        var healthRecords = _unitOfWork.HealthRecords.Value.GetAll(healthRecordFilter);
        var healthRecordModels = _mapper.Value.Map<List<HealthRecordModel>>(healthRecords);

        return healthRecordModels;
    }

    public IEnumerable<HealthRecordStatisticsModel> GetHealthRecordStatistics(HealthRecordFilter healthRecordFilter)
    {
        var healthRecords = _unitOfWork.HealthRecords.Value
            .GetAll(healthRecordFilter)
            .GroupBy(hr => hr.CreatedOnUtc.Date);

        if (!healthRecords.Any())
            return new List<HealthRecordStatisticsModel>();

        return healthRecords
            .Select(gr => new HealthRecordStatisticsModel
            {
                AverageTemperature = gr.Average(hr => hr.Temperature),
                Date = gr.Key.Date
            }).ToList();
    }
}
