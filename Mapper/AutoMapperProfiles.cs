using AutoMapper;
using IntricomMVC.DTO;
using IntricomMVC.Models;

namespace IntricomMVC.Mapper
{   
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate)).ReverseMap();

            

    }

    }

}
