using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class RentedMoviesResponseJSON
    {
        public List<RentalInfo> rentals { get; set; }
    }
}
