namespace Bars.B4.Modules.Analytics.Maps
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.B4.Modules.Analytics.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class StoredFilterMap : BaseEntityMap<StoredFilter>
    {
        public StoredFilterMap()
            : base("AL_STORED_FILTER")
        {
            Map(x => x.Name, "NAME");
            Map(x => x.Description, "DESCRIPTION");
            Map(x => x.Filter, "DATA_FILTER");
            Map(x => x.ProviderKey, "PROVIDER_KEY");
        }
    }
}
