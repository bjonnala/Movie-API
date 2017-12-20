using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieAPI.Model
{
    /* For all RequestJSON's add RequestJSON to be consistent */
    /* For all ResponseJSON's add ResponseJSON to be consistent */
    public class ResponseError
    {
        public string err { get; set; }
    }

    public class ResponseValidResult
    {
        public string result { get; set; }
    }

    //public class LoginUserResponse
    //{
    //    public string id { get; set; }
    //    public string authorizeKey { get; set; }
    //}

    public class IncrementNotificationCountRequestJSON
    {
        public string userId { get; set; }
        public string otherUserId { get; set; }
    }

    //public class LoginRequestJSON
    //{
    //    public string email { get; set; }
    //    public string passKey { get; set; }
    //    public string network { get; set; }
    //    public string socialMediaAccessToken { get; set; }
    //    public string socialMediaUserId { get; set; }
    //}

    public class GetUserDetailsRequestJSON
    {
        public string userId { get; set; }
    }

    public class ForgotPasswordRequestJSON
    {
        public string email { get; set; }
    }

    public class ReportProfileRequestJSON
    {
        public string userId { get; set; }
    }

    public class InactivateUserRequestJSON
    {
        public string userId { get; set; }
    }

    public class LogoutUserRequestJSON
    {
        public string userId { get; set; }
    }

    public class GetAdminConfigurationRequestJSON
    {
        public string userId { get; set; }
    }

    public class GetGuestModeUserDetailsRequestJSON
    {
        public string userId { get; set; }
    }

    public class VerifyPhoneRequestJSON
    {
        public string userId { get; set; }
        public string phone { get; set; }
    }

    public class VerifyPhoneCodeRequestJSON
    {
        public string userId { get; set; }
        public string code { get; set; }
        public string phone { get; set; }
    }

    public class SendInviteRequestJSON
    {
        public string userId { get; set; }
        public string[] phones { get; set; }
        public string[] emails { get; set; }
    }

    public class SendInviteResponseJSON
    {
        public string error { get; set; }
    }

    //public class RegisterUserRequestJSON
    //{
    //    public string email { get; set; }
    //    public string password { get; set; }
    //    public string socialMediaAccessToken { get; set; }
    //    public string socialMediaUserId { get; set; }
    //    public string network { get; set; }

    //    //public DateTime dob { get; set; }
    //    //public string zipcode { get; set; }
    //    public string firstName { get; set; }
    //    //public string lastName { get; set; }
    //    public string hashedpassword { get; set; }
    //    public string salt { get; set; }

    //    //public int trustLevel { get; set; }
    //    //public string phone { get; set; }
    //}

    public class ValidateLoveLabIdRequestJSON
    {
        public string loveLabId { get; set; }
    }



    public class SetPhotoAsProfilePicRequestJSON
    {
        public string userId { get; set; }
        public string photoId { get; set; }
    }

    public class UpdateReorderPhotosRequestJSON
    {
        public string userId { get; set; }
        public string[] photoIds { get; set; }
    }

    public class DeletePhotoRequestJSON
    {
        public string userId { get; set; }
        public string photoId { get; set; }
    }

    public class SearchUserRequestJSON
    {
        public string email { get; set; }
        public string phone { get; set; }
        public string loveLabId { get; set; }
    }

    public class SearchUserInGuestModeRequestJSON
    {
        public string email { get; set; }
        public string phone { get; set; }
        public string loveLabId { get; set; }
    }

    public class VerifyEmailRequestJSON
    {
        public string userId { get; set; }
        public string email { get; set; }
    }


   
}