using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

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

        public bool HasPurchasedMovie(Movie movie, DateTime now)
        {
            return PurchasedMovies.Any(x => x.MovieId == movie.Id && x.ExpirationDate.IsExpired(now) == false);
        }

        public void PurchaseMovie(Movie movie, DateTime now)
        {
            if (HasPurchasedMovie(movie, now))
                throw new Exception();

            ExpirationDate expirationDate = movie.GetExpirationDate();
            Dollars price = movie.CalculatePrice(Status, DateTime.UtcNow);

            var purchasedMovie = new PurchasedMovie(movie.Id, Id, price, now, expirationDate);
            _purchasedMovies.Add(purchasedMovie);
            
            MoneySpent += price;
        }

        public Result CanPromote(DateTime now)
        {
            if (Status.IsAdvanced(DateTime.UtcNow))
                return Result.Failure("The customer already has the Advanced status");

            if (PurchasedMovies.Count(
                x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= now.AddDays(-30)) < 2)
                return Result.Failure("The customer has to have at least 2 active movies during the last 30 days");

            if (PurchasedMovies.Where(x => x.PurchaseDate > now.AddYears(-1)).Sum(x => x.Price.Value) < 100m)
                return Result.Failure("The customer has to have at least 100 dollars spent during the last year");

            return Result.Success();
        }

        public void Promote(DateTime now)
        {
            if (CanPromote(now).IsFailure)
                throw new Exception();

            Status = Status.Promote();
        }
    }
}
