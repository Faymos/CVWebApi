using System.ComponentModel.DataAnnotations;

namespace CVWebApi.Dtos
{
    public class CVDto
{
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Surname { get; set; }
           
        [Required]
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public List<QualificationsDto>? Qualifications { get; set; }
        public List<WorkExperienceDto>? WorkExperience { get; set; }
        public List<SkillsDto>? Skills { get; set; }
        public int YearExperience { get; set; }
            
    }
}
