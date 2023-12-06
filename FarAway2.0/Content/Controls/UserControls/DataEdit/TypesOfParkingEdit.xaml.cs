using FarAway2._0.Interfaces;
using FarAway2._0.Tools.Extensions;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.DataEdit
{
    public partial class TypesOfParkingEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }

        public TypesOfParkingEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
        }
        public TypesOfParkingEdit(ContentDialog CallingDialog, TypesOfParking Instance, Func<Task> UpdateMethod)
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
