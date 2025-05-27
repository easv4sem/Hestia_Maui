using CommunityToolkit.Mvvm.Messaging;
using System.Text;
using Hestia_Maui.MessageTypes;
using System.Net;

namespace Hestia_Maui.Service
{
    class ApiServices
    {
        private const string _contentTypeJson = "application/json";

        private readonly string _localBaseUrl = "http://10.0.2.2:8888/";
        private readonly HttpClient _httpClient;
        private bool _isConfigured;

        public ApiServices()
        {
            _httpClient = new HttpClient();
            
        }




        private async Task InitializeBaseUrlAsync()
        {
            if (_isConfigured) return;

            try
            {
                var baseUrl = await SecureStorage.GetAsync("organisation_url");

                if (string.IsNullOrEmpty(baseUrl))
                {
                    Debug.WriteLine("baseUrl is null or empty");
                    throw new Exception("Organization URL is missing");
                }

                if (!baseUrl.EndsWith("/"))
                {
                    baseUrl += "/";
                }


                Debug.WriteLine($"BaseUrl: {baseUrl}");
                // Proof of concept: This logs the base URL retrieved from secure storage,
                // confirming that the user-entered URL (set at app startup) is correctly saved 
                // and retrieved, ensuring the app points to the intended organization's API endpoint.



                // NOTE ON NETWORKING WITH ANDROID EMULATOR:
                //
                // When running the app on an Android emulator, 'localhost' (127.0.0.1) refers to the emulator itself,
                // not the host development machine (your PC).
                //
                // To reach a server running on your development machine from the emulator, you must use the special IP 
                // address '10.0.2.2', which is routed by the emulator to your host machine's localhost.
                //
                // Therefore:
                // - For an API running on your local development machine, use "http://10.0.2.2:<port>" as the base URL.
                // - For an API running on another machine on the same network (e.g., a colleague’s PC), use that machine’s
                //   local network IP address like "http://192.168.x.x:<port>".

                _httpClient.BaseAddress = new Uri($"{_localBaseUrl}");
                _isConfigured = true;

            }
            catch (Exception ex) { 
            
                Debug.WriteLine($"Error [InitializaBaseUrlAsync]: {ex}");
                WeakReferenceMessenger.Default.Send(new ErrorMessage("Couldn't get connection to server"));
            }

        }





        /// <summary>
        /// Sends a POST request with serialized data and returns the HTTP response.
        /// </summary>
        /// <param name="endpoint">The endpoint to where the data will be sent</param>
        /// <param name="data">The object to be serialized and sent in the request body</param>
        /// <returns>The HTTP response message from the server.</returns>
        public async Task<HttpResponseMessage> ApiPostAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                if (!_isConfigured)
                {
                    await InitializeBaseUrlAsync();
                }

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, _contentTypeJson);

                return await _httpClient.PostAsync(endpoint, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error [ApiPostReturnResponseAsync]: {ex}");
                // returns HttpResponseMessage - code 503 ServiceUnavailable
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }


        /// <summary>
        /// Sends a PUT request with serialized data and returns the HTTP response.
        /// </summary>
        /// <param name="endpoint">The endpoint to where the data will be sent.</param>
        /// <param name="data">The object to be serialized and sent in the request body.</param>
        /// <returns>The HTTP response message from the server.</returns>
        public async Task<HttpResponseMessage> ApiPutAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                if (!_isConfigured)
                {
                    await InitializeBaseUrlAsync();
                }

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, _contentTypeJson);

                return await _httpClient.PutAsync(endpoint, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error [ApiPutAsync]: {ex}");
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }


        /// <summary>
        /// Sends a GET request and returns a list of deserialized objects.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="endpoint">The endpoint to where the data will be sent</param>
        /// <returns>A list of deserialized objects of type T</returns>
        public async Task<List<T>> ApiGetAll<T>(string endpoint)
        {
            try
            {
                if (!_isConfigured)
                {
                    await InitializeBaseUrlAsync();
                }

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();


                var items = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                    });

                if (items == null)
                {
                    return new List<T>();
                }

                return items;

            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"HTTP error [GetAll<{typeof(T).Name}>]: {httpEx}");
                return new List<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error [GetAll<{typeof(T).Name}>]: {ex}");
                throw;
            }
        }

    }
}

