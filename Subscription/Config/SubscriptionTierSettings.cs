using System.Collections.Generic;
using Skad.Subscription.Domain;

namespace Skad.Subscription.Config
{
    public class SubscriptionTierSettings
    {
        public List<SubscriptionTierEntry> SubscriptionTiers { get; set; } = new();
    }
    
    public class SubscriptionTierEntry
    {
        public string? TierName { get; set; }
        public decimal? TierPrice { get; set; }
        public int? TierDurationDays { get; set; }

        public bool IsValid()
        {
            return IsTierNameValid() && IsTierPriceValid() && IsTierDurationDaysValid();
        }

        private bool IsTierNameValid()
        {
            return TierName is { Length: > 0 };
        }

        private bool IsTierPriceValid()
        {
            return TierPrice is >= (decimal)0.00;
        }

        private bool IsTierDurationDaysValid()
        {
            return TierDurationDays is >= 1;
        }
    }
}