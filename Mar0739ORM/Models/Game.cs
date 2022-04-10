using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mar0739ORM.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Length { get; set; }
        public string State { get; set; }
    }
}
