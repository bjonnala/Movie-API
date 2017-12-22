using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class UpdateUserRequestJSON
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string ccNo { get; set; }
        public string cvc { get; set; }
        public string ccexpiration { get; set; }
        public string createdDate { get; set; }
        public string password { get; set; }
        public List<AuthData> SocialMediaLogins { get; set; }
    }
}
