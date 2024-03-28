namespace Bars.B4.Modules.Analytics.Filters
{
    using System.Collections.Generic;

    /// <summary>
    /// Системный фильтр
    /// </summary>
    public class SystemFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public SystemFilterGroup Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExprProviderKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SystemFilter> Filters { get; set; }
    }
}
