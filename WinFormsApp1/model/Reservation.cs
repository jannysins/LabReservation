using System;
using System.Collections.Generic;
using System.Text;

namespace LabReservation.model
{
    public class Reservation
    {
        public string LabRoom { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Name { get; set; }
    }
}