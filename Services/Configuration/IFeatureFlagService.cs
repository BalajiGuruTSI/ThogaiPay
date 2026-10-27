namespace ThogaiPay.Services.Configuration;

/// <summary>
/// Interface for feature flag management.
/// Feature flags control which features are visible and enabled in the application.
/// Defaults to true; can be overridden by remote configuration.
/// </summary>
public interface IFeatureFlagService
{
    /// <summary>
    /// Initializes the feature-flag service (loads remote configuration if available).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Completion task.</returns>
    Task InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a feature is enabled.
    /// </summary>
    /// <param name="featureName">Name of the feature flag (e.g., "Feature.MobileRecharge").</param>
    /// <returns>True if the feature is enabled (default is true), false if explicitly disabled.</returns>
    bool IsEnabled(string featureName);

    /// <summary>
    /// Refreshes feature flags from remote configuration.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Completion task.</returns>
    Task RefreshAsync(CancellationToken cancellationToken = default);
}
