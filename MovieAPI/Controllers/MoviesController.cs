using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.CustomAtrributes;
using MovieAPI.Validators;
using FluentValidation.Results;

namespace MovieAPI.Controllers
{
    [ValidateAuthorizeKey]
    public class MoviesController : ApiController
    {
        IMovie movie;
        HttpRequestMessage req;
        public MoviesController(IMovie movieparam)
        {
            movie = movieparam;
        }

        [Route("api/v1/addMovies")]
        [HttpPost]
        public HttpResponseMessage addMovies(AddMovieRequestJSON ureq)
        {
            req = Request;
            if (ureq == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }

            return Utils.CreateSuccessResponse(req, movie.addMovie(ureq));
        }

        //[Route("api/v1/getMovieDetails")]
        //[HttpGet]
        //public HttpResponseMessage getMovieDetails(int? movieId)
        //{
        //    req = Request;
        //    if (movieId == null)
        //    {
        //        return Utils.CreateEmptyErrorResponse(req);
        //    }

        //    return Utils.CreateSuccessResponse(req, movie.getMovieDetails(movieId));
        //}
    }
}
