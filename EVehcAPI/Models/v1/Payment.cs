using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EVehicAPI.Models;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    [JsonIgnore]
    public string? Id { get; set; }

    public string? PayableId { get; set; } // ID of the entity that the payment is for (e.g., Rental ID, Fine ID)

    public PaymentType Type { get; set; }

    public PaymentMethod Method { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string Description { get; set; } = "";

    public int Value { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public DateTime? LastUpdateAt { get; set; } = null;

    [JsonIgnore]
    public DateTime? PaymentDate { get; set; } = null;

    [JsonIgnore]
    public DateTime? ExpiryDate { get; set; } = null;

    [JsonIgnore]
    public bool Expired { get; set; } = false;
}

public enum PaymentStatus
{
    Pending,
    Complete,
    Cancelled
}

public enum PaymentType
{
    Rental,
    Fine
}

public enum PaymentMethod
{
    VISA,
    MasterCard,
    PayPal,
    BankTransfer,
    BankReference,
    MBWay
}