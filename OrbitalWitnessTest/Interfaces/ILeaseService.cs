using System;
using System.Threading.Tasks;
using OrbitalWitnessTest.Models;

namespace OrbitalWitnessTest.Interfaces
{
    public interface ILeaseService
    {
        public Task<LeaseScheduleModel> ProcessNewLeaseSchedule(LeaseScheduleModel leaseSchedule);

        public LeaseScheduleModel GetLeaseScheduleById(int id);
    }
}
