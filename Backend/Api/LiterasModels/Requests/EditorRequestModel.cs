using System.ComponentModel.DataAnnotations;

namespace LiterasModels.Requests;

public class EditorRequestModel
{
    [Required(ErrorMessage = "Editor ID missing")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Doc ID missing")]
    public Guid DocId { get; set; }

    [Required(ErrorMessage = "User ID missing")]
    public Guid UserId { get; set; }
}