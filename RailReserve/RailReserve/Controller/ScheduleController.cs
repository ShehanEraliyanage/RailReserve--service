using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailReserve.Model;
using RailReserve.Repository;
using RailReserve.Service;

namespace RailReserve.Controller
{
    [Route("api/v1/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService) => _scheduleService = scheduleService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _scheduleService.GetAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _scheduleService.GetAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Schedule schedule)
        {
            var result = await _scheduleService.CreateAsync(schedule);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Schedule schedule)
        {
            var result = await _scheduleService.UpdateAsync(schedule);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _scheduleService.Removeasync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
