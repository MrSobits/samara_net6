/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tatarstan.Map
/// {
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class GisPaymentMap : BaseEntityMap<GisPayment>
///     {
///         public GisPaymentMap() : base("GJI_TAT_GIS_PAYMENT")
///         {
///             Map(x => x.DateRecieve, "DATE_RECIEVE", true);
///             Map(x => x.Uip, "CUIP", true);
///             References(x => x.PayFine, "PAYFINE_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Property(x => x.JsonObject, m =>
///             {
///                 m.Column("JOBJ");
///                 m.NotNullable(true);
///                 m.Type<ImprovedJsonSerializedType<GisPaymentJson>>();
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.GisPayment"</summary>
    public class GisPaymentMap : BaseEntityMap<GisPayment>
    {
        
        public GisPaymentMap() : 
                base("Bars.GkhGji.Regions.Tatarstan.Entities.GisPayment", "GJI_TAT_GIS_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateRecieve, "DateRecieve").Column("DATE_RECIEVE").NotNull();
            Property(x => x.Uip, "Uip").Column("CUIP").Length(250).NotNull();
            Reference(x => x.PayFine, "PayFine").Column("PAYFINE_ID").NotNull().Fetch();
            Property(x => x.JsonObject, "JsonObject").Column("JOBJ").NotNull();
        }
    }

    public class GisPaymentNHibernateMapping : ClassMapping<GisPayment>
    {
        public GisPaymentNHibernateMapping()
        {
            Property(
                x => x.JsonObject,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<GisPaymentJson>>();
                    });
        }
    }
}
