using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AttendingEngine.Models
{
    public class Room
    {
        public int Id { get; set; }

        [StringLength(14)]
        [Index(IsUnique = true)]
        [Required]
        public string RoomIdentifier { get; set; }
    }
}