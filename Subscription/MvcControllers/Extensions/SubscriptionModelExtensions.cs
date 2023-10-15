using System.Globalization;
using Microsoft.AspNetCore.Http;
using Skad.Common.Http;
using Skad.Subscription.MvcControllers.ViewModels;

namespace Skad.Subscription.MvcControllers.Extensions
{
    public static class SubscriptionModelExtensions
    {
        public static SubscriptionModel ToSubscriptionViewModel(this Data.Model.Subscription? subscription, SubscriptionLinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            if (subscription == null)
            {
                return new SubscriptionModel()
                {
                    SubscriptionTier = "startup",
                    VulnFeedLink = linkGenerator.GenerateVulnFeedLink(),
                    LogoutLink = linkGenerator.GenerateLogoutLink(),
                    Username = httpContextAccessor.HttpContext.User.Identity.Name
                };
            }
            
            var m = new SubscriptionModel()
            {
                SubscriptionTier = subscription.Tier,
                NameOnCard = subscription.CardName,
                CardNumber = $"****-****-****-{subscription.CardLast4}",
                CurrentSubscriptionTier = subscription.Tier,
                CurrentAmountPaid = $"${subscription.AmountPaid.ToString(CultureInfo.CurrentCulture)}",
                CurrentExpires = subscription.DateExpires.ToShortDateString(),
                ReceiptLink = linkGenerator.GenerateSubscriptionReceiptLink(),
                VulnFeedLink = linkGenerator.GenerateVulnFeedLink(),
                LogoutLink = linkGenerator.GenerateLogoutLink(),
                Username = httpContextAccessor.HttpContext.User.Identity.Name
            };

            return m;
        }

        public static Data.Model.Subscription ToSubscription(this SubscriptionModel model)
        {
            return new Data.Model.Subscription
            {
                Tier = model.SubscriptionTier,
                CardName = model.NameOnCard,
                CardLast4 = model.CardNumber?.Substring(13)
            };
        }
    }
}