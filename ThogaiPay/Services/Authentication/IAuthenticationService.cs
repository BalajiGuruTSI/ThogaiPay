using ThogaiPay.Models.DTOs;

namespace ThogaiPay.Services.Authentication;

/// <summary>
/// Interface for authentication operations (login, OTP verification, logout).
/// Implementations can be mock or real API-based.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Initiates login by sending OTP to the provided mobile number.
    /// </summary>
    /// <param name="mobileNumber">Mobile number (10 digits) with optional country code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>LoginResponse indicating success and session ID.</returns>
    Task<LoginResponse> LoginAsync(string mobileNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the OTP and completes authentication.
    /// </summary>
    /// <param name="sessionId">Session ID from login response.</param>
    /// <param name="otp">Six-digit OTP.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>AuthenticationResponse with token if successful.</returns>
    Task<AuthenticationResponse> VerifyOtpAsync(string sessionId, string otp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out the current user and clears session.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Completion task.</returns>
    Task LogoutAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current authentication status.
    /// </summary>
    /// <returns>True if user is authenticated, false otherwise.</returns>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the current user's mobile number (if authenticated).
    /// </summary>
    string? CurrentUserMobileNumber { get; }
}
