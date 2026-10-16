using System.Windows.Input;

namespace ThogaiPay.Views.Recharge;

public partial class MobileRechargePage : ContentPage
{
    private bool _isPrimaryActionBusy;

    public ICommand RechargeNowCommand { get; }

    public bool IsPrimaryActionBusy
    {
        get => _isPrimaryActionBusy;
        set
        {
            if (_isPrimaryActionBusy == value)
            {
                return;
            }

            _isPrimaryActionBusy = value;
            OnPropertyChanged();
        }
    }

    public MobileRechargePage()
    {
        InitializeComponent();
        RechargeNowCommand = new Command(async () => await RechargeNowAsync());
    }

    private async Task RechargeNowAsync()
    {
        if (IsPrimaryActionBusy)
        {
            return;
        }

        try
        {
            IsPrimaryActionBusy = true;
            await Task.Delay(900);
        }
        finally
        {
            IsPrimaryActionBusy = false;
        }
    }

}
