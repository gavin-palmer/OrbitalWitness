using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using OrbitalWitnessTest.Interfaces;
using OrbitalWitnessTest.Models;
using OrbitalWitnessTest.Services;
using Xunit;

namespace OrbitalWitnessTest.Tests.Services
{
    public class LeaseServiceTest
    {


        [Fact]
        public void BasicGet()
        {
            var mockLeasesScheduleRepository = new Mock<ILeasesScheduleRepository>();
            var leaseService = new LeaseService(mockLeasesScheduleRepository.Object);
            mockLeasesScheduleRepository.Setup(x => x.GetLeasesScheduleById(It.IsAny<int>())).Returns(new Models.Repository.LeaseSchedule
            {
                ScheduleEntries = new System.Collections.Generic.List<Models.Repository.ScheduleEntry>
                {
                    new Models.Repository.ScheduleEntry
                    {
                        ScheduleData = new Models.Repository.ScheduleData()
                    }
                }
            });
            var result = leaseService.GetLeaseScheduleById(1);
            Assert.IsType<LeaseScheduleModel>(result);
            mockLeasesScheduleRepository.Verify(x => x.GetLeasesScheduleById(It.IsAny<int>()), Times.Once());
        }
        [Fact]
        public void BasicSave()
        {
            var mockLeasesScheduleRepository = new Mock<ILeasesScheduleRepository>();
            var leaseService = new LeaseService(mockLeasesScheduleRepository.Object);
            var model = new LeaseScheduleModel
            {
                ScheduleType = "test",
                ScheduleEntry = new System.Collections.Generic.List<ScheduleEntryModel>
                {
                    new ScheduleEntryModel
                    {
                        EntryText = new System.Collections.Generic.List<string>
                        {
                            "test1          test2          test3          test4",
                            "test1          test2          test3          test4",
                            "test1          test2          test3          test4"
                        }
                    }
                }
            };
            mockLeasesScheduleRepository.Setup(x => x.SaveLeasesSchedule(model)).ReturnsAsync(new Models.Repository.LeaseSchedule
            {
                ScheduleEntries = new System.Collections.Generic.List<Models.Repository.ScheduleEntry>
                {
                    new Models.Repository.ScheduleEntry
                    {
                        ScheduleData = new Models.Repository.ScheduleData
                        {
                            RegistrationDateAndPlanRef = "test1 test1 test1",
                            PropertyDescription = "test2 test2 test2",
                            DateOfLeaseAndTerm = "test3 test3 test3",
                            LesseesTitle = "test4 test4 test4"
                        }
                    }
                }
            });
            var result = leaseService.ProcessNewLeaseSchedule(model);
            Assert.Equal("test1 test1 test1", result.Result.ScheduleEntry.FirstOrDefault().ScheduleOfLease.RegistrationDateAndPlanRef);
            Assert.Equal("test2 test2 test2", result.Result.ScheduleEntry.FirstOrDefault().ScheduleOfLease.PropertyDescription);
            Assert.Equal("test3 test3 test3", result.Result.ScheduleEntry.FirstOrDefault().ScheduleOfLease.DateOfLeaseAndTerm);
            Assert.Equal("test4 test4 test4", result.Result.ScheduleEntry.FirstOrDefault().ScheduleOfLease.LesseesTitle);
            Assert.Null(result.Result.ScheduleEntry.FirstOrDefault().ScheduleOfLease.Notes);

        }

        [Fact]
        public void ExtractNotes()
        {
            var mockLeasesScheduleRepository = new Mock<ILeasesScheduleRepository>();
            var leaseService = new LeaseService(mockLeasesScheduleRepository.Object);
            var model = new LeaseScheduleModel
            {
                ScheduleType = "test",
                ScheduleEntry = new System.Collections.Generic.List<ScheduleEntryModel>
                {
                    new ScheduleEntryModel
                    {
                        EntryText = new System.Collections.Generic.List<string>
                        {
                            "NOTE: test1 test2 test3 test4",
                            "test1          test2          test3          test4",
                            "test1          test2          test3          test4"
                        }
                    }
                }
            };
            mockLeasesScheduleRepository.Setup(x => x.SaveLeasesSchedule(model)).ReturnsAsync(new Models.Repository.LeaseSchedule
            {
                ScheduleEntries = new System.Collections.Generic.List<Models.Repository.ScheduleEntry>
                {
                    new Models.Repository.ScheduleEntry
                    {
                        ScheduleData = new Models.Repository.ScheduleData
                        {
                            RegistrationDateAndPlanRef = "test1 test1",
                            PropertyDescription = "test2 test2",
                            DateOfLeaseAndTerm = "test3 test3",
                            LesseesTitle = "test4 test4",
                            Notes = "NOTE: test1 test2 test3 test4"
                        }
                    }
                }
            });
            var result = leaseService.ProcessNewLeaseSchedule(model);
        }
    }
}

