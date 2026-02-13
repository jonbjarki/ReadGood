using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Interfaces;
using System.Net.Http.Json;
using ReadGood.Application.Features.Books.SearchBooks;

namespace ReadGood.Infrastructure.Implementations
{
    public class OpenLibraryAPI : IOpenLibraryAPI
    {
        private readonly HttpClient httpClient;
        private readonly string _apiUrl = "https://openlibrary.org/search.json";

        public OpenLibraryAPI(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<BookSearchItem>?> Search(string title)
        {
            var query = $"{_apiUrl}?title={Uri.EscapeDataString(title)}&fields=title,key,first_publish_year&limit=10";
            try
            {
                var res = await httpClient.GetAsync(query);
                Console.WriteLine(res);
                if (!res.IsSuccessStatusCode)
                {
                    return null;
                }
                var response = await res.Content.ReadFromJsonAsync<OpenLibrarySearchResponse>();
                if (response?.Docs == null) return null;

                return response.Docs.Select(doc => new BookSearchItem
                {
                    Key = doc.Key ?? "",
                    Title = doc.Title ?? "",
                    FirstPublished = doc.First_publish_year
                }).ToList();
            }
            catch (Exception)
            {
                // TODO: Properly implement error handling
                return null;
            }
        }

        private class OpenLibrarySearchResponse
        {
            public Doc[]? Docs { get; set; }
        }

        private class Doc
        {
            public string? Title { get; set; }
            public string? Key { get; set; }
            public int First_publish_year { get; set; }
        }
    }
}