using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsDemo.Data.Entities
{
    public enum CarriageType { Sedentary=1, FirstClassSleeping=2, SecondClassSleeping=3 }

    public class Carriage
    {
        public int Id { get; set; }
        public CarriageType Type { get; set; }
        public decimal DefaultPrice { get; set; }
        public List<Place> Places { get; set; }
        public int TrainId { get; set; }
        public Train Train { get; set; }

        public int Number { get; set; }

        public override string ToString()
        {
            string returningStr = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", Id, Type, DefaultPrice, Places, TrainId, Number);

            return returningStr;
        }
    }
}
