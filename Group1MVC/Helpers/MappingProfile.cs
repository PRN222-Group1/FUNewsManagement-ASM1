using AutoMapper;
using BusinessServiceLayer.DTOs;
using RepositoryLayer.Entities;

namespace Group1MVC.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<SystemAccount, SystemAccountDTO>();
        }
    }
}
