namespace ThogaiPay.Services.Recharge;

public enum RechargeKind { Mobile, Dth }
public enum RechargeStatus { Successful, Pending, Failed }
public sealed record FavouriteRecipient(string Name, string SubscriberId, RechargeKind Kind, string Avatar);
public sealed record RecentRecharge(string SubscriberId, RechargeKind Kind, decimal Amount, DateTimeOffset Date, RechargeStatus Status);
public sealed record RechargeRequest(string SubscriberId, RechargeKind Kind, decimal Amount);
public sealed record RechargeResult(bool IsSuccess, string? Message, RecentRecharge? Recharge = null);

public interface IFavouriteRecipientService { Task<IReadOnlyList<FavouriteRecipient>> GetAsync(CancellationToken cancellationToken = default); }
public interface IRecentRechargeService { Task<IReadOnlyList<RecentRecharge>> GetAsync(CancellationToken cancellationToken = default); }
public interface IRechargeService { Task<RechargeResult> SubmitAsync(RechargeRequest request, CancellationToken cancellationToken = default); }

/// <summary>Development sample data only; swap registrations for API implementations when contracts are available.</summary>
public sealed class MockRechargeDataService : IFavouriteRecipientService, IRecentRechargeService, IRechargeService
{
    private static readonly IReadOnlyList<FavouriteRecipient> Favourites = new[]
    {
        new FavouriteRecipient("Amma", "9876543210", RechargeKind.Mobile, "avatar_amma.png"),
        new FavouriteRecipient("Appa", "9789012345", RechargeKind.Mobile, "avatar_appa.png"),
        new FavouriteRecipient("Home DTH", "3021876543", RechargeKind.Dth, "dth.svg")
    };
    private static readonly IReadOnlyList<RecentRecharge> Recent = new[]
    {
        new RecentRecharge("9876543210", RechargeKind.Mobile, 239m, DateTimeOffset.UtcNow.AddDays(-1), RechargeStatus.Successful),
        new RecentRecharge("3021876543", RechargeKind.Dth, 350m, DateTimeOffset.UtcNow.AddDays(-4), RechargeStatus.Pending),
        new RecentRecharge("9789012345", RechargeKind.Mobile, 299m, DateTimeOffset.UtcNow.AddDays(-9), RechargeStatus.Failed)
    };
    Task<IReadOnlyList<FavouriteRecipient>> IFavouriteRecipientService.GetAsync(CancellationToken cancellationToken) => Task.FromResult(Favourites);
    Task<IReadOnlyList<RecentRecharge>> IRecentRechargeService.GetAsync(CancellationToken cancellationToken) => Task.FromResult(Recent);
    public Task<RechargeResult> SubmitAsync(RechargeRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.SubscriberId) || request.Amount <= 0) return Task.FromResult(new RechargeResult(false, "Enter a valid subscriber and recharge amount."));
        var recent = new RecentRecharge(request.SubscriberId, request.Kind, request.Amount, DateTimeOffset.UtcNow, RechargeStatus.Successful);
        return Task.FromResult(new RechargeResult(true, "Recharge request submitted.", recent));
    }
}
