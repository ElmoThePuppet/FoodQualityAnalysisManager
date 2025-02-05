using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using QualityManager.Domain.Models;

namespace FoodQualityAnalysis.Infrastructure
{
    public class FoodQualityContext : DbContext
    {
        public FoodQualityContext(DbContextOptions<FoodQualityContext> options) : base(options) {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public DbSet<FoodBatch> FoodBatches { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodBatch>()
               .HasIndex(f => f.SerialNumber)
               .IsUnique();
            modelBuilder.Entity<AnalysisResult>()
                .HasOne<FoodBatch>()
                .WithMany()
                .HasForeignKey(a => a.FoodBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
