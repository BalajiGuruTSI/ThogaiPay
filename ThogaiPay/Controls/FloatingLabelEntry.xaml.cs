using Microsoft.Maui.Controls.Shapes;

namespace ThogaiPay.Controls;

public partial class FloatingLabelEntry : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(FloatingLabelEntry),
        string.Empty,
        BindingMode.TwoWay);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder),
        typeof(string),
        typeof(FloatingLabelEntry),
        string.Empty);

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(LabelText),
        typeof(string),
        typeof(FloatingLabelEntry),
        string.Empty);

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource),
        typeof(ImageSource),
        typeof(FloatingLabelEntry),
        null,
        propertyChanged: OnLeadingContentChanged);

    public static readonly BindableProperty PrefixTextProperty = BindableProperty.Create(
        nameof(PrefixText),
        typeof(string),
        typeof(FloatingLabelEntry),
        string.Empty,
        propertyChanged: OnLeadingContentChanged);

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard),
        typeof(Keyboard),
        typeof(FloatingLabelEntry),
        Microsoft.Maui.Keyboard.Default);

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        nameof(MaxLength),
        typeof(int),
        typeof(FloatingLabelEntry),
        int.MaxValue);

    public static readonly BindableProperty FieldHeightProperty = BindableProperty.Create(
        nameof(FieldHeight),
        typeof(double),
        typeof(FloatingLabelEntry),
        56d);

    public static readonly BindableProperty FieldPaddingProperty = BindableProperty.Create(
        nameof(FieldPadding),
        typeof(Thickness),
        typeof(FloatingLabelEntry),
        new Thickness(11, 0));

    public static readonly BindableProperty FieldCornerRadiusProperty = BindableProperty.Create(
        nameof(FieldCornerRadius),
        typeof(double),
        typeof(FloatingLabelEntry),
        20d,
        propertyChanged: OnFieldCornerRadiusChanged);

    public static readonly BindableProperty FieldBackgroundColorProperty = BindableProperty.Create(
        nameof(FieldBackgroundColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Colors.White);

    public static readonly BindableProperty FieldBorderColorProperty = BindableProperty.Create(
        nameof(FieldBorderColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Color.FromArgb("#C8D4F2"));

    public static readonly BindableProperty FieldDividerColorProperty = BindableProperty.Create(
        nameof(FieldDividerColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Color.FromArgb("#D4DDF1"));

    public static readonly BindableProperty EntryTextColorProperty = BindableProperty.Create(
        nameof(EntryTextColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Color.FromArgb("#142D78"));

    public static readonly BindableProperty EntryPlaceholderColorProperty = BindableProperty.Create(
        nameof(EntryPlaceholderColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Color.FromArgb("#8997BF"));

    public static readonly BindableProperty LabelTextColorProperty = BindableProperty.Create(
        nameof(LabelTextColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Color.FromArgb("#075BFF"));

    public static readonly BindableProperty LabelBackgroundColorProperty = BindableProperty.Create(
        nameof(LabelBackgroundColor),
        typeof(Color),
        typeof(FloatingLabelEntry),
        Colors.White);

    public static readonly BindableProperty LabelMarginProperty = BindableProperty.Create(
        nameof(LabelMargin),
        typeof(Thickness),
        typeof(FloatingLabelEntry),
        new Thickness(18, -8, 0, 0));

    public static readonly BindableProperty LabelPaddingProperty = BindableProperty.Create(
        nameof(LabelPadding),
        typeof(Thickness),
        typeof(FloatingLabelEntry),
        new Thickness(6, 0));

    public FloatingLabelEntry()
    {
        InitializeComponent();
        UpdateLeadingContentFlags();
        ApplyCornerRadius();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public ImageSource? IconSource
    {
        get => (ImageSource?)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public string PrefixText
    {
        get => (string)GetValue(PrefixTextProperty);
        set => SetValue(PrefixTextProperty, value);
    }

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public double FieldHeight
    {
        get => (double)GetValue(FieldHeightProperty);
        set => SetValue(FieldHeightProperty, value);
    }

    public Thickness FieldPadding
    {
        get => (Thickness)GetValue(FieldPaddingProperty);
        set => SetValue(FieldPaddingProperty, value);
    }

    public double FieldCornerRadius
    {
        get => (double)GetValue(FieldCornerRadiusProperty);
        set => SetValue(FieldCornerRadiusProperty, value);
    }

    public Color FieldBackgroundColor
    {
        get => (Color)GetValue(FieldBackgroundColorProperty);
        set => SetValue(FieldBackgroundColorProperty, value);
    }

    public Color FieldBorderColor
    {
        get => (Color)GetValue(FieldBorderColorProperty);
        set => SetValue(FieldBorderColorProperty, value);
    }

    public Color FieldDividerColor
    {
        get => (Color)GetValue(FieldDividerColorProperty);
        set => SetValue(FieldDividerColorProperty, value);
    }

    public Color EntryTextColor
    {
        get => (Color)GetValue(EntryTextColorProperty);
        set => SetValue(EntryTextColorProperty, value);
    }

    public Color EntryPlaceholderColor
    {
        get => (Color)GetValue(EntryPlaceholderColorProperty);
        set => SetValue(EntryPlaceholderColorProperty, value);
    }

    public Color LabelTextColor
    {
        get => (Color)GetValue(LabelTextColorProperty);
        set => SetValue(LabelTextColorProperty, value);
    }

    public Color LabelBackgroundColor
    {
        get => (Color)GetValue(LabelBackgroundColorProperty);
        set => SetValue(LabelBackgroundColorProperty, value);
    }

    public Thickness LabelMargin
    {
        get => (Thickness)GetValue(LabelMarginProperty);
        set => SetValue(LabelMarginProperty, value);
    }

    public Thickness LabelPadding
    {
        get => (Thickness)GetValue(LabelPaddingProperty);
        set => SetValue(LabelPaddingProperty, value);
    }

    public bool IsIconVisible => IconSource is not null;

    public bool IsPrefixVisible => !string.IsNullOrWhiteSpace(PrefixText);

    public bool HasLeadingSection => IsIconVisible || IsPrefixVisible;

    private static void OnLeadingContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FloatingLabelEntry control)
        {
            control.UpdateLeadingContentFlags();
        }
    }

    private static void OnFieldCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FloatingLabelEntry control)
        {
            control.ApplyCornerRadius();
        }
    }

    private void UpdateLeadingContentFlags()
    {
        OnPropertyChanged(nameof(IsIconVisible));
        OnPropertyChanged(nameof(IsPrefixVisible));
        OnPropertyChanged(nameof(HasLeadingSection));
    }

    private void ApplyCornerRadius()
    {
        if (InputBorder is not null)
        {
            InputBorder.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(FieldCornerRadius)
            };
        }
    }
}
