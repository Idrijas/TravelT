using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelt
{
    public class Trip
    {

        public int TripId { get; set; }

        public string NameOfTrip { get; set; }

        public string Country { get; set;  }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public int NumberOfPeople { get; set; }

        public string Description { get; set; }


        public Trip() { }


    }
}
