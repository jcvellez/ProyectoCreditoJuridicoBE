using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.infrastructure.utils
{
    public class HTTPRequest
    {
        private static readonly HttpClient _httpClient;

        static HTTPRequest()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> PostAsyncFirma<T>(string uri, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            var content = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, string auth, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            request.Headers.Add(auth, token);
            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, string auth, string token, string headerName, string headerValue)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            request.Headers.Add(auth, token);
            request.Headers.Add(headerName, headerValue);
            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(response, responseBody);
            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string uri, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            var content = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public static async Task<string> PostAsync<T>(string uri, string token, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string uri, string auth, string token, T data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };
            //_httpClient.DefaultRequestHeaders.Add(auth, token);
            request.Headers.Add(auth, token);
            var content = JsonConvert.SerializeObject(data);
            //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8);
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.SendAsync(request);
            //var response = await _httpClient.PostAsync(uri, httpContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            //manejarErrores(uri,response, responseBody);
            //response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
