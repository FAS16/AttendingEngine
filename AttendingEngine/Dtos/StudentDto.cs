using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using AttendingEngine.Models;

namespace AttendingEngine.Dtos
{
    public class StudentDto
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Required]
        public string Email { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public ClassDto Class { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<Attendance> Attendances { get; set; }

    }

    public class ClassDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<StudentCourseDto> Courses { get; set; }

    }

    public class StudentCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<LessonDto> Lessons { get; set; }

        public TeacherDto Teacher { get; set; }

    }

    public class LessonDto
    {

        public int Id { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public TimeSpan TestTime { get; set; }

        public Weekday Weekday { get; set; }

        public Room ClassRoom { get; set; }



    }

    public class TeacherDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        [StringLength(200)]
        [Required]
        public string Email { get; set; }


    }
}