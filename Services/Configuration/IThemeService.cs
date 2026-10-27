namespace ThogaiPay.Services.Configuration;

/// <summary>
/// Interface for managing application themes (Light, Dark, System).
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Gets or sets the current theme.
    /// </summary>
    AppTheme CurrentTheme { get; set; }

    /// <summary>
    /// Initializes the theme service and applies saved preference.
    /// </summary>
    /// <returns>Completion task.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Event raised when theme changes.
    /// </summary>
    event EventHandler<AppTheme>? ThemeChanged;
}

/// <summary>
/// Supported themes.
/// </summary>
public enum AppTheme
{
    Light,
    Dark,
    System
}
