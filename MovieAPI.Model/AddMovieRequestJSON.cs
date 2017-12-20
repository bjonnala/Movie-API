using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class AddMovieRequestJSON
    {
        /*
         {
		    "title": "The Shawshank Redemption",
		    "description":"Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
		    "genre": "Drama",
		    "language":"English",
		    "releasedate": "10/14/1994",
		    "actors": ["Tim Robbins", "Morgan Freeman","Bob Gunton"]
	    }
         */
        public int userId { get; set; }
        public List<Movie> Movies { get; set; }
        
    }

    public class Movie
    {
        public string title { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public string language { get; set; }
        public string releasedate { get; set; }
        public List<string> actors { get; set; }
    }
}
