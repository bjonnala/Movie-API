using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.Data;


namespace MovieAPI.BLL
{
    public class Movie : IMovie
    {
        public string addMovie(AddMovieRequestJSON req)
        {
            string res = string.Empty;
            using (MovieEntities db = new MovieEntities())
            {
                foreach (var item in req.Movies)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var actor in item.actors)
                    {
                        sb.Append(actor + ",");
                    }
                    DateTime dt = DateTime.Now;
                    DateTime.TryParse(item.releasedate, out dt);

                    
                    db.pr_AddMovies(item.title, item.description, item.genre, item.language, dt, sb.ToString().TrimEnd(','));
                }
            }
            return res;





        }
    }
}
