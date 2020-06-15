using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Remita.Service.Models;
using Remita.Service.Services;

namespace Remita
{
    internal class Program
    {
        public static IConfigurationRoot configuration;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Program Starting\n");

                ServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

                var rRRRequest = new RRRRequest
                {
                    Amount = "20000",
                    Description = "Payment Donation for Ophanage",
                    OrderId = "221028",
                    PayerEmail = "ayodeji.alokan@gmail.com",
                    PayerName = "Alokan John Ayodeji",
                    PayerPhone = "08030480172",
                    ServiceTypeId = ""
                };

                var getRemitaCalls = serviceProvider.GetService<HttpService>().CheckStatusByRRR("34543").Result;
                var remitaCalls = serviceProvider.GetService<HttpService>().GenerateRRR(rRRRequest).Result;

                if (remitaCalls != null)
                {
                    Console.WriteLine($"Response: {JsonConvert.SerializeObject(remitaCalls)}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton(configuration);

            //serviceCollection.AddSingleton<IConfigurationRoot>(configuration);   // IConfigurationRoot
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddTransient<HttpService>();
        }
    }
}