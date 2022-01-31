using System;
using System.Linq;
using OrbitalWitnessTest.Models;
using OrbitalWitnessTest.Models.Repository;

namespace OrbitalWitnessTest.Helpers
{
    public static class MappingHelper
    {
        public static LeaseSchedule MapLeaseScheduleModelToRepository(LeaseScheduleModel model)
        {
            return new LeaseSchedule
            {
                ScheduleType = model.ScheduleType,                
            };
        }
        public static ScheduleEntry MapScheduleEntryModelToRepository(ScheduleEntryModel model)
        {
            return new ScheduleEntry
            {
                EntryDate = model.EntryDate,
                EntryNumber = Int32.Parse(model.EntryNumber),
                EntryType = model.EntryType,                
            };
        }
        public static ScheduleData MapScheduleOfLeaseModelToRepository(ScheduleOfLeaseModel model)
        {
            return new ScheduleData
            {
                DateOfLeaseAndTerm = model.DateOfLeaseAndTerm,
                LesseesTitle = model.LesseesTitle,
                PropertyDescription = model.PropertyDescription,
                RegistrationDateAndPlanRef = model.RegistrationDateAndPlanRef,
                Notes = model.Notes
            };
        }
        public static LeaseScheduleModel MapRepositoryToLeaseScheduleModel(LeaseSchedule model)
        {
            return new LeaseScheduleModel
            {
                ScheduleEntry = model.ScheduleEntries.Select(x => MapRepositoryToScheduleEntryModel(x)).ToList(),
                ScheduleType = model.ScheduleType
            };
        }
        public static ScheduleEntryModel MapRepositoryToScheduleEntryModel(ScheduleEntry model)
        {
            return new ScheduleEntryModel
            {
                EntryDate = model.EntryDate,
                EntryNumber = model.EntryNumber.ToString(),
                EntryType = model.EntryType,
                ScheduleOfLease = MapRepositoryToScheduleOfLeaseModel(model.ScheduleData),
            };
        }
        public static ScheduleOfLeaseModel MapRepositoryToScheduleOfLeaseModel(ScheduleData model)
        {
            return new ScheduleOfLeaseModel
            {
                DateOfLeaseAndTerm = model.DateOfLeaseAndTerm,
                LesseesTitle = model.LesseesTitle,
                Notes = model.Notes,
                PropertyDescription = model.PropertyDescription,
                RegistrationDateAndPlanRef = model.RegistrationDateAndPlanRef
            };
        }

    }
}
