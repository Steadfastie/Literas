using System.Text.Json.Serialization;
using LiterasData.Entities;

namespace LiterasWebAPI.Models.Responses;

public class DocThumbnailResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public List<EditorScope> Permissions { get; set; } = new();

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EditorStatus Status { get; set; }
}
