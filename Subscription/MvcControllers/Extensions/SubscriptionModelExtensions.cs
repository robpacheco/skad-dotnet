using Skad.Subscription.MvcControllers.ViewModels;

namespace Skad.Subscription.MvcControllers.Extensions
{
    public static class SubscriptionModelExtensions
    {
        public static SubscriptionModel ToSubscriptionViewModel(this Data.Model.Subscription? subscription)
        {
            if (subscription == null)
            {
                return new SubscriptionModel
                {
                    SubscriptionTier = "startup"
                };
            }
            
            return new SubscriptionModel
            {
                SubscriptionTier = subscription.Tier,
                NameOnCard = subscription.CardName,
                CardNumber = $"****-****-****-{subscription.CardLast4}"
            };
        }

        public static Data.Model.Subscription ToSubscription(this SubscriptionModel model)
        {
            return new Data.Model.Subscription
            {
                Tier = model.SubscriptionTier,
                CardName = model.NameOnCard,
                CardLast4 = model.CardNumber.Substring(13)
            };
        }
    }
}