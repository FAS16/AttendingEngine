using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendingEngine.Models
{
    public class Course
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<Lesson> Lessons { get; set; }

        public List<Class> Classes { get; set; }

        public Teacher Teacher { get; set; }
    }
}