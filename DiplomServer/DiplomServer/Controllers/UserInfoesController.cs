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
    public class UserInfoesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserInfoes
        public IQueryable<UserInfo> GetUserInfoes()
        {
            string user = User.Identity.GetUserId();
            return db.UserInfoes.Where(c=>c.UserId == user);
        }

        [Route("getinfobyid")]
        public IQueryable<UserInfo> getinfobyid(string id)
        {
            return db.UserInfoes.Where(c => c.UserId == id);
        }
        [Route("getuserbyspec")]
        public IQueryable<UserInfo> GetUserInfoesbyspec(string part)
        {
            return db.UserInfoes.Where(c=>c.AboutMe.Contains(part));
        }
        // GET: api/UserInfoes/5
        [ResponseType(typeof(UserInfo))]
        public async Task<IHttpActionResult> GetUserInfo(int id)
        {
            UserInfo userInfo = await db.UserInfoes.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return Ok(userInfo);
        }

        // PUT: api/UserInfoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserInfo(int id, UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userInfo.Id)
            {
                return BadRequest();
            }

            db.Entry(userInfo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(id))
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

        // POST: api/UserInfoes
        [ResponseType(typeof(UserInfo))]
        public async Task<IHttpActionResult> PostUserInfo(UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserInfoes.Add(userInfo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userInfo.Id }, userInfo);
        }

        // DELETE: api/UserInfoes/5
        [ResponseType(typeof(UserInfo))]
        public async Task<IHttpActionResult> DeleteUserInfo(int id)
        {
            UserInfo userInfo = await db.UserInfoes.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            db.UserInfoes.Remove(userInfo);
            await db.SaveChangesAsync();

            return Ok(userInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserInfoExists(int id)
        {
            return db.UserInfoes.Count(e => e.Id == id) > 0;
        }
    }
}