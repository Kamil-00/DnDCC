using Projekt.ViewModel.VM;

namespace Projekt.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Error, AuthResultVm? Result)> RegisterAsync(RegisterUserVm input, string role = "User");
    Task<(bool Success, string? Error, AuthResultVm? Result)> LoginAsync(LoginUserVm input);
}