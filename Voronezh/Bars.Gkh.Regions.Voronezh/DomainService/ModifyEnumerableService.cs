namespace Bars.Gkh.Regions.Voronezh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис модификации коллекции <see cref="IEnumerable{T}"/>
    /// </summary>
    public class ModifyEnumerableService : IModifyEnumerableService
    {
        /// <inheritdoc />
        public IEnumerable<T> ReplaceProperty<T>(IEnumerable<T> collection, string strValue, params Expression<Func<T, string>>[] propertySelector)
            where T : new()
        {
            return this.ReplaceProperty(collection, strValue, string.Empty, propertySelector);
        }

        /// <inheritdoc />
        public IEnumerable<T> ReplaceProperty<T>(IEnumerable<T> collection, string oldValue, string newValue, params Expression<Func<T, string>>[] propertySelector)
            where T : new()
        {
            if (propertySelector == null)
            {
                return collection;
            }

            return propertySelector.Aggregate(collection, (q, p) => q.ReplaceStringValue(p, oldValue, newValue));
        }
    }
}