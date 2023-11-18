using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace FarAway2._0.Tools
{
    internal static class Helper
    {
        #region Animations
        public static void SwapPannels(UIElement FirstPanel, Duration Duration, UIElement SecondPanel) // первая панель - скрывается, вторая появляется
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = Duration,
            };
            animation.Completed += (sender, e) =>
            {
                DoubleAnimation animation1 = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = Duration
                };
                SecondPanel.BeginAnimation(UIElement.OpacityProperty, animation1);
            };
            FirstPanel.BeginAnimation(UIElement.OpacityProperty, animation);
            ChangeZIndexes(SecondPanel, FirstPanel);
        }

        public static void SwapPannelToCaptcha(UIElement FirstPanel, Duration Duration, UIElement SecondPanel, Action CaptchaCreateMethod) // первая панель - скрывается, вторая появляется, вторая всегда Captcha
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = Duration,
            };
            animation.Completed += (sender, e) =>
            {
                DoubleAnimation animation1 = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = Duration
                };
                SecondPanel.BeginAnimation(UIElement.OpacityProperty, animation1);

                CaptchaCreateMethod.Invoke();
                //Метод создания капчи

            };
            FirstPanel.BeginAnimation(UIElement.OpacityProperty, animation);
            ChangeZIndexes(SecondPanel, FirstPanel);
        }

        private static void ChangeZIndexes(UIElement FirstPanel, UIElement SecondPanel) // первыя наверх, вторая назад
        {
            Panel.SetZIndex(FirstPanel, 5);
            Panel.SetZIndex(SecondPanel, 1);
        }
        #endregion
    }
}
