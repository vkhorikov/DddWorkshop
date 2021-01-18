using System;

namespace Domain
{
    public class MovieService
    {
        public ExpirationDate GetExpirationDate(LicensingModel licensingModel)
        {
            ExpirationDate result = licensingModel switch
            {
                LicensingModel.TwoDays => ExpirationDate.Create(DateTime.UtcNow.AddDays(2)).Value,
                LicensingModel.LifeLong => ExpirationDate.Infinite,
                _ => throw new ArgumentOutOfRangeException()
            };

            return result;
        }
    }
}
