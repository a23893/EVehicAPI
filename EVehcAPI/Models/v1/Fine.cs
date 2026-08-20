using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EVehicAPI.Models;

public class Fine
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    [JsonIgnore]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = "";

    [BsonRepresentation(BsonType.ObjectId)]
    public string RentalId { get; set; } = "";

    public string Name { get; set; } = "";

    public string Type { get; set; } = "";

    public string Description { get; set; } = "";

    public double Value { get; set; }

    public DateTime ExpireDate { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime? LastUpdateAt { get; set; } = null;

    [JsonIgnore]
    public bool Payed { get; set; } = false;

    [JsonIgnore]
    public DateTime? PayedAt { get; set; } = null;
}