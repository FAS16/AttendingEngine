
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using AttendingEngine.Managers;
using AttendingEngine.Models;

namespace AttendingEngine.Controllers
{
    [RoutePrefix("api/Attendances")]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Attendances
//        [Authorize(Roles = "Admin, Teacher")]
        public IQueryable<Attendance> GetAttendances()
        {
            return db.Attendances;
        }

        [Route("{email}")]
        public async Task<IHttpActionResult> GetTestAttendance(string email)
        {
            var userEmail = User.Identity.Name;

//            if (userEmail != email)
//            {
//                return Unauthorized();
//            }

            var student = await db.Students.SingleOrDefaultAsync(a => a.Email == email);

            if (student == null)
            {
                NotFound();
            }

            var attendances = await db.Attendances
                .Include(a => a.Lesson.Course)
                .Where(a => a.StudentId == student.Id)
                .ToListAsync();

            return Ok(attendances);
        }

        // GET: api/Attendances/5
        [ResponseType(typeof(Attendance))]
        [Authorize]
        public async Task<IHttpActionResult> GetAttendance(int id)
        {
            Attendance attendance = await db.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        // PUT: api/Attendances/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IHttpActionResult> PutAttendance(int id, Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendance.Id)
            {
                return BadRequest();
            }

            db.Entry(attendance).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
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


        // POST: api/Attendances
        [ResponseType(typeof(Attendance))]
        [Authorize]
        public async Task<IHttpActionResult> PostAttendance(Attendance attendance)
        {

            const string tag = "POST: api/Attendances";

            Trace.WriteLine(tag);
            //TODO: UNCOMMENT THIS
//            if (!Request.IsIpAllowed())
//            {
//                Trace.WriteLine(tag+": Request IP not allowed: " + Request.GetOwinContext().Request.RemoteIpAddress);
//
//                return Content(HttpStatusCode.Forbidden, "Du skal være på skolens netværk!");
//            }

//            Trace.WriteLine(tag + ": Request IP permission granted: " + Request.GetOwinContext().Request.RemoteIpAddress);


            if (!ModelState.IsValid)
            {
                Trace.WriteLine(tag + ": ModelState is invalid!");

                return BadRequest(ModelState);
            }
            Trace.WriteLine(tag + ": ModelState was valid!");

            var userEmail = User.Identity.Name;
            var student = db.Students.SingleOrDefault(s => s.Id == attendance.StudentId);

            if (student == null)
            {
                Trace.WriteLine(tag + ": No such student with id = " + attendance.StudentId);

                return NotFound();

            }

            Trace.WriteLine(tag + ": Student was found!");


            if (student.Email != userEmail)
            {
                Trace.WriteLine(tag + ": Unauthorized, student id sent with request does not correspond to signed in user");
                return Unauthorized();
            }

            Trace.WriteLine(tag + ": Authorized!");

            var lesson = await db.Lessons.Include(l => l.Weekday).SingleOrDefaultAsync(l => l.Id == attendance.LessonId);
            if (lesson == null)
            {
                return BadRequest("Ingen lektion med dette id!");
            }

            //TODO: UNCOMMENT THIS
//            if (lesson.Weekday.Value != (int) DateTime.Now.DayOfWeek)
//            {
//
//                return BadRequest("Denne lektion finder ikke sted i dag!");
//
//            }

            // Fraud detection
            var atts = await db.Attendances.ToListAsync();
            var fraudulentAttendances = atts
                .Where(a => a.StudentId != attendance.StudentId)
                .Where(a => a.DeviceId == attendance.DeviceId)
                .Where(a => a.LessonId == attendance.LessonId)
                .Where(a => a.Timestamp.Date == DateTime.Now.Date)
                .ToList();

            if (fraudulentAttendances.Any())
            { 
                Trace.WriteLine(tag + ": Fraud detected: Found attendance record with different student ids recorded from same device");
                fraudulentAttendances.ForEach(a => a.PossibleFraud = true);
                var fraudAttendance = attendance;
                fraudAttendance.PossibleFraud = true;

                db.Attendances.Add(fraudAttendance);
                await db.SaveChangesAsync();

                return BadRequest("Snyd! Der er kommet forskellige check ind, fra forskellige elever, fra samme telefon");

            }

            var attendances = await db.Attendances.ToListAsync();
            var similarAttendances = attendances
                .Where(a => a.LessonId == attendance.LessonId)
                .Where(a => a.StudentId == attendance.StudentId)
                .Where(a => a.Timestamp.Date == attendance.Timestamp.Date)
                .Where(a => a.TagId == attendance.TagId)
                .Where(a => a.DeviceId == attendance.DeviceId).ToList();

            Trace.WriteLine(tag + ": similarAttendances.Count = " + similarAttendances.Count);

            if (attendance.CheckType == CheckTypes.CheckOut)
            {
                if (similarAttendances.Count == 0 )
                {
                    return BadRequest("Ingen check ind, derfor kan du ikke check ud!");
                }
            }

            if (similarAttendances.Count == 1  /*&& similarAttendances[0].CheckType == CheckTypes.CheckIn*/)
            {
                var start = lesson.StartTime.Subtract(TimeSpan.FromMinutes(30));
                var end = lesson.EndTime.Add(TimeSpan.FromMinutes(15));
                //TODO: UNCOMMENT THIS
//                if (!ValidateTime(start, end))
//                {
//                    return BadRequest("Du kan ikke foretage et check ud, da lektionen ikke er aktuel!");
//
//                }
                Trace.WriteLine(tag + ": Similar attendance record found with id =" + similarAttendances[0].Id + ". Recording as check out..");
                var checkOutAttendance = attendance;
                checkOutAttendance.CheckType = CheckTypes.CheckOut;
                db.Attendances.Add(checkOutAttendance);
                await db.SaveChangesAsync();


                return CreatedAtRoute("DefaultApi", new { id = checkOutAttendance.Id }, checkOutAttendance);
            }

            if (similarAttendances.Count == 2 && similarAttendances[0].CheckType != similarAttendances[1].CheckType)
            {
                var id1 = similarAttendances[0].Id;
                var id2 = similarAttendances[1].Id;
                Trace.WriteLine(tag + ": Found 2 Similar attendance records with different check types, id1 =" + id1 + " and id2 = "+ id2 +". Has the student already checked in and out?");

                return Content(HttpStatusCode.Forbidden, "Du har allerede checket ind og ud!");

            }

            var startForCheckIn = lesson.StartTime.Subtract(TimeSpan.FromMinutes(30));
            var endForCheckIn = lesson.EndTime;

            //TODO: UNCOMMENT THIS
//            if (!ValidateTime(startForCheckIn, endForCheckIn))
//            {
//                return BadRequest("Du kan ikke foretage et check ind, da lektionen ikke er aktuel!");
//
//            }

            db.Attendances.Add(attendance);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = attendance.Id }, attendance);
        }


        // DELETE: api/Attendances/5
        [ResponseType(typeof(Attendance))] 
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IHttpActionResult> DeleteAttendance(int id)
        {
            Attendance attendance = await db.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            db.Attendances.Remove(attendance);
            await db.SaveChangesAsync();

            return Ok(attendance);
        }

        private bool ValidateTime(TimeSpan start, TimeSpan end)
        {
            var now = DateTime.Now.TimeOfDay;
            return (now >= start) && (now <= end);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendanceExists(int id)
        {
            return db.Attendances.Count(e => e.Id == id) > 0;
        }




    }
}