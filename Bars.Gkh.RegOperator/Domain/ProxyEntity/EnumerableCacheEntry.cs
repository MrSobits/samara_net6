namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    using System.Collections.Generic;

    public class EnumerableCacheEntry<T>
    {
        public string Id { get; set; }

        public List<T> Values { get; set; }

        public EnumerableCacheEntry()
        {
            Values = new List<T>();
        }
    }
}