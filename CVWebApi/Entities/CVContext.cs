
using Microsoft.EntityFrameworkCore;


namespace CVWebApi.Entities
{
    public partial class CVContext : DbContext
    {
        public CVContext() 
        {
        
        }
        public CVContext(DbContextOptions<CVContext> options) : base(options)
        {
        }

        public  DbSet<Users> Users { get; set; }
        public  DbSet<WorkExperience> WorkExperience { get; set; }
        public  DbSet<Skills> Skills { get; set; }
        public  DbSet<Qualifications> Qualifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasMany(u => u.Skills)
                .WithOne(s => s.Users)
                .HasForeignKey(s => s.UsersId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasMany(u => u.Qualifications)
                .WithOne(q => q.Users)
                .HasForeignKey(q => q.UsersId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasMany(u => u.WorkExperience)
                .WithOne(w => w.Users)
                .HasForeignKey(w => w.UsersId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


