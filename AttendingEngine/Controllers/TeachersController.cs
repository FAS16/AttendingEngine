using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AttendingEngine.Models;
using Microsoft.Owin.Logging;

namespace AttendingEngine.Controllers
{
    public class TeachersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        // GET: api/Teachers
        public IQueryable<Teacher> GetTeachers()
        {
            Trace.WriteLine("GET api/Teachers");

            return db.Teachers;
        }

        // GET: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public async Task<IHttpActionResult> GetTeacher(int id)
        {
            Trace.WriteLine("GET api/Teachers/"+id);

            Teacher teacher = await db.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        // PUT: api/Teachers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeacher(int id, Teacher teacher)
        {
            Trace.WriteLine("PUT api/Teachers/" + id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacher.Id)
            {
                return BadRequest();
            }

            db.Entry(teacher).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // POST: api/Teachers
        [ResponseType(typeof(Teacher))]
        public async Task<IHttpActionResult> PostTeacher(Teacher teacher)
        {
            Trace.WriteLine("POST api/Teachers/");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(teacher);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = teacher.Id }, teacher);
        }

        // DELETE: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public async Task<IHttpActionResult> DeleteTeacher(int id)
        {
            Trace.WriteLine("DELETE api/Teachers/" + id);

            Teacher teacher = await db.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(teacher);
            await db.SaveChangesAsync();

            return Ok(teacher);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.Id == id) > 0;
        }
    }
}