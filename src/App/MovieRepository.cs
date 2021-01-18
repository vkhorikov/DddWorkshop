using System.Linq;
using Domain;

namespace App
{
    public class MovieRepository
    {
        private static readonly Movie[] _allMovies =
        {
            new Movie("Great Gatsby", LicensingModel.TwoDays)
            {
                Id = 1
            },
            new Movie("Secret Life of Pets", LicensingModel.LifeLong)
            {
                Id = 2
            }
        };

        public Movie GetById(long movieId)
        {
            return _allMovies.SingleOrDefault(x => x.Id == movieId);
        }
    }
}
