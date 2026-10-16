using System.Collections.Concurrent;
using System.Security.Cryptography;
using ThogaiPay.Models.DTOs;

namespace ThogaiPay.Services.Authentication;

/// <summary>
/// Development-only mock authentication service.
/// Simulates login + OTP verification flow until backend API is available.
/// </summary>
public sealed class MockAuthenticationService : IAuthenticationService
{
    private sealed record OtpSession(string MobileNumber, string Otp, DateTimeOffset ExpiresAtUtc);

    private readonly ConcurrentDictionary<string, OtpSession> _sessions = new();
    private string? _token;
    private string? _currentMobile;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_token);

    public string? CurrentUserMobileNumber => _currentMobile;

    public async Task<LoginResponse> LoginAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Delay(400, cancellationToken);

        var normalized = NormalizeIndianMobile(mobileNumber);
        if (normalized is null)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Please enter a valid 10-digit Indian mobile number.",
                SessionId = string.Empty
            };
        }

        var sessionId = Guid.NewGuid().ToString("N");
        var otp = GenerateOtp();
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(2);

        _sessions[sessionId] = new OtpSession(normalized, otp, expiresAt);

        return new LoginResponse
        {
            IsSuccess = true,
            Message = $"OTP sent successfully. Dev mock OTP: {otp}",
            SessionId = sessionId
        };
    }

    public async Task<AuthenticationResponse> VerifyOtpAsync(string sessionId, string otp, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Delay(500, cancellationToken);

        if (!_sessions.TryGetValue(sessionId, out var session))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Session expired. Please request OTP again."
            };
        }

        if (DateTimeOffset.UtcNow > session.ExpiresAtUtc)
        {
            _sessions.TryRemove(sessionId, out _);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "OTP expired. Please request a new OTP."
            };
        }

        if (!string.Equals(session.Otp, otp?.Trim(), StringComparison.Ordinal))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Invalid OTP. Please try again."
            };
        }

        _token = $"mock-token-{Guid.NewGuid():N}";
        _currentMobile = session.MobileNumber;
        _sessions.TryRemove(sessionId, out _);

        return new AuthenticationResponse
        {
            IsSuccess = true,
            Message = "Authentication successful.",
            Token = _token,
            UserId = "mock-user-001",
            MobileNumber = session.MobileNumber
        };
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _token = null;
        _currentMobile = null;
        _sessions.Clear();
        return Task.CompletedTask;
    }

    private static string? NormalizeIndianMobile(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var digits = new string(input.Where(char.IsDigit).ToArray());
        if (digits.Length == 12 && digits.StartsWith("91", StringComparison.Ordinal))
        {
            digits = digits[2..];
        }

        if (digits.Length != 10)
        {
            return null;
        }

        return digits;
    }

    private static string GenerateOtp()
    {
        var bytes = RandomNumberGenerator.GetBytes(4);
        var value = BitConverter.ToUInt32(bytes, 0) % 1_000_000;
        return value.ToString("D6");
    }
}
