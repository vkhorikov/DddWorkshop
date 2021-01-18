using System;

namespace Domain
{
    public class Movie : Entity
    {
        public string Name { get; }
        public LicensingModel LicensingModel { get; }

        public Movie(string name, LicensingModel licensingModel)
        {
            Name = name;
            LicensingModel = licensingModel;
        }

        public ExpirationDate GetExpirationDate()
        {
            ExpirationDate result = LicensingModel switch
            {
                LicensingModel.TwoDays => ExpirationDate.Create(DateTime.UtcNow.AddDays(2)).Value,
                LicensingModel.LifeLong => ExpirationDate.Infinite,
                _ => throw new ArgumentOutOfRangeException()
            };

            return result;
        }
    }
}
