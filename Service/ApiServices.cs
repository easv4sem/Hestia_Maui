using Hestia_Maui.Models;
using System.Text;
using Hestia_Maui.MessageTypes;
using System.Net;
using CommunityToolkit.Maui.Converters;
using Hestia_Maui.Interface;

namespace Hestia_Maui.Service
{
    public class ApiServices : IApiService
    {
        private HttpClient _httpClient;
        private CookieContainer _cookieContainer;
        private const string _contentTypeJson = "application/json";
        private bool _hasBaseUrl = false;
        private bool _httpClientIsConfigured = false;

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

        private string _baseUrl;

        
        public ApiServices()
        {
            _httpClient = new HttpClient();
            
        }


        private async Task InitializeBaseUrlAsync()
        {
            if (_hasBaseUrl) return;

            try
            {
                var organizationUrl = await SecureStorage.GetAsync("organisation_url");

                if (string.IsNullOrEmpty(organizationUrl))
                {
                    Debug.WriteLine("baseUrl is null or empty");
                    throw new Exception("Organization URL is missing");
                }

                if (!organizationUrl.EndsWith("/"))
                {
                    organizationUrl += "/";
                }

                _baseUrl = organizationUrl;
                _hasBaseUrl = true;
                Debug.WriteLine($"BaseUrl set to: {_baseUrl}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error [InitializaBaseUrlAsync]: {ex}");
            }

        }


        

        
        private async Task InitializeHttpClientAsync()
        {
            if (_httpClientIsConfigured) return;

            _cookieContainer = new CookieContainer();

            try
            {
                var authCookie = await SecureStorage.GetAsync("session_cookie");
                Debug.WriteLine($"Init httpClient: authCookie = {authCookie}");

                if (!string.IsNullOrWhiteSpace(authCookie) && _hasBaseUrl)
                {
                    var baseUri = new Uri(_baseUrl);
                    _cookieContainer.Add(baseUri, new Cookie("user", authCookie));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting authCookie: {ex}");
            }


            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                UseCookies = true
            };

            _httpClient.Dispose(); 
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_baseUrl)
            };

            _httpClientIsConfigured = true;
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
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, _contentTypeJson);

                HttpResponseMessage response;

                if (!_hasBaseUrl)
                {
                    await InitializeBaseUrlAsync();
                    response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Couldn't post with base Url");
                    }
                }
                else
                {
                    if (!_httpClientIsConfigured)
                    {
                        await InitializeHttpClientAsync();
                    }

                    response = await _httpClient.PostAsync(endpoint, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Failed to post with configured http client");
                    }
                }
                
                return response;
                
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
                if (!_httpClientIsConfigured)
                {
                    await InitializeHttpClientAsync();
                }

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, _contentTypeJson);

                var response = await _httpClient.PutAsync(endpoint, content);

                if (!response.IsSuccessStatusCode) 
                {
                    Debug.WriteLine("Failed to update");
                }

                return response;
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
                if (!_httpClientIsConfigured)
                {
                    Debug.WriteLine("HttpClient is not configured");
                    await InitializeHttpClientAsync();
                }


                var response = await _httpClient.GetAsync(endpoint);

                
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Error. Status code: {response.StatusCode}");
                    return new List<T>();
                }

                var json = await response.Content.ReadAsStringAsync();

                var items = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
                {
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

