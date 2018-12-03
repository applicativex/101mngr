using Microsoft.EntityFrameworkCore;

namespace _101mngr.WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>().HasData(new Team
            {
                Id = 1,
                Name = "Carpathia",
                CountryCode = "UA",
            }, new Team
            {
                Id = 2,
                Name = "Seacrew",
                CountryCode = "UA",
            });
            modelBuilder.Entity<Tournament>().HasData(new Tournament
            {
                Id = 1,
                Name = "Ukrainian Division",
                CountryCode = "UA",
                Description = "Ukrainian Division"
            });
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<PlayerMatchHistory> MatchHistory { get; set; }
    }
}