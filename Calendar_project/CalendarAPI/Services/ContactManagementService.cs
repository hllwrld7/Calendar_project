using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Windows.ApplicationModel.Appointments;

namespace CalendarAPI.Services
{
    public class ContactManagementService : IContactManagementService
    {
        public List<Contact> Contacts => _sqliteService.GetContacts();
        private readonly ISQLiteService _sqliteService;

        public ContactManagementService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;
        }

        public void AddContact(Contact contact, out string result)
        {
            result = "Contact added successfully!";
            _sqliteService.WriteContact(contact);
        }
        public void DeleteContact(Contact contact, out string result)
        {
            result = "Contact deleted successfully!";
            _sqliteService.RemoveContact(contact);
        }

        public void EditContact(Contact contact, out string result)
        {
            result = "Contact edited successfully!";
            _sqliteService.UpdateContact(contact);
        }
    }
}
