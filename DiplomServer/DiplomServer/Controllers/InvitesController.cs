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
using DiplomServer.Models;
using Microsoft.AspNet.Identity;

namespace DiplomServer.Controllers
{
    public class InvitesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Invites
        public IQueryable<Invite> GetInvites()
        {
            string user = User.Identity.GetUserId();
            return db.Invites.Where(c=>c.Userid == user);
        }

        // GET: api/Invites/5
        [ResponseType(typeof(Invite))]
        public async Task<IHttpActionResult> GetInvite(int id)
        {
            Invite invite = await db.Invites.FindAsync(id);
            if (invite == null)
            {
                return NotFound();
            }

            return Ok(invite);
        }

        // PUT: api/Invites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInvite(int id, Invite invite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invite.Id)
            {
                return BadRequest();
            }

            db.Entry(invite).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InviteExists(id))
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

        // POST: api/Invites
        [ResponseType(typeof(Invite))]
        public async Task<IHttpActionResult> PostInvite(Invite invite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string user = User.Identity.GetUserId();
            string username = User.Identity.GetUserName();
            invite.OwnerEmail = username;

            db.Invites.Add(invite);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = invite.Id }, invite);
        }

        // DELETE: api/Invites/5
        [ResponseType(typeof(Invite))]
        public async Task<IHttpActionResult> DeleteInvite(int id)
        {
            Invite invite = await db.Invites.FindAsync(id);
            if (invite == null)
            {
                return NotFound();
            }

            db.Invites.Remove(invite);
            await db.SaveChangesAsync();

            return Ok(invite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InviteExists(int id)
        {
            return db.Invites.Count(e => e.Id == id) > 0;
        }
    }
}