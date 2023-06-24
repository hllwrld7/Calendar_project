using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class SettingsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:5001";

        public SettingsService()
        {
            // get all appoinments
            _httpClient = new HttpClient() { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async void ScheduleNotification()
        {
            await _httpClient.PostAsJsonAsync(_apiUrl + "/Settings?operation=schedule", "");
        }

        public async void FireNotification()
        {
            await _httpClient.PostAsJsonAsync(_apiUrl + "/Settings?operation=fire", "");
        }
    }
}
