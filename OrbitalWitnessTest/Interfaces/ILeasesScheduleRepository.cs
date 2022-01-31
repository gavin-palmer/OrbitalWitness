using System;
using System.Threading.Tasks;
using OrbitalWitnessTest.Models;
using OrbitalWitnessTest.Models.Repository;

namespace OrbitalWitnessTest.Interfaces
{
    public interface ILeasesScheduleRepository
    {
        Task<LeaseSchedule> SaveLeasesSchedule(LeaseScheduleModel schedule);
        LeaseSchedule GetLeasesScheduleById(int id);
    }
}
