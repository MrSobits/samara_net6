/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.CommonEstateObject
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.CommonEstateObject;
/// 
///     public class StructuralElementMap : BaseImportableEntityMap<StructuralElement>
///     {
///         public StructuralElementMap()
///             : base("OVRHL_STRUCT_EL")
///         {
///             References(x => x.Group, "GROUP_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Name, "NAME", true);
///             Map(x => x.Code, "SE_CODE", true);
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.LifeTime, "LIFE_TIME", true);
///             Map(x => x.LifeTimeAfterRepair, "LIFE_TIME_AFTER_REPAIR", true, 0);
///             References(x => x.NormativeDoc, "NORM_DOC_ID");
///             Map(x => x.MutuallyExclusiveGroup, "MUT_EXCLUS_GROUP", false, 300);
///             Map(x => x.CalculateBy, "CALCULATE_BY", true, (object) 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.CommonEstateObject
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums;
    
    
    /// <summary>Маппинг для "Конструктивный элемент"</summary>
    public class StructuralElementMap : BaseImportableEntityMap<StructuralElement>
    {
        
        public StructuralElementMap() : 
                base("Конструктивный элемент", "OVRHL_STRUCT_EL")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Group, "Группа").Column("GROUP_ID").NotNull().Fetch();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            this.Property(x => x.Code, "Код").Column("SE_CODE").Length(250).NotNull();
            this.Reference(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE_ID").NotNull().Fetch();
            this.Property(x => x.LifeTime, "Срок эксплуатации").Column("LIFE_TIME").NotNull();
            this.Property(x => x.LifeTimeAfterRepair, "Срок эксплуатации после ремонта").Column("LIFE_TIME_AFTER_REPAIR").NotNull();
            this.Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORM_DOC_ID");
            this.Property(x => x.MutuallyExclusiveGroup, "Группа взаимоисключаемости").Column("MUT_EXCLUS_GROUP").Length(300);
            this.Property(x => x.CalculateBy, "Значение при вычислении стоимости ремонта").Column("CALCULATE_BY").DefaultValue(PriceCalculateBy.Volume).NotNull();
            this.Property(x => x.ReformCode, "Код реформы ЖКХ").Column("REFORM_CODE").Length(300);
        }

        /// <summary>ReadOnly ExportId</summary>
        public class StructuralElementNhMapping : BaseHaveExportIdMapping<StructuralElement>
        {
        }
    }
}
