using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DiplomServer.Models;

namespace DiplomServer.Controllers
{[Authorize]
    public class TasksFilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TasksFiles
        public IQueryable<TasksFile> GetTasksFiles()
        {
            return db.TasksFiles;
        }
        [Route("GetTaskFilesbyid")]
        public IQueryable<TasksFile> GetTasksFiles(int id)
        {
            return db.TasksFiles.Where(c=>c.TaskId == id);
        }
        [Route("GettaskFiles")]
        [HttpGet]
        public async Task<byte[]> GetFiles(string path)
        {
            if (File.Exists(path))
            {
                var bites = File.ReadAllBytes(path);
                return bites;
            }

            else
            {
                var ret = "Error".ToCharArray();
                var retbytes = new byte[5];
                int n = 0;
                foreach (var bite in ret)
                {
                    retbytes[n++] = (byte)bite;

                }
                return retbytes;
            }
        }
        [Route("SendTaskFile")]
        public async Task<IHttpActionResult> Sendfiles()
        {
            MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents != null && provider.Contents.Count > 0)
            {
                foreach (var cont in provider.Contents)
                {
                    if (!string.IsNullOrEmpty(cont.Headers.ContentDisposition.FileName))
                    {
                        var file = new TasksFile();
                        file.Name = cont.Headers.ContentDisposition.Name;
                        file.TaskName= cont.Headers.ContentDisposition.FileName;
                        file.TaskId = (int)cont.Headers.ContentDisposition.Size;
                        file.ProjectFolder = cont.Headers.ContentDisposition.FileNameStar;
                        file.senderName = User.Identity.Name;
                        file.Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + file.ProjectFolder;
                        db.TasksFiles.Add(file);
                        if (!Directory.Exists(file.Path))
                        {
                            Directory.CreateDirectory(file.Path);
                        }
                        var bites = await cont.ReadAsByteArrayAsync();
                        if (bites != null && bites.Length > 0)
                        {
                            using (var streamMemory = new MemoryStream(bites))
                            {
                                streamMemory.Seek(0, SeekOrigin.Begin);
                                using (var fileStream = File.Create(file.Path + "/" + file.Name))
                                {
                                    streamMemory.CopyTo(fileStream);
                                }
                            }
                        }
                    }
                }
            }
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = dirDev.Id }, dirDev);
            return Ok();

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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TasksFileExists(int id)
        {
            return db.TasksFiles.Count(e => e.Id == id) > 0;
        }
    }
}