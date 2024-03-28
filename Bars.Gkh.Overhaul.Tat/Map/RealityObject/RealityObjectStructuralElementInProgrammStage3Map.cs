/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.RealityObject
/// {
///     using System.Collections.Generic;
/// 
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.DataAccess.UserTypes;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class RealityObjectStructuralElementInProgrammStage3Map : BaseEntityMap<RealityObjectStructuralElementInProgrammStage3>
///     {
///         public RealityObjectStructuralElementInProgrammStage3Map()
///             : base("OVRHL_RO_STRUCT_EL_IN_PRG_3")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.CommonEstateObjects, "CEO_STRING", true);
///             Map(x => x.Point, "POINT", true, 0m);
///             Map(x => x.IndexNumber, "INDEX_NUM", true, 0);
///             
///             Property(x => x.StoredCriteria,
///                 m =>
///                 {
///                     m.Type<ImprovedJsonSerializedType<List<StoredPriorityParam>>>();
///                     m.Column("CRITERIA");
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.RealityObjectStructuralElementInProgrammStage3"</summary>
    public class RealityObjectStructuralElementInProgrammStage3Map : BaseEntityMap<RealityObjectStructuralElementInProgrammStage3>
    {
        
        public RealityObjectStructuralElementInProgrammStage3Map() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.RealityObjectStructuralElementInProgrammStage3", "OVRHL_RO_STRUCT_EL_IN_PRG_3")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.CommonEstateObjects, "CommonEstateObjects").Column("CEO_STRING").Length(250).NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUM").NotNull();
            Property(x => x.Point, "Point").Column("POINT").DefaultValue(0m).NotNull();
            Property(x => x.StoredCriteria, "StoredCriteria").Column("CRITERIA");
        }
    }

    public class RealityObjectStructuralElementInProgrammStage3NHibernateMapping : ClassMapping<RealityObjectStructuralElementInProgrammStage3>
    {
        public RealityObjectStructuralElementInProgrammStage3NHibernateMapping()
        {
            Property(x => x.StoredCriteria,
                m =>
                {
                    m.Type<ImprovedJsonSerializedType<List<StoredPriorityParam>>>();
                });
        }
    }
}
