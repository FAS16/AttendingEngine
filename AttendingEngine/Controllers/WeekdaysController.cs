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
    public class WeekdaysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Weekdays
        public IQueryable<Weekday> GetWeekdays()
        {
            Trace.WriteLine("GET: api/Weekdays");

            return db.Weekdays;
        }

        // GET: api/Weekdays/5
        [ResponseType(typeof(Weekday))]
        public async Task<IHttpActionResult> GetWeekday(int id)
        {
            Trace.WriteLine("GET: api/Weekdays/" + id);

            Weekday weekday = await db.Weekdays.FindAsync(id);
            if (weekday == null)
            {
                return NotFound();
            }

            return Ok(weekday);
        }

        // PUT: api/Weekdays/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWeekday(int id, Weekday weekday)
        {
            Trace.WriteLine("PUT: api/Weekdays/"+id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weekday.Id)
            {
                return BadRequest();
            }

            db.Entry(weekday).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeekdayExists(id))
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

        // POST: api/Weekdays
        [ResponseType(typeof(Weekday))]
        public async Task<IHttpActionResult> PostWeekday(Weekday weekday)
        {
            Trace.WriteLine("POST: api/Weekdays");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Weekdays.Add(weekday);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = weekday.Id }, weekday);
        }

        // DELETE: api/Weekdays/5
        [ResponseType(typeof(Weekday))]
        public async Task<IHttpActionResult> DeleteWeekday(int id)
        {
            Trace.WriteLine("DELETE: api/Weekdays/" + id);

            Weekday weekday = await db.Weekdays.FindAsync(id);
            if (weekday == null)
            {
                return NotFound();
            }

            db.Weekdays.Remove(weekday);
            await db.SaveChangesAsync();

            return Ok(weekday);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WeekdayExists(int id)
        {
            return db.Weekdays.Count(e => e.Id == id) > 0;
        }
    }
}