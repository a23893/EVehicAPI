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

    public string Name { get; set; } = "";

    public string Type { get; set; } = "";

    public string Description { get; set; } = "";

    public int Value { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}