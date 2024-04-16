namespace BLL.Infrastructure.Models.HealthRecord;
public class HealthRecordModel
{
    public Guid HealthRecordId { get; set; }

    public double Temperature { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
