using MongoDB.Driver;
using EVehicAPI.Models;

namespace EVehicAPI.Services;

public class RentalService
{
    private readonly IMongoCollection<Rental> _rentals;
    private readonly VehicleService _vehicleService;
    private readonly UserService _userService;

    public RentalService(
        IConfiguration config,
        VehicleService vehicleService,
        UserService userService)
    {
        _vehicleService = vehicleService;
        _userService = userService;

        var client = new MongoClient(config["MongoDB:ConnectionURI"]);

        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);

        _rentals = database.GetCollection<Rental>(
            config["MongoDB:CollectionNameRentals"]);
    }

    public async Task<List<Rental>> GetAsync() =>
        await _rentals.Find(_ => true).ToListAsync();

    public async Task<Rental?> GetAsync(string id) =>
        await _rentals.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Rental rental)
    {
        ArgumentNullException.ThrowIfNull(rental);

        if (rental.EndDate <= rental.StartDate)
            throw new ArgumentException("End date must be after start date.");

        var vehicle = await _vehicleService.GetAsync(rental.VehicleId);

        if (vehicle == null)
            throw new KeyNotFoundException("Vehicle not found.");

        var user = await _userService.GetAsync(rental.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!user.IsActive)
            throw new InvalidOperationException("User is not active.");

        if (!vehicle.IsActive)
            throw new InvalidOperationException("Vehicle is not active.");

        int durationMinutes = (int)(rental.EndDate - rental.StartDate).TotalMinutes;

        rental.TotalPrice = durationMinutes * vehicle.Price;

        rental.Status = RentalStatus.Pending;

        rental.Delivered = false;

        await _rentals.InsertOneAsync(rental);
    }

    public async Task UpdateAsync(string id, Rental rental) =>
        await _rentals.ReplaceOneAsync(x => x.Id == id, rental);

    public async Task DeleteAsync(string id) =>
        await _rentals.DeleteOneAsync(x => x.Id == id);

    /// <summary>
    /// Marks a rental as paid.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> PayAsync(string id)
    {
        var rental = await _rentals.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (rental == null)
            return false;

        rental.Payed = true;
        await _rentals.ReplaceOneAsync(r => r.Id == id, rental);
        return true;
    }

    /// <summary>
    /// Marks a rental as delivered.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeliverAsync(string id)
    {
        var rental = await _rentals.Find(r => r.Id == id).FirstOrDefaultAsync();

        if (rental == null)
            return false;

        rental.Delivered = true;
        await _rentals.ReplaceOneAsync(r => r.Id == id, rental);
        return true;
    }
}