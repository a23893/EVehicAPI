using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EVehicAPI.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    [JsonIgnore]
    public string? Id { get; set; }

    public string UserName { get; set; } = "";

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Email { get; set; } = "";

    public string PhoneNumber { get; set; } = "";

    public double Balance { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime? LastUpdateAt { get; set; } = null;

    [JsonIgnore]
    public DateTime? LastLoginAt { get; set; } = null;
}