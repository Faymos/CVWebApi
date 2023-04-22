using AutoMapper;
using CVWebApi.Dtos;
using CVWebApi.Entities;
using CVWebApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace CVWebApi.Services
{
    public class CVServices : ICVServices
    {
        private readonly ICVRepo _cVRepo;
        private readonly IMapper _mapper;
        public CVServices(ICVRepo cVRepo,IMapper mapper)
        {
            _cVRepo = cVRepo;
            _mapper = mapper;
        }

        public  Task<ResponseDto> AddCVAsync(CVDto cv)
        {
            return _cVRepo.AddCVAsync(cv);
        }

        public Task DeleteCVAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CVDto> GetCVByEmailAsync(string email)
        {
            var result = await _cVRepo.GetCVByEmailAsync(email);
            var cv = _mapper.Map<CVDto>(result);
            return cv;
        }

        public async Task<List<CVDto>> GetCVsAsync()
        {
            var result = await _cVRepo.GetCVsAsync();
           
            var cVList = _mapper.Map<List<Users>, List<CVDto>>(result);
            return cVList;
        }

        public async Task<List<CVDto>> GetCVsByExperience(string jobtitle)

        {
           var result = await _cVRepo.GetCVsByExperience(jobtitle);
           var cVList = _mapper.Map<List<Users>, List<CVDto>>(result);
            return cVList;
            
        }

        public async Task<List<CVDto>> GetCVsByQualification(string qualification)
        {
            var result = await _cVRepo.GetCVsByQualification(qualification);
            var cVList = _mapper.Map<List<Users>, List<CVDto>>(result);
            return cVList;
        }

        public async Task<List<CVDto>> GetCVsBySkill(string skill)
        {
            var result = await _cVRepo.GetCVsBySkill(skill);
            var cVList = _mapper.Map<List<Users>, List<CVDto>>(result);
           
            return cVList;
        }

        public async Task<List<CVDto>> GetCVsByYearOfExperiences(int year)
        {
            var result = await _cVRepo.GetCVsByYearsOfExperience(year);
            var cVList = _mapper.Map<List<Users>, List<CVDto>>(result);
            return cVList;
        }

        public async Task<List<CVDto>> GetCVsByYearOfExperiencesMax(int year)
        {
            var result = await _cVRepo.GetCVsByYearsOfExperienceMax(year);
            var cVList = _mapper.Map<List<Users>, List<CVDto>>(result);
            return cVList;
        }
        public Task<ResponseDto> UpdateCVAsync(CVDto cv)
        {
            var resut = _cVRepo.UpdateCVAsync(cv);
            return resut;
        }
    }

}
