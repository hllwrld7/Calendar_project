using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Metadata;
using Windows.ApplicationModel.Appointments;

namespace CalendarAPI.Services
{
    public class SQLiteService: ISQLiteService
    {
        private SqliteContext _db;

        public SQLiteService() 
        {
            _db = new SqliteContext();
            _db.Database.EnsureCreated();
        }

        public List<DataTypes.Appointment> GetAppointments()
        {
            return _db.Appointments.ToList();
        }

        public List<Contact> GetContacts()
        {
            return _db.Contacts.ToList();
        }

        public void RemoveAppointment(DataTypes.Appointment appointment)
        {
            _db.Remove(appointment);
            _db.SaveChanges();
        }

        public void RemoveContact(Contact contact)
        {
            _db.Remove(contact);
            _db.SaveChanges();
        }

        public void UpdateAppointment(DataTypes.Appointment appointment)
        {
            var oldEntry = _db.Appointments.FirstOrDefault(x => x.Id == appointment.Id);
            _db.Entry(oldEntry).CurrentValues.SetValues(appointment);
            _db.SaveChanges();
        }

        public void UpdateContact(Contact contact)
        {
            var oldEntry = _db.Contacts.FirstOrDefault(x => x.Id == contact.Id);
            _db.Entry(oldEntry).CurrentValues.SetValues(contact);
            _db.SaveChanges();
        }

        public void WriteAppointment(DataTypes.Appointment appointment)
        {
            _db.Add(appointment);
            _db.SaveChanges();
        }

        public void WriteContact(Contact contact)
        {
            _db.Add(contact);
            _db.SaveChanges();
        }
    }
}
