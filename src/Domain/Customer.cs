using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Customer : Entity
    {
        public CustomerName Name { get; set; }
        public Email Email { get; }
        public CustomerStatus Status { get; private set; }
        public Dollars MoneySpent { get; private set; }

        private readonly IList<PurchasedMovie> _purchasedMovies = new List<PurchasedMovie>();
        public IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

        public Customer(CustomerName name, Email email)
        {
            Name = name;
            Email = email;
            MoneySpent = Dollars.Of(0);
            Status = CustomerStatus.Regular;
        }

        public void PurchaseMovie(Movie movie, DateTime now)
        {
            ExpirationDate expirationDate = movie.GetExpirationDate();
            Dollars price = movie.CalculatePrice(Status, DateTime.UtcNow);

            var purchasedMovie = new PurchasedMovie(movie.Id, Id, price, now, expirationDate);
            _purchasedMovies.Add(purchasedMovie);
            
            MoneySpent += purchasedMovie.Price;
        }

        public bool Promote(DateTime now)
        {
            // at least 2 active movies during the last 30 days
            if (PurchasedMovies.Count(
                x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= now.AddDays(-30)) < 2)
                return false;

            // at least 100 dollars spent during the last year
            if (PurchasedMovies.Where(x => x.PurchaseDate > now.AddYears(-1)).Sum(x => x.Price.Value) < 100m)
                return false;

            Status = Status.Promote();

            return true;
        }
    }
}
