using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailReserve.Model;
using RailReserve.Repository;

namespace RailReserve.Controller
{
    [Route("api/v1/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservation) => _reservationService = reservation;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _reservationService.GetAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _reservationService.GetAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Reservation reservation)
        {
            var result = await _reservationService.CreateAsync(reservation);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Reservation reservation)
        {
            var result = await _reservationService.UpdateAsync(reservation);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _reservationService.Removeasync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetPendingByTravelerID/{id}")]
        public async Task<IActionResult> GetPendingByTravelerID(string id)
        {
            var result = await _reservationService.GetPendingByTravelerIDAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetFineshByTravelerID/{id}")]
        public async Task<IActionResult> GetFineshByTravelerID(string id)
        {
            var result = await _reservationService.GetFineshByTravelerIDAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetByScheduleID/{id}")]
        public async Task<IActionResult> GetByScheduleID(string id)
        {
            var result = await _reservationService.GetByScheduleIDAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
