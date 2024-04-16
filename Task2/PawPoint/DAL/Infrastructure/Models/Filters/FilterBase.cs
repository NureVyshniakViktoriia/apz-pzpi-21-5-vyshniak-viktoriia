using Microsoft.EntityFrameworkCore;

namespace DAL.Infrastructure.Models.Filters;
public abstract class FilterBase<T>
         where T : class
{
    public PagingModel PagingModel { get; set; }

    public virtual IQueryable<T> Filter(DbSet<T> entities)
    {
        return entities.AsQueryable();
    }
}
