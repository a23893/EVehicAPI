using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EVehicAPI.Models;

public class Rental
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    [JsonIgnore]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = "";

    [BsonRepresentation(BsonType.ObjectId)]
    public string VehicleId { get; set; } = "";

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime EndDate { get; set; }

    [JsonIgnore]
    public double TotalPrice { get; set; }

    public RentalStatus Status { get; set; } = RentalStatus.Pending;

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime? LastUpdateAt { get; set; } = null;

    [JsonIgnore]
    public bool Payed { get; set; } = false;

    [JsonIgnore]
    public bool Fined { get; set; } = false;

    [JsonIgnore]
    public DateTime? PayedAt { get; set; } = null;
}

public enum RentalStatus
{
    Pending,
    Active,
    Completed,
    Cancelled
}