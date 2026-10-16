namespace ThogaiPay.Models.DTOs;

/// <summary>
/// Request DTO for OTP verification.
/// </summary>
public class OtpVerificationRequest
{
    /// <summary>
    /// Session ID from the login response.
    /// </summary>
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// Six-digit OTP entered by the user.
    /// </summary>
    public string Otp { get; set; } = string.Empty;
}
