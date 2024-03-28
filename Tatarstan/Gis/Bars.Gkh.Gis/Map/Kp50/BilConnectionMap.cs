namespace Bars.Gkh.Gis.Map.Kp50
{
    using B4.Modules.Mapping.Mappers;
    using Entities.Kp50;

    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Kp50.BilDictSchema"</summary>
    public class BilConnectionMap : BaseEntityMap<BilConnection>
    {
        public BilConnectionMap()
            :
                base("Bars.Gkh.Gis.Entities.Kp50.BilConnection", "BIL_CONNECTION")
        {
        }

        protected override void Map()
        {
            Property(x => x.Connection, "Connection").Column("CONNECTION").NotNull();
            Property(x => x.ConnectionType, "ConnectionType").Column("CONNECTION_TYPE").NotNull();
            Property(x => x.AppUrl, "AppUrl").Column("APP_URL").NotNull();
        }
    }
}
