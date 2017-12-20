using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.IBLL
{
    public interface IUserSessions
    {
        string getAccessToken(int userId);
        string updateAccessToken(int userId, string accessToken);
        bool isValidUserId(int userId);
    }
}
