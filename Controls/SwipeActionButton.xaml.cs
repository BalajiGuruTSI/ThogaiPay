using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class SwipeActionButton : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(SwipeActionButton), string.Empty);

    public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(
        nameof(ActionCommand), typeof(ICommand), typeof(SwipeActionButton));

    public static readonly BindableProperty ActionCommandParameterProperty = BindableProperty.Create(
        nameof(ActionCommandParameter), typeof(object), typeof(SwipeActionButton));

    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
        nameof(IsBusy), typeof(bool), typeof(SwipeActionButton), false, propertyChanged: OnIsBusyChanged);

    public static readonly BindableProperty ButtonBackgroundProperty = BindableProperty.Create(
        nameof(ButtonBackground), typeof(Brush), typeof(SwipeActionButton), Brush.Transparent);

    public static readonly BindableProperty ProgressFillColorProperty = BindableProperty.Create(
        nameof(ProgressFillColor), typeof(Color), typeof(SwipeActionButton), Color.FromArgb("#14000000"));

    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(
        nameof(IndicatorColor), typeof(Color), typeof(SwipeActionButton), Color.FromArgb("#0759D8"));

    public static readonly BindableProperty ThumbIconSourceProperty = BindableProperty.Create(
        nameof(ThumbIconSource), typeof(ImageSource), typeof(SwipeActionButton), ImageSource.FromFile("arrow_right.png"));

    public static readonly BindableProperty ButtonHeightProperty = BindableProperty.Create(
        nameof(ButtonHeight), typeof(double), typeof(SwipeActionButton), 54d);

    public static readonly BindableProperty TextStyleProperty = BindableProperty.Create(
        nameof(TextStyle), typeof(Style), typeof(SwipeActionButton));

    public static readonly BindableProperty SwipeThresholdProperty = BindableProperty.Create(
        nameof(SwipeThreshold), typeof(double), typeof(SwipeActionButton), 0.72d);

    private double _maxThumbTravel;
    private double _currentProgress;
    private bool _isInteractionLocked;

    public SwipeActionButton()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ICommand? ActionCommand
    {
        get => (ICommand?)GetValue(ActionCommandProperty);
        set => SetValue(ActionCommandProperty, value);
    }

    public object? ActionCommandParameter
    {
        get => GetValue(ActionCommandParameterProperty);
        set => SetValue(ActionCommandParameterProperty, value);
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public Brush ButtonBackground
    {
        get => (Brush)GetValue(ButtonBackgroundProperty);
        set => SetValue(ButtonBackgroundProperty, value);
    }

    public Color ProgressFillColor
    {
        get => (Color)GetValue(ProgressFillColorProperty);
        set => SetValue(ProgressFillColorProperty, value);
    }

    public Color IndicatorColor
    {
        get => (Color)GetValue(IndicatorColorProperty);
        set => SetValue(IndicatorColorProperty, value);
    }

    public ImageSource ThumbIconSource
    {
        get => (ImageSource)GetValue(ThumbIconSourceProperty);
        set => SetValue(ThumbIconSourceProperty, value);
    }

    public double ButtonHeight
    {
        get => (double)GetValue(ButtonHeightProperty);
        set => SetValue(ButtonHeightProperty, value);
    }

    public Style? TextStyle
    {
        get => (Style?)GetValue(TextStyleProperty);
        set => SetValue(TextStyleProperty, value);
    }

    public double SwipeThreshold
    {
        get => (double)GetValue(SwipeThresholdProperty);
        set => SetValue(SwipeThresholdProperty, value);
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (IsBusy || _isInteractionLocked)
        {
            return;
        }

        _isInteractionLocked = true;
        try
        {
            await UpdateProgressAsync(1d, animate: true);
            await ExecuteActionAsync();
        }
        finally
        {
            _isInteractionLocked = false;
        }
    }

    private async void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (IsBusy || _isInteractionLocked || _maxThumbTravel <= 0)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Running:
                var progress = Math.Clamp(e.TotalX / _maxThumbTravel, 0d, 1d);
                await UpdateProgressAsync(progress, animate: false);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                _isInteractionLocked = true;
                try
                {
                    if (_currentProgress >= SwipeThreshold)
                    {
                        await UpdateProgressAsync(1d, animate: true);
                        await ExecuteActionAsync();
                    }
                    else
                    {
                        await ResetVisualsAsync();
                    }
                }
                finally
                {
                    _isInteractionLocked = false;
                }
                break;
        }
    }

    private async Task ExecuteActionAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var parameter = ActionCommandParameter;
        if (ActionCommand?.CanExecute(parameter) != true)
        {
            await ResetVisualsAsync();
            return;
        }

        ActionCommand.Execute(parameter);
        await WaitForBusyCompletionAsync();
        await ResetVisualsAsync();
    }

    private async Task WaitForBusyCompletionAsync()
    {
        var waitedForStart = 0;
        while (!IsBusy && waitedForStart < 250)
        {
            await Task.Delay(25);
            waitedForStart += 25;
        }

        if (!IsBusy)
        {
            return;
        }

        var waited = 0;
        while (IsBusy && waited < 30000)
        {
            await Task.Delay(80);
            waited += 80;
        }
    }

    private async Task UpdateProgressAsync(double progress, bool animate)
    {
        _currentProgress = Math.Clamp(progress, 0d, 1d);
        var targetX = _maxThumbTravel * _currentProgress;
        var fillWidth = Math.Max(Thumb.Width + targetX + 10, 0);

        if (animate)
        {
            await Task.WhenAll(
                Thumb.TranslateTo(targetX, 0, 160, Easing.CubicOut),
                ProgressFill.LayoutTo(new Rect(0, 0, fillWidth, ProgressFill.Height > 0 ? ProgressFill.Height : Height), 160, Easing.CubicOut));
        }
        else
        {
            Thumb.TranslationX = targetX;
            ProgressFill.WidthRequest = fillWidth;
        }
    }

    private async Task ResetVisualsAsync()
    {
        _currentProgress = 0;
        await Task.WhenAll(
            Thumb.TranslateTo(0, 0, 170, Easing.CubicOut),
            ProgressFill.LayoutTo(new Rect(0, 0, 0, ProgressFill.Height > 0 ? ProgressFill.Height : Height), 170, Easing.CubicOut));

        ProgressFill.WidthRequest = 0;
    }

    private static void OnIsBusyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SwipeActionButton control)
        {
            control.UpdateBusyVisualState();
        }
    }

    private void UpdateBusyVisualState()
    {
        var busy = IsBusy;
        ArrowIcon.IsVisible = !busy;
        BusyIndicator.IsVisible = busy;
        BusyIndicator.IsRunning = busy;
        this.InputTransparent = busy;
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        _maxThumbTravel = Math.Max(0, Width - Thumb.Width - 20);

        if (_currentProgress <= 0)
        {
            ProgressFill.WidthRequest = 0;
            Thumb.TranslationX = 0;
        }
    }
}
