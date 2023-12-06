using FarAway2._0.Interfaces;
using FarAway2._0.Tools.Extensions;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.DataEdit
{
    public partial class FrequencyOfServicesEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        public FrequencyOfServicesEdit(ContentDialog CallingDialog)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
        }
        public FrequencyOfServicesEdit(ContentDialog CallingDialog, FrequencyOfServices Instance)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseContentDialog();
        }
    }
}
