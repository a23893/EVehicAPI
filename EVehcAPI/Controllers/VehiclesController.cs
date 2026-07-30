using Microsoft.AspNetCore.Mvc;
using EVehicAPI.Models;
using EVehicAPI.Services;

namespace EVehicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly VehicleService _service;

    public VehiclesController(VehicleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Vehicle>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> Get(string id)
    {
        var vehicle = await _service.GetAsync(id);

        if (vehicle == null)
            return NotFound();

        return vehicle;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Vehicle vehicle)
    {

        vehicle.Id = null;

        if(vehicle.IsElectric){
            vehicle.Battery = 0;
        }
        else
        {
            vehicle.Battery = null;
        }

        await _service.CreateAsync(vehicle);

        return CreatedAtAction(nameof(Get),
            new { id = vehicle.Id }, vehicle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Vehicle vehicle)
    {
        vehicle.Id = id;
        vehicle.LastUpdateAt = DateTime.UtcNow;
        
        await _service.UpdateAsync(id, vehicle);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<Vehicle>>> GetActive()
    {
        return await _service.GetActiveAsync();
    }

    [HttpPatch("{id}/charge")]
    public async Task<IActionResult> Charge(string id)
    {
        var success = await _service.ChargeBatteryAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }

}