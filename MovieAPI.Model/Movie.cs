using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
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
