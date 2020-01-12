using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendingEngine.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Title { get; set; }


        public List<Course> Courses { get; set; }
        public List<Student> Students { get; set; }
    }
}