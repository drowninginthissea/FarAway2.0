using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FarAway2._0.Tools
{
    internal class PanelSwapper
    {
        private Dictionary<string, FrameworkElement> _panels;
        public Duration DurationTime;
        public FrameworkElement this[string PanelName]
        {
            get
            {
                if (_panels != null && _panels.ContainsKey(PanelName))
                {
                    return _panels[PanelName];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                _panels[PanelName] = value;
            }
        }
        public PanelSwapper(double AnimationTime)
        {
            _panels = new Dictionary<string, FrameworkElement>();
            DurationTime = TimeSpan.FromSeconds(AnimationTime);
        }
        public void Add(FrameworkElement Panel) => this[Panel.Name] = Panel;
        public bool Delete(string Name) => _panels.Remove(Name);
        private void ChangeZIndex(string PanelToUp, string PanelToDown)
        {
            Panel.SetZIndex(this[PanelToUp], 5);
            Panel.SetZIndex(this[PanelToDown], 1);
        }
        public void SwapPannels(string HidePanel, string OpenPanel, Action OnComplited = null)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = DurationTime,
            };
            animation.Completed += (sender, e) =>
            {
                DoubleAnimation animation1 = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = DurationTime,
                };
                this[OpenPanel].BeginAnimation(UIElement.OpacityProperty, animation1);

                OnComplited?.Invoke();
            };
            this[HidePanel].BeginAnimation(UIElement.OpacityProperty, animation);
            ChangeZIndex(OpenPanel, HidePanel);
        }
    }
}
