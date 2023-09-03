using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiterasData.Entities;


public class Doc : IBaseEntity
{
    public Guid Id { get; init; }

    [ConcurrencyCheck] 
    public int Version { get; private set; }

    public DateTime CreatedAt { get; init; }

    public string Title { get; private set; }

    /// <remarks>
    /// <see cref="JsonDocument" /> is chosen for its versatility,
    /// enabling <see href="https://quilljs.com/">Quill JS</see> usage in designated SPAs
    /// </remarks>
    public JsonDocument TitleDelta { get; private set; }

    public string Content { get; private set; }

    /// <remarks>
    /// <see cref="JsonDocument" /> is chosen for its versatility,
    /// enabling <see href="https://quilljs.com/">Quill JS</see> usage in designated SPAs
    /// </remarks>
    public JsonDocument ContentDelta { get; private set; }

    public ICollection<Editor> Editors { get; set; }

    public Doc(Guid? id,
        (string titleCoy, JsonDocument titleDelta) title,
        (string contentCoy, JsonDocument contentDelta) content)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Title = title.titleCoy;
        TitleDelta = title.titleDelta;
        Content = content.contentCoy;
        ContentDelta = content.contentDelta;
    }

    public void ChangeTitle((string titleCoy, JsonDocument titleDelta) title)
    {
        Title = title.titleCoy;
        TitleDelta = title.titleDelta;
    }

    public void ChangeContent((string contentCoy, JsonDocument contentDelta) content)
    {
        Content = content.contentCoy;
        ContentDelta = content.contentDelta;
    }

    public void UpdateVersion()
    {
        Version++;
    }
}
