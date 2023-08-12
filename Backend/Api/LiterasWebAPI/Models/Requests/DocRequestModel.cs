using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using LiterasCore.ValidationAttributes;

namespace LiterasWebAPI.Models.Requests;

public class DocRequestModel
{
    [Required(ErrorMessage = "Doc ID missing")]
    public Guid Id { get; set; }

    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [DeltasCount(1, MaxDeltasAmount = 1)]
    public JsonDocument? TitleDelta { get; set; }

    [StringLength(10000, MinimumLength = 3)]
    public string Content { get; set; }

    [DeltasCount(1)]
    public JsonDocument? ContentDelta { get; set; }
}