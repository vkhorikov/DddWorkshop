using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Domain
{
    public class CustomerStatus : ValueObject
    {
        public static readonly CustomerStatus Regular = new CustomerStatus(CustomerStatusType.Regular, ExpirationDate.Infinite);

        public CustomerStatusType Type { get; }
        public ExpirationDate ExpirationDate { get; }

        private CustomerStatus()
        {
        }

        private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate)
            : this()
        {
            Type = type;
            ExpirationDate = expirationDate;
        }

        public bool IsAdvanced(DateTime now)
        {
            if (Type != CustomerStatusType.Advanced)
                return false;

            return ExpirationDate.IsExpired(now) == false;
        }

        public decimal GetDiscount(DateTime now)
        {
            return IsAdvanced(now) ? 0.25m : 0m;
        }

        public CustomerStatus Promote()
        {
            return new CustomerStatus(CustomerStatusType.Advanced, ExpirationDate.Create(DateTime.UtcNow.AddYears(1)).Value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return ExpirationDate;
        }
    }

    public enum CustomerStatusType
    {
        Regular = 1,
        Advanced = 2
    }
}
