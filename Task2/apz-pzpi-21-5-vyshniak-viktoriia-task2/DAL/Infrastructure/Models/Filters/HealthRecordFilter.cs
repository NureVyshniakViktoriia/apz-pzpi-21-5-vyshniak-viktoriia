using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Infrastructure.Models.Filters;
public class HealthRecordFilter : FilterBase<HealthRecord>
{
    public Guid PetId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public override IQueryable<HealthRecord> Filter(DbSet<HealthRecord> healthRecords)
    {
        var query = healthRecords.Where(hr => hr.PetId == PetId);

        if (StartDate.HasValue)
            query = query.Where(hr => hr.CreatedOnUtc >= StartDate.Value);

        if (EndDate.HasValue)
            query = query.Where(hr => hr.CreatedOnUtc < EndDate.Value);

        return query;
    }
}
