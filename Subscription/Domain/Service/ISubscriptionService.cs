using System.Threading.Tasks;

namespace Skad.Subscription.Domain.Service
{
    public interface ISubscriptionService
    {
        Task<Data.Model.Subscription?> FindLatestActiveSubscription();
        Task<Data.Model.Subscription> AddSubscription(Data.Model.Subscription subscription, SubscriptionTier tier);
        string? FindReceipt();
        bool HasReceipt();
    }
}