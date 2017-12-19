using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPI.Model;


namespace MovieAPI.IBLL
{
    public interface IUsers
    {
        string getUserDetails(int UserId);
        bool checkDuplicateEmail(string email);
        bool createUser();
    }
}
