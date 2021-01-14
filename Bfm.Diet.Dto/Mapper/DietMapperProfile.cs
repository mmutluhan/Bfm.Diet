using AutoMapper;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Dto.Authorization;

namespace Bfm.Diet.Dto.Mapper
{
    public class DietMapperProfile : Profile
    {
        public DietMapperProfile()
        {
            CreateMap<KullaniciDto, Kullanici>().IncludeAllDerived().ReverseMap();
            CreateMap<KullaniciKayitDto, Kullanici>().IncludeAllDerived().ReverseMap();
            CreateMap<GrupDto, Grup>().IncludeAllDerived().ReverseMap();
        }
    }
}