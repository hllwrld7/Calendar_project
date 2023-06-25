using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Contact
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Name";
        public string PhoneNumber { get; set; } = "Phone";
        public string Email { get; set; } = "Email";

        public Contact() { }

        public Contact(int id, string name, string phoneNumber, string email)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public Contact(string name, string phoneNumber, string email)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
