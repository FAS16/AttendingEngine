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
    public class LessonsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Lessons
        public IQueryable<Lesson> GetLessons()
        {
            Trace.WriteLine("GET: api/Lessons");

            return db.Lessons;
        }

        // GET: api/Lessons/5
        [ResponseType(typeof(Lesson))]
        public async Task<IHttpActionResult> GetLesson(int id)
        {
            Trace.WriteLine("GET: api/Lessons/"+id);


            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

        // PUT: api/Lessons/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLesson(int id, Lesson lesson)
        {
            Trace.WriteLine("PUT: api/Lessons/"+id);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lesson.Id)
            {
                return BadRequest();
            }

            db.Entry(lesson).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(id))
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

        // POST: api/Lessons
        [ResponseType(typeof(Lesson))]
        public async Task<IHttpActionResult> PostLesson(Lesson lesson)
        {
            Trace.WriteLine("POST: api/Lessons");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lessons.Add(lesson);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = lesson.Id }, lesson);
        }

        // DELETE: api/Lessons/5
        [ResponseType(typeof(Lesson))]
        public async Task<IHttpActionResult> DeleteLesson(int id)
        {
            Trace.WriteLine("DELETE: api/Lessons/" + id);

            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            db.Lessons.Remove(lesson);
            await db.SaveChangesAsync();

            return Ok(lesson);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LessonExists(int id)
        {
            return db.Lessons.Count(e => e.Id == id) > 0;
        }
    }
}