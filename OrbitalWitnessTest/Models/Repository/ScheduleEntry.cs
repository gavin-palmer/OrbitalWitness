using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrbitalWitnessTest.Models.Repository
{
    [Table(nameof(ScheduleEntry))]
    public class ScheduleEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LeasesScheduleId { get; set; }
        public int EntryNumber { get; set; }
        public string EntryDate { get; set; }
        public string EntryType { get; set; }

        [ForeignKey("LeasesScheduleId")]
        public LeaseSchedule LeasesSchedule { get; set; }

        public ScheduleData ScheduleData { get; set; }
    }
}
