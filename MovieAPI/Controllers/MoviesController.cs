using System.Net.Http;
using System.Web.Http;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.CustomAtrributes;

namespace MovieAPI.Controllers
{

    public class MoviesController : ApiController
    {
        IMovie movie;
        HttpRequestMessage req;
        public MoviesController(IMovie movieparam)
        {
            movie = movieparam;
        }

        [ValidateAuthorizeKey]
        [Route("api/v1/addMovies")]
        [HttpPost]
        public HttpResponseMessage addMovies(AddMovieRequestJSON ureq)
        {
            req = Request;
            if (ureq == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }

            string res = movie.addMovie(ureq);

            if (res.Contains("ERR"))
            {
                return Utils.CreateErrorResponse(req, res);
            }
            
            return Utils.CreateSuccessResponse(req,res);
        }

        
        [Route("api/v1/searchMovieCatalog")]
        [HttpPost]
        public HttpResponseMessage searchMovieCatalog(SearchMovieCatalogRequestJSON ureq)
        {
            req = Request;
            if (ureq == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }

            if (string.IsNullOrWhiteSpace(ureq.pageNumber.ToString()))
            {
                return Utils.CreateErrorResponse(req, "pageNumber is required");
            }

            if (string.IsNullOrWhiteSpace(ureq.pageSize.ToString()))
            {
                return Utils.CreateErrorResponse(req, "pageSize is required");
            }

            return Utils.CreateSuccessResponse(req, movie.searchMovieCatalog(ureq));
        }

        [Route("api/v1/getMovieDetails")]
        [HttpGet]
        public HttpResponseMessage getMovieDetails(int? movieId)
        {
            req = Request;
            if (movieId == null)
            {
                return Utils.CreateErrorResponse(req,"MovieId is required");
            }

            return Utils.CreateSuccessResponse(req, movie.getMovieDetails(movieId));
        }
        [ValidateAuthorizeKey]
        [Route("api/v1/deleteMovie")]
        [HttpDelete]
        public HttpResponseMessage deleteMovie(DeleteMovieRequestJSON des)
        {
            req = Request;
            if (des == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }
            if (string.IsNullOrWhiteSpace(des.movieId.ToString()))
            {
                return Utils.CreateErrorResponse(req, "MovieId is required");
            }
            if (string.IsNullOrWhiteSpace(des.userId.ToString()))
            {
                return Utils.CreateErrorResponse(req, "userId is required");
            }
            movie.deleteMovie(des.movieId);
            return Utils.CreateSuccessResponse(req,"Movie Deleted successfully");
        }
    }
}
