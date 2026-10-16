namespace ThogaiPay.Models.DTOs;

/// <summary>
/// Response DTO after successful authentication (login + OTP verification).
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// Indicates successful authentication.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// User-facing message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Authentication token for future API calls.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// User ID or reference.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The authenticated mobile number.
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;
}
