namespace Skad.Subscription.Domain.Service;

public interface IReceiptManager
{
    string? Receipt { get; }
    bool HasReceipt { get; }
    void AddReceipt(Data.Model.Subscription subscription);
}
