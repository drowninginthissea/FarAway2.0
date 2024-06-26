﻿using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FarAway2._0.Content.Controls.CustomControls
{
    public class PhotoSelector : Control
    {
        public event EventHandler OnConfigured;
        private void RaiserOnConfiguredEvent()
        {
            OnConfigured?.Invoke(this, EventArgs.Empty);
        }

        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register(
                "TextSize",
                typeof(double),
                typeof(PhotoSelector),
                new PropertyMetadata(12.0, OnTextSizePropertyChanged));
        private static void OnTextSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PhotoSelector photoSelector)
                photoSelector.textBlock.FontSize = (double)e.NewValue;
        }
        static PhotoSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PhotoSelector), new FrameworkPropertyMetadata(typeof(PhotoSelector)));
        }
        Grid grid;
        Image img;
        TextBlock textBlock;
        Rectangle rectangle;
        Button ChooseImageButton;
        public PhotoSelector()
        {
            DefaultStyleKey = typeof(PhotoSelector);
            grid = new Grid();
            img = new Image();
            textBlock = new TextBlock();
            rectangle = new Rectangle();
            ConfigureFields();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                ChooseImageButton = Template.FindName("ChooseImageButton", this) as Button;
                ChooseImageButton.Click += ChooseImageButton_Click;
                ChooseImageButton.MouseEnter += ChooseImageButton_MouseEnter;
                ChooseImageButton.MouseLeave += ChooseImageButton_MouseLeave;
                ChooseImageButton.Content = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = TextSize,
                    Text = "Выбор изображения"
                };
            }
            RaiserOnConfiguredEvent();
        }
        private void SetContentImageButton()
        {
            ChooseImageButton.Content = grid;
        }
        private void ResetContentImageButton()
        {
            ChooseImageButton.Content = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = TextSize,
                Text = "Выбор изображения"
            };
        }
        public void SetImage(byte[] Photo)
        {
            using (MemoryStream stream = new MemoryStream(Photo))
            {
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.StreamSource = stream;
                imageSource.EndInit();

                img.Source = imageSource;
            }
            SetContentImageButton();
        }
        private void ConfigureFields()
        {
            textBlock.Text = "Выбор изображения";
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Visibility = Visibility.Collapsed;
            textBlock.FontSize = TextSize;
            rectangle.Width = img.Width;
            rectangle.Height = img.Height;
            grid.Children.Add(img);
            grid.Children.Add(textBlock);
            grid.Children.Add(rectangle);
        }
        public void SetImage(string FileName)
        {
            img.Source = new BitmapImage(new Uri(FileName));
            SetContentImageButton();
        }
        public byte[] GetImage()
        {
            if (img == null) return null;
            if (img.Source is BitmapSource bitmapSource)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            return null;
        }
        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Picture = new OpenFileDialog();
            Picture.Filter = "Image files|*.jpg;*.png;*.jpeg";
            if (Picture.ShowDialog() == true)
            {
                SetImage(Picture.FileName);
            }
        }
        private void ChooseImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ChooseImageButton.Content == grid)
            {
                textBlock.Visibility = Visibility.Visible;
                rectangle.Fill = (Brush)new BrushConverter().ConvertFrom("#643682C7");
            }
        }
        private void ChooseImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ChooseImageButton.Background = (Brush)new BrushConverter().ConvertFrom("#00BEBEBE");
            if (ChooseImageButton.Content == grid)
            {
                textBlock.Visibility = Visibility.Collapsed;
                rectangle.Fill = (Brush)new BrushConverter().ConvertFrom("#003682C7");
            }
        }
        public bool IsImageSet() => img.Source == null ? false : true;
        public void ResetImage()
        {
            ResetContentImageButton();
        }
    }
}