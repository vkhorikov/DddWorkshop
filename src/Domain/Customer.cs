using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Customer : Entity
    {
        public CustomerName Name { get; set; }
        public Email Email { get; set; }
        public CustomerStatus Status { get; set; }
        public ExpirationDate StatusExpirationDate { get; set; }
        public Dollars MoneySpent { get; set; }

        private readonly IList<PurchasedMovie> _purchasedMovies = new List<PurchasedMovie>();
        public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

        public Customer(CustomerName name, Email email)
        {
            Name = name;
            Email = email;
            MoneySpent = Dollars.Of(0);
            Status = CustomerStatus.Regular;
            StatusExpirationDate = ExpirationDate.Infinite;
        }

        public void PurchaseMovie(PurchasedMovie purchasedMovie)
        {
            _purchasedMovies.Add(purchasedMovie);
            MoneySpent += purchasedMovie.Price;
        }
    }
}
