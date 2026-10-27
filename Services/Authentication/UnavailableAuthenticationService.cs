using ThogaiPay.Models.DTOs;

namespace ThogaiPay.Services.Authentication;

/// <summary>Release safeguard until the real API integration is supplied.</summary>
public sealed class UnavailableAuthenticationService : IAuthenticationService
{
    public bool IsAuthenticated => false;
    public string? CurrentUserMobileNumber => null;
    public Task<LoginResponse> LoginAsync(string mobileNumber, CancellationToken cancellationToken = default) =>
        Task.FromResult(new LoginResponse { IsSuccess = false, Message = "Sign-in service is not configured." });
    public Task<AuthenticationResponse> VerifyOtpAsync(string sessionId, string otp, CancellationToken cancellationToken = default) =>
        Task.FromResult(new AuthenticationResponse { IsSuccess = false, Message = "Sign-in service is not configured." });
    public Task LogoutAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
