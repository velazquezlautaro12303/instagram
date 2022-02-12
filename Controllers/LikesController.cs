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
    public class LikesController : Controller
    {
        private readonly InstagramEntities db = new InstagramEntities();

        // GET: Likes
        public async Task<ActionResult> Index()
        {
            var like = db.Like.Include(l => l.Tweet).Include(l => l.User);
            return View(await like.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = await db.Like.FindAsync(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            return View(like);
        }

        // GET: Likes/Create
        public ActionResult Create()
        {
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description");
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser");
            return View();
        }

        // POST: Likes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserID,TweetID")] Like like)
        {
            if (ModelState.IsValid)
            {
                db.Like.Add(like);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", like.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", like.UserID);
            return View(like);
        }

        // GET: Likes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = await db.Like.FindAsync(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", like.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", like.UserID);
            return View(like);
        }

        // POST: Likes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserID,TweetID")] Like like)
        {
            if (ModelState.IsValid)
            {
                db.Entry(like).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", like.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", like.UserID);
            return View(like);
        }

        // GET: Likes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = await db.Like.FindAsync(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Like like = await db.Like.FindAsync(id);
            db.Like.Remove(like);
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
