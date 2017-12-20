using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class LoginResponseJSON
    {
        public string userId { get; set; }
        public string authorizeKey { get; set; }
    }
}
