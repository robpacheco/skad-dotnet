using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Skad.Common.Auth;
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
        private readonly SubscriptionLinkGenerator _linkGenerator;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUserInfoAccessor _userInfoAccessor;

        public SubscriptionController(ISubscriptionService subscriptionService, IOptions<SubscriptionTierSettings> tierSettings, SubscriptionLinkGenerator linkGenerator, IActionContextAccessor actionContextAccessor, IUserInfoAccessor userInfoAccessor)
        {
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _tiers = new SubscriptionTiers(tierSettings.Value ?? throw new ArgumentNullException(nameof(tierSettings)));
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
            _actionContextAccessor = actionContextAccessor;
            _userInfoAccessor = userInfoAccessor ?? throw new ArgumentNullException(nameof(userInfoAccessor));
        }
        
        public async Task<IActionResult> Index()
        {
            var userInfo = await _userInfoAccessor.FetchUserInfo();
            var username = userInfo.Email;
            var subscription = await _subscriptionService.FindLatestActiveSubscription();
            var model = subscription.ToSubscriptionViewModel(_linkGenerator, username);          
            return View("Subscription", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SubscriptionModel model)
        {
            bool modelValid = model.Validate(_tiers, _actionContextAccessor.ActionContext.ModelState);
            
            if (!modelValid)
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
            
            return Redirect(_linkGenerator.GenerateSubscriptionLink());
        }
        
        [HttpGet("receipt")]
        public IActionResult Receipt()
        {
            var receiptContent = _subscriptionService.FindReceipt();

            if (receiptContent == null)
            {
                return BadRequest("no receipt found");
            }
            
            return Content(receiptContent!, "text/html");
        }
    }
}