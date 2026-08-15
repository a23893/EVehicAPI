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

        return Ok(user);
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

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);

        return Ok("User deleted successfully.");
    }

    [HttpPatch("deactivate/{id}")]
    public async Task<IActionResult> SoftDelete(string id)
    {
        await _service.SoftDeleteAsync(id);

        return Ok("User deactivated successfully.");
    }

    /// <summary>
    /// Returns a list of all active users.
    /// </summary>
    /// <returns></returns>
    [HttpGet("active")]
    public async Task<ActionResult<List<User>>> GetActive()
    {
        return await _service.GetActiveAsync();
    }

    /// <summary>
    /// Adds money to a user's balance.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    [HttpPatch("{id}/add-money")]
    public async Task<IActionResult> AddMoney(string id, [FromBody] double amount)
    {
        var result = await _service.AddMoneyAsync(id, amount);

        if (!result)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Changes the status of a user (active/inactive).
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("change-status/{id}")]
    public async Task<IActionResult> ChangeStatus(string id)
    {
        await _service.ChangeStatusAsync(id);

        return Ok("User status changed successfully.");
    }
}