using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Contrib.HttpClient;

namespace ReadGood.Tests.OpenLibrary
{
    public class OpenLibraryTestsBase
    {
        private static string baseUrl = "https://openlibrary.org";

        protected static string BaseUrl { get => baseUrl; set => baseUrl = value; }

        protected static Mock<HttpMessageHandler> CreateMockHandler()
        {
            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            return handler;
        }

        protected static HttpClient CreateMockHttpClient(Mock<HttpMessageHandler> handler)
        {
            var client = handler.CreateClient();
            client.BaseAddress = new Uri(BaseUrl);
            return client;
        }
    }
}