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
        [Route("GetprojectFilesbyid")]
        public IQueryable<ProjectFile> GetProjectFiles(int id)
        {
            return db.ProjectFiles.Where(c=>c.ProjectId == id);
        }

        [Route("GetprFiles")]
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
        [Route("SendProjectFiles")]
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
                        var file = new ProjectFile();
                        file.Name = cont.Headers.ContentDisposition.Name;
                        file.ProjectName = cont.Headers.ContentDisposition.FileName;
                        file.ProjectId = (int)cont.Headers.ContentDisposition.Size;
                        file.ProjectFolder = file.ProjectName + "-" + file.ProjectId;
                        file.Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + file.ProjectFolder;
                        db.ProjectFiles.Add(file);
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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectFileExists(int id)
        {
            return db.ProjectFiles.Count(e => e.Id == id) > 0;
        }
    }
}