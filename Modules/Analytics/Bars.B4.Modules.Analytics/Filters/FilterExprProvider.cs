namespace Bars.B4.Modules.Analytics.Filters
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 
    /// </summary>
    public abstract class FilterExprProvider
    {
        /// <summary>
        /// Ключ для регистрации в контейнере.
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract Expression GetExpression(Type queryType);
    }
}
