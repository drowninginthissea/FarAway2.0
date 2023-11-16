using FarAway2._0.Entities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FarAway2._0.Windows
{
    /// <summary>
    /// Interaction logic for AuthAndReg.xaml
    /// </summary>
    public partial class AuthAndReg : Window
    {
        private int CountOfLigIn;

        private static AuthAndReg ThisWindows;
        public AuthAndReg()
        {
            InitializeComponent();
            SettingsWindow();
        }
        private void SettingsWindow()
        {
            ThisWindows = this;
            CountOfLigIn = 1;

            CreateCaptcha();
        }


        #region Window


        private void MovingWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ThisWindows.DragMove();
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
        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
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
        private void CopyPasswordToTwoBoxes() =>
            _ = PasswordBoxForPassword.Visibility == Visibility.Visible ?
                TextBoxForPassword.Text = PasswordBoxForPassword.Password :
                PasswordBoxForPassword.Password = TextBoxForPassword.Text;
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPasswordToTwoBoxes();
            if (DbUtils.Authorization(LoginTextBox.Text, PasswordBoxForPassword.Password))
            {
                //открытие окна основного
                return;
            }
            if (CountOfLigIn == 3)
            {
                Tools.SwapPannelToCaptcha(Main, TimeSpan.FromSeconds(1), Captcha, CreateCaptcha);
            }
            CountOfLigIn++;
            WrongLogOrPass.Visibility = Visibility.Visible;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.SwapPannels(Main, TimeSpan.FromSeconds(1), Registration);
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
                Tools.SwapPannels(Captcha, TimeSpan.FromSeconds(1), Main);
                CaptchaEnterTextBox.Text = "";
                CountOfLigIn = 1;
            }
            else
            {
                CaptchaEnterTextBox.Text = "";
                CreateCaptcha();
            }
        }


        #endregion

    }
}