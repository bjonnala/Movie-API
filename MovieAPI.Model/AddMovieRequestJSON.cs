﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Model
{
    public class AddMovieRequestJSON
    {
        public int userId { get; set; }
        public List<Movie> Movies { get; set; }
        
    }

    
}
