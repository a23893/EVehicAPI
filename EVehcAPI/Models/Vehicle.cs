using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EVehicAPI.Models;

public class Vehicle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    [JsonIgnore]
    public string? Id { get; set; }

    public VehicleType Type { get; set; }

    public double Price { get; set; }

    [JsonIgnore]
    public int? Battery { get; set; } = 0;

    public bool IsActive { get; set; }

    public bool IsElectric { get; set; }

    public bool IsAvailable { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime? LastUpdateAt { get; set; } = null;
}

public enum VehicleType
{
    Bicycle,
    Scooter,
    Skateboard,
    Bike
}