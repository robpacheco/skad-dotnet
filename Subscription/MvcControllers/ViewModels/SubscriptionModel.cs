using Microsoft.AspNetCore.Mvc.ModelBinding;
using Skad.Common.Http;
using Skad.Subscription.Domain;

namespace Skad.Subscription.MvcControllers.ViewModels
{
    public class SubscriptionModel
    {
        public string? SubscriptionTier { get; set; }
        public string? NameOnCard { get; set; }
        public string? CardNumber { get; set; }

        public string CurrentSubscriptionTier { get; set; } = "no current subscription";
        public string CurrentAmountPaid { get; set; } = "N/A";
        public string CurrentExpires { get; set; } = "N/A";

        public bool HasReceipt => ReceiptLink != null;

        public string? ReceiptLink { get; set; }
        
        public string? VulnFeedLink { get; set;  }
        
        public string? LogoutLink { get; set; }
        
        public string? Username { get; set; }
        
        public bool Validate(SubscriptionTiers tiers, ModelStateDictionary modelStateDictionary)
        {
            if (SubscriptionTier == null)
            {
                modelStateDictionary.AddModelError("SubscriptionTier", "Subscription needs to be selected.");
            } 
            else if (!tiers.IsValidTierName(SubscriptionTier))
            {
                modelStateDictionary.AddModelError("SubscriptionTier",
                    $"Subscription is not valid: {SubscriptionTier}");
            }
            else if (!SubscriptionTier.Equals("free"))
            {
                if (string.IsNullOrEmpty(NameOnCard))
                {
                    modelStateDictionary.AddModelError("NameOnCard", "Name is required.");
                }

                if (string.IsNullOrEmpty(CardNumber))
                {
                    modelStateDictionary.AddModelError("CardNumber", "Card number is required.");
                }
            }

            return modelStateDictionary.IsValid;
        }
    }
}