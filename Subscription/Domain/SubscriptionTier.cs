using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Skad.Subscription.Config;

namespace Skad.Subscription.Domain
{
    public class SubscriptionTier
    {
        public SubscriptionTier(string tierName, decimal tierPrice, int tierDurationDays)
        {
            TierName = tierName;
            TierPrice = tierPrice;
            TierDurationDays = tierDurationDays;
        }

        public string TierName { get; }
        public decimal TierPrice { get; }
        public int TierDurationDays { get; }
    }

    public class SubscriptionTiers
    {
        private readonly IList<SubscriptionTier> _tiers;

        // public SubscriptionTiers() : this(new List<SubscriptionTier>
        // {
        //     new SubscriptionTier(tierName: "free", tierPrice: (decimal)0.00, tierDurationDays: 30),
        //     new SubscriptionTier(tierName: "developer", tierPrice: (decimal)9.99, tierDurationDays: 30),
        //     new SubscriptionTier(tierName: "startup", tierPrice: (decimal)99.99, tierDurationDays: 30),
        //     new SubscriptionTier(tierName: "enterprise", tierPrice: (decimal)100000.00, tierDurationDays: 365)
        // })
        // {
        // }

        public SubscriptionTiers(SubscriptionTierSettings settings)
        {
            _tiers = settings.SubscriptionTiers
                .Where(c => c.IsValid())
                .Select(c => new SubscriptionTier(c.TierName!, c.TierPrice!.Value, c.TierDurationDays!.Value))
                .ToImmutableList();
        }

        public bool IsValidTierName(string tierName)
        {
            return _tiers.Any(t => t.TierName.Equals(tierName, StringComparison.CurrentCultureIgnoreCase));
        }

        public SubscriptionTier? FindTier(string? tierName)
        {
            if (tierName == null)
            {
                return null;
            }
            
            return _tiers.FirstOrDefault(t => t.TierName.Equals(tierName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}