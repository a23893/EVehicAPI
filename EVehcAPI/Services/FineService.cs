using MongoDB.Driver;
using EVehicAPI.Models;

namespace EVehicAPI.Services;

public class FineService
{
    private readonly IMongoCollection<Fine> _fines;
    private readonly RentalService _rentalService;
    private readonly UserService _userService;

    public FineService(
        IConfiguration config,
        RentalService rentalService,
        UserService userService)
    {
        _rentalService = rentalService;
        _userService = userService;

        var client = new MongoClient(config["MongoDB:ConnectionURI"]);

        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);

        _fines = database.GetCollection<Fine>(
            config["MongoDB:CollectionNameFines"]);
    }

    public async Task<List<Fine>> GetAsync() =>
        await _fines.Find(_ => true).ToListAsync();

    public async Task<Fine?> GetAsync(string id) =>
        await _fines.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Fine fine)
    {
        ArgumentNullException.ThrowIfNull(fine);

        var rental = await _rentalService.GetAsync(fine.RentalId);

        if (rental == null)
            throw new KeyNotFoundException("Rental not found.");

        var user = await _userService.GetAsync(fine.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!user.IsActive)
            throw new InvalidOperationException("User is not active.");

        if (rental.UserId != fine.UserId)
            throw new InvalidOperationException("Rental ID does not match the fine's user ID.");

        fine.CreatedAt = DateTime.UtcNow;

        fine.Value = rental.TotalPrice * 10 / 100; // 10% of the rental total price

        await _fines.InsertOneAsync(fine);
    }

    /// <summary>
    ///  Creates a fine with a custom value
    /// </summary>
    /// <param name="fine"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task CreateCustomValueAsync(Fine fine, double customValue)
    {
        ArgumentNullException.ThrowIfNull(fine);

        var rental = await _rentalService.GetAsync(fine.RentalId);

        if (rental == null)
            throw new KeyNotFoundException("Rental not found.");

        var user = await _userService.GetAsync(fine.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!user.IsActive)
            throw new InvalidOperationException("User is not active.");

        fine.CreatedAt = DateTime.UtcNow;

        fine.Value = customValue;

        await _fines.InsertOneAsync(fine);
    }

    public async Task UpdateAsync(string id, Fine fine) =>
        await _fines.ReplaceOneAsync(x => x.Id == id, fine);

    public async Task DeleteAsync(string id) =>
        await _fines.DeleteOneAsync(x => x.Id == id);

    public async Task PayAsync(string id) =>
        await _fines.UpdateOneAsync(x => x.Id == id, Builders<Fine>.Update.Set(f => f.Payed, true).Set(f => f.PayedAt, DateTime.UtcNow));

}