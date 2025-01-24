using Data.entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.repository
{
    public class SubscriptionRepository
    {
        private readonly MonedasContext _context;
        public SubscriptionRepository(MonedasContext context)
        {
            _context = context;
        }

        public Subscription GetSubscriptionByName(string name)
        {
            return _context.Subscriptions.FirstOrDefault(s => s.Name == name);
        }


        public User GetUserById(int userId)// obtener un usuario específico por su ID, incluyendo su suscripción
        {
            return _context.Users
                .Include(u => u.Subscription)  
                .FirstOrDefault(u => u.Id == userId);
        }

        public User AssignSubscriptionToUser(int userId, int subscriptionId)
        {
            var user = _context.Users.Include(u => u.Subscription).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            user.SubscriptionId = subscriptionId;
            user.canMakeConversions = true;
            _context.SaveChanges();

            return user;
        }
        public int GetRemainingConversions(int userId)
        {
            var user = _context.Users.Include(u => u.Subscription).FirstOrDefault(u => u.Id == userId);
            if (user == null || user.Subscription == null)
                return 0;

            return user.Subscription.MaxConversions - user.ConversionsUsed;
        }






    }
}
