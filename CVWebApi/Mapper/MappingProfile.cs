using AutoMapper;
using CVWebApi.Dtos;
using CVWebApi.Entities;

namespace CVWebApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Users, CVDto>();
            CreateMap<WorkExperience, WorkExperienceDto>();
            CreateMap<Skills, SkillsDto>();
            CreateMap<Qualifications, QualificationsDto>();
        }
    }
}
