using FarAway2._0.Interfaces;

namespace FarAway2._0.Tools.Extensions
{
    public static class ContentDialogExtensions
    {
        public static void CloseContentDialog(this IContentDialogParent InstanceWithInterface)
        {
            if (InstanceWithInterface.ParentDialog != null)
            {
                InstanceWithInterface.ParentDialog.Hide();
            }
        }
    }
}
