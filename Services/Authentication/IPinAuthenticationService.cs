namespace ThogaiPay.Services.Authentication;

/// <summary>Local device-unlock credential service. PIN material is never exposed or stored in preferences.</summary>
public interface IPinAuthenticationService
{
    Task<bool> HasPinAsync();
    Task<PinOperationResult> SetPinAsync(string pin, CancellationToken cancellationToken = default);
    Task<PinOperationResult> VerifyPinAsync(string pin, CancellationToken cancellationToken = default);
    Task ClearPinAsync();
}

public sealed record PinOperationResult(bool IsSuccess, string? Message = null);
