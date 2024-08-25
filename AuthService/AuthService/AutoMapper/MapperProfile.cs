using AuthMicroservice.Models.Auth;
using AuthMicroservice.Models.Auth.ResponseModels;
using AutoMapper;

namespace AuthMicroservice.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AuthResult, AuthResponse>();
        }
    }
}
