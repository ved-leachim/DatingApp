using API.Entities;

namespace API.Data.Interfaces;

public interface ITokenService
{
 string CreateToken(AppUser user);
}
