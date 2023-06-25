using CalendarAPI.DataTypes;

namespace CalendarAPI.Interfaces
{
    public interface IContactManagementService
    {
        public List<Contact> Contacts { get; }
        public void AddContact(Contact contact, out string result);
        public void EditContact(Contact contact, out string result);
        public void DeleteContact(Contact contact, out string result);
    }
}
