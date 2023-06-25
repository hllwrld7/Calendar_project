namespace CalendarAPI.DataTypes
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Contact() { }

        public Contact(int id, string name, string phoneNumber, string email)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
