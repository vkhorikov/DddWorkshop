using System;

namespace Domain
{
    public class PurchasedMovie : Entity
    {
        public long MovieId { get; set; }
        public long CustomerId { get; set; }
        public Dollars Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ExpirationDate ExpirationDate { get; set; }
    }
}
