using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Domain
{
    public class Customer : Entity
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CustomerStatus Status { get; set; }

        public DateTime? StatusExpirationDate { get; set; }

        public decimal MoneySpent { get; set; }

        public IList<PurchasedMovie> PurchasedMovies { get; set; }
    }
}
