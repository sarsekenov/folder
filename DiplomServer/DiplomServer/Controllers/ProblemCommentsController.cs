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
    public class ProblemCommentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProblemComments
        public IQueryable<ProblemComment> GetProblemComments(int id)
        {
            return db.ProblemComments.Where(c=>c.ProblemId==id);
        }

        // GET: api/ProblemComments/5
        [ResponseType(typeof(ProblemComment))]
        public async Task<IHttpActionResult> GetProblemComment(int id)
        {
            ProblemComment problemComment = await db.ProblemComments.FindAsync(id);
            if (problemComment == null)
            {
                return NotFound();
            }

            return Ok(problemComment);
        }

        // PUT: api/ProblemComments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProblemComment(int id, ProblemComment problemComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != problemComment.Id)
            {
                return BadRequest();
            }

            db.Entry(problemComment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProblemCommentExists(id))
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

        // POST: api/ProblemComments
        [ResponseType(typeof(ProblemComment))]
        public async Task<IHttpActionResult> PostProblemComment(ProblemComment problemComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProblemComments.Add(problemComment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = problemComment.Id }, problemComment);
        }

        // DELETE: api/ProblemComments/5
        [ResponseType(typeof(ProblemComment))]
        public async Task<IHttpActionResult> DeleteProblemComment(int id)
        {
            ProblemComment problemComment = await db.ProblemComments.FindAsync(id);
            if (problemComment == null)
            {
                return NotFound();
            }

            db.ProblemComments.Remove(problemComment);
            await db.SaveChangesAsync();

            return Ok(problemComment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProblemCommentExists(int id)
        {
            return db.ProblemComments.Count(e => e.Id == id) > 0;
        }
    }
}