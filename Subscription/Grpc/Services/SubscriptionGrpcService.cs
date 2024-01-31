using System;
using System.Threading.Tasks;
using Grpc.Core;
using Skad.Subscription.Domain.Service;

namespace Skad.Subscription.Grpc.Services;

public class SubscriptionGrpcService : Subscription.SubscriptionBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionGrpcService(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
    }

    public override async Task<SubscriptionReply> GetActiveSubscription(SubscriptionRequest request, ServerCallContext context)
    {
        var subscription = await _subscriptionService.FindLatestActiveSubscription();

        return new SubscriptionReply
        {
            Tier = subscription?.Tier ?? "No Subscription"
        };
    }
}