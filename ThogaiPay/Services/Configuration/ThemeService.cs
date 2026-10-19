using System;
using System.Threading.Tasks;

namespace ThogaiPay.Services.Configuration;

public class ThemeService : IThemeService
{
    private AppTheme _currentTheme = AppTheme.System;

    public AppTheme CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme == value) return;
            _currentTheme = value;
            ThemeChanged?.Invoke(this, _currentTheme);
        }
    }

    public event EventHandler<AppTheme>? ThemeChanged;

    public Task InitializeAsync()
    {
        // Minimal implementation: no persistent storage yet. Default to System.
        _currentTheme = AppTheme.System;
        return Task.CompletedTask;
    }
}
