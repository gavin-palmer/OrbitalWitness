using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrbitalWitnessTest.Models.Repository;

namespace OrbitalWitnessTest.Persistance
{
    public class OrbitalWitnessDbContext: DbContext
    {
        /*
         * This is just a quick sqlite db to demonstrate how we might manage the data. 
         * Obviously in real life this would be an actual sql db but using entity framework 
         * would be basically the same
         */
        public DbSet<LeaseSchedule> LeaseSchedules { get; set; }
        public DbSet<ScheduleEntry> ScheduleEntries { get; set; }
        public DbSet<ScheduleData> ScheduleData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=OrbitalWitnessdb.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaseSchedule>().ToTable("LeaseSchedule", "Leases");
            modelBuilder.Entity<ScheduleEntry>().ToTable("ScheduleEntry", "Leases");
            modelBuilder.Entity<ScheduleData>().ToTable("ScheduleData", "Leases");

            modelBuilder.Entity<LeaseSchedule>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasMany(entity => entity.ScheduleEntries);

            });
            modelBuilder.Entity<ScheduleEntry>
                (entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasOne(entity => entity.ScheduleData);
                entity.HasOne(entity => entity.LeasesSchedule);
            });
            modelBuilder.Entity<ScheduleData>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasOne(entity => entity.ScheduleEntry);

            });

             base.OnModelCreating(modelBuilder);
        }
    }
}
