using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using MovieAPI.IBLL;
using MovieAPI.Model;
using System.Linq;
using MovieAPI.Validators;
using MovieAPI.CustomAtrributes;

namespace MovieAPI.Controllers
{
    public class PaymentController : ApiController
    {
        IUserSessions usersession;
        IUsers users;
        HttpRequestMessage req;

        public PaymentController(IUsers usersparam, IUserSessions usersessionparam)
        {
            users = usersparam;
            usersession = usersessionparam;
        }
        [Route("api/v1/rentMovies")]
        [HttpPost]
        public HttpResponseMessage rentMovies(RentMoviesRequestJSON ureq)
        {
            req = Request;
            if (ureq == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }
            if (string.IsNullOrWhiteSpace(ureq.userId.ToString()))
            {
                return Utils.CreateErrorResponse(req, "userId is required");
            }
            if (ureq.moviesIds.Count() == 0)
            {
                return Utils.CreateErrorResponse(req, "At least one moveid(s) is required.");
            }

            if (string.IsNullOrWhiteSpace(ureq.RentalDateFrom))
            {
                return Utils.CreateErrorResponse(req, "RentalDateFrom is required");
            }

            if (string.IsNullOrWhiteSpace(ureq.RentalDateTo))
            {
                return Utils.CreateErrorResponse(req, "RentalDateTo is required");
            }

            if (!Utils.checkDate(ureq.RentalDateFrom))
            {
                return Utils.CreateErrorResponse(req, "RentalDateFrom is not a valid datetime.");
            }

            if (!Utils.checkDate(ureq.RentalDateTo))
            {
                return Utils.CreateErrorResponse(req, "RentalDateTo is not a valid datetime.");
            }
            return Utils.CreateSuccessResponse(req, users.rentMovies(ureq));
        }






    }
}
