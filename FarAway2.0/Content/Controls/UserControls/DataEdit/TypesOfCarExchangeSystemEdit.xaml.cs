using FarAway2._0.Interfaces;
using FarAway2._0.Tools.Extensions;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace FarAway2._0.Content.Controls.UserControls.DataEdit
{
    public partial class TypesOfCarExchangeSystemEdit : UserControl, IContentDialogParent
    {
        public ContentDialog ParentDialog { get; set; }
        public TypesOfCarExchangeSystemEdit(ContentDialog CallingDialog, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
        }
        public TypesOfCarExchangeSystemEdit(ContentDialog CallingDialog, TypesOfCarExchangeSystem Instance, Func<Task> UpdateMethod)
        {
            InitializeComponent();
            ParentDialog = CallingDialog;
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseContentDialog();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
