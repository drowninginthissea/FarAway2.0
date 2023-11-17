using System;
using System.Collections.Generic;

namespace FarAway2._0.Entities
{
    public partial class ListOfActions
    {
        public ListOfActions()
        {
            HistoryOfFreezing = new HashSet<HistoryOfFreezing>();
        }

        public int id { get; set; }
        public string ActionName { get; set; } = null!;

        public virtual ICollection<HistoryOfFreezing> HistoryOfFreezing { get; set; }
    }
}
