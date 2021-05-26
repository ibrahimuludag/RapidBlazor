using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Infra.HttpClients
{
    public class RapidBlazorApiClient : IRapidBlazorApiClient
    {
        private readonly HttpClient _httpClient;

        public RapidBlazorApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return _httpClient.SendAsync(request, completionOption, cancellationToken);
        }
        public void Dispose()
        {
            throw new NotImplementedException("This method should never be called");
        }


    }
}
