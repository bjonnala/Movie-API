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
        bool checkDuplicateEmail(string email);
        int createUser(RegisterRequestJSON req);
        int signIn(LoginRequestJSON req);
        UserResponseJSON getUserDetails(int UserId);
        bool isValidUserId(int UserId);
        UpdateUserResponseJSON updateUserDetails(UpdateUserRequestJSON req);
    }
}
