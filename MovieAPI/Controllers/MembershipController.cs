using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.Validators;
using FluentValidation.Results;

namespace MovieAPI.Controllers
{
    public class MembershipController : ApiController
    {

        IUsers users;
        IUserSessions usersessions;
        HttpRequestMessage req;
        usersInsertValidator ivalidator;
        usersLoginValidator lvalidator;

        public MembershipController(IUsers usersparam, IUserSessions usersessionsparam)
        {
            users = usersparam;
            usersessions = usersessionsparam;
            ivalidator = new usersInsertValidator();
            lvalidator = new usersLoginValidator();
        }

        [Route("api/v1/Register")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterRequestJSON ureq)
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
                ureq.salt = salt;
                ureq.hashedpassword = hashedpassword;

            }

           


            int userId = users.createUser(ureq);
            if (userId!= 0)
            {
                RegisterResponseJSON respo = new RegisterResponseJSON();
                respo.userId = userId.ToString();
                return Utils.CreateSuccessResponse(req, respo);
            }
            else
            {
                return Utils.CreateErrorResponse(req, "Error registering a user. Please try again");
            }



        }

        [Route("api/v1/Login")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody] LoginRequestJSON lq)
        {
            req = Request;
            if (lq == null)
            {
                return Utils.CreateEmptyErrorResponse(req);
            }

            string hashpassword = string.Empty;
            string errors = "";
            ValidationResult loginres = lvalidator.Validate(lq);

            if (!loginres.IsValid)
            {
                errors = loginres.Errors[0].ErrorMessage; // shows the first error.
                return Utils.CreateErrorResponse(req, errors.ToString());
            }

            int userId = users.signIn(lq);

            if (userId != 0)
            {
                // generates an JWT access token valid for 24hrs.
                string authorizekey = Utils.GenerateLogintoken(userId.ToString());
                usersessions.updateAccessToken(userId, authorizekey);

                LoginResponseJSON respo = new LoginResponseJSON();
                respo.userId = userId.ToString();
                respo.authorizeKey = authorizekey;
                return Utils.CreateSuccessResponse(req, respo);
            }
            else
            {
                return Utils.CreateErrorResponse(req, "Invalid login credentials.");
            }

            
        }
    }
}
