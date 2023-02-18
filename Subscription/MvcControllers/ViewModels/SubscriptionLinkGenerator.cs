using System;
using Skad.Common.Http;
using Skad.Subscription.Domain.Service;

namespace Skad.Subscription.MvcControllers.ViewModels;

public class SubscriptionLinkGenerator
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly LinkGenerator _linkGenerator;

    public SubscriptionLinkGenerator(ISubscriptionService subscriptionService, LinkGenerator linkGenerator)
    {
        _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

    public string? GenerateSubscriptionReceiptLink()
    {
        if (_subscriptionService.HasReceipt())
        {
            return _linkGenerator.GenerateSubscriptionUri("subscription", "receipt").ToString();
        }
        
        return null;
    }

    public string GenerateVulnFeedLink()
    {
        return _linkGenerator.GenerateVulnerabilityFeedUri("vulnerability-feed").ToString();
    }

    public string GenerateSubscriptionLink()
    {
        return _linkGenerator.GenerateSubscriptionUri("subscription").ToString();
    }
}