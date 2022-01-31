using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrbitalWitnessTest.Models.Repository
{
    [Table(nameof(ScheduleData))]
    public class ScheduleData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ScheduleEntryId { get; set; }
        public string RegistrationDateAndPlanRef { get; set; }
        public string PropertyDescription { get; set; }
        public string DateOfLeaseAndTerm { get; set; }
        public string LesseesTitle { get; set; }
        public string Notes { get; set; }

        [ForeignKey("ScheduleEntryId")]
        public ScheduleEntry ScheduleEntry { get; set; }

    }
}
