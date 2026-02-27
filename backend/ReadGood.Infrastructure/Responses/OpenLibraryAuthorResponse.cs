using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.Infrastructure.Responses
{
    using System.Text.Json.Serialization;

    public class OpenLibraryAuthorResponse
    {
        [JsonPropertyName("birth_date")]
        public string? BirthDate { get; set; }

        [JsonPropertyName("fuller_name")]
        public string? FullerName { get; set; }

        [JsonPropertyName("photos")]
        public List<int>? Photos { get; set; }

        [JsonPropertyName("links")]
        public List<OpenLibraryLink>? Links { get; set; }

        [JsonPropertyName("entity_type")]
        public string? EntityType { get; set; }

        [JsonPropertyName("personal_name")]
        public string? PersonalName { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("bio")]
        public OpenLibraryTextValue? Bio { get; set; }

        [JsonPropertyName("source_records")]
        public List<string>? SourceRecords { get; set; }

        [JsonPropertyName("alternate_names")]
        public List<string>? AlternateNames { get; set; }

        [JsonPropertyName("remote_ids")]
        public OpenLibraryRemoteIds? RemoteIds { get; set; }

        [JsonPropertyName("type")]
        public OpenLibraryTypeKey? Type { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("latest_revision")]
        public int LatestRevision { get; set; }

        [JsonPropertyName("revision")]
        public int Revision { get; set; }

        [JsonPropertyName("created")]
        public OpenLibraryDateTimeValue? Created { get; set; }

        [JsonPropertyName("last_modified")]
        public OpenLibraryDateTimeValue? LastModified { get; set; }
    }

    public class OpenLibraryLink
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("type")]
        public OpenLibraryTypeKey? Type { get; set; }
    }
    public class OpenLibraryTypeKey
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
    }
    public class OpenLibraryTextValue
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
    public class OpenLibraryDateTimeValue
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("value")]
        public DateTime Value { get; set; }
    }
    public class OpenLibraryRemoteIds
    {
        [JsonPropertyName("viaf")]
        public string? Viaf { get; set; }

        [JsonPropertyName("goodreads")]
        public string? Goodreads { get; set; }

        [JsonPropertyName("storygraph")]
        public string? Storygraph { get; set; }

        [JsonPropertyName("isni")]
        public string? Isni { get; set; }

        [JsonPropertyName("librarything")]
        public string? LibraryThing { get; set; }

        [JsonPropertyName("amazon")]
        public string? Amazon { get; set; }

        [JsonPropertyName("wikidata")]
        public string? Wikidata { get; set; }

        [JsonPropertyName("imdb")]
        public string? Imdb { get; set; }

        [JsonPropertyName("musicbrainz")]
        public string? MusicBrainz { get; set; }

        [JsonPropertyName("lc_naf")]
        public string? LcNaf { get; set; }

        [JsonPropertyName("opac_sbn")]
        public string? OpacSbn { get; set; }
    }

}