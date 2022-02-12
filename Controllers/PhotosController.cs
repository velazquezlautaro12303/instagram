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
    public class PhotosController : Controller
    {
        private readonly InstagramEntities db = new InstagramEntities();

        // GET: Photos
        public async Task<ActionResult> Index()
        {
            var photo = db.Photo.Include(p => p.Tweet);
            return View(await photo.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photo.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description");
            return View();
        }

        // POST: Photos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Path,TweetID")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Photo.Add(photo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", photo.TweetID);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photo.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", photo.TweetID);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Path,TweetID")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", photo.TweetID);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photo.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Photo photo = await db.Photo.FindAsync(id);
            db.Photo.Remove(photo);
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
