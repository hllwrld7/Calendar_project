using CalendarAPI.DataTypes;

namespace CalendarAPI.Interfaces
{
    public interface ISQLiteService
    {
        public List<Appointment> GetAppointments();
        public void WriteAppointment(Appointment appointment);
        public void RemoveAppointment(Appointment appointment);
    }
}
