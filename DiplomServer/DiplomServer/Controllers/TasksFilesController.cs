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
    public class TasksFilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TasksFiles
        public IQueryable<TasksFile> GetTasksFiles()
        {
            return db.TasksFiles;
        }

        // GET: api/TasksFiles/5
        [ResponseType(typeof(TasksFile))]
        public async Task<IHttpActionResult> GetTasksFile(int id)
        {
            TasksFile tasksFile = await db.TasksFiles.FindAsync(id);
            if (tasksFile == null)
            {
                return NotFound();
            }

            return Ok(tasksFile);
        }

        // PUT: api/TasksFiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTasksFile(int id, TasksFile tasksFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tasksFile.Id)
            {
                return BadRequest();
            }

            db.Entry(tasksFile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksFileExists(id))
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

        // POST: api/TasksFiles
        [ResponseType(typeof(TasksFile))]
        public async Task<IHttpActionResult> PostTasksFile(TasksFile tasksFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TasksFiles.Add(tasksFile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tasksFile.Id }, tasksFile);
        }

        // DELETE: api/TasksFiles/5
        [ResponseType(typeof(TasksFile))]
        public async Task<IHttpActionResult> DeleteTasksFile(int id)
        {
            TasksFile tasksFile = await db.TasksFiles.FindAsync(id);
            if (tasksFile == null)
            {
                return NotFound();
            }

            db.TasksFiles.Remove(tasksFile);
            await db.SaveChangesAsync();

            return Ok(tasksFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TasksFileExists(int id)
        {
            return db.TasksFiles.Count(e => e.Id == id) > 0;
        }
    }
}