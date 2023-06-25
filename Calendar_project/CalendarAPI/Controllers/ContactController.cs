using CalendarAPI.DataTypes;
using CalendarAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactManagementService _contactManagementService;

        public ContactController(IContactManagementService contactManagementServic)
        {
            _contactManagementService = contactManagementServic;
        }

        [HttpGet(Name = "GetContacts")]
        public IEnumerable<Contact> Get()
        {
            return _contactManagementService.Contacts;
        }

        [HttpPost(Name = "PostContacts")]
        public string Post(string operation, [FromBody] Contact contact)
        {
            var response = String.Empty;
            if(operation == "add")
                _contactManagementService.AddContact(contact, out response);

            if (operation == "edit")
                _contactManagementService.EditContact(contact, out response);

            if (operation == "delete")
                _contactManagementService.DeleteContact(contact, out response);

            return response;
        }
    }
}