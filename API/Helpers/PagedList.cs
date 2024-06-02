using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public class PagedList<T> : List<T>
{
  private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
  {
    CurrentPage = pageNumber;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    PageSize = pageSize;
    TotalCount = count;
    AddRange(items);
  }

  public int CurrentPage { get; set; }
  public int TotalPages { get; set; }
  public int PageSize { get; set; }
  public int TotalCount { get; set; }

  // This is a Factory Method
  // Allowing for async Creation of the PagedList-Object, what a Constructor cannot do
  public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
  {
    var count = await source.CountAsync();
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return new PagedList<T>(items, count, pageNumber, pageSize);
  }
}
