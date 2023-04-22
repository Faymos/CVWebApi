namespace CVWebApi.Entities
{
    public class Qualifications
    {
        public int Id { get; set; }
       
        public string? Qualification { get; set; }
        public string? TypeOfQualifiction { get; set; }
        public string? YearObtain { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UsersId { get; set; }
        public Users? Users { get; set; }
    }
}
