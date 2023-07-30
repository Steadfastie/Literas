using System.ComponentModel.DataAnnotations;

namespace LiterasModels.Requests;

public class UserRequestModel
{
    [Required(ErrorMessage = "Login is required")]
    [StringLength(25, MinimumLength = 3)]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(20, MinimumLength = 5)]
    public string Password { get; set; }

    public string? Fullname { get; set; }
}