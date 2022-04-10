using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mar0739ORM.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
