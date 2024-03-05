using System.Text.Json.Serialization;

namespace ZmaReference.Features.Docs.Models;

public class DocumentCategory
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("files")]
    public List<Document> Documents { get; set; }
}