using System.Threading.Tasks;

namespace Skad.Subscription.Domain.Repository
{
    public interface ISubscriptionRepository
    {
        Task<Data.Model.Subscription?> FindLatestActiveSubscription();
        Task<Data.Model.Subscription> AddSubscription(Data.Model.Subscription subscription);
        Task InactivateCurrentSubscriptions();
    }
}