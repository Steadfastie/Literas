using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using LiterasCore.ValidationAttributes;

namespace LiterasWebAPI.Models.Requests;

public class DocRequestModel
{
    [Required(ErrorMessage = "Doc ID missing")]
    [NotEmptyGuid]
    public Guid Id { get; set; }

    [Required]
    [NotEmptyGuid]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [Required]
    [DeltasCount(1, MaxDeltasAmount = 1)]
    public JsonDocument? TitleDelta { get; set; }

    [Required]
    [StringLength(10000, MinimumLength = 3)]
    public string Content { get; set; }

    [Required]
    [DeltasCount(1)]
    public JsonDocument? ContentDelta { get; set; }
}
