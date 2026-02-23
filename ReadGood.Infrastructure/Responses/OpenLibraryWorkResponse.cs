using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReadGood.Infrastructure.Responses
{
    public class OpenLibraryWorkResponse
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        // description can be either a string or an object { value: "..." }
        [JsonPropertyName("description")]
        public object? Description { get; set; }

        [JsonPropertyName("covers")]
        public int[]? Covers { get; set; }

        [JsonPropertyName("authors")]
        public AuthorRef[]? Authors { get; set; }

        [JsonPropertyName("First_publish_date")]
        public int? First_publish_date { get; set; }

        [JsonPropertyName("created")]
        public CreatedInfo? Created { get; set; }
    }

    public class AuthorRef
    {
        [JsonPropertyName("author")]
        public AuthorKey? Author { get; set; }
    }

    public class AuthorKey
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
    }

    public class CreatedInfo
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
