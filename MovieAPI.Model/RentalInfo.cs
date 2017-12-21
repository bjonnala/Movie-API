using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class RentalInfo
    {
        public string title { get; set; }
        public string rentaldatefrom { get; set; }
        public string rentaldateto { get; set; }
        public decimal price { get; set; }
    }
}
