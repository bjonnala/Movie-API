using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using MovieAPI.IBLL;
using MovieAPI.Data;
using MovieAPI.Model;
using System.Security.Cryptography;
using System.Text;
using MovieAPI;
using System.Text.RegularExpressions;

namespace MovieAPI.BLL
{
    public class Users : IUsers
    {
       
        public bool checkDuplicateEmail(string email)
        {

            bool isValid = false;
            using (MovieEntities db = new MovieEntities())
            {
                var query = db.Users.Where(x => x.email.ToLower() == email.ToLower()).FirstOrDefault();
                if (query!= null)
                {
                    isValid = true;
                }
            }
            return isValid;

        }

        public int signIn(LoginRequestJSON req)
        {

            int userId = 0;
            using (MovieEntities db = new MovieEntities())
            {
                if (req.network.ToLower() == "local")
                {
                    string salt = string.Empty;
                    string hashpassword = string.Empty;
                    var salt_query = (from u in db.Users
                                    where u.email.ToLower() == req.email.ToLower()
                                    select u).ToList();
                    foreach (var item in salt_query)
                    {
                        salt = item.salt;
                    }
                    if (!string.IsNullOrWhiteSpace(salt))
                    {
                        hashpassword = ComputeHash(salt, req.password);
                    }

                    var usr_query = (from u in db.Users
                                 where u.email.ToLower() == req.email.ToLower() && u.passwordHash == hashpassword
                                 select new { id = u.Users_ID}).FirstOrDefault();
                    if (usr_query != null)
                    {
                        userId = usr_query.id;
                    }

                }
                else
                {
                    var usr_social = (from u in db.Users
                                 join usm in db.UsersSocialMedias on u.Users_ID equals usm.Users_ID
                                 where u.email.ToLower() == req.email.ToLower() 
                                 && usm.network.ToLower() == req.network.ToLower()
                                 select new { id = u.Users_ID }).FirstOrDefault();
                    if (usr_social != null)
                    {
                        userId = usr_social.id;
                    }
                }               
            }

            return userId;
        }

        public UserResponseJSON getUserDetails(int userId)
        {

            UserResponseJSON res = new UserResponseJSON();
            using (MovieEntities db = new MovieEntities())
            {
                var usr = (from u in db.Users
                                  where u.Users_ID == userId
                                  select new
                                  {
                                      UserId = u.Users_ID,
                                      firstName = u.firstName,
                                      lastName = u.lastName,
                                      email = u.email,
                                      address = u.address,
                                      city = u.city,
                                      state = u.state,
                                      country = u.country,
                                      zipcode = u.zipcode,
                                      ccNo = u.ccNo,
                                      cvc = u.cvc,
                                      ccexpiration = u.ccexpiration,
                                      createdDate = u.createdDate
                                  }).ToList();
               
               
                    foreach (var item in usr)
                    {
                        res.UserId = item.UserId;
                        res.firstName = item.firstName;
                        res.lastName = item.lastName;
                        res.email = item.email;
                        res.address = item.address;
                        res.city = item.city;
                        res.state = item.state;
                        res.country = item.country;
                        res.zipcode = item.zipcode;
                        res.ccNo = item.ccNo;
                        res.cvc = item.cvc;
                        res.ccexpiration = item.ccexpiration;
                        if (item.createdDate.HasValue)
                        {
                            res.createdDate = item.createdDate.Value.ToString("g");

                        }
                    }
                
                var usrexternallogins = db.UsersSocialMedias.Where(x => x.Users_ID == userId).ToList();
                List<AuthData> lst = new List<AuthData>();
                foreach (var item in usrexternallogins)
                {
                    AuthData ad = new AuthData();
                    ad.network = item.network;
                    ad.socialMediaAccessToken = item.socialMediaAccessToken;
                    ad.socialMediaUserId = item.socialMediaUserId;

                    lst.Add(ad);
                }

                res.SocialMediaLogins = lst;
                return res;
            }
        }

