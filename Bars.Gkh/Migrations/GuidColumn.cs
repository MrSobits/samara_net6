namespace Bars.Gkh.Migrations
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// 
    /// </summary>
    public class GuidColumn : Column
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columnProperty"></param>
        public GuidColumn(string name, ColumnProperty columnProperty)
            : base(name, DbType.Guid, columnProperty)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public GuidColumn(string name) : this(name, ColumnProperty.None)
        {

        }
    }
}