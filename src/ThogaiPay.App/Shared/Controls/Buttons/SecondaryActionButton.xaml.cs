using Microsoft.Maui.Controls;

namespace ThogaiPay.App.Shared.Controls.Buttons;

/// <summary>
/// Compatibility alias for the shared CTA control.
/// Uses the unified CTAButton implementation and secondary styling.
/// This is intentionally code-only: do not add a matching XAML file.
/// </summary>
[Obsolete("Use CTAButton with Variant=\"Secondary\".")]
public sealed class SecondaryActionButton : CTAButton
{
    public SecondaryActionButton()
    {
        Variant = CTAButtonVariant.Secondary;
    }
}
