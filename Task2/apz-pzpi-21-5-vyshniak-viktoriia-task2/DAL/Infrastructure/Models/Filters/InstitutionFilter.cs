using Common.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DAL.Infrastructure.Models.Filters;
public class InstitutionFilter : FilterBase<Institution>
{
    public string SearchQuery { get; set; }

    public InstitutionType? Type { get; set; } = InstitutionType.All;

    public bool SortByRatingAscending { get; set; }

    public override IQueryable<Institution> Filter(DbSet<Institution> institutions)
    {
        var query = institutions.AsQueryable();

        if (!SearchQuery.IsNullOrEmpty())
            query = query.Where(r => r.Name.ToLower().Contains(SearchQuery.ToLower()));

        if (Type != InstitutionType.All)
            query = query.Where(i => i.InstitutionType == Type);

        return query;
    }
}
