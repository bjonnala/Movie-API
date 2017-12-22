using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPI.Model;

namespace MovieAPI.IBLL
{
    public interface IMovie
    {
        string addMovie(AddMovieRequestJSON req);
        GetMovieResponseJSON getMovieDetails(int? movieId);
        void deleteMovie(int? movieId);
        SearchMovieCatalogResponseJSON searchMovieCatalog(SearchMovieCatalogRequestJSON req);
        bool isAdmin(int userId);
    }
}
