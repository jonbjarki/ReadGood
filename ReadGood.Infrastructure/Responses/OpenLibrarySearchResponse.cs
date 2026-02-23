using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReadGood.Infrastructure.Responses
{
    public class OpenLibrarySearchResponse
    {
        [JsonPropertyName("docs")]
        public Doc[]? Docs { get; set; }

        [JsonPropertyName("num_found")]
        public int Num_found { get; set; }
    }

    public class Doc
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("author_name")]
        public List<string> Author_name { get; set; } = new List<string>();

        [JsonPropertyName("author_key")]
        public List<string> Author_key { get; set; } = new List<string>();

        [JsonPropertyName("cover_i")]
        public int? Cover_i { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? First_publish_year { get; set; }
    }
}
