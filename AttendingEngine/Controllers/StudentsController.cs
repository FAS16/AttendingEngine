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
using AttendingEngine.Dtos;
using AttendingEngine.Models;
using AutoMapper;

namespace AttendingEngine.Controllers
{
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Students
        public IQueryable<Student> GetStudents()
        {
            Trace.WriteLine("GET api/Students/");

            return db.Students;
        }

        // GET: api/Students/{email}/
        [Authorize]
        [ResponseType(typeof(Student))]
        [Route("{email}")]
        public async Task<IHttpActionResult> GetStudentData(string email)
        {
            var userEmail = User.Identity.Name;

            if (userEmail != email)
            {
                return Unauthorized();
            }

            var student = await db.Students
                .Include(s => s.Class.Courses.Select(c => c.Lessons.Select(l => l.ClassRoom)))
                .Include(s => s.Class.Courses.Select(c => c.Lessons.Select(l => l.Weekday)))
                .Include(s => s.Class.Courses.Select(c => c.Lessons))
                .Include(s => s.Class.Courses.Select(c => c.Teacher))
                .Include(s => s.Attendances)
                .SingleOrDefaultAsync(s => s.Email == userEmail);

            var dto = Mapper.Map<Student, StudentDto>(student);

            return Ok(dto);
        }

        //        // GET: api/Students/5
        //        [ResponseType(typeof(Student))]
        //        public async Task<IHttpActionResult> GetStudent(int id)
        //        {
        //            Trace.WriteLine("GET api/Students/"+id);
        //
        //            Student student = await db.Students.FindAsync(id);
        //            if (student == null)
        //            {
        //                return NotFound();
        //            }
        //
        //            return Ok(student);
        //        }

//        // GET: api/Students/5
//        [ResponseType(typeof(Student))]
//        [Route("get/{id}")]
//
//        public async Task<IHttpActionResult> RetrieveStudent(int id, [FromUri]int? classId)
//        {
//            Trace.WriteLine("GET api/Students/"+id);
//            Student student = null;
//            if (classId != 0)
//            {
//                 student = await db.Students.Where(s => s.Class.Id == classId).SingleOrDefaultAsync();
//
//                 if (student == null)
//                 {
//                     return NotFound();
//                 }
//
//                 return Ok();
//
//            }
//        
//            student = await db.Students.FindAsync(id);
//            if (student == null)
//            {
//                return NotFound();
//            }
//        
//            return Ok(student);
//        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudent(int id, Student student)
        {
            Trace.WriteLine("PUT api/Students/" + id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> PostStudent(Student student)
        {
            Trace.WriteLine("POST api/Students/");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteStudent(int id)
        {
            Trace.WriteLine("DELETE api/Students/" + id);

            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.Id == id) > 0;
        }
    }
}