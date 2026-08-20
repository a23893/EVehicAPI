using MongoDB.Driver;
using EVehicAPI.Models;

namespace EVehicAPI.Services;

public class UserService
{
    private readonly IMongoCollection<User> _user;

    public UserService(IConfiguration config)
    {
        var client = new MongoClient(
            config["MongoDB:ConnectionURI"]);

        var database = client.GetDatabase(
            config["MongoDB:DatabaseName"]);

        _user = database.GetCollection<User>(
            config["MongoDB:CollectionNameUsers"]);
    }

    public async Task<List<User>> GetAsync() =>
        await _user.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _user.InsertOneAsync(user);

    public async Task UpdateAsync(string id, User user) =>
        await _user.ReplaceOneAsync(x => x.Id == id, user);

    public async Task DeleteAsync(string id) =>
        await _user.DeleteOneAsync(x => x.Id == id);

    public async Task SoftDeleteAsync(string id) =>
        await _user.UpdateOneAsync(x => x.Id == id, Builders<User>.Update.Set(u => u.IsActive, false));

    public async Task<List<User>> GetActiveAsync()
    {
        return await _user
            .Find(u => u.IsActive)
            .ToListAsync();
    }

    /// <summary>
    /// Adds money to a user's balance.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public async Task<bool> AddMoneyAsync(string id, double amount)
    {
        var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();

        if (user == null)
            return false;

        user.Balance += amount;
        
        await _user.ReplaceOneAsync(u => u.Id == id, user);

        return true;
    }

    /// <summary>
    /// Changes the status of a user (active/inactive).
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task ChangeStatusAsync(string id)
    {
        var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (user.IsActive)
            user.IsActive = false;
        else
            user.IsActive = true;

        await _user.ReplaceOneAsync(u => u.Id == id, user);
    }
        
}