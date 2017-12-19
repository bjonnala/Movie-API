using System.Web.Http.Controllers;
using Newtonsoft.Json;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using MovieAPI.IBLL;
using System;

namespace MovieAPI.CustomAtrributes
{
    public class ValidateAuthorizeKey : AuthorizeAttribute
    {
        string va_errors = string.Empty;

        private IUserSessions _usersession;

        public ValidateAuthorizeKey(IUserSessions usersessionparam)
        {
            _usersession = usersessionparam;
        }

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
            Task<string> content = actionContext.Request.Content.ReadAsStringAsync();
            HttpRequestHeaders reqheaders = actionContext.Request.Headers;
            string resauthorizeKey = "";


            // to get Authorization key from Headers
            IEnumerable<string> headerlst = new List<string>();
            if (reqheaders.TryGetValues("Authorization", out headerlst))
            {
                foreach (string value in headerlst)
                {
                    resauthorizeKey = value;
                }
            }

            string body = content.Result;  // gets request body
            var results = JsonConvert.DeserializeObject<dynamic>(body); // dynamic means the JSON object can be resolved at run time.
            if (results != null)
            {
                if (!string.IsNullOrWhiteSpace(resauthorizeKey))
                {
                    if (results.userId != null)
                    {
                        string DBtoken = _usersession.getAccessToken(Convert.ToInt32(results.userId.ToString()));
                        if (DBtoken == resauthorizeKey) // checks if authorizekey exists in DB and makes sure its equal to passed in header  
                        {
                            if (!Utils.VerifyLogintoken(resauthorizeKey)) // chk validity of token
                            {
                                va_errors = "Not authorized [Expired Authorization key]";
                            }
                        }
                    }
                }
                else
                {
                    va_errors = "Not authorized [Invalid Authorization key]";
                }
            }
            else
            {
                va_errors = "Missing Userid";
            }

            return va_errors;


        }
    }
}