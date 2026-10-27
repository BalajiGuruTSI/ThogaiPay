using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ThogaiPay.Controls;

public enum CTAButtonVariant
{
    Primary,
    BrandedPrimary,
    Secondary,
    Text,
    Recharge,
    Destructive
}

/// <summary>
/// Unified, accessible CTA. Variants and colors are resolved from centralized theme resources.
/// Keep business logic in the bound command, not in this control.
/// </summary>
public partial class CTAButton : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(CTAButton), string.Empty,
            propertyChanged: OnPresentationChanged);
    public static readonly BindableProperty BusyTextProperty =
        BindableProperty.Create(nameof(BusyText), typeof(string), typeof(CTAButton), "Please wait…",
            propertyChanged: OnPresentationChanged);
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CTAButton),
            propertyChanged: OnCommandChanged);
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CTAButton));
    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(CTAButton), false,
            propertyChanged: OnPresentationChanged);
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(CTAButtonVariant), typeof(CTAButton), CTAButtonVariant.Primary,
            propertyChanged: OnVariantChanged);
    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.Create(nameof(LeadingIcon), typeof(string), typeof(CTAButton), string.Empty,
            propertyChanged: OnPresentationChanged);
    public static readonly BindableProperty LeadingIconSourceProperty =
        BindableProperty.Create(nameof(LeadingIconSource), typeof(ImageSource), typeof(CTAButton), null,
            propertyChanged: OnPresentationChanged);
    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.Create(nameof(TrailingIcon), typeof(string), typeof(CTAButton), string.Empty,
            propertyChanged: OnPresentationChanged);
    public static readonly BindableProperty AccessibilityLabelProperty =
        BindableProperty.Create(nameof(AccessibilityLabel), typeof(string), typeof(CTAButton), string.Empty);
    public static readonly BindableProperty AccessibilityHintProperty =
        BindableProperty.Create(nameof(AccessibilityHint), typeof(string), typeof(CTAButton), string.Empty);
    public static readonly BindableProperty AutomationIdentifierProperty =
        BindableProperty.Create(nameof(AutomationIdentifier), typeof(string), typeof(CTAButton), string.Empty);

    public event EventHandler? Clicked;

    public CTAButton()
    {
        InitializeComponent();
        ActionButton.Clicked += OnActionButtonClicked;
        ApplyVariant();
        UpdateState();
    }

    private void OnActionButtonClicked(object? sender, EventArgs e)
        => Clicked?.Invoke(this, e);

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public string BusyText { get => (string)GetValue(BusyTextProperty); set => SetValue(BusyTextProperty, value); }
    public ICommand? Command { get => (ICommand?)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
    public object? CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
    public bool IsBusy { get => (bool)GetValue(IsBusyProperty); set => SetValue(IsBusyProperty, value); }
    public CTAButtonVariant Variant { get => (CTAButtonVariant)GetValue(VariantProperty); set => SetValue(VariantProperty, value); }
    public string LeadingIcon { get => (string)GetValue(LeadingIconProperty); set => SetValue(LeadingIconProperty, value); }
    public ImageSource? LeadingIconSource { get => (ImageSource?)GetValue(LeadingIconSourceProperty); set => SetValue(LeadingIconSourceProperty, value); }
    public string TrailingIcon { get => (string)GetValue(TrailingIconProperty); set => SetValue(TrailingIconProperty, value); }
    public string AccessibilityLabel { get => (string)GetValue(AccessibilityLabelProperty); set => SetValue(AccessibilityLabelProperty, value); }
    public string AccessibilityHint { get => (string)GetValue(AccessibilityHintProperty); set => SetValue(AccessibilityHintProperty, value); }
    public string AutomationIdentifier { get => (string)GetValue(AutomationIdentifierProperty); set => SetValue(AutomationIdentifierProperty, value); }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == IsEnabledProperty.PropertyName)
            UpdateState();
    }

    private static void OnPresentationChanged(BindableObject bindable, object oldValue, object newValue)
        => ((CTAButton)bindable).UpdateState();

    private static void OnVariantChanged(BindableObject bindable, object oldValue, object newValue)
        => ((CTAButton)bindable).ApplyVariant();

    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CTAButton)bindable;
        if (oldValue is ICommand oldCommand)
            oldCommand.CanExecuteChanged -= control.OnCommandCanExecuteChanged;
        if (newValue is ICommand newCommand)
            newCommand.CanExecuteChanged += control.OnCommandCanExecuteChanged;
        control.UpdateState();
    }

    private void OnCommandCanExecuteChanged(object? sender, EventArgs e)
        => Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(UpdateState);

    private void ApplyVariant()
    {
        if (ButtonSurface is null || AccentSurface is null || ActionText is null)
            return;

        var variant = Variant;
        switch (variant)
        {
            case CTAButtonVariant.Secondary:
                ButtonSurface.SetDynamicResource(Border.BackgroundProperty, "CTASurfaceBrush");
                ButtonSurface.SetDynamicResource(Border.StrokeProperty, "CTASecondaryBorderBrush");
                ButtonSurface.StrokeThickness = 1.5;
                AccentSurface.SetDynamicResource(Border.BackgroundProperty, "CTASurfaceBrush");
                AccentSurface.Padding = 0;
                ActionText.SetDynamicResource(Label.TextColorProperty, "CTASecondaryForeground");
                LeadingIconLabel.SetDynamicResource(Label.TextColorProperty, "CTASecondaryForeground");
                TrailingIconLabel.SetDynamicResource(Label.TextColorProperty, "CTASecondaryForeground");
                BusyTextLabel.SetDynamicResource(Label.TextColorProperty, "CTASecondaryForeground");
                ProgressIndicator.SetDynamicResource(ActivityIndicator.ColorProperty, "CTASecondaryForeground");
                break;
            case CTAButtonVariant.Text:
                ButtonSurface.SetDynamicResource(Border.BackgroundProperty, "CTATransparentBrush");
                ButtonSurface.SetDynamicResource(Border.StrokeProperty, "CTATransparentBrush");
                ButtonSurface.StrokeThickness = 0;
                AccentSurface.SetDynamicResource(Border.BackgroundProperty, "CTATransparentBrush");
                AccentSurface.Padding = 0;
                ActionText.SetDynamicResource(Label.TextColorProperty, "CTATextForeground");
                LeadingIconLabel.SetDynamicResource(Label.TextColorProperty, "CTATextForeground");
                TrailingIconLabel.SetDynamicResource(Label.TextColorProperty, "CTATextForeground");
                BusyTextLabel.SetDynamicResource(Label.TextColorProperty, "CTATextForeground");
                ProgressIndicator.SetDynamicResource(ActivityIndicator.ColorProperty, "CTATextForeground");
                break;
            case CTAButtonVariant.Recharge:
                ButtonSurface.SetDynamicResource(Border.BackgroundProperty, "CTARechargeBrush");
                ButtonSurface.SetDynamicResource(Border.StrokeProperty, "CTARechargeBrush");
                ButtonSurface.StrokeThickness = 0;
                AccentSurface.SetDynamicResource(Border.BackgroundProperty, "CTARechargeAccentBrush");
                AccentSurface.Padding = new Thickness(0, 0, 0, 3);
                SetForeground("CTARechargeForeground");
                break;
            case CTAButtonVariant.Destructive:
                ButtonSurface.SetDynamicResource(Border.BackgroundProperty, "CTADestructiveBrush");
                ButtonSurface.SetDynamicResource(Border.StrokeProperty, "CTADestructiveBrush");
                ButtonSurface.StrokeThickness = 0;
                AccentSurface.SetDynamicResource(Border.BackgroundProperty, "CTADestructiveBrush");
                AccentSurface.Padding = 0;
                SetForeground("CTADestructiveForeground");
                break;
            case CTAButtonVariant.BrandedPrimary:
                ButtonSurface.SetDynamicResource(Border.BackgroundProperty, "CTAPrimaryGradient");
                ButtonSurface.SetDynamicResource(Border.StrokeProperty, "CTAPrimaryBorder");
                ButtonSurface.StrokeThickness = 0;
                AccentSurface.SetDynamicResource(Border.BackgroundProperty, "CTAAccentBrush");
                AccentSurface.Padding = new Thickness(0, 0, 0, 5);
                SetForeground("CTAPrimaryForeground");
                break;
            default:
                ButtonSurface.SetDynamicResource(Border.BackgroundProperty, "CTAPrimarySolidBrush");
                ButtonSurface.SetDynamicResource(Border.StrokeProperty, "CTAPrimaryBorder");
                ButtonSurface.StrokeThickness = 0;
                AccentSurface.SetDynamicResource(Border.BackgroundProperty, "CTAPrimarySolidBrush");
                AccentSurface.Padding = 0;
                SetForeground("CTAPrimaryForeground");
                break;
        }

        AccentSurface.IsVisible = variant is CTAButtonVariant.BrandedPrimary or CTAButtonVariant.Recharge;
        UpdateState();
    }

    private void SetForeground(string resourceKey)
    {
        ActionText.SetDynamicResource(Label.TextColorProperty, resourceKey);
        LeadingIconLabel.SetDynamicResource(Label.TextColorProperty, resourceKey);
        TrailingIconLabel.SetDynamicResource(Label.TextColorProperty, resourceKey);
        BusyTextLabel.SetDynamicResource(Label.TextColorProperty, resourceKey);
        ProgressIndicator.SetDynamicResource(ActivityIndicator.ColorProperty, resourceKey);
    }

    private void UpdateState()
    {
        if (ActionButton is null) return;
        ActionButton.IsEnabled = IsEnabled && !IsBusy &&
            (Command?.CanExecute(CommandParameter) ?? (Clicked is not null));
        Opacity = IsEnabled ? 1.0 : 0.62;
        ActionText.IsVisible = !IsBusy;
        BusyTextLabel.IsVisible = IsBusy;
        ProgressIndicator.IsVisible = IsBusy;
        ProgressIndicator.IsRunning = IsBusy;
        LeadingIconLabel.IsVisible = !IsBusy && !string.IsNullOrWhiteSpace(LeadingIcon);
        LeadingIconImage.IsVisible = !IsBusy && LeadingIconSource is not null;
        TrailingIconLabel.IsVisible = !IsBusy && !string.IsNullOrWhiteSpace(TrailingIcon);
    }
}
