namespace ThogaiPay.Models.DTOs;

/// <summary>
/// Response DTO for login operation. Indicates that OTP was sent.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Indicates whether OTP was successfully sent.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message for the user.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Unique identifier for this login session (for OTP verification).
    /// </summary>
    public string SessionId { get; set; } = string.Empty;
}
