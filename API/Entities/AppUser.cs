using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser 
{
   // EF automatically uses Id as PK
   public int Id { get; set; }
   public string UserName { get; set; }
   public byte[] PasswordHash { get; set; }
   public byte[] PasswordSalt { get; set; }
}
