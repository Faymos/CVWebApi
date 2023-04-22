using CVWebApi.Dtos;
using CVWebApi.Entities;

namespace CVWebApi.Repository
{
    public interface ICVRepo
    {
        Task<List<Users>> GetCVsAsync();
        Task<Users> GetCVByEmailAsync(string email);
        Task<ResponseDto> AddCVAsync(CVDto cv);
        Task<ResponseDto> UpdateCVAsync(CVDto cv);
        Task DeleteCVAsync(int id);
        Task<List<Users>> GetCVsByExperience(string JobTitle);
        Task<List<Users>> GetCVsBySkill(string skill);
        Task<List<Users>> GetCVsByQualification(string qualification);
        Task<List<Users>> GetCVsByYearsOfExperience(int years);
        Task<List<Users>> GetCVsByYearsOfExperienceMax(int years);
    }
}
