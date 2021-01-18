using Newtonsoft.Json;

namespace Domain
{
    public class Movie : Entity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public LicensingModel LicensingModel { get; set; }
    }
}
