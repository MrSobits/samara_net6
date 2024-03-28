namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>Маппинг для "Поставщик ресурсов"</summary>
    public class QualityLevelMap : BaseImportableEntityMap<PublicOrgServiceQualityLevel>
    {
        public QualityLevelMap() : 
                base("Показатели качества", "GKH_QUALITY_LEVEL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование показателя").Column("NAME").Length(255).NotNull();
            this.Property(x => x.Value, "Установленное значение").Column("VALUE").NotNull();
            this.Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").NotNull();
            this.Reference(x => x.ServiceOrg, "Услуга по договору поставщика ресурсов").Column("PUB_SERVORG_ID").NotNull();
        }
    }
}
