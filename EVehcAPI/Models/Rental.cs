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

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double TotalPrice { get; set; }

    public RentalStatus Status { get; set; } = RentalStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum RentalStatus
{
    Pending,
    Active,
    Completed,
    Cancelled
}