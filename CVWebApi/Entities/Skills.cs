namespace CVWebApi.Entities
{
    public class Skills
    {
        public int Id { get; set; }
        public string? Skill { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UsersId { get; set; }
        public Users? Users { get; set; }
    }
}
