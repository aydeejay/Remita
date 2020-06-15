using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remita.Service.Extension;
using Remita.Service.Interfaces;
using Remita.Service.Models;

namespace Remita.Service.Services
{
    public class HttpService : IHttpService
    {
        private readonly string _apiKey;
        private readonly string _remitaBaseUrl;
        private readonly string _serviceTypeId;
        private readonly string _merchantId;
        private readonly IConfiguration _configuration;
        private readonly Lazy<HttpClient> _httpClient;

        public HttpService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetValue<string>("Appsettings:ApiKey");
            _remitaBaseUrl = _configuration.GetValue<string>("Appsettings:RemitaBaseUrl");
            _serviceTypeId = _configuration.GetValue<string>("Appsettings:ServiceTypeId");
            _merchantId = _configuration.GetValue<string>("Appsettings:MerchantId");

            _httpClient = new Lazy<HttpClient>(CreateHttpClient);
        }

        private HttpClient Client => _httpClient.Value;

        public async Task<RRRResponse> GenerateRRR(RRRRequest rRRRequest)
        {
            rRRRequest.ServiceTypeId = _serviceTypeId;
            var token = $"remitaConsumer={_merchantId},remitaConsumerToken={Security.Hash512(_merchantId + _serviceTypeId + rRRRequest.OrderId + rRRRequest.Amount + _apiKey)}";
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JObject.FromObject(rRRRequest).ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType.CharSet = "";

            var request = await Client
                .PostAsync("/remita/exapp/api/v1/send/api/echannelsvc/merchant/api/paymentinit", content)
                .ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(true);

            return JsonConvert.DeserializeObject<RRRResponse>(response);
        }

        public async Task<RRRResponse> CheckStatusByRRR(string rRRR)
        {
            var hash = $"{Security.Hash512(rRRR + _apiKey + _merchantId)}";

            var request = await Client
                .GetAsync($"/remita/ecomm/{_merchantId}/{rRRR}/{hash}/status.reg")
                .ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(true);

            return JsonConvert.DeserializeObject<RRRResponse>(response);
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_remitaBaseUrl) };

            return client;
        }
    }
}