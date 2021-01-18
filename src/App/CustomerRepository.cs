using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace App
{
    public class CustomerRepository
    {
        private static readonly List<Customer> _customers = new List<Customer>();
        private static long _lastId;

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

            // Saving to the database
            _customers.RemoveAll(x => x.Id == customer.Id);
            _customers.Add(customer);
        }

        private static void SetId(Entity entity, long id)
        {
            // The use of reflection to set up the Id emulates the ORM behavior
            entity.GetType().GetProperty(nameof(Entity.Id)).SetValue(entity, id);
        }
    }
}
