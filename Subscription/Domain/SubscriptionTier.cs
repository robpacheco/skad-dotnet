using System;
using System.Collections.Generic;
using System.Linq;

namespace Skad.Subscription.Domain
{
    public class SubscriptionTier
    {
        public string? TierName { get; set; }
        public decimal? TierPrice { get; set; }
        public int? TierDurationDays { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }

    public class SubscriptionTiers
    {
        private readonly IList<SubscriptionTier> _tiers;

        public SubscriptionTiers() : this(new List<SubscriptionTier>
        {
            new SubscriptionTier
            {
                TierName = "free",
                TierPrice = (decimal)0.00,
                TierDurationDays = 30
            },
            new SubscriptionTier
            {
                TierName = "developer",
                TierPrice = (decimal)9.99,
                TierDurationDays = 30
            },
            new SubscriptionTier
            {
                TierName = "startup",
                TierPrice = (decimal)99.99,
                TierDurationDays = 30
            },
            new SubscriptionTier
            {
                TierName = "enterprise",
                TierPrice = (decimal)100000.00,
                TierDurationDays = 365
            }
        })
        {
        }

        public SubscriptionTiers(IEnumerable<SubscriptionTier> tiers)
        {
            _tiers = tiers.Where(t => t.IsValid()).ToList();
        }

        public bool IsValidTierName(string tierName)
        {
            return _tiers.Any(t => t.TierName != null && t.TierName.Equals(tierName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}