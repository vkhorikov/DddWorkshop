using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.DataContracts
{
    public class GetCustomerResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime? StatusExpirationDate { get; set; }
        public decimal MoneySpent { get; set; }
        public List<PurchasedMovieDto> PurchasedMovies { get; set; }
    }

    public class PurchasedMovieDto
    {
        public MovieDto Movie { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class MovieDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UpdateCustomerRequest
    {
        public string Name { get; set; }
    }
}
