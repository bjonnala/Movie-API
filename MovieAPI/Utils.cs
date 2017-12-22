using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using JWT;
using MovieAPI.Model;


namespace MovieAPI
{
    public static class Utils
    {

        public static HttpResponseMessage CreateErrorResponse(HttpRequestMessage req, string errormessage)
        {
            ResponseError res_err = new ResponseError();
            res_err.err = errormessage;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.BadRequest,res_err);
            return response;
        }

        public static HttpResponseMessage CreateSuccessResponse(HttpRequestMessage req, Object obj)
        {
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        public static HttpResponseMessage CreateEmptyErrorResponse(HttpRequestMessage req)
        {
            ResponseError res_err = new ResponseError();
            res_err.err = "Request JSON Empty";
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.BadRequest, res_err);
            return response;
        }

        public static void SendEmail(string to, string from, string body, string subject)
        {
            if (isValidEmail(to) && isValidEmail(from))
            {
                SmtpClient ct = new SmtpClient("localhost");
                MailMessage msg = new MailMessage();
                msg.Subject = subject;
                msg.From = new MailAddress(from, "Love Lab");
                msg.To.Add(new MailAddress(to));

                msg.Body = body;
                msg.IsBodyHtml = true;
                try
                {
                    ct.Send(msg);
                }
                catch (Exception e)
                {

                }
            }
        }

        

        public static string GenerateLogintoken(string userid)
        {
            /*
             the exp claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing." 
             If an exp claim is present and is prior to the current time the token will fail verification. 
             The exp (expiry) value must be specified as the number of seconds since 1/1/1970 UTC.*/
            string token = string.Empty;
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expirationtime = Math.Round((DateTime.UtcNow.AddDays(1) - unixEpoch).TotalSeconds);

            // Generate a token
            var payload = new Dictionary<string, object>()
            {
                { "iss", userid },
                { "exp", expirationtime }
            };
            string secretKey = WebConfigurationManager.AppSettings["JSONWebTokenSecretKey"];
            token = JsonWebToken.Encode(payload, secretKey, JwtHashAlgorithm.HS256);


            return token;

        }

        public static bool VerifyLogintoken(string token)
        {

            bool isValid = true;


            //// decode token
            string secretKey = WebConfigurationManager.AppSettings["JSONWebTokenSecretKey"];
            try
            {

                var payload = JsonWebToken.DecodeToObject(token, secretKey) as IDictionary<string, object>;

                var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var expirationtime = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
                //handle token here
                if (Convert.ToDouble(payload["exp"]) <= expirationtime)
                {
                    isValid = false;
                }
            }

            catch (JWT.SignatureVerificationException)
            {
                isValid = false;
            }

            return isValid;

        }

        public static string ComputeHash(string salt, string password)
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

        public static string ComputeSalt()
        {
            Random rd = new Random();
            return rd.Next(100000000, 999999999).ToString();
        }

        public static string Generatetoken()
        {
            Random rd = new Random();
            return rd.Next(100000000, 999999999).ToString();
        }

        public static double calculatedistance(double lat1, double lon1, double lat2, double lon2, string unit)
        {
            if (string.IsNullOrEmpty(unit))
            {
                unit = "M";
            }
            var radlat1 = Math.PI * lat1 / 180;
            var radlat2 = Math.PI * lat2 / 180;
            var radlon1 = Math.PI * lon1 / 180;
            var radlon2 = Math.PI * lon2 / 180;
            var theta = lon1 - lon2;
            var radtheta = Math.PI * theta / 180;
            var dist = Math.Sin(radlat1) * Math.Sin(radlat2) + Math.Cos(radlat1) * Math.Cos(radlat2) * Math.Cos(radtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            if (unit == "M")
            {
                dist = dist * 60 * 1.1515;
            }
            if (unit == "K")
            {
                dist = dist * 1.609344;
            }
            if (unit == "N")
            {
                dist = dist * 0.8684;
            }
            return dist;
        }

        //public static decimal[] CalculateLocation(string location)
        //{

        //    decimal[] arr_loc = new decimal[2];
        //    JObject o1 = JObject.Parse(File.ReadAllText("~/App_Data/locations.json"));

        //    // read JSON directly from a file
        //    using(StreamReader file = File.OpenText("~/App_Data/locations.json"))
        //    using(JsonTextReader reader = new JsonTextReader(file))
        //    {
        //          JObject o2 = (JObject)JToken.ReadFrom(reader);
        //          o2.GetValue("")
        //    }


        //    return arr_loc;
        //}

        public static string GenerateRandomCode()
        {
            Random rd = new Random();
            return rd.Next(100000, 999999).ToString();
        }

        public static string GenerateRandomtransactionId()
        {
            Random rd = new Random();
            return rd.Next(10000000, 999999999).ToString();
        }

        public static bool CompareforEquality(object[] a, object[] b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            if (a.Length != b.Length) { return false; }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) { return false; }

            }

