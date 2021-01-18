using System;

namespace Domain
{
    public class MovieService
    {
        public DateTime? GetExpirationDate(LicensingModel licensingModel)
        {
            DateTime? result = licensingModel switch
            {
                LicensingModel.TwoDays => DateTime.UtcNow.AddDays(2),
                LicensingModel.LifeLong => null,
                _ => throw new ArgumentOutOfRangeException()
            };

            return result;
        }
    }
}
