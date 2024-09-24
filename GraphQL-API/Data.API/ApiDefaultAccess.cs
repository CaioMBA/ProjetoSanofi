using Domain.Models.GeneralSettings;
using System.Text;

namespace Data.API
{
    public class ApiDefaultAccess
    {
        public HttpResponseMessage ApiRequest(ApiRequestModel ApiRequest)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpMethod Method = ApiRequest.TypeRequest.ToUpper() switch
                {
                    "GET" => HttpMethod.Get,
                    "DELETE" => HttpMethod.Delete,
                    "POST" => HttpMethod.Post,
                    "PUT" => HttpMethod.Put,
                    "HEAD" => HttpMethod.Head,
                    "PATCH" => HttpMethod.Patch,
                    "OPTIONS" => HttpMethod.Options,
                    "TRACE" => HttpMethod.Trace,
                    _ => throw new Exception($"[ {ApiRequest.TypeRequest} ] is not an available method"),
                };
                var request = new HttpRequestMessage(Method, ApiRequest.Url);



                if (ApiRequest.TimeOut != null)
                {
                    httpClient.Timeout = TimeSpan.FromSeconds((double)ApiRequest.TimeOut);
                }
                if (ApiRequest.Auth != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(ApiRequest.Auth.Type, ApiRequest.Auth.Authorization);
                }
                if (ApiRequest.Headers != null)
                {
                    foreach (CustomHeaderModel header in ApiRequest.Headers!)
                    {
                        request.Headers.Add(header.Header, header.Value);
                    }
                }
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (!String.IsNullOrEmpty(ApiRequest.Body))
                {
                    request.Content = new StringContent(ApiRequest.Body, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? response = httpClient.SendAsync(request).Result;


                return response;
            }

        }
    }
}
