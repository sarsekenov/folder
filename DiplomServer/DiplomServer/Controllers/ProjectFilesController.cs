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
    [Authorize]
    public class ProjectFilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProjectFiles
        public IQueryable<ProjectFile> GetProjectFiles()
        {
            return db.ProjectFiles;
        }

        // GET: api/ProjectFiles/5
        [ResponseType(typeof(ProjectFile))]
        public async Task<IHttpActionResult> GetProjectFile(int id)
        {
            ProjectFile projectFile = await db.ProjectFiles.FindAsync(id);
            if (projectFile == null)
            {
                return NotFound();
            }

            return Ok(projectFile);
        }

        // PUT: api/ProjectFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProjectFile(int id, ProjectFile projectFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectFile.Id)
            {
                return BadRequest();
            }

            db.Entry(projectFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectFileExists(id))
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

        // POST: api/ProjectFiles
        [ResponseType(typeof(ProjectFile))]
        public async Task<IHttpActionResult> PostProjectFile(ProjectFile projectFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProjectFiles.Add(projectFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = projectFile.Id }, projectFile);
        }

        // DELETE: api/ProjectFiles/5
        [ResponseType(typeof(ProjectFile))]
        public async Task<IHttpActionResult> DeleteProjectFile(int id)
        {
            ProjectFile projectFile = await db.ProjectFiles.FindAsync(id);
            if (projectFile == null)
            {
                return NotFound();
            }

            db.ProjectFiles.Remove(projectFile);
            await db.SaveChangesAsync();

            return Ok(projectFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectFileExists(int id)
        {
            return db.ProjectFiles.Count(e => e.Id == id) > 0;
        }
    }
}