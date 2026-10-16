namespace ThogaiPay.Models.DTOs;

/// <summary>
/// Request DTO for login operation with mobile number.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Indian mobile number (10 digits).
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Country code (typically +91 for India).
    /// </summary>
    public string CountryCode { get; set; } = "+91";
}
