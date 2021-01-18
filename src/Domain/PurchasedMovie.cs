using System;

namespace Domain
{
    public class PurchasedMovie : Entity
    {
        public long MovieId { get; }
        public long CustomerId { get; }
        public Dollars Price { get; }
        public DateTime PurchaseDate { get; }
        public ExpirationDate ExpirationDate { get; }

        public PurchasedMovie(long movieId, long customerId, Dollars price, DateTime now, ExpirationDate expirationDate)
        {
            if (price == null || price.IsZero)
                throw new ArgumentException(nameof(price));
            if (expirationDate == null || expirationDate.IsExpired(now))
                throw new ArgumentException(nameof(expirationDate));

            MovieId = movieId;
            CustomerId = customerId;
            Price = price;
            PurchaseDate = now;
            ExpirationDate = expirationDate;
        }
    }
}
