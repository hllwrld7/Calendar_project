using CalendarAPI.DataTypes;
using Common;

namespace CalendarAPI.Interfaces
{
    public interface IAppointmentManagementService
    {
        public List<Appointment> Appointments { get; }
        public void AddAppointment(IAppointment appointment, out string result);
        public void EditAppointment(IAppointment appToUpdate, out string result);
        public void DeleteAppointment(IAppointment appointment, out string result);
    }
}
