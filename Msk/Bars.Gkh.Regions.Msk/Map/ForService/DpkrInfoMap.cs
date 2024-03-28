/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.UserTypes;
///     using B4.DataAccess.ByCode;
///     using Regions.Msk.Entities;
/// 
///     public class DpkrInfoMap : BaseEntityMap<DpkrInfo>
///     {
///         public DpkrInfoMap()
///             : base("MSK_DPKR_INFO")
///         {
///             Map(x => x.CeoType, "CEO_TYPE");
///             Map(x => x.CeoState, "CEO_STATE");
///             Map(x => x.Delay, "DELAY");
///             Map(x => x.LifeTime, "LIFETIME");
///             Map(x => x.LastRepairYear, "LAST_REPAIR_YEAR");
///             Map(x => x.Period, "PERIOD");
///             Map(x => x.Point, "POINT");
/// 
///             References(x => x.RealityObjectInfo, "RO_INFO_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Property(x => x.CeoStates,
///                 m =>
///                 {
///                     m.Type<JsonSerializedType<List<string>>>();
///                     m.Column("CEO_STATES");
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Msk.Map
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Msk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Regions.Msk.Entities.DpkrInfo"</summary>
    public class DpkrInfoMap : BaseEntityMap<DpkrInfo>
    {
        
        public DpkrInfoMap() : 
                base("Bars.Gkh.Regions.Msk.Entities.DpkrInfo", "MSK_DPKR_INFO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObjectInfo, "RealityObjectInfo").Column("RO_INFO_ID").NotNull().Fetch();
            Property(x => x.CeoType, "CeoType").Column("CEO_TYPE");
            Property(x => x.CeoState, "CeoState").Column("CEO_STATE");
            Property(x => x.CeoStates, "CeoStates").Column("CEO_STATES");
            Property(x => x.Delay, "Delay").Column("DELAY");
            Property(x => x.LifeTime, "LifeTime").Column("LIFETIME");
            Property(x => x.LastRepairYear, "LastRepairYear").Column("LAST_REPAIR_YEAR");
            Property(x => x.Period, "Period").Column("PERIOD").Length(250);
            Property(x => x.Point, "Point").Column("POINT");
        }
    }

    public class DpkrInfoNHibernateMapping : ClassMapping<DpkrInfo>
    {
        public DpkrInfoNHibernateMapping()
        {
            Property(
                x => x.CeoStates,
                m =>
                    {
                        m.Type<JsonSerializedType<List<string>>>();
                    });
        }
    }
}
