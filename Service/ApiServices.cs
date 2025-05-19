using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hestia_Maui.Service
{
    class ApiServices
    {

        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public ApiServices()
        {
            _httpClient = new HttpClient();
            
        }

        public async Task InitAsync()
        {
            _baseUrl = await SecureStorage.GetAsync("organisation_url");
        }


        /// <summary>
        /// Sends data to a specified URL using an HTTP POST request.
        /// </summary>
        /// <param name="endpoint">The endpoint where the data will be sent.</param>
        /// <param name="data">The object to be serialized and sent in the request body.</param>
        /// <returns>Returns true if the request was successful, otherwise false.</returns>
        public async Task<bool> ApiPostData(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{"lL"}{endpoint}", content);

            // checks if the response indicates success (status code 200–299)
            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Success response: {resultJson}");
                return true;
            }
            else
            {
                Debug.WriteLine("Failed to post data");
                return false;
            }
        }


        /// <summary>
        /// Sends data to a specified URL using an HTTP PUT request.
        /// </summary>
        /// <param name="endpoint">The endpoint where the data will be sent.</param>
        /// <param name="data">The object to be serialized and sent in the request body.</param>
        /// <returns>Returns true if the request was successful, otherwise false.</returns>
        public async Task<bool> ApiPutData(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{"lala"}{endpoint}", content);

            // checks if the response indicates success (status code 200–299)
            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Succes response: {resultJson}");
                return true;
            }
            else
            {
                Debug.WriteLine("Failed to put data");
                return false;
            }
        }
    }
}

