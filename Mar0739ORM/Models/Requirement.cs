using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mar0739ORM.Models
{
    public class Requirement
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }
        public Reservation Reservation { get; set; }
        public Stock Stock { get; set; }
    }
}
