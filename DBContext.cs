using dotnetlearningclass.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace dotnetlearningclass
{
    public class LearningClassDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public LearningClassDbContext(DbContextOptions<LearningClassDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Students> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Retrieve the connection string from appsettings.json
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity mappings here
            // Entity properties should match the column names by default convention
        }

        //Add a Parameterless Constructor For moq!!
        public LearningClassDbContext()
        {
        }
    }
}
