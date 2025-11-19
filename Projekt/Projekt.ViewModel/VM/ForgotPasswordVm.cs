using System.ComponentModel.DataAnnotations;

namespace Projekt.ViewModel.VM;

public class ForgotPasswordVm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
}