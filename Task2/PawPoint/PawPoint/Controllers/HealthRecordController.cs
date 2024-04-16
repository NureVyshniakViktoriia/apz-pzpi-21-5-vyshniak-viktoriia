using BLL.Contracts;
using DAL.Infrastructure.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Area("health-record")]
    [Route("api/[area]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet("get-health-records")]
        public ActionResult GetAllHealthRecords([FromBody] HealthRecordFilter recordFilter)
        {
            var healthRecords = _healthRecordService.GetAllHealthRecords(recordFilter);

            return Ok(healthRecords);
        }

        [HttpGet("get-health-record-statistics")]
        public ActionResult GetHealthRecordStatistics([FromBody] HealthRecordFilter recordFilter)
        {
            var healthRecords = _healthRecordService.GetHealthRecordStatistics(recordFilter);

            return Ok(healthRecords);
        }
    }
}
