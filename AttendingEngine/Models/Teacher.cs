using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendingEngine.Models
{
    public class Teacher
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        [StringLength(200)]
        [Required]
        public string Email { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}