        public UpdateUserResponseJSON updateUserDetails(UpdateUserRequestJSON req)
        {
            using (MovieEntities db = new MovieEntities())
            {
                var query = db.Users.Where(x => x.Users_ID == req.userId).FirstOrDefault();
                if (query != null)
                {
                    
                    query.address = !string.IsNullOrWhiteSpace(req.address) ? req.address : null;
                    query.ccexpiration = !string.IsNullOrWhiteSpace(req.ccexpiration) ? req.ccexpiration : null;
                    query.ccNo = !string.IsNullOrWhiteSpace(req.ccNo) ? req.ccNo : null;
                    query.city = !string.IsNullOrWhiteSpace(req.city) ? req.city : null;
                    query.country = !string.IsNullOrWhiteSpace(req.country) ? req.country : null;
                    query.cvc = !string.IsNullOrWhiteSpace(req.cvc) ? req.cvc : null;
                    query.email = !string.IsNullOrWhiteSpace(req.email) ? req.email : null;
                    query.firstName = !string.IsNullOrWhiteSpace(req.firstName) ? req.firstName : null;
                    query.lastName = !string.IsNullOrWhiteSpace(req.lastName) ? req.lastName : null;
                    query.state =  !string.IsNullOrWhiteSpace(req.state) ? req.state : null;
                    query.zipcode = !string.IsNullOrWhiteSpace(req.zipcode) ? req.zipcode : null;
                    if (!string.IsNullOrWhiteSpace(req.password))
                    {
                        string salt = query.salt;
                        string hashedpassword = ComputeHash(salt, req.password);

                        query.passwordHash = hashedpassword;

                    }

                    query.createdDate = DateTime.UtcNow;
                    db.SaveChanges();
                }


                if (req.SocialMediaLogins.Count > 0)
                {
                    // Delete existing and insert from UsersSocialMedia

                    var result = from r in db.UsersSocialMedias where r.Users_ID == req.userId select r;
                    if (result.Count() > 0)
                    {
                        foreach (UsersSocialMedia p in result)
                        {
                            db.UsersSocialMedias.Remove(p);
                        }
                        db.SaveChanges();
                    }

                    // Insert 
                    foreach (var item in req.SocialMediaLogins)
                    {
                        UsersSocialMedia usm = new UsersSocialMedia();
                        usm.network = item.network;
                        usm.Users_ID = req.userId;
                        usm.socialMediaAccessToken = item.socialMediaAccessToken;
                        usm.socialMediaUserId = item.socialMediaUserId;

                        db.UsersSocialMedias.Add(usm);
                        db.SaveChanges();
                    }
                }
            }

            // return whole user object except password.
            UserResponseJSON usr = getUserDetails(req.userId);
            UpdateUserResponseJSON res = new UpdateUserResponseJSON();
            res.address = usr.address;
            res.ccexpiration = usr.ccexpiration;
            res.ccNo = usr.ccNo;
            res.city = usr.city;
            res.country = usr.country;
            res.createdDate = usr.createdDate;
            res.cvc = usr.cvc;
            res.email = usr.email;
            res.firstName = usr.firstName;
            res.lastName = usr.lastName;
            res.SocialMediaLogins = usr.SocialMediaLogins;
            res.state = usr.state;
            res.zipcode = usr.zipcode;

            return res;

        }

        public bool isValidUserId(int userId)
        {
            using (MovieEntities db = new MovieEntities())
            {
                var count = (from u in db.Users
                             where u.Users_ID == userId
                             select u).Count();
                return  count > 0 ? true: false;
            }
        }

        public string ComputeHash(string salt, string password)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(salt);
            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(password);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);


