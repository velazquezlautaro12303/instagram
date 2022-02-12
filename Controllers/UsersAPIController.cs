using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Instagram.Models;

namespace Instagram.Controllers
{
    public class Usuario {
        public String NameUser { get; set; }

        public Usuario(String NameUser)
        {
            this.NameUser = NameUser;
        }
    }

    public class UsersAPIController : ApiController
    {
        private readonly InstagramEntities db = new InstagramEntities();

        // GET: api/UsersAPI
        public IQueryable<String> GetUser()
        {
            return from user in db.User select user.NameUser;
        }

        // GET: api/UsersAPI/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/UsersAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UsersAPI
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE: api/UsersAPI/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.ID == id) > 0;
        }
    }
}