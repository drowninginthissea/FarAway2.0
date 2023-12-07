using System.Windows.Controls;
using ModernWpf.Controls;

namespace FarAway2._0.Content.Controls.UserControls
{
    public partial class AccountControl : UserControl
    {
        public Users User { get; set; }
        public AccountControl(Users user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;

            AvatarSelector.Loaded += AvatarSelector_Loaded;
        }

        private void AvatarSelector_Loaded(object sender, RoutedEventArgs e)
        {
            AvatarSelector.SetImage(User.Photo);
        }
        private double GetSpacing()
        {
            double HeightOfTBs = SurnameTB.ActualHeight * 4;
            double EmptySpace = LeftSideContainer.ActualHeight - HeightOfTBs;
            return EmptySpace / 3;
        }
        private void TextBoxesGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            LeftStackPanel.Spacing = GetSpacing();
            RightStackPanel.Spacing = GetSpacing();
        }
    }
}
