using System.Linq;
using Domain;

namespace App
{
    public class MovieRepository
    {
        private static readonly Movie[] _allMovies =
        {
            new Movie
            {
                Id = 1,
                Name = "Great Gatsby",
                LicensingModel = LicensingModel.TwoDays
            },
            new Movie
            {
                Id = 2,
                Name = "Secret Life of Pets",
                LicensingModel = LicensingModel.LifeLong
            }
        };

        public Movie GetById(long movieId)
        {
            return _allMovies.SingleOrDefault(x => x.Id == movieId);
        }
    }
}
