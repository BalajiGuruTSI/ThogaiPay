using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ThogaiPay.App.Shared.Controls.Inputs;

/// <summary>A reusable border-only entry with a floating label.</summary>
public partial class FloatingLabelEntry : ContentView
{
    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(FloatingLabelEntry), string.Empty, propertyChanged: OnDisplayPropertyChanged);
    public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(FloatingLabelEntry), string.Empty);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FloatingLabelEntry), string.Empty, BindingMode.TwoWay, propertyChanged: OnDisplayPropertyChanged);
    public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(nameof(LeadingIcon), typeof(ImageSource), typeof(FloatingLabelEntry), null);
    public static readonly BindableProperty LeadingTextProperty = BindableProperty.Create(nameof(LeadingText), typeof(string), typeof(FloatingLabelEntry), string.Empty);
    public static readonly BindableProperty InputKeyboardProperty = BindableProperty.Create(nameof(InputKeyboard), typeof(Microsoft.Maui.Keyboard), typeof(FloatingLabelEntry), Microsoft.Maui.Keyboard.Default);
    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(FloatingLabelEntry), int.MaxValue);
    public static readonly BindableProperty EntryReturnTypeProperty = BindableProperty.Create(nameof(EntryReturnType), typeof(Microsoft.Maui.ReturnType), typeof(FloatingLabelEntry), Microsoft.Maui.ReturnType.Default);
    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand), typeof(ICommand), typeof(FloatingLabelEntry));

    public FloatingLabelEntry()
    {
        InitializeComponent();
        UpdateFloatingLabel();
    }

    public string LabelText { get => (string)GetValue(LabelTextProperty); set => SetValue(LabelTextProperty, value); }
    public string PlaceholderText { get => (string)GetValue(PlaceholderTextProperty); set => SetValue(PlaceholderTextProperty, value); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public ImageSource? LeadingIcon { get => (ImageSource?)GetValue(LeadingIconProperty); set => SetValue(LeadingIconProperty, value); }
    public string LeadingText { get => (string)GetValue(LeadingTextProperty); set => SetValue(LeadingTextProperty, value); }
    public Microsoft.Maui.Keyboard InputKeyboard { get => (Microsoft.Maui.Keyboard)GetValue(InputKeyboardProperty); set => SetValue(InputKeyboardProperty, value); }
    public int MaxLength { get => (int)GetValue(MaxLengthProperty); set => SetValue(MaxLengthProperty, value); }
    public Microsoft.Maui.ReturnType EntryReturnType { get => (Microsoft.Maui.ReturnType)GetValue(EntryReturnTypeProperty); set => SetValue(EntryReturnTypeProperty, value); }
    public ICommand? ReturnCommand { get => (ICommand?)GetValue(ReturnCommandProperty); set => SetValue(ReturnCommandProperty, value); }

    private void OnEntryFocused(object? sender, FocusEventArgs e) => UpdateFloatingLabel();
    private void OnEntryUnfocused(object? sender, FocusEventArgs e) => UpdateFloatingLabel();

    private static void OnDisplayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((FloatingLabelEntry)bindable).UpdateFloatingLabel();

    private void UpdateFloatingLabel()
    {
        if (FloatingLabel is null || InnerEntry is null) return;
        var shouldFloat = InnerEntry.IsFocused || !string.IsNullOrWhiteSpace(Text);
        FloatingLabel.IsVisible = shouldFloat;
        InnerEntry.Placeholder = shouldFloat ? string.Empty : PlaceholderText;
        InputSurface.Stroke = InnerEntry.IsFocused
            ? (Color)Microsoft.Maui.Controls.Application.Current!.Resources["InputFocusedBorder"]
            : (Color)Microsoft.Maui.Controls.Application.Current!.Resources["InputBorder"];
    }
}