            var sb = new StringBuilder();
            foreach (byte b in hashmessage)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();

        }

        public int createUser(RegisterRequestJSON req)
        {
            int userId = 0;
            using (MovieEntities db = new MovieEntities())
            {
                string email_input = !string.IsNullOrWhiteSpace(req.email) ? req.email : null;
                string hashedpassword_input = !string.IsNullOrWhiteSpace(req.hashedpassword) ? req.hashedpassword : null;
                string salt_input = !string.IsNullOrWhiteSpace(req.salt) ? req.salt : null;
                string accesstoken_input = !string.IsNullOrWhiteSpace(req.socialMediaAccessToken) ? req.socialMediaAccessToken : null;
                string socialmediauserid_input = !string.IsNullOrWhiteSpace(req.socialMediaUserId) ? req.socialMediaUserId : null;
                string firstname_input = !string.IsNullOrWhiteSpace(req.firstName) ? req.firstName : null;
                string network_input = !string.IsNullOrWhiteSpace(req.network) ? req.network : null;

                ObjectParameter output_userId = new ObjectParameter("userId", typeof(Int32));
                int userid = db.pr_RegisterUser(email_input, salt_input, hashedpassword_input, network_input, accesstoken_input, socialmediauserid_input, firstname_input, output_userId);
                if (output_userId.Value != null)
                {
                    int ouserId = (int)output_userId.Value;
                    if (ouserId != 0)
                    {
                        userId = ouserId;
                    }
                    
                }
            }

            return userId;

        }

        private DateTime convertToDate(string dateValue)
        {
            DateTime dDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(dateValue))
            {
               
                try
                {
                    dDate = DateTime.Parse(dateValue);
                }
                catch
                {
                    return dDate;
                }
                if (dDate.Year < 1901) { return dDate; }
                return dDate;
            }
            else
            {
                return dDate;
            }

        }

        public string rentMovies(RentMoviesRequestJSON res)
        {
            string status = "";
            try
            {

                using (MovieEntities db = new MovieEntities())
                {

                    foreach (var item in res.moviesIds)
                    {
                        UsersRental ur = new UsersRental();
                        ur.Users_ID = res.userId;
                        ur.Movies_ID = item;
                        DateTime datefrom = convertToDate(res.RentalDateFrom);
                        DateTime dateto = convertToDate(res.RentalDateTo);
                        if (datefrom == DateTime.Now)
                        {
                            ur.RentalDateFrom = null;
                        }
                        else
                        {
                            ur.RentalDateFrom = datefrom;
                        }
                        if (dateto == DateTime.Now)
                        {
                            ur.RentalDateTo = null;
                        }
                        else
                        {
                            ur.RentalDateTo = dateto;
                        }
                        ur.UsersTransaction_ID = null;
                        ur.CreatedDate = DateTime.UtcNow;
                        db.UsersRentals.Add(ur);
                        db.SaveChanges();
                    }

                    /* if authorizePayment is set to true user credit card details can obtained from Users table.
                    if true, we can get these details from Users table and charge accordingly using Stripe or Authorize.net library
                    */
                    status = "Rented successfully";
                }
            }
            catch
            {

                status = "ERR: There seems to be issue with rental. Please try again.";
            }

            return status;


        }

        public RentedMoviesResponseJSON getUserRentals(int userId)
        {

            RentedMoviesResponseJSON res = new RentedMoviesResponseJSON();
            using (MovieEntities db = new MovieEntities())
            {
                var rental = (from u in db.UsersRentals
                              join m in db.Movies on u.Movies_ID equals m.Movies_ID
                              join p in db.MoviesRentalPrices on m.Movies_ID equals p.Movies_ID
                              where u.Users_ID == userId
                              select new
                              {
                                  UserId = u.Users_ID,
                                  movieTitle = m.Title,
                                  rentaldatefrom = u.RentalDateFrom,
                                  rentaldateto = u.RentalDateTo,
                                  price = p.Price
                           }).ToList();

                List<RentalInfo> lstrentals = new List<RentalInfo>();
                foreach (var item in rental)
                {
                    RentalInfo ri = new RentalInfo();
                    if (item.rentaldatefrom.HasValue)
                    {
                        ri.rentaldatefrom = item.rentaldatefrom.Value.ToShortDateString();
                    }
                    if (item.rentaldateto.HasValue)
                    {
                        ri.rentaldateto = item.rentaldateto.Value.ToShortDateString();
                    }
                    ri.title = item.movieTitle;
                    if (item.price.HasValue)
                    {
                        ri.price = item.price.Value;
                    }
                    lstrentals.Add(ri);
                }

                res.rentals = lstrentals;
                return res;
            }
        }
    }
}
