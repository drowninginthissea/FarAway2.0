using System.Windows.Controls;

namespace FarAway2._0.BaseClasses
{
    public class SearchableTableView : UserControl, ISearchable
    {
        private string _textToSearch = "";
        public string TextToSearch
        {
            get { return _textToSearch; }
            set
            {
                _textToSearch = value;
                UpdateDataAsync();
            }
        }

        /// <summary>
        /// It is necessary to override this method in inheritor classes
        /// </summary>
        public virtual Task UpdateDataAsync()
        {
            return null;
        }
    }

}
