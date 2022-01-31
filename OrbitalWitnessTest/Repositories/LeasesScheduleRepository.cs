using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrbitalWitnessTest.Helpers;
using OrbitalWitnessTest.Interfaces;
using OrbitalWitnessTest.Models;
using OrbitalWitnessTest.Models.Repository;
using OrbitalWitnessTest.Persistance;

namespace OrbitalWitnessTest.Repositories
{
    public class LeasesScheduleRepository: ILeasesScheduleRepository
    {

        private readonly OrbitalWitnessDbContext _context;
        public LeasesScheduleRepository(OrbitalWitnessDbContext context)
        {
            _context = context;
        }

        public LeaseSchedule GetLeasesScheduleById(int id)
        {
            /* 
             * we're bringing back all the data here which could get huge if we don't need it all,
             * but this is just an example of the kind of thing we could do/ we could have more interesting 
             * ways to query the data and extract specific entries
             */
            var schedule = _context.LeaseSchedules.Include("ScheduleEntries.ScheduleData").FirstOrDefault(x => x.Id == id);
            if(schedule == null)
            {
                throw new Exception($"No Lease Schedule was found with Id {id}");
            }
            return schedule;
        }

        public async Task<LeaseSchedule> SaveLeasesSchedule(LeaseScheduleModel schedule)
        {
            var newSchedule = await _context.LeaseSchedules.AddAsync(MappingHelper.MapLeaseScheduleModelToRepository(schedule));
            await _context.SaveChangesAsync();
            foreach (var entry in schedule.ScheduleEntry)
            {
                var repositoryEntry = MappingHelper.MapScheduleEntryModelToRepository(entry);

                repositoryEntry.LeasesScheduleId = newSchedule.Entity.Id;
                var newScheduleEntry = await _context.ScheduleEntries.AddAsync(repositoryEntry);
                await _context.SaveChangesAsync();

                var scheduleOfLeaseEntry = MappingHelper.MapScheduleOfLeaseModelToRepository(entry.ScheduleOfLease);
                scheduleOfLeaseEntry.ScheduleEntryId = newScheduleEntry.Entity.Id;
                var newscheduleOfLeaseEntry = await _context.ScheduleData.AddAsync(scheduleOfLeaseEntry);
            }
            await _context.SaveChangesAsync();

            return GetLeasesScheduleById(newSchedule.Entity.Id);
        }
    }
}
