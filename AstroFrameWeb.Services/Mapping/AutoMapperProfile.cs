
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.ViewModels;
using AutoMapper;

namespace AstroFrameWeb.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GalaxyCreateViewModel, Galaxy>().ReverseMap();
            CreateMap<Galaxy, GalaxyViewModel>();

            CreateMap<PlanetCreateViewModel, Planet>().ReverseMap();
            CreateMap<Planet, PlanetViewModel>()
                .ForMember(dest => dest.GalaxyName, opt => opt.MapFrom(p => p.Galaxy.Name))
                .ForMember(dest => dest.StarName, opt => opt.MapFrom(p => p.Star.Name));

            CreateMap<StarCreateViewModel, Star>().ReverseMap();

            CreateMap<Star, StarViewModel>()
                .ForMember(dest => dest.GalaxyName, opt => opt.MapFrom(src => src.Galaxy.Name))
                .ForMember(dest => dest.StarTypeName, opt => opt.MapFrom(src => src.StarType.Name));
         
        }

    }
}
