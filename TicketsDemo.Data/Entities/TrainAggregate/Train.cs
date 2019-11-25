using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsDemo.Data.Entities
{
    public class Train
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public List<Carriage> Carriages { get; set; }
        public BookingCompany Booking { get; set; }
        public int BookingId { get; set; }

        public override string ToString()
        {
            string returningStr = string.Format("{0}, {1}, \"{2}\", \"{3}\", {4}", Id, Number, StartLocation, EndLocation, BookingId);

            return returningStr;
        }
    }
}
