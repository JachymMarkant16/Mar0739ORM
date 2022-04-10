using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mar0739ORM.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string Adresee { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
