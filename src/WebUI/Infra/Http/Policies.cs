using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Infra.Http
{
    public static class Policies
    {
		public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions
			.HandleTransientHttpError()
			.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}

		public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
		{
			return HttpPolicyExtensions
			.HandleTransientHttpError()
			.CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
		}
	}
}
