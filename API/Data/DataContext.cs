using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions options) : base(options)
  {
    Console.WriteLine("Hello");
  }

  public DbSet<AppUser> Users { get; set; }
}
