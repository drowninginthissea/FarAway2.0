using FarAway2._0.Tools;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FarAway2._0.Content.Windows.Other
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        private int CountOfLogIn;
        private PanelSwapper swapper;
        public AuthenticationWindow()
        {
            InitializeComponent();
            SettingsWindow();
        }
        private async void SettingsWindow()
        {
            CountOfLogIn = 1;

            swapper = new PanelSwapper(0.5);
            swapper[Main.Name] = Main;
            swapper[Captcha.Name] = Captcha;
            swapper[Registration.Name] = Registration;
        }

        #region Window

        private void MovingWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void TopPannelButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "CloseButton")
                Application.Current.Shutdown();
            if (((Button)sender).Name == "RollDownButton")
                this.WindowState = WindowState.Minimized;
        }

        #endregion


        #region Authorization

        private void ChangeVisibility(Control element1, Visibility visibility1, Control element2, Visibility visibility2)
        {
            element1.Visibility = visibility1;
            element2.Visibility = visibility2;
        }
        private void CopyPasswordToTextBox() => TextBoxForPassword.Text = PasswordBoxForPassword.Password;
        private void CopyPasswordToPasswordBox() => PasswordBoxForPassword.Password = TextBoxForPassword.Text;
        private void CopyPasswordToTwoBoxes()
        {
            if (PasswordBoxForPassword.Visibility == Visibility.Visible)
            {
                CopyPasswordToTextBox();
                ChangeVisibility(PasswordBoxForPassword, Visibility.Collapsed, TextBoxForPassword, Visibility.Visible);
            }
            else
            {
                CopyPasswordToPasswordBox();
                ChangeVisibility(TextBoxForPassword, Visibility.Collapsed, PasswordBoxForPassword, Visibility.Visible);
            }
        }
        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e) => CopyPasswordToTwoBoxes();
        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxForPassword.Visibility == Visibility.Visible)
                CopyPasswordToPasswordBox();
            string Login = LoginTextBox.Text;
            string Password = PasswordBoxForPassword.Password;

            EnterButton.IsEnabled = false;
            RegistrationButton.IsEnabled = false;
            await Task.Run(async () =>
            {
                if (DbUtils.Authorization(Login, Password))
                {
                    //открытие окна основного
                    MessageBox.Show("Openning of main window...");
                }
                else
                {
                    if (CountOfLogIn == 3)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                            swapper.SwapPannels(Main.Name, Captcha.Name, CreateCaptcha));
                    }
                    CountOfLogIn++;
                    await Application.Current.Dispatcher.InvokeAsync(() => WrongLogOrPass.Visibility = Visibility.Visible);
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    EnterButton.IsEnabled = true;
                    RegistrationButton.IsEnabled = true;
                });
            });


        }
        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            swapper.SwapPannels(Main.Name, Registration.Name);
        }

        #endregion

            
        #region Captcha

        string? RightAnswer;
        public void CreateCaptcha()
        {
            RightAnswer = "";
            string AllSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                RightAnswer += AllSymbols[rand.Next(0, AllSymbols.Length)];
            }

            var random = new Random();
            var pixels = new byte[256 * 256 * 4];
            random.NextBytes(pixels);
            BitmapSource bitmapSource = BitmapSource.Create(256, 256, 96, 96, PixelFormats.Pbgra32, null, pixels, 256 * 4);
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSource, new Rect(0, 0, 600, 334));
                drawingContext.DrawText(
                    new FormattedText(RightAnswer, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        new Typeface("Segoe UI"), 70, Brushes.CadetBlue), new Point(rand.Next(0, 315), rand.Next(0, 215)));
            }
            var image = new DrawingImage(visual.Drawing);
            CaptchaImage.Source = image;
        }
        private void RecreateCaptchaButton_Click(object sender, RoutedEventArgs e) => CreateCaptcha();
        private void ApplyCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            if (RightAnswer == CaptchaEnterTextBox.Text)
            {
                swapper.SwapPannels(Captcha.Name, Main.Name);
                CaptchaEnterTextBox.Text = "";
                WrongIntupCaptchaText.Visibility = Visibility.Collapsed;
                CountOfLogIn = 1;
            }
            else
            {
                CaptchaEnterTextBox.Text = "";
                CreateCaptcha();
                WrongIntupCaptchaText.Visibility = Visibility.Visible;
            }
        }

        #endregion


        #region Registration
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RegSurnameTB.Clear();
            RegNameTB.Clear();
            RegPatronymicTB.Clear();
            RegEmailTB.Clear();
            RegLoginTB.Clear();
            RegPhoneNumberTB.Clear();
            RegPasswordTB.Clear();
            RegPasswordRepeatTB.Clear();
            //сброс выбора фото
            swapper.SwapPannels(Registration.Name, Main.Name);
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(1.ToString());
        }
        private bool ValidateAll()
        {
            return false;
        }
        #endregion

    }
}
