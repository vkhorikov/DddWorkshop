using System.Collections.Generic;

namespace Domain
{
    public class Customer : Entity
    {
        public CustomerName Name { get; set; }
        public Email Email { get; set; }
        public CustomerStatus Status { get; set; }
        public ExpirationDate StatusExpirationDate { get; set; }
        public Dollars MoneySpent { get; set; }
        public IList<PurchasedMovie> PurchasedMovies { get; set; }
    }
}
