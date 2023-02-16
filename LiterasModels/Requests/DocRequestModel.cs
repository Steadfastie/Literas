using System.ComponentModel.DataAnnotations;

namespace LiterasModels.Requests;

public class DocRequestModel
{
    [Required(ErrorMessage = "Doc ID missing")]
    public Guid Id { get; set; }

    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [StringLength(10000, MinimumLength = 3)]
    public string Content { get; set; }
}