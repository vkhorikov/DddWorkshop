using System;
using Newtonsoft.Json;

namespace Domain
{
    public class PurchasedMovie : Entity
    {
        public long MovieId { get; set; }

        [JsonIgnore]
        public long CustomerId { get; set; }

        public decimal Price { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
