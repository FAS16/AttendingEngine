using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendingEngine.Models
{
    public class Attendance
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(255)]
        public string DeviceId { get; set; }

       

        [Required]
        public Student Student { get; set; }



        [Required]
        public CheckTypes CheckType { get; set; }

        [Required]
        public string TagId { get; set; }



        [Required]
        public bool PossibleFraud { get; set; }
    }
}