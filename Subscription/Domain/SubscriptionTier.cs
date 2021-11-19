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