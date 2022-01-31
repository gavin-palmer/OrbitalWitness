using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrbitalWitnessTest.Models.Repository
{
    [Table(nameof(LeaseSchedule))]
    public class LeaseSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ScheduleType { get; set; }

        public List<ScheduleEntry> ScheduleEntries { get; set; }
    }
}
    