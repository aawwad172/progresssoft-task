using ProgressSoft.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace ProgressSoft.Infrastructure.Pagination;

public static class QueryableExtensions
{
    // This method is an extension method that extends the IQueryable interface.
    // The default behavior of this method is to return everything from the query.
    public static async Task<PaginationResult<T>> ToPagedQueryAsync<T>(this IQueryable<T> query, int? pageNumber, int? pageSize)
    {
        int totalRecords = await query.CountAsync();

        if (pageNumber.HasValue && pageSize.HasValue && pageNumber.Value > 0 && pageSize.Value > 0)
        {
            int skipAmount = pageSize.Value * (pageNumber.Value - 1);

            query = query
                .Skip(skipAmount)
                .Take(pageSize.Value);
        }
        else if (pageSize.HasValue)
            query = query.Take(pageSize.Value);

        List<T> result = await query.ToListAsync();

        return new PaginationResult<T>
        {
            Page = result,
            TotalRecords = totalRecords,
            TotalDisplayRecords = result.Count
        };
    }
}
