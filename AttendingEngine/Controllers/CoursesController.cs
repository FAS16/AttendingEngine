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

namespace AttendingEngine.Controllers
{
    public class CoursesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Courses
        [Authorize]
        public List<Course> GetCourses()
        {
            Trace.WriteLine("GET: api/Courses");

            var courses = db.Courses.Include(c => c.Classes).Include(c => c.Teacher).Include(c => c.Lessons).ToList();
 
            return courses;
        }

        // GET: api/Courses/5
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> GetCourse(int id)
        {
            Trace.WriteLine("GET: api/Courses/"+id);

            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourse(int id, Course course)
        {
            Trace.WriteLine("PUT: api/Courses/"+id);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.Id)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> PostCourse(Course course)
        {
            Trace.WriteLine("POST: api/Courses/");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> DeleteCourse(int id)
        {
            Trace.WriteLine("DELETE: api/Courses/"+id);

            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            await db.SaveChangesAsync();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.Id == id) > 0;
        }
    }
}