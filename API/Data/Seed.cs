using System.Security.Cryptography;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
  public static async Task SeedUsers(DataContext context)
  {
    if (await context.Users.AnyAsync()) return;

    var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

    foreach (var user in users)
    {
      user.UserName = user.UserName.ToLower();

      byte[] salt = new byte[16];

      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(salt);
        user.PasswordSalt = salt;
      }

      using (var pbkdf2 = new Rfc2898DeriveBytes("Pa$$w0rd", salt, iterations: 10000, HashAlgorithmName.SHA512))
      {
        byte[] hash = pbkdf2.GetBytes(20);
        user.PasswordHash = hash;
      }

      context.Users.Add(user);
    }

    await context.SaveChangesAsync();
  }
}
