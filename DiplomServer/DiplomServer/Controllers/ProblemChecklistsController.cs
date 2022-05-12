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

namespace DiplomServer.Controllers
{
    public class ProblemChecklistsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProblemChecklists
        public IQueryable<ProblemChecklist> GetProblemChecklists()
        {
            return db.ProblemChecklists;
        }

        // GET: api/ProblemChecklists/5
        [ResponseType(typeof(ProblemChecklist))]
        public async Task<IHttpActionResult> GetProblemChecklist(int id)
        {
            ProblemChecklist problemChecklist = await db.ProblemChecklists.FindAsync(id);
            if (problemChecklist == null)
            {
                return NotFound();
            }

            return Ok(problemChecklist);
        }

        // PUT: api/ProblemChecklists/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProblemChecklist(int id, ProblemChecklist problemChecklist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != problemChecklist.Id)
            {
                return BadRequest();
            }

            db.Entry(problemChecklist).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProblemChecklistExists(id))
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

        // POST: api/ProblemChecklists
        [ResponseType(typeof(ProblemChecklist))]
        public async Task<IHttpActionResult> PostProblemChecklist(ProblemChecklist problemChecklist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProblemChecklists.Add(problemChecklist);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = problemChecklist.Id }, problemChecklist);
        }

        // DELETE: api/ProblemChecklists/5
        [ResponseType(typeof(ProblemChecklist))]
        public async Task<IHttpActionResult> DeleteProblemChecklist(int id)
        {
            ProblemChecklist problemChecklist = await db.ProblemChecklists.FindAsync(id);
            if (problemChecklist == null)
            {
                return NotFound();
            }

            db.ProblemChecklists.Remove(problemChecklist);
            await db.SaveChangesAsync();

            return Ok(problemChecklist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProblemChecklistExists(int id)
        {
            return db.ProblemChecklists.Count(e => e.Id == id) > 0;
        }
    }
}