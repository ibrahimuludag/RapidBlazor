using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Infra.HttpClients
{
    public interface IRapidBlazorApiClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        // This is a workaroud to compile generated Api Client. Nswag add dispose method event it is set to false.
        // This method should never be called since HttpClient is added as scoped thus Singleton in WASM.
        void Dispose();
    }
}
