using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skad.Subscription.Data.Model;
using Skad.Subscription.Domain.Repository;

namespace Skad.Subscription.Data
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SubscriptionDbContext _context;
        private readonly ILogger<SubscriptionDbContext> _logger;

        public SubscriptionRepository(SubscriptionDbContext context, ILogger<SubscriptionDbContext> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Model.Subscription?> FindLatestActiveSubscription()
        {
            var subscriptions = await _context.Subscriptions
                .Where(s => s.Active)
                .OrderByDescending(s => s.DatePurchased)
                .ToListAsync();

            if (!subscriptions.Any())
            {
                // TODO: Log
                return null;
            }

            if (subscriptions.Count > 1)
            {
                // TODO: Log
            }

            return subscriptions.First();
        }

        public async Task<Data.Model.Subscription> AddSubscription(Data.Model.Subscription subscription)
        {
            _context.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task InactivateCurrentSubscriptions()
        {
            var expireTime = DateTime.Now;
            var currentSubscriptions = await _context.Subscriptions
                .Where(s => s.Active)
                .ToListAsync();

            foreach (var sub in currentSubscriptions)
            {
                sub.DateExpires = expireTime;
                sub.Active = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}