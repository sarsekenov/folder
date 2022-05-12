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
    public class ProjectMembersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProjectMembers
        public IQueryable<ProjectMember> GetProjectMembers()
        {
            string user = User.Identity.GetUserId();
            return db.ProjectMembers.Where(c => c.UserID == user);
        }
        [HttpGet]
        [Route("GetbyId")]
        public IEnumerable<ProjectMember> GetbyId(int Id)
        {
            return db.ProjectMembers.Where(c => c.ProjectID == Id);
        }

        // GET: api/ProjectMembers/5
        [ResponseType(typeof(ProjectMember))]
        public async Task<IHttpActionResult> GetProjectMember(int id)
        {
            ProjectMember projectMember = await db.ProjectMembers.FindAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return Ok(projectMember);
        }

        // PUT: api/ProjectMembers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProjectMember(int id, ProjectMember projectMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectMember.Id)
            {
                return BadRequest();
            }

            db.Entry(projectMember).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectMemberExists(id))
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

        // POST: api/ProjectMembers
        [ResponseType(typeof(ProjectMember))]
        public async Task<IHttpActionResult> PostProjectMember(ProjectMember projectMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProjectMembers.Add(projectMember);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = projectMember.Id }, projectMember);
        }

        // DELETE: api/ProjectMembers/5
        [ResponseType(typeof(ProjectMember))]
        public async Task<IHttpActionResult> DeleteProjectMember(int id)
        {
            ProjectMember projectMember = await db.ProjectMembers.FindAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }

            db.ProjectMembers.Remove(projectMember);
            await db.SaveChangesAsync();

            return Ok(projectMember);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectMemberExists(int id)
        {
            return db.ProjectMembers.Count(e => e.Id == id) > 0;
        }
    }
}