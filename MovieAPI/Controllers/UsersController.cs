using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.Validators;
using MovieAPI.CustomAtrributes;
using FluentValidation.Results;

namespace MovieAPI.Controllers
{
    [ValidateAuthorizeKey]
    public class UsersController : ApiController
    {
        IUserSessions usersession;
        IUsers users;
        HttpRequestMessage req;
        usersInsertValidator ivalidator;

        public UsersController(IUsers usersparam, IUserSessions usersessionparam)
        {
            users = usersparam;
            usersession = usersessionparam;
            ivalidator = new usersInsertValidator();
        }

        
        [Route("api/v1/getUserDetails")]
        [HttpGet]
        public HttpResponseMessage getUserDetails(int? userId)
        {
            req = Request;
            if (userId == null)
            {
                return Utils.CreateErrorResponse(req, "UserId is required");
            }
            if (string.IsNullOrWhiteSpace(userId.ToString()))
            {
                return Utils.CreateErrorResponse(req, "UserId is required");
            }
            return Utils.CreateSuccessResponse(req, users.getUserDetails(userId.Value));
        }

        [Route("api/v1/updateUser")]
        [HttpPut]
        public HttpResponseMessage updateUserDetails(UpdateUserRequestJSON ureq)
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
            if (users.checkDuplicateEmail(ureq.email))
            {
                return Utils.CreateErrorResponse(req, "email already exists. Please update to a different one.");
            }
            return Utils.CreateSuccessResponse(req, users.updateUserDetails(ureq));
        }


    }
}
