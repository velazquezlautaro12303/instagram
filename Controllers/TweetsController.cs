using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;

namespace Instagram.Controllers
{
    public class TweetsController : Controller
    {
        private readonly InstagramEntities db = new InstagramEntities();

        // GET: Tweets
        public async Task<ActionResult> Index()
        {
            var tweet = db.Tweet.Include(t => t.User);
            return View(await tweet.ToListAsync());
        }

        // GET: Tweets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = await db.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            return View(tweet);
        }

        // GET: Tweets/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser");
            return View();
        }

        // POST: Tweets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Description,UserID,Date")] Tweet tweet)
        {
            if (ModelState.IsValid)
            {
                db.Tweet.Add(tweet);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", tweet.UserID);
            return View(tweet);
        }

        // GET: Tweets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = await db.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", tweet.UserID);
            return View(tweet);
        }

        // POST: Tweets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Description,UserID,Date")] Tweet tweet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tweet).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", tweet.UserID);
            return View(tweet);
        }

        // GET: Tweets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = await db.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            return View(tweet);
        }

        // POST: Tweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tweet tweet = await db.Tweet.FindAsync(id);
            db.Tweet.Remove(tweet);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
