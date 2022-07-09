using AutoMapper;
using UrlShortener.Api.Dto.Requests;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Api.Infrastructure
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<UserRegistrRequest, User>();
            CreateMap<UserLoginRequest, User>();
        }
    }
}
