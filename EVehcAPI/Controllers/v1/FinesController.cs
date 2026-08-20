using Microsoft.AspNetCore.Mvc;
using EVehicAPI.Models;
using EVehicAPI.Services;

namespace EVehicAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FinesController : ControllerBase
{
    private readonly FineService _service;

    public FinesController(FineService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Fine>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Fine>> Get(string id)
    {
        var fine = await _service.GetAsync(id);

        if (fine == null)
            return NotFound();

        return Ok(fine);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Fine fine)
    {
        try
        {
            fine.Id = null;

            await _service.CreateAsync(fine);

            return CreatedAtAction(nameof(Get),
                new { id = fine.Id }, fine);
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
    public async Task<IActionResult> Put(string id, Fine fine)
    {
        fine.Id = id;
        fine.LastUpdateAt = DateTime.UtcNow;

        await _service.UpdateAsync(id, fine);

        return Ok(fine);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);

        return Ok("Fine deleted successfully.");
    }

    [HttpPatch("{id}/pay")]
    public async Task<IActionResult> Pay(string id) 
    {
        var fine = await _service.GetAsync(id);

        if (fine == null)
            return NotFound();

        if (fine.Payed)
            return BadRequest("Fine is already payed.");

        await _service.PayAsync(id);

        return Ok("Fine payed successfully.");
    }

    [HttpPost("custom-value")]
    public async Task<IActionResult> CustomPost(Fine fine, [FromQuery] double customValue)
    {
        try
        {
            fine.Id = null;

            await _service.CreateCustomValueAsync(fine, customValue);

            return CreatedAtAction(nameof(Get),
                new { id = fine.Id }, fine);
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
}