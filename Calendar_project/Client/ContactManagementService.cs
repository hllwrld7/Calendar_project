using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace Client
{
    internal class ContactManagementService
    {
        private List<Contact> _contactList = new List<Contact>();
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:5001";

        public ContactManagementService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _contactList = GetContacts().Result;
        }

        public async Task<List<Contact>> GetContacts()
        {
            try
            {
                var httpResponseMessage = _httpClient.GetAsync(_apiUrl + "/Contact").Result;
                var response = httpResponseMessage.Content;
                var result = response.ReadAsStringAsync().Result;
                var deserializedObj = JsonConvert.DeserializeObject<List<Contact>>(result);
                return deserializedObj;
            }
            catch
            {
                return new List<Contact>();
            }
        }

        public Contact GetContactByName(string name)
        {
            foreach (var contact in _contactList) 
            {
                if (contact.Name == name) return contact;
            }
            return new Contact();
        }

        public async Task<string> AddContact(Contact contact)
        {
            if (!FieldsAreValid(contact, out var result))
                return result;
            var response = await _httpClient.PostAsJsonAsync(_apiUrl + "/Contact?operation=add", contact);
            result = response.Content.ReadAsStringAsync().Result;
            var deserializedObj = JsonConvert.DeserializeObject<string>(result);
            _contactList = await GetContacts();
            return deserializedObj;
        }

        public async Task<string> EditContact(Contact contact)
        {
            if (!FieldsAreValid(contact, out var result))
                return result;

            var response = await _httpClient.PostAsJsonAsync(_apiUrl + "/Contact?operation=edit", contact);
            result = response.Content.ReadAsStringAsync().Result;
            var deserializedObj = JsonConvert.DeserializeObject<string>(result);
            _contactList = await GetContacts();
            return deserializedObj;
        }

        public async Task<string> DeleteContact(Contact contact)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl + "/Contact?operation=delete", contact);
            var result = response.Content.ReadAsStringAsync().Result;
            var deserializedObj = JsonConvert.DeserializeObject<string>(result);
            _contactList = await GetContacts();
            return deserializedObj;
        }

        public int GetNewId()
        {
            return _contactList.Count + 1;
        }

        private bool FieldsAreValid(Contact contact, out string result)
        {
            result = "Success";
            Regex phoneRegex = new Regex("[0-9]");
            Regex emailRegex = new Regex("@.");
            if(!phoneRegex.IsMatch(contact.PhoneNumber))
            {
                result = "Input correct phone number";
                return false;
            }
            if (!emailRegex.IsMatch(contact.Email))
            {
                result = "Input correct email";
                return false;
            }
            return true;
        }

        public string ExportContacts()
        {
            try
            {
                var path = $"Contacts.json";
                var contents = JsonConvert.SerializeObject(_contactList, Formatting.Indented);
                File.WriteAllText(path, contents);
                return $"Contacts have been exported";
            }
            catch
            {
                return "An Exception has occured";
            }
        }

        public async Task<string> ImportContacts()
        {
            try
            {
                var path = $"Contacts.json";
                string text = File.ReadAllText(path);
                var deserializedObj = JsonConvert.DeserializeObject<List<Contact>>(text);
                if (deserializedObj == null)
                    return "No contacts to import";
                foreach(var contact in deserializedObj) 
                {
                    var l = _contactList.Where(x =>  x.Id == contact.Id).Count();
                    if (l != 0)
                        await EditContact(contact);
                    else
                        await AddContact(contact);
                }
                return $"Contacts have been imported";
            }
            catch
            {
                return "An Exception has occured";
            }
        }
    }
}
