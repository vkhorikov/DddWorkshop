using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace App
{
    public class CustomerRepository
    {
        private static readonly List<Customer> _customers = new List<Customer>
        {
            Alice(),
            Bob()
        };
        private static long _lastId = _customers.Max(x => x.Id);

        public IReadOnlyList<Customer> GetList()
        {
            return _customers
                .Select(x => x)
                .ToList();
        }

        public Customer GetByEmail(Email email)
        {
            return _customers.SingleOrDefault(x => x.Email == email);
        }

        public Customer GetById(long id)
        {
            return _customers.SingleOrDefault(x => x.Id == id);
        }

        public void Save(Customer customer)
        {
            // Setting up the id for new students emulates the ORM behavior
            if (customer.Id == 0)
            {
                _lastId++;
                SetId(customer, _lastId);
            }

            if (customer.PurchasedMovies == null)
            {
                customer.PurchasedMovies = new List<PurchasedMovie>();
            }

            // Saving to the database
            _customers.RemoveAll(x => x.Id == customer.Id);
            _customers.Add(customer);
        }

        private static void SetId(Entity entity, long id)
        {
            // The use of reflection to set up the Id emulates the ORM behavior
            entity.GetType().GetProperty(nameof(Entity.Id)).SetValue(entity, id);
        }

        private static Customer Alice()
        {
            var alice = new Customer
            {
                Id = 1,
                Email = Email.Create("alice@gmail.com").Value,
                MoneySpent = 4,
                Name = CustomerName.Create("Alice Alison").Value,
                PurchasedMovies = new List<PurchasedMovie>
                {
                    new PurchasedMovie
                    {
                        CustomerId = 1,
                        ExpirationDate = DateTime.Now.AddDays(2),
                        MovieId = 1,
                        Price = 4,
                        PurchaseDate = DateTime.Now
                    }
                },
                Status = CustomerStatus.Regular,
                StatusExpirationDate = null
            };

            return alice;
        }

        private static Customer Bob()
        {
            var bob = new Customer
            {
                Id = 2,
                Email = Email.Create("bob@gmail.com").Value,
                MoneySpent = 0,
                Name = CustomerName.Create("Bob Bobson").Value,
                PurchasedMovies = new List<PurchasedMovie>(),
                Status = CustomerStatus.Regular,
                StatusExpirationDate = null
            };

            return bob;
        }
    }
}
