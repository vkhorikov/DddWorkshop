using System;
using System.Linq;

namespace Domain
{
    public class CustomerService
    {
        public void PurchaseMovie(Customer customer, Movie movie)
        {
            ExpirationDate expirationDate = movie.GetExpirationDate();
            Dollars price = movie.CalculatePrice(customer.Status, DateTime.UtcNow);
            customer.PurchaseMovie(movie, expirationDate, price, DateTime.UtcNow);
        }

        public bool PromoteCustomer(Customer customer)
        {
            // at least 2 active movies during the last 30 days
            if (customer.PurchasedMovies.Count(
                x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
                return false;

            // at least 100 dollars spent during the last year
            if (customer.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price.Value) < 100m)
                return false;

            customer.Status = customer.Status.Promote();

            return true;
        }
    }
}
