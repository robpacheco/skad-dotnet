using System;
using System.Collections.Generic;

#nullable disable

namespace Skad.Subscription.Data.Model
{
    public partial class Subscription
    {
        public long SubscriptionId { get; set; }
        public string Tier { get; set; }
        public bool Active { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime DateExpires { get; set; }
        public string CardName { get; set; }
        public string CardLast4 { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
