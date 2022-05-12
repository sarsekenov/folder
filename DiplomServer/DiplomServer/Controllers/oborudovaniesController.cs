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
    public class oborudovaniesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/oborudovanies
        public IQueryable<oborudovanie> Getoborudovanies()
        {
            return db.oborudovanies;
        }

        // GET: api/oborudovanies/5
        [ResponseType(typeof(oborudovanie))]
        public async Task<IHttpActionResult> Getoborudovanie(int id)
        {
            oborudovanie oborudovanie = await db.oborudovanies.FindAsync(id);
            if (oborudovanie == null)
            {
                return NotFound();
            }

            return Ok(oborudovanie);
        }

        // PUT: api/oborudovanies/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putoborudovanie(int id, oborudovanie oborudovanie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oborudovanie.Id)
            {
                return BadRequest();
            }

            db.Entry(oborudovanie).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!oborudovanieExists(id))
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

        // POST: api/oborudovanies
        [ResponseType(typeof(oborudovanie))]
        public async Task<IHttpActionResult> Postoborudovanie(oborudovanie oborudovanie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.oborudovanies.Add(oborudovanie);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = oborudovanie.Id }, oborudovanie);
        }

        // DELETE: api/oborudovanies/5
        [ResponseType(typeof(oborudovanie))]
        public async Task<IHttpActionResult> Deleteoborudovanie(int id)
        {
            oborudovanie oborudovanie = await db.oborudovanies.FindAsync(id);
            if (oborudovanie == null)
            {
                return NotFound();
            }

            db.oborudovanies.Remove(oborudovanie);
            await db.SaveChangesAsync();

            return Ok(oborudovanie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool oborudovanieExists(int id)
        {
            return db.oborudovanies.Count(e => e.Id == id) > 0;
        }
    }
}