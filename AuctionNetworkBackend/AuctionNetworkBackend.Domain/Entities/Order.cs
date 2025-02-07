using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Order
    {
        public long Id { get; set; } 
        public long UserId { get; set; } 
        public long ListingId { get; set; } 
        public string ProductName { get; set; } 
        public decimal Price { get; set; } 
        public string Status { get; set; } = "Pending"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime? CompletedAt { get; set; } 


        public Listing Listing { get; set; }
        public Payment Payment { get; set; } 
        public long? PaymentId { get; set; } 

        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Country { get; set; } 
        public string City { get; set; } 
        public string Street { get; set; } 
        public string PostalCode { get; set; } 
        public string PhoneNumber { get; set; }
    }
}
