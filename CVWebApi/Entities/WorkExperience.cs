namespace CVWebApi.Entities
{
    public class WorkExperience
    {
        public int Id { get; set; } 
        public string? JobTitle { get; set; }
        public string? Organization { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UsersId { get; set; }
        public Users? Users { get; set; }
       
    }

}