            return true;

        }

        
        public static string GenerateRandomNonce()
        {
            Random rd = new Random();
            return rd.Next(100000000, 999999999).ToString();
        }

        public static string GenerateRandomPassword(int passwordlength)
        {
            string valid = "abcdefghijklmnozABCDEFGHIJKLMNOZ1234567890";
            StringBuilder strB = new StringBuilder(100);
            Random random = new Random();
            while (0 < passwordlength--)
            {
                strB.Append(valid[random.Next(valid.Length)]);
            }
            return strB.ToString();
        }

        public static string HttpClientPostRequest(string uri, string request)
        {
            string res = string.Empty;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                if (uri.Contains("Accurint"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var byteArray = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["accurintUsername"] + ":" + ConfigurationManager.AppSettings["accurintPassword"]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Add("ContentLength", request.Length.ToString());
                }
                else
                {

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                }

                // HTTP POST
                HttpResponseMessage response = client.PostAsync(uri, new StringContent(request)).Result;


                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    res = response.ReasonPhrase;
                }
            }

            //WebRequest req = WebRequest.Create(uri);
            //req.Method = "POST";
            //byte[] byteArray = Encoding.UTF8.GetBytes(request);
            //req.PreAuthenticate = true;
            //req.ContentType = "text/xml";
            //req.ContentLength = byteArray.Length;
            //req.UseDefaultCredentials = false;
            //req.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["accurintUsername"], ConfigurationManager.AppSettings["accurintPassword"]); ;
            //Stream dataStream = req.GetRequestStream();
            //dataStream.Write(byteArray, 0, byteArray.Length);
            //dataStream.Close();
            //WebResponse response = req.GetResponse();
            //dataStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream);
            //string responseFromServer = reader.ReadToEnd();
            //reader.Close();
            //dataStream.Close();
            //response.Close();

            return res;


        }

        public static string PostWebRequest(string uri, string request, string username, string password)
        {
            WebRequest req = WebRequest.Create(uri);
            string responseFromServer = "";
            req.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(request);
            req.PreAuthenticate = true;
            req.ContentType = "text/xml";
            req.ContentLength = byteArray.Length;
            req.PreAuthenticate = true;
            req.UseDefaultCredentials = false;
            req.Credentials = new NetworkCredential(username, password);
            Stream dataStream = req.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            try
            {
                WebResponse response = req.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

            }
            catch (Exception e)
            {
                responseFromServer = e.Message;
            }




            return responseFromServer;

        }

        public static string GetWebRequest(string uri)
        {
            WebRequest req = WebRequest.Create(uri);
            string responseFromServer = "";
            req.Method = "GET";
            req.ContentType = "application/json";

            try
            {
                WebResponse response = req.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

            }
            catch (Exception e)
            {
                responseFromServer = e.Message + "Exception";
            }

            return responseFromServer;

        }
        public static bool UnorderedEqual<T>(ICollection<T> a, ICollection<T> b)
        {

            if (a == null || b == null)
            {
                return false;
            }

            // 1
            // Require that the counts are equal
            if (a.Count != b.Count)
            {
                return false;
            }
            // 2
            // Initialize new Dictionary of the type
            Dictionary<T, int> d = new Dictionary<T, int>();
            // 3
            // Add each key's frequency from collection A to the Dictionary
            foreach (T item in a)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add(item, 1);
                }
            }
            // 4
            // Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch
            foreach (T item in b)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    // Not in dictionary
                    return false;
                }
            }
            // 5
            // Verify that all frequencies are zero
            foreach (int v in d.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }
            // 6
            // We know the collections are equal
            return true;
        }

        public static decimal convertToNumber(string numberValue)
        {
            if (numberValue.Trim().ToString() != "")
            {
                decimal rtv = 0m;
                try
                {
                    rtv = decimal.Parse(numberValue);
                }
                catch
                {
                    rtv = 0m;
                }
                return rtv;
            }
            else
            {
                return 0;
            }
        }

        public static DateTime convertToDate(string dateValue)
        {
            DateTime dDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(dateValue.Trim()))
            {
                if (DateTime.TryParse(dateValue, out dDate))
                {
                    return dDate;
                }
            }
            return dDate;
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public static bool checkDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}