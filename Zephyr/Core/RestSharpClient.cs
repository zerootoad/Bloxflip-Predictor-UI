using RestSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zephyr.Core
{
    public class RestSharpClient
    {
        private readonly RestClient _client;

        public RestSharpClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public async Task<JObject> GetAsync(string resource, Dictionary<string, string> headers = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest(resource, Method.Get);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                try
                {
                    return JObject.Parse(response.Content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing JSON response: {ex.Message}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"Error sending request: {response.ErrorMessage}");
                return null;
            }
        }

        public async Task<JObject> PostAsync(string resource, object body, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(resource, Method.Post);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            request.AddJsonBody(body);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                try
                {
                    return JObject.Parse(response.Content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing JSON response: {ex.Message}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"Error sending request: {response.ErrorMessage}");
                return null;
            }
        }
    }
}
