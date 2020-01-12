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

    [RoutePrefix("api/classes")]
    public class ClassesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public AttendancesController AttendancesController
        {
            get => default;
            set
            {
            }
        }

        // GET: api/Classes
        public IQueryable<Class> GetClasses()
        {
            Trace.WriteLine("GET: api/Classes");

            return db.Classes;
        }

        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        public async Task<IHttpActionResult> GetClass(int id)
        {
            Trace.WriteLine("GET: api/Classes/" + id);

            Class @class = await db.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }



        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        [Route("{id}/students")]
        public async Task<IHttpActionResult> GetClassStudents(int id)
        {
            Trace.WriteLine("GET: api/Classes/" + id);

            var students = await db.Students.Where(s => s.Class.Id == id).ToListAsync();

            return Ok(students);

        }

        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        [Route("{id}/courses")]
        public async Task<IHttpActionResult> GetClassCourses(int id)
        {
            Trace.WriteLine("GET: api/Classes/" + id+"/courses");

            var courses = await db.Classes.Where(c => c.Id == id).SelectMany(c => c.Courses).Include(c => c.Teacher).ToListAsync();

            return Ok(courses);

        }

        [ResponseType(typeof(Class))]
        [Route("{classId}/courses/{courseId}/attendances")]
        public async Task<IHttpActionResult> GetClassAttendances(int classId, int courseId)
        {
            Trace.WriteLine("GET: api/Classes/" + classId + "/courses/"+courseId+"/attendances");

            var attendances = await db.Attendances.Include(a => a.Student).Include(a => a.Lesson.Course).Include(a => a.Lesson.Weekday).Include(a => a.Lesson.ClassRoom).Where(a => a.Student.Class.Id == classId).ToListAsync();
            var aa = attendances.Where(a => a.Lesson.Course.Id == courseId).ToList();


            return Ok(aa);

        }

        // PUT: api/Classes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClass(int id, Class @class)
        {
            Trace.WriteLine("PUT: api/Classes/"+id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @class.Id)
            {
                return BadRequest();
            }

            db.Entry(@class).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/Classes
        [ResponseType(typeof(Class))]
        public async Task<IHttpActionResult> PostClass(Class @class)
        {
            Trace.WriteLine("POST: api/Classes");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Classes.Add(@class);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = @class.Id }, @class);
        }

        // DELETE: api/Classes/5
        [ResponseType(typeof(Class))]
        public async Task<IHttpActionResult> DeleteClass(int id)
        {
            Trace.WriteLine("DELETE: api/Classes/" + id);

            Class @class = await db.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            db.Classes.Remove(@class);
            await db.SaveChangesAsync();

            return Ok(@class);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassExists(int id)
        {
            return db.Classes.Count(e => e.Id == id) > 0;
        }
    }
}