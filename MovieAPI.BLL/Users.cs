using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using MovieAPI.IBLL;
using MovieAPI.Data;
using MovieAPI.Model;

namespace MovieAPI.BLL
{
    public class Users : IUsers
    {
        public string getUserDetails(int id)
        {

            return "testresponse";
        }

        public bool checkDuplicateEmail(string email)
        {

            bool isValid = false;
            using (MovieEntities db = new MovieEntities())
            {
                var query = db.Users.Where(x => x.email.ToLower() == email.ToLower()).FirstOrDefault();
                if (query != null)
                {
                    isValid = true;
                }
            }
            return isValid;

        }

        public void signIn(LoginRequestJSON req)
        {
            using (MovieEntities db = new MovieEntities())
            {
                var query = (from u in db.Users
                             join usm in db.UsersSocialMedias on u.Users_ID equals usm.Users_ID
                             where u.email.ToLower() == req.email.ToLower() && usm.network.ToLower() == req.network.ToLower()
                             select u);
                if (query != null)
                {
                    //isValid = true;
                }
            }
        }

        public int createUser(RegisterUserRequestJSON req)
        {
            int userId = 0;
            using (MovieEntities db = new MovieEntities())
            {
                string email_input = !string.IsNullOrWhiteSpace(req.email) ? req.email : null;
                string hashedpassword_input = !string.IsNullOrWhiteSpace(req.hashedpassword) ? req.hashedpassword : null;
                string salt_input = !string.IsNullOrWhiteSpace(req.salt) ? req.salt : null;
                string accesstoken_input = !string.IsNullOrWhiteSpace(req.socialMediaAccessToken) ? req.socialMediaAccessToken : null;
                string socialmediauserid_input = !string.IsNullOrWhiteSpace(req.socialMediaUserId) ? req.socialMediaUserId : null;
                string firstname_input = !string.IsNullOrWhiteSpace(req.firstName) ? req.firstName : null;
                string network_input = !string.IsNullOrWhiteSpace(req.network) ? req.network : null;

                ObjectParameter output_userId = new ObjectParameter("userId", typeof(Int32));
                int userid = db.pr_RegisterUser(email_input, salt_input, hashedpassword_input, network_input, accesstoken_input, socialmediauserid_input, firstname_input, output_userId);
                if (output_userId.Value != null)
                {
                    int ouserId = (int)output_userId.Value;
                    if (ouserId != 0)
                    {
                        userId = ouserId;
                    }
                    
                }
            }

            return userId;

        }
    }
}
