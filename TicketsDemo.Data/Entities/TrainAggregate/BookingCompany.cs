using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsDemo.Data.Entities
{
    public class BookingCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float OverPrice { get; set; }
        public override string ToString()
        {
            string returningStr = string.Format("{0}, \"{1}\", {2}, {3}, ", Id, Name, OverPrice);

            return returningStr;
        }
    }
}
