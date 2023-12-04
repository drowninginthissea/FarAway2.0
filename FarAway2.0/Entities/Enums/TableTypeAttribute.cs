using System.Reflection;

namespace FarAway2._0.Entities.Enums
{
    internal class TableTypeAttribute : Attribute
    {
        public TableTypes Type { get; set; }
        public TableTypeAttribute(TableTypes Type)
        {
            this.Type = Type;
        }
        public static TableTypes GetAttribute(TableNames value)
        {
            FieldInfo FieldInfo = value.GetType().GetField(value.ToString());
            TableTypeAttribute Attribute = 
                FieldInfo.GetCustomAttribute(typeof(TableTypeAttribute), false) as TableTypeAttribute;
            return Attribute.Type;
        }
    }
}
