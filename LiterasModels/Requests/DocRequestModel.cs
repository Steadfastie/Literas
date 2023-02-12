using System.ComponentModel.DataAnnotations;

namespace LiterasModels.Requests;

public class DocRequestModel
{
    [Required(ErrorMessage = "Doc ID missing")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Doc creator ID missing")]
    public Guid CreatorId { get; set; }

    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    public string Content { get; set; }
}