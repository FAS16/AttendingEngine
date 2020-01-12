using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendingEngine.Models
{
    public class 
        Weekday
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public short Value { get; set; }

        public static readonly byte Sunday = 0;
        public static readonly byte Monday = 1;
        public static readonly byte Tuesday = 2;
        public static readonly byte Wednesday = 3;
        public static readonly byte Thursday = 4;
        public static readonly byte Friday = 5;
        public static readonly byte Saturday = 6;
    }
}