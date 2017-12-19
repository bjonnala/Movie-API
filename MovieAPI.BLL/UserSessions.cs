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
    }
}
