using AutoMapper;
using Models;
using Web.Dto;

namespace Web.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<Book, BookResponseDto>();
        }        
    }
}