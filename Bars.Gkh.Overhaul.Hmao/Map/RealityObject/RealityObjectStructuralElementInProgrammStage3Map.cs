/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using System.Collections.Generic;
/// 
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.DataAccess.UserTypes;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class RealityObjectStructuralElementInProgrammStage3Map : BaseImportableEntityMap<RealityObjectStructuralElementInProgrammStage3>
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
/// 
///             Property(x => x.StoredPointParams,
///                 m =>
///                 {
///                     m.Type<ImprovedJsonSerializedType<List<StoredPointParam>>>();
///                     m.Column("POINT_PARAMS");
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Конструктивный элемент дома в ДПКР"</summary>
    public class RealityObjectStructuralElementInProgrammStage3Map : BaseImportableEntityMap<RealityObjectStructuralElementInProgrammStage3>
    {
        
        public RealityObjectStructuralElementInProgrammStage3Map() : 
                base("Конструктивный элемент дома в ДПКР", "OVRHL_RO_STRUCT_EL_IN_PRG_3")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Year, "Плановый Год").Column("YEAR").NotNull();
            Property(x => x.CommonEstateObjects, "Строка объектов общего имущества").Column("CEO_STRING").Length(250).NotNull();
            Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            Property(x => x.IndexNumber, "Порядковый номер").Column("INDEX_NUM").NotNull();
            Property(x => x.Point, "Балл").Column("POINT").DefaultValue(0m).NotNull();
            Property(x => x.StoredCriteria, "Значения критериев сортировки").Column("CRITERIA");
            Property(x => x.StoredPointParams, "Значения параметров очередности по баллам").Column("POINT_PARAMS");
        }
    }

    public class RealityObjectStructuralElementInProgrammStage3NHibernateMapping : ClassMapping<RealityObjectStructuralElementInProgrammStage3>
    {
        public RealityObjectStructuralElementInProgrammStage3NHibernateMapping()
        {
            Property(
                x => x.StoredCriteria,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<StoredPriorityParam>>>();
                    });

            Property(
                x => x.StoredPointParams,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<StoredPointParam>>>();
                    });
        }
    }
}
