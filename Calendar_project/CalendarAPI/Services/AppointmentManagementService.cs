using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Common;
using Quartz;
using Quartz.Impl;
using System.Data.SQLite;
using Windows.ApplicationModel.Appointments;

namespace CalendarAPI.Services
{
    public class AppointmentManagementService: IAppointmentManagementService
    {
        public List<DataTypes.Appointment> Appointments => _sqliteService.GetAppointments();
        private readonly ISQLiteService _sqliteService;
        private readonly ISchedulingService _schedulingService;
        private List<DataTypes.Appointment> _appointments;

        public AppointmentManagementService(ISQLiteService sqliteService, ISchedulingService schedulingService)
        {
            _sqliteService = sqliteService;
            _appointments = _sqliteService.GetAppointments();
            _schedulingService = schedulingService;
        }

        public void AddAppointment(IAppointment appointment, out string result)
        {
            result = "Appointment added successfully!";
            if (!IsDateRangeValid(appointment))
            {
                result = "Date is invalid, has a conflict with other appointments";
                return;
            }
            _appointments.Add((DataTypes.Appointment)appointment);
            _sqliteService.WriteAppointment((DataTypes.Appointment)appointment);
            _schedulingService.ScheduleNotification(appointment.Title, appointment.Id, appointment.StartDate.AddMinutes(-15));
        }

        public void EditAppointment(IAppointment appToUpdate, out string result) 
        {
            result = "Appointment edited successfully";
            if (!IsDateRangeValid(appToUpdate, appToUpdate.Id))
            {
                result = "Date is invalid, has a conflict with other appointments";
                return;
            }
            var appToUpdateIndex = _appointments.FindIndex(app => app.Id == appToUpdate.Id);
            if (appToUpdateIndex == -1)
            {
                result = "Error :(";
                return;
            }
            _appointments[appToUpdateIndex] = (DataTypes.Appointment)appToUpdate;
            _schedulingService.UnscheduleNotification(appToUpdate.Id);
            _schedulingService.ScheduleNotification(appToUpdate.Title, appToUpdate.Id, appToUpdate.StartDate.AddMinutes(-15));
        }

        public void DeleteAppointment(IAppointment appointment, out string result)
        {
            result = "Appointment deleted successfully";
            var appToRemoveIndex = _appointments.FindIndex(app => app.Id == appointment.Id);
            if(appToRemoveIndex == -1)
            {
                result = "Error :(";
                return;
            }
            _appointments.RemoveAt(appToRemoveIndex);
            _sqliteService.RemoveAppointment((DataTypes.Appointment)appointment);
            _schedulingService.UnscheduleNotification(appointment.Id);
        }

        private bool IsDateRangeValid(IAppointment appointment, int idToExclude = -1)
        {
            foreach(var existingApp in _appointments) 
            {
                if (existingApp.Id == idToExclude)
                    continue;
                if (existingApp.IsAllDay || appointment.IsAllDay)
                {
                    if (appointment.StartDate.DayOfYear == existingApp.StartDate.DayOfYear)
                        return false;
                    continue;
                }
                if (appointment.StartDate <= existingApp.StartDate && appointment.StartDate >= existingApp.EndDate 
                    || appointment.EndDate <= existingApp.StartDate && appointment.EndDate >= existingApp.EndDate)
                    return false;
            }
            return true;
        }
    }
}
