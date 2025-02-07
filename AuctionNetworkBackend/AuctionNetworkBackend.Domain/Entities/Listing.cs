using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Listing
    {
        public long Id { get; set; } // Id ogłoszenia
        public string Title { get; set; } // Tytuł ogłoszenia
        public string Description { get; set; } // Opis przedmiotu
        public decimal Price { get; set; } // Cena
        public decimal? BuyNowPrice { get; set; } // Cena "Kup Teraz"
        public DateTime StartDate { get; set; } // Data rozpoczęcia
        public DateTime? EndDate { get; set; } // Data zakończenia
        public ListingStatus Status { get; set; } // Status ogłoszenia
        public long ListingStatusId { get; set; }
        public bool IsAuction { get; set; } // Czy to aukcja

        // Relacje
        public long SellerId { get; set; }
        public User Seller { get; set; }
        public long? BuyerId { get; set; }
        public User? Buyer { get; set; }
        public long? WinnerId { get; set; } 
        public User? Winner { get; set; } 
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public long PhotoId { get; set; }
        public Photo Photo { get; set; }
        public List<Bid>? Bids { get; set; } // Oferty w przypadku aukcji (może być puste w przypadku sprzedaży)
        public List<ListingReview>? ListingReviews { get; set; } // Lista recenzji ogłoszenia
    }
}
