/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Entities;
/// 
///     public class RealityObjectStructuralElementMap : BaseImportableEntityMap<RealityObjectStructuralElement>
///     {
///         public RealityObjectStructuralElementMap() : base("OVRHL_RO_STRUCT_EL")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.StructuralElement, "STRUCT_EL_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Volume, "VOLUME", true, 0m);
///             Map(x => x.Wearout, "WEAROUT", true, 0m);
///             Map(x => x.Repaired, "REPAIRED", true, false);
///             Map(x => x.LastOverhaulYear, "LAST_OVERHAUL_YEAR", true, 0);
///             Map(x => x.Name, "NAME");
///             Map(x => x.Condition, "CONDITION", true, (object)0);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
/// 
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>Маппинг для "Конструктивный элемент дома"</summary>
    public class RealityObjectStructuralElementMap : BaseImportableEntityMap<RealityObjectStructuralElement>
    {
        public RealityObjectStructuralElementMap() :
                base("Конструктивный элемент дома", "OVRHL_RO_STRUCT_EL")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RealityObject, "Объект недвижимости").Column("RO_ID").NotNull();
            this.Reference(x => x.StructuralElement, "Конструктивный элемент").Column("STRUCT_EL_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.Volume, "Объем").Column("VOLUME").DefaultValue(0m).NotNull();
            this.Property(x => x.Repaired, "Флаг: ремонт проведен").Column("REPAIRED").DefaultValue(false).NotNull();
            this.Property(x => x.LastOverhaulYear, "Год последнего ремонта").Column("LAST_OVERHAUL_YEAR").NotNull();
            this.Property(x => x.Wearout, "Износ").Column("WEAROUT").DefaultValue(0m).NotNull();
            this.Property(x => x.WearoutActual, "Износ").Column("WEAROUT_ACUAL").DefaultValue(0m).NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(250);
            this.Property(x => x.Condition, "Состояние КЭ").Column("CONDITION").DefaultValue(0).NotNull();
            this.Property(x => x.SystemType, "Тип системы").Column("SYSTEM_TYPE").DefaultValue(0).NotNull();
            this.Property(x => x.NetworkLength, "Протяженность сетей").Column("NETWORK_LENGTH").Length(50);
            this.Property(x => x.NetworkPower, "Мощность").Column("NETWORK_POWER").Length(50);
            this.Reference(x => x.FileInfo, "Файл").Column("STRUCT_EL_FILE_ID");
        }
    }
}
