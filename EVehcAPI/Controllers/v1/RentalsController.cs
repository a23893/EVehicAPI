using Microsoft.AspNetCore.Mvc;
using EVehicAPI.Models;
using EVehicAPI.Services;

namespace EVehicAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly RentalService _service;

    public RentalsController(RentalService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Rental>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> Get(string id)
    {
        var rental = await _service.GetAsync(id);

        if (rental == null)
            return NotFound();

        return Ok(rental);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Rental rental)
    {
        try
        {
            rental.Id = null;

            await _service.CreateAsync(rental);

            return CreatedAtAction(nameof(Get),
                new { id = rental.Id }, rental);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Rental rental)
    {
        rental.Id = id;
        rental.LastUpdateAt = DateTime.UtcNow;

        await _service.UpdateAsync(id, rental);

        return Ok(rental);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);

        return Ok("Rental deleted successfully.");
    }

    [HttpPatch("pay/{id}")]
    public async Task<IActionResult> Pay(string id)
    {
        var result = await _service.PayAsync(id);

        if (!result)
            return NotFound();

        return Ok("Rental paid successfully.");
    }

    [HttpPatch("deliver/{id}")]
    public async Task<IActionResult> Deliver(string id)
    {
        var result = await _service.DeliverAsync(id);

        if (!result)
            return NotFound();

        return Ok("Rental delivered successfully.");
    }
}