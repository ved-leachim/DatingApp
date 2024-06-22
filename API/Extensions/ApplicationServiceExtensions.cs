using API.Data;
using API.Data.Interfaces;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services,
    IConfiguration config)
  {
    services.AddDbContext<DataContext>(opt =>
    {
      opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });

    services.AddCors();
    // Scoped means it is scoped to the lifetime of the HTTP-Request
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    // Add configuration for Cloudinary
    services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
    services.AddScoped<IPhotoService, PhotoService>();

    return services;
  }
}
