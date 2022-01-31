using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrbitalWitnessTest.Interfaces;
using OrbitalWitnessTest.Models;
using Serilog;

namespace OrbitalWitnessTest.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LeaseController : ControllerBase
    {
        private readonly ILeaseService _leaseService;

        public LeaseController(ILeaseService leaseService)
        {
            _leaseService = leaseService;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(ScheduleOfNoticesOfLeaseModel schedule)
        {
            try
            {
                var newSchedule =  await _leaseService.ProcessNewLeaseSchedule(schedule.LeaseSchedule);
                return Ok(newSchedule);

            }
            catch(Exception ex)
            {
                Log.Error("Orbital Witness Project Error {Controller}, {Method}, {params}, {Exception}, {ExtraInfo}", "Lease", "PostAsync", JsonConvert.SerializeObject(schedule), ex, "");
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Ok(new { responseText = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var schedule = _leaseService.GetLeaseScheduleById(id);
                return Ok(schedule);

            }
            catch (Exception ex)
            {
                Log.Error("Orbital Witness Project Error {Controller}, {Method}, {params}, {Exception}, {ExtraInfo}", "Lease", "Get", id, ex, "");
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Ok(new { responseText = ex.Message });
            }
        }
    }
}
