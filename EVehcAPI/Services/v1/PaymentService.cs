using MongoDB.Driver;
using EVehicAPI.Models;

namespace EVehicAPI.Services;

public class PaymentService
{
    private readonly IMongoCollection<Payment> _payments;
    private readonly RentalService _rentalService;
    private readonly FineService _fineService;

    public PaymentService(
        IConfiguration config,
        RentalService rentalService,
        FineService fineService)
    {
        _rentalService = rentalService;
        _fineService = fineService;

        var client = new MongoClient(config["MongoDB:ConnectionURI"]);

        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);

        _payments = database.GetCollection<Payment>(
            config["MongoDB:CollectionNamePayments"]);
    }

    public async Task<List<Payment>> GetAsync() =>
        await _payments.Find(_ => true).ToListAsync();

    public async Task<Payment?> GetAsync(string id) =>
        await _payments.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        Payment newPayment = new Payment();

        try{

            if (payment.CreatedAt <= payment.ExpiryDate)
                throw new ArgumentException("Expiry date must be after creation date.");

            if (payment.Type == PaymentType.Rental)
            {
                var rental = await _rentalService.GetAsync(payment.PayableId);

                if (rental != null && rental.Id == payment.PayableId)
                {
                    newPayment.PayableId = payment.PayableId;
                    newPayment.Type = PaymentType.Rental;
                }
                else if (payment.Type == PaymentType.Fine)
                {
                    var fine = await _fineService.GetAsync(payment.PayableId);
                    if (fine != null && fine.Id == payment.PayableId)
                    {
                        newPayment.PayableId = payment.PayableId;
                        newPayment.Type = PaymentType.Fine;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Rental or Fine not found.");
                    }
                }

            }
            else
            {
                throw new ArgumentException("Invalid payment type.");
            }

            newPayment.Status = PaymentStatus.Pending;
            newPayment.CreatedAt = DateTime.UtcNow;
            newPayment.Method = payment.Method;
            newPayment.Description = payment.Description;
            newPayment.Value = payment.Value;
            newPayment.ExpiryDate = DateTime.UtcNow.AddDays(30); // Set expiry date to 30 days from now
            newPayment.Expired = false;

            await _payments.InsertOneAsync(newPayment);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating payment: {ex.Message}");
        }
    }

    public async Task UpdateAsync(string id, Payment payment) =>
        await _payments.ReplaceOneAsync(x => x.Id == id, payment);

    public async Task DeleteAsync(string id) =>
        await _payments.DeleteOneAsync(x => x.Id == id);

}