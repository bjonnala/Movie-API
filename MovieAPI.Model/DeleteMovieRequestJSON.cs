using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class DeleteMovieRequestJSON
    {
        public int movieId { get; set; }
        public int userId { get; set; }
    }
}
