using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPI.IBLL;
using MovieAPI.Data;
using System.Data.Entity;

namespace MovieAPI.BLL
{
    public class UserSessions : IUserSessions
    {
        public bool isValidUserId(int userId)
        {
            using (MovieEntities db = new MovieEntities())
            {
                var count = (from u in db.Users
                             where u.Users_ID == userId
                             select u).Count();
                return count > 0 ? true : false;
            }
        }
        public string getAccessToken(int userId)
        {
            string token = string.Empty;
            using (MovieEntities db = new MovieEntities())
            {
                var query = db.UsersSessions.Where(x => x.Users_ID == userId).ToList();
                foreach (var item in query)
                {
                    token = item.token;
                }
            }
            return token;
        }

        public string updateAccessToken(int userId,string accessToken)
        {
            string status = "";
            try
            {
                using (MovieEntities db = new MovieEntities())
                {
                    var query = db.UsersSessions.Where(x => x.Users_ID == userId).FirstOrDefault();
                    if (query != null)
                    {
                        query.token = accessToken;
                        query.createdDate = DateTime.UtcNow;
                        db.SaveChanges();
                    }
                    else
                    {
                        var usersession = new UsersSession();
                        usersession.Users_ID = userId;
                        usersession.token = accessToken;
                        usersession.createdDate = DateTime.UtcNow;

                        db.UsersSessions.Add(usersession);
                        db.SaveChanges();
                    }
                }
            }
            catch
            {

                status = "There seems to be an issue with updating access token. Please try again.";
            }
            
            return status;
        }
    }
}
