using Microsoft.AspNetCore.Mvc;
using RegulationLib.Models;

namespace RegulationServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemperatureSensorController : ControllerBase
{
    private readonly Dictionary<int, TemperatureSensor> _sensors;

    public TemperatureSensorController(Dictionary<int, TemperatureSensor> sensors)
    {
        _sensors = sensors;
    }

    [HttpGet("{id:int}")]
    public IActionResult GetTemperature(int id)
    {
        TemperatureSensor? sensor = _sensors.GetValueOrDefault(id);
        if (sensor is null)
        {
            return NotFound("Temperature sensor not found.");
        }

        double? temperature = sensor.GetTemperature();
        if (temperature is null)
        {
            return StatusCode(500, "Failed to read temperature.");
        }

        return Ok($"#{id} - {sensor.Name} - temperature: {temperature}°C.");
    }
}