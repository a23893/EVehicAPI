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

    public string Type { get; set; } = "";

    public double Price { get; set; }

    [JsonIgnore]
    public int? Battery { get; set; } = 0;

    public bool Active { get; set; }

    public bool IsElectric { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime? LastUpdateAt { get; set; } = null;
}