using CalendarAPI.DataTypes;

namespace CalendarAPI.Interfaces
{
    public interface IAppointmentManagementService
    {
        public List<Appointment> Appointments { get; }
        public void AddAppointment(Appointment appointment, out string result);
        public void EditAppointment(Appointment appToUpdate, out string result);
        public void DeleteAppointment(Appointment appointment, out string result);
    }
}
