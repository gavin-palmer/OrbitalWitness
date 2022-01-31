using System;
using System.Collections.Generic;

namespace OrbitalWitnessTest.Models
{
    public class ScheduleEntryModel
    {

        public string EntryNumber { get; set; }
        public string EntryDate { get; set; }
        public string EntryType { get; set; }
        public List<string> EntryText { get; set; }
        public ScheduleOfLeaseModel ScheduleOfLease { get; set; }
    }
}
