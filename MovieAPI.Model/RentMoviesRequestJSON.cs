using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class RentMoviesRequestJSON
    {
        public int userId { get; set; }
        public int[] moviesIds { get; set; }
        public string RentalDateFrom { get; set; }
        public string RentalDateTo { get; set; }
        public bool authorizePayment { get; set; }
    }

}
