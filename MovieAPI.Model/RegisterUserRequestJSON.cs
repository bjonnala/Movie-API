using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MovieAPI.Model
{
    public class RegisterUserRequestJSON
    {
        public string email { get; set; }
        public string password { get; set; }
        public string socialMediaAccessToken { get; set; }
        public string socialMediaUserId { get; set; }
        public string network { get; set; }
        public string firstName { get; set; }
        public string hashedpassword { get; set; }
        public string salt { get; set; }


    }
}
