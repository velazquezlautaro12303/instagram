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
    public class CommentsController : Controller
    {
        private readonly InstagramEntities db = new InstagramEntities();

        // GET: Comments
        public async Task<ActionResult> Index()
        {
            var comment = db.Comment.Include(c => c.Tweet).Include(c => c.User);
            return View(await comment.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comment.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description");
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser");
            return View();
        }

        // POST: Comments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserID,TweetID,Description")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comment.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", comment.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comment.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", comment.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", comment.UserID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserID,TweetID,Description")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", comment.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comment.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comment.FindAsync(id);
            db.Comment.Remove(comment);
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
