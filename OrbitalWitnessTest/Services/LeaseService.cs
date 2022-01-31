using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrbitalWitnessTest.Enums;
using OrbitalWitnessTest.Helpers;
using OrbitalWitnessTest.Interfaces;
using OrbitalWitnessTest.Models;
using Serilog;

namespace OrbitalWitnessTest.Services
{
    public class LeaseService: ILeaseService
    {
        private readonly ILeasesScheduleRepository _leasesScheduleRepository;
        public LeaseService(ILeasesScheduleRepository leasesScheduleRepository)
        {
            _leasesScheduleRepository = leasesScheduleRepository;
        }

        public async Task<LeaseScheduleModel> ProcessNewLeaseSchedule(LeaseScheduleModel leaseSchedule)
        {
            foreach(var schedule in leaseSchedule.ScheduleEntry)
            {
                schedule.ScheduleOfLease = GetScheduleOfLeaseData(schedule.EntryText);
            }
            var newSchedule = await _leasesScheduleRepository.SaveLeasesSchedule(leaseSchedule);
            return MappingHelper.MapRepositoryToLeaseScheduleModel(newSchedule);
        }

        private ScheduleOfLeaseModel GetScheduleOfLeaseData(List<string> entryText)
        {
            var scheduleOfLease = new ScheduleOfLeaseModel();
            
            foreach(var entry in entryText)
            {
                //split when there are enough spaces that it's not just a sentence
                var entries = System.Text.RegularExpressions.Regex.Split(entry, @"\s{3,}");
                var skipEntries = 0;
                for (int i = 0; i < entries.Length; i++)
                {
                    /* basically if there is a big gap in the entry text, that column is 
                     * probably done, and so we can just progress everything else to the next 
                     * column along
                     */                     
                    if(EntryBelongsInNextField(entry, entries[i]))
                    {
                        skipEntries++;
                    }
                    var entryPosition = i + skipEntries;

                    var currentText =  entries[i];

                    switch ((ScheduleOfLeaseFields)entryPosition)
                    {
                        case ScheduleOfLeaseFields.RegistrationDateAndPlanRef:
                            if(entries[i].Contains("NOTE:")) // Making the assumption that notes will always start with 'NOTE:'
                            {
                                scheduleOfLease.Notes = $"{scheduleOfLease.Notes} {currentText}".Trim();
                            }
                            else
                            {
                                scheduleOfLease.RegistrationDateAndPlanRef = $"{scheduleOfLease.RegistrationDateAndPlanRef} {currentText}".Trim();
                            }
                            break;
                        case ScheduleOfLeaseFields.PropertyDescription:
                            scheduleOfLease.PropertyDescription = $"{scheduleOfLease.PropertyDescription} {currentText}".Trim();
                            break;
                        case ScheduleOfLeaseFields.DateOfLeaseAndTerm:
                            scheduleOfLease.DateOfLeaseAndTerm = $"{scheduleOfLease.DateOfLeaseAndTerm} {currentText}".Trim();
                            break;
                        case ScheduleOfLeaseFields.LesseesTitle:
                            scheduleOfLease.LesseesTitle = $"{scheduleOfLease.LesseesTitle} {currentText}".Trim();
                            break;

                    }
                }
            }
            VerifyEntry(scheduleOfLease);
            return scheduleOfLease;
        }


        /* 
         * Usually there are 4 to 6 spaces between fields so if there are more, we can 
         * assume that one is empty and move the field to the next place. This isn't really 
         * an exact practice but it's pretty tough to account for different numbers of spaces 
         * 
        */
        private bool EntryBelongsInNextField(string entryText, string entry)
        {
            if(string.IsNullOrEmpty(entry))
            {
                return false;
            }
            var spacesBeforeEntry = 0;
            var entryPosition = entryText.IndexOf(entry);
            var preText = entryText.Substring(0, entryPosition);
            if(preText.Length == 0)
            {
                return false;
            }
            for(var i=preText.Length-1;i>=0;i--)
            {
                if(char.IsWhiteSpace(preText[i]))
                {
                    spacesBeforeEntry++;

                    if(spacesBeforeEntry == 30)
                    {
                        /*
                            * 30 is a magic number I got through trial and error - 
                            * if there are more than 20 spaces between text then it probably means 
                            * the column doesnt have anything in that line, and I added more to give us 
                            * a margin for error (all the entries seem correct). I was going to do it 
                            * based on the number of spaces in the other parts of the 
                            * column (eg if there are 20 characters in one of them and
                            * more than 20 spaces then it probably doesn't belong in that section),
                            * but there still seems to be differences in the numbers of spaces either side 
                            * of that gap, so we'd still need to come up with another magic number to account for
                            * those. 
                        */
                        return true;
                    }

                } else
                {
                    break;
                }
            }
            return false; 
        }
        private void VerifyEntry(ScheduleOfLeaseModel lease)
        {
            /* 
             * Because it looks like the data coming in is fairly inconsistant, we can do something like this to flag up any entries that look wrong.
             * I'm not doing anything fancy with these at the moment other than logging, but we could potentially exclude them or store them in some sort 
             * of 'poison' table until we fix them
             * I made a few assumptions here that this is always the format, but it's just to give you the idea of what I'd do/how I'd mitigate the risk of 
             * incorrect data as much as possible
            */
            Regex date = new Regex(@"\d{2}.\d{2}.\d{4}");
            var validRegistrationDate = date.Match(lease.RegistrationDateAndPlanRef).Success;
            var validLeaseDate = date.Match(lease.DateOfLeaseAndTerm).Success;
            var validLesseeTitle = lease.LesseesTitle.StartsWith("EGL");
            if (!validLeaseDate || !validRegistrationDate || validLesseeTitle)
            {
                Log.Error($"Invalid Entry, {JsonConvert.SerializeObject(lease)} - Please investigate");
            }
        }

        public LeaseScheduleModel GetLeaseScheduleById(int id)
        {
            return MappingHelper.MapRepositoryToLeaseScheduleModel(_leasesScheduleRepository.GetLeasesScheduleById(id));
        }
    }
}
