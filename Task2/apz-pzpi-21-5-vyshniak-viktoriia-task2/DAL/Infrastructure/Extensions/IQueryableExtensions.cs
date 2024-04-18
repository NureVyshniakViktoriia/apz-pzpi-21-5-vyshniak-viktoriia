using DAL.Infrastructure.Models.Filters;

namespace DAL.Infrastructure.Extensions;
public static class IQueryableExtensions
{
    public static IOrderedQueryable<T> GetPage<T>(this IOrderedQueryable<T> query, PagingModel model)
    {
        if (model is null)
            return query;

        return (IOrderedQueryable<T>)query
            .Skip((model.PageCount - 1) * model.PageSize)
            .Take(model.PageSize);
    }
}
