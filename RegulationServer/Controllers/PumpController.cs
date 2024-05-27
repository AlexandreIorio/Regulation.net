using Microsoft.AspNetCore.Mvc;
using RegulationLib.Models;

namespace RegulationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PumpController : ControllerBase
    {
        private Dictionary<int, Pump> _pumps;

        public PumpController(Dictionary<int, Pump> pumps)
        {
            _pumps = pumps;
        }

        [HttpGet("{id:int}/{speed:int}")]
        public IActionResult SetSpeed(int id, int speed)
        {
            Pump? pump = _pumps.GetValueOrDefault(id);
            if (pump is null)
            {
                return NotFound("Pump not found.");
            }
            pump.SetSpeed(speed);
            return Ok($"#{id} - {pump.Name} speed set to {speed}%.");
        }

        [HttpGet("{id:int}/start")]
        public IActionResult Start(int id)
        {
            Pump? pump = _pumps.GetValueOrDefault(id);
            if (pump is null)
            {
                return NotFound("Pump not found.");
            }
            pump.Start();
            return Ok($"#{id} - {pump.Name} started.");
        }
    }
}