namespace Bars.Gkh.DataResult
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;

    /// <summary>
    /// Типизированный ListDataResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListDataResult<T> : ListDataResult
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ListDataResult()
        {
            Data = Enumerable.Empty<T>();
            TotalCount = 0;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public ListDataResult(IEnumerable<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Data
        /// </summary>
        public new IEnumerable<T> Data
        {
            get { return base.Data.As<IEnumerable<T>>(); }
            set { base.Data = value; }
        }
    }
}