﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieAPI.IBLL;
using MovieAPI.Model;
using MovieAPI.Data;
using System.Data.Entity.Core.Objects;


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

                    ObjectParameter output_result = new ObjectParameter("Result", typeof(Int32));
                    string title = !string.IsNullOrWhiteSpace(item.title) ? item.title : null;
                    string description = !string.IsNullOrWhiteSpace(item.description) ? item.description : null;
                    string language = !string.IsNullOrWhiteSpace(item.language) ? item.language : null;
                    string genre = !string.IsNullOrWhiteSpace(item.genre) ? item.genre : null;
                    string actors = !string.IsNullOrWhiteSpace(sb.ToString().TrimEnd(',')) ? sb.ToString().TrimEnd(',') : null;
                    decimal finalprice = 0.0m;
                    if (item.price.HasValue)
                    {
                        finalprice = item.price.Value;

                    }
                    db.pr_AddMovies(title, description, genre, language, dt, actors,finalprice,output_result);
                    if (output_result.Value != null)
                    {
                        int ouserId = (int)output_result.Value;
                        if (ouserId == 0)
                        {
                            res = "ERR:There seems to be an issue while adding Movies. Please try again.";
                            return res;
                        }

                    }
                }
            }
            return "Movies Added Successfully";





        }

        public GetMovieResponseJSON getMovieDetails(int? movieId)
        {
            GetMovieResponseJSON movieres = new GetMovieResponseJSON();
            using (MovieEntities db = new MovieEntities())
            {


                var actors = (from actm in db.ActorMovies
                              join act in db.Actors on actm.Actor_ID equals act.Actor_ID
                              join m in db.Movies on actm.Movies_ID equals m.Movies_ID
                              where m.Movies_ID == movieId
                              select new
                              {
                                  name = act.name

                              }).ToList();

                List<string> actorslst = new List<string>();

                foreach (var item in actors)
                {
                    actorslst.Add(item.name);
                }


                var moviedetails = (from lang in db.Languages
                            join m in db.Movies on lang.Language_ID equals m.Language_ID
                            join mg in db.MoviesGenres on m.Movies_ID equals mg.Movies_ID
                            join g in db.Genres on mg.Genre_ID equals g.Genre_ID
                            where m.Movies_ID == movieId
                            select new
                            {
                                title = m.Title,
                                description = m.Description,
                                releasedate = m.ReleaseDate,
                                language = lang.Language1,
                                genre = g.Genre1
                               
                            }).ToList();
                foreach (var item in moviedetails)
                {
                    MovieAPI.Model.Movie m = new MovieAPI.Model.Movie();
                    m.description = item.description;
                    m.genre = item.genre;
                    m.language = item.language;
                    if (item.releasedate.HasValue)
                    {
                        m.releasedate = item.releasedate.Value.ToShortDateString();
                    }
                    m.title = item.title;
                    m.actors = actorslst;
                    movieres.Movie = m;
                }
            }
            return movieres;
        }

        public void deleteMovie(int? movieId)
        {
            using (MovieEntities db = new MovieEntities())
            {
               

                // delete from MoviesGenre

                var genre = (from m in db.MoviesGenres where m.Movies_ID == movieId select m).ToList();
                if (genre.Count() > 0)
                {
                    foreach (var item in genre)
                    {
                        db.MoviesGenres.Remove(item);
                        db.SaveChanges();
                    }

                }


                // delete from ActorMovies

                var actor = (from m in db.ActorMovies where m.Movies_ID == movieId select m).ToList();
                if (actor.Count() > 0)
                {
                    foreach (var item in actor)
                    {
                        db.ActorMovies.Remove(item);
                        db.SaveChanges();
                    }

                }

                // delete from Movies
                var result = (from m in db.Movies where m.Movies_ID == movieId select m).ToList();
                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        db.Movies.Remove(item);
                        db.SaveChanges();
                    }

                }
            }

        }

        public SearchMovieCatalogResponseJSON searchMovieCatalog(SearchMovieCatalogRequestJSON req)
        {
            SearchMovieCatalogResponseJSON res = new SearchMovieCatalogResponseJSON();
            using (MovieEntities db = new MovieEntities())
            {
                string keyword = !string.IsNullOrWhiteSpace(req.keyword) ? req.keyword : "";

                var query = db.pr_SearchMovieCatalog(keyword, req.pageNumber, req.pageSize).ToList();
                List<Model.Movie> lst = new List<Model.Movie>();
                foreach (var item in query)
                {
                    Model.Movie m = new Model.Movie();
                    m.description = item.description;
                    m.genre = item.genre;
                    m.language = item.language;
                    if (item.releasedate.HasValue)
                    {
                        m.releasedate = item.releasedate.Value.ToShortDateString();
                    }
                    else
                    {
                        m.releasedate = "";
                    }
                    m.title = item.title;
                    m.price = item.price;
                    List<string> actors = new List<string>();
                    if (!string.IsNullOrWhiteSpace(item.actors))
                    {
                        string[] actorarr = item.actors.Split(',');
                        foreach (var actor in actorarr)
                        {
                            actors.Add(actor);
                        }
                        m.actors = actors;
                    }
                    else
                    {
                        m.actors = actors;
                    }
                    lst.Add(m);
                }
                res.Movies = lst;
            }
            return res;
        }

        public bool isAdmin(int userId)
        {
            bool isValid = false;
            using (MovieEntities db = new MovieEntities())
            {
                var userrole = (from u in db.Users
                              join ur in db.UserRoles on u.Users_ID equals ur.Users_ID
                              join r in db.Roles on ur.Roles_ID equals r.Roles_ID
                              where ur.Users_ID == userId
                              select new
                              {
                                  role = r.RoleName

                              }).ToList();

                foreach (var item in userrole)
                {
                    if (item.role.ToLower() == "admin")
                    {
                        isValid = true;
                    }
                }

            }

            return isValid;
        }
    }
}
