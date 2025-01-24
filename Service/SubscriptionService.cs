using Data.entidades;
using Data.repository;
using DTO.SUBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly SubscriptionRepository _subscriptionRepository;
        private readonly CurrencyRepository _currencyRepository;


        public SubscriptionService(SubscriptionRepository subscriptionRepository, CurrencyRepository currencyRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _currencyRepository = currencyRepository;
        }

        // Asignar una suscripción a un usuario
        public UserSubStatusDTO AssignSubscriptionToUser(UserSubscriptionDTO userSubscriptionDto)
        {
            var updatedUser = _subscriptionRepository.AssignSubscriptionToUser(userSubscriptionDto.UserId, userSubscriptionDto.SubscriptionId);

            if (updatedUser == null)
            {
                return null; // Retornar null si no se encontró el usuario o la suscripción
            }

            // Obtener las conversiones usadas para calcular las restantes
            int conversionsUsed = _subscriptionRepository.GetRemainingConversions(updatedUser.Id);

            return new UserSubStatusDTO
            {
                UserId = updatedUser.Id,
                IsActive = updatedUser.canMakeConversions,
                SubscriptionName = updatedUser.Subscription.Name,
                MaxConversions = updatedUser.Subscription.MaxConversions,
                ConversionsUsed = conversionsUsed,
                ConversionsRemaining = updatedUser.Subscription.MaxConversions == int.MaxValue
                                       ? int.MaxValue
                                       : updatedUser.Subscription.MaxConversions - conversionsUsed
            };
        }
       

        
    }   }
