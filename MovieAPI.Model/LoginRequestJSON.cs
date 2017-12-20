using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class LoginRequestJSON
    {
        public string email { get; set; }
        public string password { get; set; }
        public string network { get; set; }
        public string socialMediaAccessToken { get; set; }
        public string socialMediaUserId { get; set; }
    }
}
