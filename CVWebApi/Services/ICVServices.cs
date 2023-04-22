using CVWebApi.Dtos;
using CVWebApi.Entities;

namespace CVWebApi.Services
{
    public interface ICVServices
    {
        Task<List<CVDto>> GetCVsAsync();
        Task<CVDto> GetCVByEmailAsync(string email);
        Task<ResponseDto> AddCVAsync(CVDto cv);
        Task<ResponseDto> UpdateCVAsync(CVDto cv);
        Task DeleteCVAsync(int id);
        Task<List<CVDto>> GetCVsByExperience(string jobTitle);
        Task<List<CVDto>> GetCVsBySkill(string skill);
        Task<List<CVDto>> GetCVsByQualification(string qualification);
        Task<List<CVDto>> GetCVsByYearOfExperiences(int year);
        Task<List<CVDto>> GetCVsByYearOfExperiencesMax(int year);
    }
}
