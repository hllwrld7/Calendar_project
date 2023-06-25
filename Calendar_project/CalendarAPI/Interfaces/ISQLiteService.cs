using CalendarAPI.DataTypes;

namespace CalendarAPI.Interfaces
{
    public interface ISQLiteService
    {
        public List<Appointment> GetAppointments();
        public void WriteAppointment(Appointment appointment);
        public void RemoveAppointment(Appointment appointment);
        public void UpdateAppointment(Appointment appointment);
        public List<Contact> GetContacts();
        public void WriteContact(Contact contact);
        public void RemoveContact(Contact contact);
        public void UpdateContact(Contact contact);

    }
}
