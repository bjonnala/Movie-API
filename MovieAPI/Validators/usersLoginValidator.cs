using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Results;
using MovieAPI.Model;

namespace MovieAPI.Validators
{
    public class usersLoginValidator : AbstractValidator<LoginRequestJSON>
    {
        public usersLoginValidator()
        {
            List<string> allowednetworks = new List<string>();
            allowednetworks.Add("facebook");
            allowednetworks.Add("twitter");
            allowednetworks.Add("instagram");
            allowednetworks.Add("googleplus");
            allowednetworks.Add("linkedin");
            allowednetworks.Add("local");

            RuleFor(x => x.email).NotEmpty().WithMessage("Email Address cannot be blank")
             .EmailAddress().WithMessage("Please enter a valid email address");

            RuleFor(x => x.network).NotEmpty().WithMessage("network cannot be blank");

            

            Custom(rm =>
            {
                if (rm.network != null)
                {
                    if (!allowednetworks.Contains(rm.network.ToLower().Trim()))
                    {
                        return new ValidationFailure("network", "network not supported");
                    }

                    if (rm.network == "local")
                    {
                        if (string.IsNullOrWhiteSpace(rm.email.Trim()))
                        {
                            return new ValidationFailure("email", "email cannot be blank");
                        }

                        if (string.IsNullOrWhiteSpace(rm.password.Trim()))
                        {
                            return new ValidationFailure("password", "password cannot be blank");
                        }
                    }

                    bool rtv = true;
                    switch (rm.network.ToLower().Trim())
                    {
                        case "facebook":
                            rtv = true;
                            break;
                        case "twitter":
                            rtv = true;
                            break;
                        case "linkedin":
                            rtv = true;
                            break;
                        case "instagram":
                            rtv = true;
                            break;
                        case "googleplus":
                            rtv = true;
                            break;
                        default:
                            rtv = false;
                            break;
                    }

                    if (rtv)
                    {

                        if (rm.socialMediaAccessToken == null)
                        {
                            return new ValidationFailure("Social Media Access Token", "Social Media Access Token is required for " + rm.network);
                        }
                        if (rm.socialMediaUserId == null)
                        {
                            return new ValidationFailure("Social Media User Id", "Social Media User Id is required for " + rm.network);
                        }
                        if (string.IsNullOrWhiteSpace(rm.socialMediaAccessToken))
                        {
                            return new ValidationFailure("Social Media Access Token", "Social Media Access Token is required for " + rm.network);
                        }
                        if (string.IsNullOrWhiteSpace(rm.socialMediaUserId))
                        {
                            return new ValidationFailure("Social Media User Id", "Social Media User Id is required for " + rm.network);
                        }
                    }
                }
                return null;
            });
        }
    }
}