using ModernWpf.Controls;

namespace FarAway2._0.Tools
{
    public class StaticTools
    {
        public static void OpenDialog(ContentDialog Dialog, IContentDialogParent Control)
        {
            Dialog.Content = Control;
            Dialog.ShowAsync();
        }
    }
}
