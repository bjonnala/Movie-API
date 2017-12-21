using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class SearchMovieCatalogRequestJSON
    {
        public string keyword { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
