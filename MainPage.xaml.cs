using System.Windows.Input;

namespace ThogaiPay
{
    public partial class MainPage : MainTabPage
    {
        protected override string CurrentTab => "Home";

        private bool _isPrimaryActionBusy;

        public ICommand ContinueToRechargeCommand { get; }

        public ICommand ProfileCommand { get; }

        public bool IsPrimaryActionBusy
        {
            get => _isPrimaryActionBusy;
            set
            {
                if ( _isPrimaryActionBusy == value )
                {
                    return;
                }

                _isPrimaryActionBusy = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            ContinueToRechargeCommand = new Command(async () => await ContinueToRechargeAsync());
            ProfileCommand = new Command(async () => await NavigateToProfileAsync());
        }

        private async Task ContinueToRechargeAsync()
        {
            if ( IsPrimaryActionBusy || Shell.Current is null )
            {
                return;
            }

            try
            {
                IsPrimaryActionBusy = true;
                await Shell.Current.GoToAsync("//MainTabs/RechargePage");
            }
            finally
            {
                IsPrimaryActionBusy = false;
            }
        }

        private async Task NavigateToProfileAsync()
        {
            if ( Shell.Current is null )
            {
                return;
            }

            await Shell.Current.GoToAsync("//MainTabs/ProfilePage");
        }
    }
}
