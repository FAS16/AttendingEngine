using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendingEngine.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Weekday Weekday { get; set; }

        public Room ClassRoom { get; set; }

        public Course Course { get; set; }

    }
}