using Microsoft.AspNetCore.Mvc;
using EVehicAPI.Models;
using EVehicAPI.Services;

namespace EVehicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _service.GetAsync(id);

        if (user == null)
            return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User user)
    {

        user.Id = null;

        await _service.CreateAsync(user);

        return CreatedAtAction(nameof(Get),
            new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, User user)
    {
        user.Id = id;
        user.LastUpdateAt = DateTime.UtcNow;

        await _service.UpdateAsync(id, user);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }

    [HttpPatch("deactivate/{id}")]
    public async Task<IActionResult> SoftDelete(string id)
    {
        await _service.SoftDeleteAsync(id);

        return NoContent();
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<User>>> GetActive()
    {
        return await _service.GetActiveAsync();
    }

    [HttpPost("{id}/add-money")]
    public async Task<IActionResult> AddMoney(string id, [FromBody] double amount)
    {
        var result = await _service.AddMoneyAsync(id, amount);

        if (!result)
            return NotFound();

        return NoContent();
    }
}