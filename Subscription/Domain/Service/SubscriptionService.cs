using System;
using System.Threading.Tasks;
using System.Transactions;
using Skad.Subscription.Domain.Repository;

namespace Skad.Subscription.Domain.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
        }

        public async Task<Data.Model.Subscription?> FindLatestActiveSubscription()
        {
            return await _subscriptionRepository.FindLatestActiveSubscription();
        }

        public async Task<Data.Model.Subscription> AddSubscription(Data.Model.Subscription subscription)
        {
            subscription.DatePurchased = DateTime.Now;
            subscription.DateExpires = subscription.DatePurchased.Add(TimeSpan.FromDays(365));
            subscription.AmountPaid = (decimal)500.00;
            subscription.Active = true;

            using var scope =
                new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
            
            await _subscriptionRepository.InactivateCurrentSubscriptions();
            var created = await _subscriptionRepository.AddSubscription(subscription);
            
            scope.Complete();
            
            return created;
        }
    }
}