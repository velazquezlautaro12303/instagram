using Instagram.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Instagram.Controllers
{
    public class HomeController : Controller
    {
        private readonly InstagramEntities db = new InstagramEntities();
        public int idMyUser = 2;

        // GET: Home
        public ActionResult Index(String nameUser)
        {
            ViewBag.idMyUser = idMyUser;
            var listTweets = new List<Tweet>();

            if (nameUser == null || nameUser.Count() == 0)
            {
                listTweets = db.Tweet.ToList();
            }
            else
            {
                listTweets = db.Tweet.Where(u => u.User.NameUser == nameUser).ToList();
            }

            return View(listTweets);
        }

        [HttpPost]
        public ActionResult LoadTweet(String Description, String imagenEnviar)
        {
            if (ModelState.IsValid)
            {
                var newTweet = new Tweet();
                newTweet.Description = Description;
                newTweet.Date = DateTime.Now;
                newTweet.UserID = idMyUser;

                db.Tweet.Add(newTweet);

                if (imagenEnviar != "")
                {
                    String base64 = imagenEnviar.Substring(22);
                    string fullpath = Server.MapPath("~");
                    string extension = GetFileExtension(imagenEnviar);

                    Guid miGuid = Guid.NewGuid();
                    string token = Convert.ToBase64String(miGuid.ToByteArray());
                    token = token.Replace("/", "").Replace("\\", "").Replace("=", "").Replace("+", "");

                    string photo = "Resources\\Images\\" + token + extension;

                    var newPhoto = new Photo();
                    newPhoto.Tweet = newTweet;
                    newPhoto.Path = photo;

                    db.Photo.Add(newPhoto);
                    
                    System.IO.File.WriteAllBytes(fullpath + photo, Convert.FromBase64String(base64));
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult SetLike(int idTweet)
        {
            bool state = false;
            
            try
            {
                Like like = db.Tweet.Find(idTweet).Like.First(t => t.UserID == idMyUser);
                db.Like.Remove(like);
            }
            catch (InvalidOperationException)
            {
                var like = new Like() { TweetID = idTweet, UserID = idMyUser };
                db.Like.Add(like);
                state = true;
            }
            
            db.SaveChanges();

            return Json(state);
        }

        private static string GetFileExtension(string data)
        {
            if (data.Substring(11).StartsWith("png")) 
            {
                return ".png";
            }
            return "";
        }
    }
}