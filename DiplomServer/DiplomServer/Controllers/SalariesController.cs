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
    public class SalariesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Salaries
        public IQueryable<Salary> GetSalaries()
        {
            string user = User.Identity.GetUserId();
            return db.Salaries.Where(c=>c.UserId==user);
        } 
        public IQueryable<Salary> GetProjectSalaries(int id)
        {
            return db.Salaries.Where(c=>c.ProjectId == id);
        }
        

        // GET: api/Salaries/5
        [ResponseType(typeof(Salary))]
        public async Task<IHttpActionResult> GetSalary(int id)
        {
            Salary salary = await db.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            return Ok(salary);
        }

        // PUT: api/Salaries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSalary(int id, Salary salary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salary.Id)
            {
                return BadRequest();
            }

            db.Entry(salary).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(id))
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

        // POST: api/Salaries
        [ResponseType(typeof(Salary))]
        public async Task<IHttpActionResult> PostSalary(Salary salary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Salaries.Add(salary);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = salary.Id }, salary);
        }

        // DELETE: api/Salaries/5
        [ResponseType(typeof(Salary))]
        public async Task<IHttpActionResult> DeleteSalary(int id)
        {
            Salary salary = await db.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            db.Salaries.Remove(salary);
            await db.SaveChangesAsync();

            return Ok(salary);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalaryExists(int id)
        {
            return db.Salaries.Count(e => e.Id == id) > 0;
        }
    }
}