/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tatarstan.Map
/// {
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class GisChargeToSendMap : BaseEntityMap<GisChargeToSend>
///     {
///         public GisChargeToSendMap() : base("GJI_TAT_GIS_CHARGE")
///         {
///             Map(x => x.DateSend, "DATE_SEND");
///             Map(x => x.IsSent, "IS_SENT", true, false);
/// 
///             References(x => x.Resolution, "RESOL_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Property(x => x.JsonObject, m =>
///             {
///                 m.Column("JOBJ");
///                 m.NotNullable(true);
///                 m.Type<ImprovedJsonSerializedType<GisChargeJson>>();
///             });
/// 
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.GisChargeToSend"</summary>
    public class GisChargeToSendMap : BaseEntityMap<GisChargeToSend>
    {
        
        public GisChargeToSendMap() : 
                base("Bars.GkhGji.Regions.Tatarstan.Entities.GisChargeToSend", "GJI_TAT_GIS_CHARGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Document, "Document").Column("DOC_ID").NotNull().Fetch();
            Property(x => x.DateSend, "DateSend").Column("DATE_SEND");
            Property(x => x.IsSent, "IsSent").Column("IS_SENT").DefaultValue(false).NotNull();
            Property(x => x.JsonObject, "JsonObject").Column("JOBJ").NotNull();
            Property(x => x.SendLog, "SendLog").Column("SEND_LOG");
        }
    }

    public class GisChargeToSendNHibernateMapping : ClassMapping<GisChargeToSend>
    {
        public GisChargeToSendNHibernateMapping()
        {
            Property(
                x => x.JsonObject,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<GisChargeJson>>();
                    });
        }
    }
}
