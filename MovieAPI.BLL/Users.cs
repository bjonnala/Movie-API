using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPI.IBLL;
using MovieAPI.Data;

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
    }
}
