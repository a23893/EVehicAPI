using Microsoft.AspNetCore.Mvc;
using EVehicAPI.Models;
using EVehicAPI.Services;

namespace EVehicAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _service;

    public PaymentsController(PaymentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<Payment>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> Get(string id)
    {
        var payment = await _service.GetAsync(id);

        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Payment payment)
    {
        try
        {
            payment.Id = null;

            await _service.CreateAsync(payment);

            return CreatedAtAction(nameof(Get),
                new { id = payment.Id }, payment);
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
    public async Task<IActionResult> Put(string id, Payment payment)
    {
        payment.Id = id;
        payment.LastUpdateAt = DateTime.UtcNow;

        await _service.UpdateAsync(id, payment);

        return Ok(payment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);

        return Ok("Payment deleted successfully.");
    }
}