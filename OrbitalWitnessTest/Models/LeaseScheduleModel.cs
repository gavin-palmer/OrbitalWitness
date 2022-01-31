using System.Collections.Generic;

namespace OrbitalWitnessTest.Models
{
    public class LeaseScheduleModel
    {
        public string ScheduleType { get; set; }
        public List<ScheduleEntryModel> ScheduleEntry { get; set; }
    }
}
