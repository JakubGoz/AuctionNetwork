using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Payment
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string StripePaymentIntentId { get; set; } // Nowe pole do przechowywania PaymentIntent z Stripe

        public Order Order { get; set; }
    }
}
