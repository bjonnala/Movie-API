using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.Validators;
using FluentValidation.Results;

namespace MovieAPI.Controllers
{
    public class UsersController : ApiController
    {

        IUsers users;
        IUserSessions usersessions;
        HttpRequestMessage req;
        usersInsertValidator ivalidator;

        public UsersController(IUsers usersparam, IUserSessions usersessionsparam)
        {
            users = usersparam;
            usersessions = usersessionsparam;
        }

        public UsersController()
        {
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
            return Utils.CreateSuccessResponse(req, users.getUserDetails(1));
        }
        [Route("api/v1/Register")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterUserRequestJSON ureq)
        {
            req = Request;
            if (ureq == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }


            List<string> allowedlogins = new List<string>();
            allowedlogins.Add("facebook");
            allowedlogins.Add("twitter");
            allowedlogins.Add("instagram");
            allowedlogins.Add("googleplus");
            allowedlogins.Add("linkedin");


            ValidationResult results_register = ivalidator.Validate(ureq);

            if (!results_register.IsValid)
            {
                return Utils.CreateErrorResponse(req, results_register.Errors[0].ErrorMessage);
            }

            if (allowedlogins.Contains(ureq.network.ToLower().Trim()))
            {
                if (string.IsNullOrWhiteSpace(ureq.socialMediaAccessToken))
                {
                    return Utils.CreateErrorResponse(req, "Social Media Access Token is required for " + ureq.network);
                }
                if (string.IsNullOrWhiteSpace(ureq.socialMediaUserId))
                {
                    return Utils.CreateErrorResponse(req, "Social Media User Id is required for " + ureq.network);
                }
            }

            if (ureq.network.ToLower() == "local")
            {
                if (string.IsNullOrWhiteSpace(ureq.password))
                {
                    return Utils.CreateErrorResponse(req, "password cannot be blank");
                }
               
            }
            if (users.checkDuplicateEmail(ureq.email))
            {
                return Utils.CreateErrorResponse(req, "email already exists");
            }

            // Calculate salt
            // Calculate hash of the password based on salt

            if (!string.IsNullOrWhiteSpace(ureq.password) && ureq.network == "local")
            {
                string salt = Utils.ComputeSalt();
                string hashedpassword = Utils.ComputeHash(salt, ureq.password);

            }
            //else
            //{

            //    // This happens only when network is either facebook,twitter,linkedin,instagram,googleplus (social media) 
            //    // random password and salt are set just for security purposes in case the user logins in with email. **********/
            //    string salt = Utils.ComputeSalt();
            //    string hashedpassword = Utils.ComputeHash(salt, "q*@/E&pJcz");
            //    ureq.hashedpassword = hashedpassword;
            //    ureq.salt = salt;
            //}



            
            int userId = users.createUser(ureq);
            if (userId != 0)
            {
                string authorizekey = Utils.GenerateLogintoken(userId.ToString());
                string res = usersessions.updateAccessToken(userId, authorizekey);
                if (!string.IsNullOrWhiteSpace(res))
                {

                    return Utils.CreateErrorResponse(req, "Error registering a user. Please try again");
                }
                else
                {
                    LoginRegisterUserResponse respo = new LoginRegisterUserResponse();
                    respo.id = userId.ToString();
                    respo.authorizeKey = authorizekey;

                    return Utils.CreateSuccessResponse(req, respo);

                }
            }
            else
            {
                return Utils.CreateErrorResponse(req, "Error registering a user. Please try again");
            }

            

        }

        //[Route("api/v1/Login")]
        //[HttpPost]
        //public HttpResponseMessage Login([FromBody] RegisterUserRequestJSON req)
        //{
        //    req = Request;
        //    if (userId == null)
        //    {
        //        return Utils.CreateErrorResponse(req, "UserId is required");
        //    }
        //    if (string.IsNullOrWhiteSpace(userId.ToString()))
        //    {
        //        return Utils.CreateErrorResponse(req, "UserId is required");
        //    }
        //    return Utils.CreateSuccessResponse(req, users.getUserDetails(1));
        //}

    }
}
