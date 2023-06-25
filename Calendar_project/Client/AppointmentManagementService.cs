using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client
{
    internal class AppointmentManagementService: IDisposable
    {
        private List<Appointment> _appointmentList = new List<Appointment>();
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:5001";

        public AppointmentManagementService() 
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _appointmentList = GetAppointments().Result;
        }

        private async Task<List<Appointment>> GetAppointments()
        {      
            try
            {
                var httpResponseMessage = _httpClient.GetAsync(_apiUrl + "/Appointment").Result;
                var response = httpResponseMessage.Content;
                var result = response.ReadAsStringAsync().Result;
                var deserializedObj = JsonConvert.DeserializeObject<List<Appointment>>(result);
                return deserializedObj;
            }
            catch
            {
                return new List<Appointment>();
            }
        }

        public async Task<string> AddAppointment(Appointment appointment)
        {
            if (GetAppointmentsForTheDay(appointment.StartDate).Count >= 4)
                return "Can't add more than 4 appointments for a day";
            var response = await _httpClient.PostAsJsonAsync(_apiUrl + "/Appointment?operation=add", appointment);
            var result = response.Content.ReadAsStringAsync().Result;
            var deserializedObj = JsonConvert.DeserializeObject<string>(result);
            _appointmentList = await GetAppointments();
            return deserializedObj;
        }

        public async Task<string> EditAppointment(Appointment appointment)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl + "/Appointment?operation=edit", appointment);
            var result = response.Content.ReadAsStringAsync().Result;
            var deserializedObj = JsonConvert.DeserializeObject<string>(result);
            _appointmentList = await GetAppointments();
            return deserializedObj;
        }

        public async Task<string> DeleteAppointment(Appointment appointment)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl + "/Appointment?operation=delete", appointment);
            var result = response.Content.ReadAsStringAsync().Result;
            var deserializedObj = JsonConvert.DeserializeObject<string>(result);
            _appointmentList = await GetAppointments();
            return deserializedObj;
        }

        public List<Appointment> GetAppointmentsForTheDay(DateTime date)
        {
            var appList = new List<Appointment>();
            foreach(var appointment in _appointmentList)
            {
                if (appointment.StartDate.Day == date.Day
                    && appointment.StartDate.Month == date.Month
                    && appointment.StartDate.Year == date.Year)
                    appList.Add(appointment);
            }
            return appList;
        }

        public int GetNewId()
        {
            return _appointmentList.Count + 1;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    interface AppointmentList
    {
        public List<Appointment> Appointments { get; }
    }
}
