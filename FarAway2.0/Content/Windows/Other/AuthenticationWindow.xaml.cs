using FarAway2._0.Tools;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
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
        private void ChangeControlsStates(bool State, params UIElement[] Controls)
        {
            foreach (UIElement control in Controls)
            {
                control.IsEnabled = State;
            }
        }
        private void ChangeControls(Action<FrameworkElement> action, params FrameworkElement[] Controls)
        {
            foreach (FrameworkElement control in Controls)
                action?.Invoke(control);
        }

        #region Window

        private void MovingWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void TopPannelButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "CloseButton")
                Application.Current.Shutdown();
            if (((Button)sender).Name == "RollDownButton")
                WindowState = WindowState.Minimized;
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

            ChangeControlsStates(false, EnterButton, RegistrationButton, LoginTextBox, PasswordBoxForPassword, TextBoxForPassword);
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
                    ChangeControlsStates(true, EnterButton, RegistrationButton, LoginTextBox, PasswordBoxForPassword, TextBoxForPassword);
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
            ChangeControls(x => ((TextBox)x).Clear(), 
                RegSurnameTB, 
                RegNameTB, 
                RegPatronymicTB, 
                RegEmailTB, 
                RegLoginTB, 
                RegPhoneNumberTB);
            ChangeControls(x => ((PasswordBox)x).Clear(),
                RegPasswordTB,
                RegPasswordRepeatTB);
            //сброс выбора фото
            swapper.SwapPannels(Registration.Name, Main.Name);
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAll())
                return;
            MessageBox.Show(1.ToString());
        }
        private bool ValidateAll()
        {
            if (!ValidateLeftSide())
                return false;
            return true;
        }
        private bool ValidateLeftSide()
        {
            if (string.IsNullOrWhiteSpace(RegSurnameTB.Text) || RegSurnameTB.Text.Length > 40)
            {
                MessageBox.Show("Фамилия пользователя не введена, либо лимит символов превышен (40)!", "Ошибка");
                return false;
            }
            if (string.IsNullOrWhiteSpace(RegNameTB.Text) || RegNameTB.Text.Length > 30)
            {
                MessageBox.Show("Имя пользователя не введено, либо лимит символов превышен (30)!", "Ошибка");
                return false;
            }
            if (RegPatronymicTB.Text.Length > 50)
            {
                MessageBox.Show("Лимит символов на ввод отчества (50) превышен!", "Ошибка");
                return false;
            }
            if (!ValidateEmail(RegEmailTB.Text))
                return false;
            if (string.IsNullOrWhiteSpace(RegLoginTB.Text) || RegLoginTB.Text.Length > 30)
            {
                MessageBox.Show("Логин пользователя не введён, либо лимит символов превышен (30)!", "Ошибка");
                return false;
            }
            if (!RegPhoneNumberTB.IsMaskCompleted)
            {
                MessageBox.Show("Номер телефона введён не полностью!", "Ошибка");
                return false;
            }
            return true;
        }
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Электронная почта не введена, либо введены пустые символы!", "Ошибка");
                return false;
            }
            if (!Regex.IsMatch(email, pattern))
            {
                MessageBox.Show("Электронная почта введена неверно! Проверьте правильность ввода!", "Ошибка");
                return false;
            }
            return true;
        }
        private bool ReturnFalseWithError(string Text, string Header)
        {
            MessageBox.Show(Text, Header);
            return false;
        }
        #endregion

    }
}
