namespace Bars.B4.Modules.Analytics.Reports.Params
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public static class CatalogRegistry
    {
        private static readonly Dictionary<string, Catalog> Catalogs = new Dictionary<string, Catalog>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalog"></param>
        public static void Add(Catalog catalog)
        {
            var key = catalog.Id.ToLower();
            if (Catalogs.ContainsKey(key))
            {
                Catalogs[key] = catalog;
            }
            else
            {
                Catalogs.Add(key, catalog);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Catalog Get(string id)
        {
            return Catalogs[id.ToLower()];
        }

        public static IEnumerable<Catalog> GetAll()
        {
            return Catalogs.Values;
        }
    }
}
