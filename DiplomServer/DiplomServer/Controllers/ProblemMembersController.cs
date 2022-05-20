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
    [Authorize]
    public class ProblemMembersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProblemMembers
        public IQueryable<ProblemMember> GetProblemMembers()
        {
            string user = User.Identity.GetUserId();
            return db.ProblemMembers.Where(c=>c.UserId==user);
        }

        // GET: api/ProblemMembers/5
        [ResponseType(typeof(ProblemMember))]
        public async Task<IHttpActionResult> GetProblemMember(int id)
        {
            ProblemMember problemMember = await db.ProblemMembers.FindAsync(id);
            if (problemMember == null)
            {
                return NotFound();
            }

            return Ok(problemMember);
        }

        // PUT: api/ProblemMembers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProblemMember(int id, ProblemMember problemMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != problemMember.Id)
            {
                return BadRequest();
            }

            db.Entry(problemMember).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProblemMemberExists(id))
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

        // POST: api/ProblemMembers
        [ResponseType(typeof(ProblemMember))]
        public async Task<IHttpActionResult> PostProblemMember(ProblemMember problemMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProblemMembers.Add(problemMember);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = problemMember.Id }, problemMember);
        }

        // DELETE: api/ProblemMembers/5
        [ResponseType(typeof(ProblemMember))]
        public async Task<IHttpActionResult> DeleteProblemMember(int id)
        {
            ProblemMember problemMember = await db.ProblemMembers.FindAsync(id);
            if (problemMember == null)
            {
                return NotFound();
            }

            db.ProblemMembers.Remove(problemMember);
            await db.SaveChangesAsync();

            return Ok(problemMember);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProblemMemberExists(int id)
        {
            return db.ProblemMembers.Count(e => e.Id == id) > 0;
        }
    }
}