using MongoDB.Driver;
using EVehicAPI.Models;

namespace EVehicAPI.Services;

public class VehicleService
{
    private readonly IMongoCollection<Vehicle> _vehicle;

    public VehicleService(IConfiguration config)
    {
        var client = new MongoClient(
            config["MongoDB:ConnectionURI"]);

        var database = client.GetDatabase(
            config["MongoDB:DatabaseName"]);

        _vehicle = database.GetCollection<Vehicle>(
            config["MongoDB:CollectionNameVehicles"]);
    }

    public async Task<List<Vehicle>> GetAsync() =>
        await _vehicle.Find(_ => true).ToListAsync();

    public async Task<Vehicle?> GetAsync(string id) =>
        await _vehicle.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Vehicle vehicle) =>
        await _vehicle.InsertOneAsync(vehicle);

    public async Task UpdateAsync(string id, Vehicle vehicle) =>
        await _vehicle.ReplaceOneAsync(x => x.Id == id, vehicle);

    public async Task DeleteAsync(string id) =>
        await _vehicle.DeleteOneAsync(x => x.Id == id);

    public async Task<List<Vehicle>> GetActiveAsync()
    {
        return await _vehicle
            .Find(v => v.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ChargeBatteryAsync(string id)
    {
        var vehicle = await _vehicle.Find(v => v.Id == id).FirstOrDefaultAsync();

        if (vehicle == null)
            return false;

        if (!vehicle.IsElectric)
            throw new Exception("This vehicle is not electric.");

        vehicle.Battery = 100;
        
        await _vehicle.ReplaceOneAsync(v => v.Id == id, vehicle);

        return true;
    }
}