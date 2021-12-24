using System;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Skad.Subscription.Domain.Repository;

namespace Skad.Subscription.Domain.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly SubscriptionTiers _tiers;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, SubscriptionTiers tiers, ILogger<SubscriptionService> logger)
        {
            _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
            _tiers = tiers ?? throw new ArgumentNullException(nameof(tiers));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Data.Model.Subscription?> FindLatestActiveSubscription()
        {
            return await _subscriptionRepository.FindLatestActiveSubscription();
        }

        public async Task<Data.Model.Subscription> AddSubscription(Data.Model.Subscription subscription, SubscriptionTier tier)
        {
            subscription.DatePurchased = DateTime.Now.ToUniversalTime();
            subscription.DateExpires = subscription.DatePurchased.Add(TimeSpan.FromDays(tier.TierDurationDays));
            subscription.AmountPaid = tier.TierPrice;
            subscription.Active = true;

            using var scope =
                new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
            
            await _subscriptionRepository.InactivateCurrentSubscriptions();
            var created = await _subscriptionRepository.AddSubscription(subscription);
            
            scope.Complete();
            
            _logger.LogDebug("Added a new active subscription.");
            
            return created;
        }
    }
}