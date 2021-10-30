using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Skad.Common.Http;
using Skad.Subscription.Config;
using Skad.Subscription.Domain;
using Skad.Subscription.Domain.Service;
using Skad.Subscription.MvcControllers.Extensions;
using Skad.Subscription.MvcControllers.ViewModels;

namespace Skad.Subscription.MvcControllers
{
    [Route("subscription")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly SubscriptionTiers _tiers;
        private readonly IActionContextAccessor _actionContextAccessor;

        public SubscriptionController(ISubscriptionService subscriptionService, IOptions<SubscriptionTierSettings> tierSettings, IActionContextAccessor actionContextAccessor)
        {
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _tiers = new SubscriptionTiers(tierSettings.Value ?? throw new ArgumentNullException(nameof(tierSettings)));
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var subscription = await _subscriptionService.FindLatestActiveSubscription();
            var model = subscription.ToSubscriptionViewModel();          
            return View("Subscription", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SubscriptionModel model)
        {
            if (!model.Validate(_tiers, _actionContextAccessor.ActionContext.ModelState))
            {
                return View("Subscription", model);
            }

            var tier = _tiers.FindTier(model.SubscriptionTier);

            if (tier == null)
            {
                ModelState.AddModelError("SubscriptionTier", $"Subscription tier was not found: {model.SubscriptionTier}");
                return View("Subscription", model);
            }

            await _subscriptionService.AddSubscription(model.ToSubscription(), tier);
            
            return Redirect("http://localhost:5000/vulnerability-feed");
        }
    }
}