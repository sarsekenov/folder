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
    public class ProblemsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Problems
        public IQueryable<Problem> GetProblems()
        {
            
            return db.Problems;
        }
        [Route("GetProblemsbyId")]
        public IQueryable<Problem> GetProblemsbyId(int id)
        {
            return db.Problems.Where(c=>c.ProjectId == id);
        }
        // GET: api/Problems/5
        [ResponseType(typeof(Problem))]
        public async Task<IHttpActionResult> GetProblem(int id)
        {
            Problem problem = await db.Problems.FindAsync(id);
            if (problem == null)
            {
                return NotFound();
            }

            return Ok(problem);
        }

        // PUT: api/Problems/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProblem(int id, Problem problem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != problem.Id)
            {
                return BadRequest();
            }

            db.Entry(problem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProblemExists(id))
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

        // POST: api/Problems
        [ResponseType(typeof(Problem))]
        public async Task<IHttpActionResult> PostProblem(Problem problem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Problems.Add(problem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = problem.Id }, problem);
        }

        // DELETE: api/Problems/5
        [ResponseType(typeof(Problem))]
        public async Task<IHttpActionResult> DeleteProblem(int id)
        {
            Problem problem = await db.Problems.FindAsync(id);
            if (problem == null)
            {
                return NotFound();
            }

            db.Problems.Remove(problem);
            await db.SaveChangesAsync();

            return Ok(problem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProblemExists(int id)
        {
            return db.Problems.Count(e => e.Id == id) > 0;
        }
    }
}