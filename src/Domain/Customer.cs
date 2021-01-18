using System;
using System.Collections.Generic;

namespace Domain
{
    public class Customer : Entity
    {
        public CustomerName Name { get; set; }
        public Email Email { get; set; }
        public CustomerStatus Status { get; set; }
        public DateTime? StatusExpirationDate { get; set; }
        public decimal MoneySpent { get; set; }
        public IList<PurchasedMovie> PurchasedMovies { get; set; }
    }
}
