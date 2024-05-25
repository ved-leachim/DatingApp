using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

// Profile is a class from AutoMapper
public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<AppUser, MemberDto>()
      .ForMember(
        dest =>
          dest.PhotoUrl, opt =>
          opt.MapFrom(src =>
            src.Photos.FirstOrDefault(x =>
              x.IsMain).Url))
      .ForMember(dest => dest.Age,
        opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
    CreateMap<Photo, PhotoDto>();
    CreateMap<MemberUpdateDto, AppUser>();
  }
}
