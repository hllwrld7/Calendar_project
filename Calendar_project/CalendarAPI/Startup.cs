using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

namespace CalendarAPI
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            var schedulingService = app.ApplicationServices.GetService<ISchedulingService>();
            var sqliteService = app.ApplicationServices.GetService<ISQLiteService>();
            ScheduleAppointmentsOnStartup(schedulingService, sqliteService);
            //Process.Start("C:\\Calendar_project\\Calendar_project\\Client\\bin\\Debug\\net6.0-windows\\Client.exe");
        }

        private void ScheduleAppointmentsOnStartup(ISchedulingService schedulingService, ISQLiteService sqliteService)
        {
            var appointments = sqliteService.GetAppointments();
            if (appointments == null) return;
            if (appointments.Count == 0) return;

            foreach (var appointment in appointments)
            {
                if (!IsDateValid(appointment.StartDate))
                    return;
                else
                    ScheduleNotification(appointment, schedulingService);
            }
        }

        private void ScheduleNotification(Appointment appointment, ISchedulingService schedulingService)
        {
            var time = appointment.StartDate.AddMinutes(-15);
            if (time < DateTime.Now)
                time = DateTime.Now.AddMinutes(1);

            schedulingService.ScheduleNotification(appointment, time);
        }

        private bool IsDateValid(DateTime date) 
        {
            return date >= DateTime.Now;
        }
    }
}
