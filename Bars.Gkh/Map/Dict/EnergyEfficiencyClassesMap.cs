namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для "Классы энергетической эффективности"
    /// </summary>
    public class EnergyEfficiencyClassesMap : BaseEntityMap<EnergyEfficiencyClasses>
    {
        public EnergyEfficiencyClassesMap() : 
                base("Классы энергетической эффективности", "GKH_DICT_ENERGY_EFFICIENCY_CLASSES")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Designation, "Обозначение").Column("DESIGNATION").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            this.Property(x => x.DeviationValue, "Величина отклонения").Column("DEVIATION_VALUE").NotNull();
        }
    }
}
