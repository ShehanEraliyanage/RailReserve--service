using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RailReserve.Model;
using RailReserve.Repository;

namespace RailReserve.Controllers
{
    [Route("api/v1/train")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService) => _trainService = trainService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _trainService.GetAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _trainService.GetAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Train train)
        {
          var result = await _trainService.CreateAsync(train);
          return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Train train)
        {
            var result = await _trainService.UpdateAsync(train);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _trainService.Removeasync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }


    }
}
