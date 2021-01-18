using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Domain
{
    public class ExpirationDate : ValueObject
    {
        public static readonly ExpirationDate Infinite = new ExpirationDate(null);

        public DateTime? Date { get; }

        private ExpirationDate(DateTime? date)
        {
            Date = date;
        }

        public bool IsExpired(DateTime now)
        {
            if (this == Infinite)
                return false;
            
            return Date < now;
        }

        public static Result<ExpirationDate> Create(DateTime date)
        {
            return new ExpirationDate(date);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }
    }
}
