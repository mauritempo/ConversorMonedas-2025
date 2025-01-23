using Data.entidades;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{

    public class MonedasContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<CUrrency> currencyConversions { get; set; }


        public MonedasContext(DbContextOptions<MonedasContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la relación entre User y Subscription
            modelBuilder.Entity<User>()
                .HasOne(u => u.Subscription)
                .WithMany()
                .HasForeignKey(u => u.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription { Id = 1, Name = "Free", MaxConversions = 5 },
                new Subscription { Id = 2, Name = "Trial", MaxConversions = 100 },
                new Subscription { Id = 3, Name = "Pro", MaxConversions = int.MaxValue }
            );

        }

    }
}