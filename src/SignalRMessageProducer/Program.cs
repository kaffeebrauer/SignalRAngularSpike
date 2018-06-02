using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SignalRMessageProducer
{
    class Program
    {
        private static HubConnection _hubConnection;

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            var randomQuotes = new RandomQuoteGenerator();
            await SetupSignalRHubAsync();
            _hubConnection.On<string>("Send", (message) =>
            {
                Console.WriteLine($"Received Message: {message}");
            });
            Console.WriteLine("Connected to Hub");
            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    await _hubConnection.SendAsync("Send", randomQuotes.GetRandomQuote());
                    Console.WriteLine("SendAsync to Hub");
                }
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            await _hubConnection.DisposeAsync();
        }

        public static async Task SetupSignalRHubAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44324/message")
                .AddMessagePackProtocol()
                .ConfigureLogging(factory =>
                {
                    factory.AddConsole();
                    factory.AddFilter("Console", level => level >= LogLevel.Trace);
                }).Build();

            await _hubConnection.StartAsync();
        }
    }

    class RandomQuoteGenerator
    {
        Random random = new Random();
        List<string> Quotes = new List<string>();

        public RandomQuoteGenerator()
        {
            Quotes.Add("Sample Quote 1");
            Quotes.Add("Sample Quote 2");
            Quotes.Add("Sample Quote 3");
            Quotes.Add("Sample Quote 4");
            Quotes.Add("Sample Quote 5");
            Quotes.Add("Sample Quote 6");
            Quotes.Add("Sample Quote 7");
            Quotes.Add("Sample Quote 8");
        }
        public String GetRandomQuote()
        {
            Thread.Sleep(5000);
            var ranNumber = random.Next(0, Quotes.Count - 1);
            return Quotes[ranNumber];
        }
    }
}
