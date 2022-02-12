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
    public class SharesController : Controller
    {
        private readonly InstagramEntities db = new InstagramEntities();

        // GET: Shares
        public async Task<ActionResult> Index()
        {
            var share = db.Share.Include(s => s.Tweet).Include(s => s.User);
            return View(await share.ToListAsync());
        }

        // GET: Shares/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = await db.Share.FindAsync(id);
            if (share == null)
            {
                return HttpNotFound();
            }
            return View(share);
        }

        // GET: Shares/Create
        public ActionResult Create()
        {
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description");
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser");
            return View();
        }

        // POST: Shares/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,TweetID,UserID")] Share share)
        {
            if (ModelState.IsValid)
            {
                db.Share.Add(share);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", share.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", share.UserID);
            return View(share);
        }

        // GET: Shares/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = await db.Share.FindAsync(id);
            if (share == null)
            {
                return HttpNotFound();
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", share.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", share.UserID);
            return View(share);
        }

        // POST: Shares/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TweetID,UserID")] Share share)
        {
            if (ModelState.IsValid)
            {
                db.Entry(share).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TweetID = new SelectList(db.Tweet, "ID", "Description", share.TweetID);
            ViewBag.UserID = new SelectList(db.User, "ID", "NameUser", share.UserID);
            return View(share);
        }

        // GET: Shares/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = await db.Share.FindAsync(id);
            if (share == null)
            {
                return HttpNotFound();
            }
            return View(share);
        }

        // POST: Shares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Share share = await db.Share.FindAsync(id);
            db.Share.Remove(share);
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
