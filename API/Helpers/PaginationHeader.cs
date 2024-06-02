namespace API.Helpers;

public class PaginationHeader
{
  // Object returned within the HTTP-Response Header
  public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
  {
    CurrentPage = currentPage;
    ItemsPerPage = itemsPerPage;
    TotalItems = totalItems;
    TotalPages = totalPages;
  }

  // Client can fetch Pagination details from this Header
  public int CurrentPage { get; set; }
  public int ItemsPerPage { get; set; }
  public int TotalItems { get; set; }
  public int TotalPages { get; set; }
}
