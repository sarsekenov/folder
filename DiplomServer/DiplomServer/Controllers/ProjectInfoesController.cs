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
    public class ProjectInfoesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProjectInfoes
        public IQueryable<ProjectInfo> GetProjectInfoes()
        {
            return db.ProjectInfoes;
        }

        // GET: api/ProjectInfoes/5
        [ResponseType(typeof(ProjectInfo))]
        public async Task<IHttpActionResult> GetProjectInfo(int id)
        {
            ProjectInfo projectInfo = await db.ProjectInfoes.FindAsync(id);
            if (projectInfo == null)
            {
                return NotFound();
            }

            return Ok(projectInfo);
        }

        // PUT: api/ProjectInfoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProjectInfo(int id, ProjectInfo projectInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectInfo.Id)
            {
                return BadRequest();
            }

            db.Entry(projectInfo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectInfoExists(id))
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

        // POST: api/ProjectInfoes
        [ResponseType(typeof(ProjectInfo))]
        public async Task<IHttpActionResult> PostProjectInfo(ProjectInfo projectInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProjectInfoes.Add(projectInfo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = projectInfo.Id }, projectInfo);
        }

        // DELETE: api/ProjectInfoes/5
        [ResponseType(typeof(ProjectInfo))]
        public async Task<IHttpActionResult> DeleteProjectInfo(int id)
        {
            ProjectInfo projectInfo = await db.ProjectInfoes.FindAsync(id);
            if (projectInfo == null)
            {
                return NotFound();
            }

            db.ProjectInfoes.Remove(projectInfo);
            await db.SaveChangesAsync();

            return Ok(projectInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectInfoExists(int id)
        {
            return db.ProjectInfoes.Count(e => e.Id == id) > 0;
        }
    }
}