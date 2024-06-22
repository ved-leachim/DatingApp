using System.Security.Cryptography;
using API.Data;
using API.Data.Interfaces;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
  private readonly DataContext _context;
  private readonly ITokenService _tokenService;
  private readonly IMapper _mapper;

  public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
  {
    _context = context;
    _tokenService = tokenService;
    _mapper = mapper;
  }

  [HttpPost("register")]
  // api/account/register
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

    var user = _mapper.Map<AppUser>(registerDto);

    byte[] salt = new byte[16];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }

    using (var pbkdf2 = new Rfc2898DeriveBytes(registerDto.Password, salt, 10000, HashAlgorithmName.SHA512))
    {
      byte[] hash = pbkdf2.GetBytes(20);

      user.UserName = registerDto.Username.ToLower();
      user.PasswordHash = hash;
      user.PasswordSalt = salt;

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return new UserDto()
      {
        Username = user.UserName,
        KnownAs = user.KnownAs,
        Token = _tokenService.CreateToken(user),
        Gender = user.Gender
      };
    }
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    var user = await _context.Users
      .Include(x => x.Photos)
      .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

    if (user == null) return Unauthorized("Invalid username");

    using (var pbkdf2 = new Rfc2898DeriveBytes(loginDto.Password, user.PasswordSalt, 10000, HashAlgorithmName.SHA512))
    {
      byte[] hash = pbkdf2.GetBytes(20);

      for (int i = 0; i < hash.Length; i++)
        if (hash[i] != user.PasswordHash[i])
          return Unauthorized("Invalid Password");
    }

    return new UserDto()
    {
      Username = user.UserName,
      Token = _tokenService.CreateToken(user),
      PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
      KnownAs = user.KnownAs,
      Gender = user.Gender
    };
  }

  private async Task<bool> UserExists(string username)
  {
    return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
  }
}
