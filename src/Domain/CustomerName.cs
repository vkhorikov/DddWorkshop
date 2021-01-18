using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Domain
{
    public class CustomerName : ValueObject
    {
        public string Value { get; }

        private CustomerName(string value)
        {
            Value = value;
        }

        public static Result<CustomerName> Create(string customerName)
        {
            customerName = (customerName ?? string.Empty).Trim();

            if (customerName.Length == 0)
                return Result.Failure<CustomerName>("Customer name should not be empty");

            if (customerName.Length > 100)
                return Result.Failure<CustomerName>("Customer name is too long");

            return new CustomerName(customerName);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
