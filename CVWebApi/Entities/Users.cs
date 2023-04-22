namespace CVWebApi.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Surname { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public int YearExperience { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ICollection<WorkExperience>? WorkExperience { get; set; }
        public ICollection<Skills>? Skills { get; set; }
        public ICollection<Qualifications>? Qualifications { get; set; }
    }
}
