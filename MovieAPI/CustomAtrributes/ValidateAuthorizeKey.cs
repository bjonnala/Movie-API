using System.Web.Http.Controllers;
using Newtonsoft.Json;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using MovieAPI.IBLL;
using MovieAPI.BLL;
using System;

namespace MovieAPI.CustomAtrributes
{
    public class ValidateAuthorizeKey : AuthorizeAttribute
    {
        string va_errors = string.Empty;

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (string.IsNullOrWhiteSpace(Authorize(filterContext)))
            {
                return;
            }

            HandleUnauthorizedRequest(filterContext);
        }

        //Processes requests that fail authorization.
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {

            filterContext.Response = Utils.CreateErrorResponse(filterContext.Request, va_errors);
            return;

        }

        private string Authorize(HttpActionContext actionContext)
        {

            va_errors = string.Empty;
            string requrl = actionContext.Request.RequestUri.AbsoluteUri;
            Task<string> content = actionContext.Request.Content.ReadAsStringAsync();
            HttpRequestHeaders reqheaders = actionContext.Request.Headers;
            string resauthorizeKey = "";

            IUserSessions _usersession = new UserSessions();
            // to get Authorization key from Headers
            IEnumerable<string> headerlst = new List<string>();
            if (reqheaders.TryGetValues("Authorization", out headerlst))
            {
                foreach (string value in headerlst)
                {
                    resauthorizeKey = value;
                }
            }

            string userId = string.Empty;
            // If userId is passed as a querystring via URL
            if (requrl.Contains("userId"))
            {
                string[] reqparams = requrl.Split('=');
                userId = reqparams[1];
            }
            else
            {
                // If userId is passed via request body
                string body = content.Result;  // gets request body
                var results = JsonConvert.DeserializeObject<dynamic>(body); // dynamic means the JSON object can be resolved at run time.
                if (results.userId != null)
                {
                    userId = results.userId.ToString();
                }
                else
                {
                    va_errors = "Missing userId";
                    return va_errors;
                }
                
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                    int res = 0;
                    if (Int32.TryParse(userId, out res))
                    {
                        if (!_usersession.isValidUserId(res))
                        {
                            va_errors = "Invalid UserId.";
                            return va_errors;
                        }
                    }
                    else
                    {
                        va_errors = "Invalid UserId.";
                        return va_errors;
                    }
                    if (!string.IsNullOrWhiteSpace(resauthorizeKey))
                    {
                        if (userId != null)
                        {
                            string DBtoken = _usersession.getAccessToken(res);
                            if (DBtoken == resauthorizeKey) // checks if authorizekey exists in DB and makes sure its equal to passed in header  
                            {
                                if (!Utils.VerifyLogintoken(resauthorizeKey)) // chk validity of token
                                {
                                    va_errors = "Not authorized [Expired Authorization key]";
                                    return va_errors;
                                }
                            }
                            else
                            {
                                va_errors = "Invalid Authorization key";
                                return va_errors;
                            }

                        }
                    }
                    else
                    {
                        va_errors = "Not authorized [Invalid Authorization key]";
                        return va_errors;
                    }
            }
            else
            {
                va_errors = "Missing userId";
            }
            return va_errors;


        }
    }
}