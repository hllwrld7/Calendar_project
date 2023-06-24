using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Appointment : IAppointment
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public string Location { get; set; }
        public bool IsLocationOnline { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Appointment(int id, string title, string description, DateTime startDate, DateTime endDate, bool isAllDay, string location, bool isLocationOnline)
        {
            Id = id;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            IsAllDay = isAllDay;
            Location = location;
            IsLocationOnline = isLocationOnline;
        }

        public Appointment()
        {

        }
    }
}
