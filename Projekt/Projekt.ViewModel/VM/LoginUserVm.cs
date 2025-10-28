using System.ComponentModel.DataAnnotations;

namespace Projekt.ViewModel.VM;
public class LoginUserVm
{
    public string UserNameOrEmail { get; set; } = default!;
    public string Password { get; set; } = default!;
}