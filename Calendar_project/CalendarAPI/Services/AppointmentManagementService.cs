using CalendarAPI.Interfaces;

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

        public void AddAppointment(DataTypes.Appointment appointment, out string result)
        {
            result = "Appointment added successfully!";
            if (!IsDateRangeValid(appointment))
            {
                result = "Date is invalid.";
                return;
            }
            _appointments.Add((DataTypes.Appointment)appointment);
            _sqliteService.WriteAppointment((DataTypes.Appointment)appointment);
            ScheduleNotification(appointment);
            _schedulingService.ScheduleNotification(appointment, appointment.StartDate.AddMinutes(-15));
        }

        public void EditAppointment(DataTypes.Appointment appToUpdate, out string result) 
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
            _sqliteService.UpdateAppointment(appToUpdate);
            _schedulingService.UnscheduleNotification(appToUpdate.Id);
            _schedulingService.ScheduleNotification(appToUpdate, appToUpdate.StartDate.AddMinutes(-15));
        }

        public void DeleteAppointment(DataTypes.Appointment appointment, out string result)
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

        private bool IsDateRangeValid(DataTypes.Appointment appointment, int idToExclude = -1)
        {
            if (appointment.StartDate < DateTime.Now || (appointment.StartDate < DateTime.Now && !appointment.IsAllDay))
                return false;
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

        private void ScheduleNotification(DataTypes.Appointment appointment)
        {
            var time = appointment.StartDate.AddMinutes(-15);
            if(time < DateTime.Now)
                time = DateTime.Now.AddMinutes(1);

            _schedulingService.ScheduleNotification(appointment, time);
        }
    }
}
