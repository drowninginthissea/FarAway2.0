using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FarAway2._0.Content.Controls.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        private DispatcherTimer _timer;
        public Loading()
        {
            InitializeComponent();
            _timer = new();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += _timer_Tick;
            _timer.IsEnabled = true;
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            LoadingText.Text = GetLoadingText(LoadingText.Text);
        }

        private string GetLoadingText(string CurrentText) => CurrentText switch
        {
            "Идёт загрузка, пожалуйста подождите" => "Идёт загрузка, пожалуйста подождите.",
            "Идёт загрузка, пожалуйста подождите." => "Идёт загрузка, пожалуйста подождите..",
            "Идёт загрузка, пожалуйста подождите.." => "Идёт загрузка, пожалуйста подождите...",
            "Идёт загрузка, пожалуйста подождите..." => "Идёт загрузка, пожалуйста подождите"
        };
    }
}
