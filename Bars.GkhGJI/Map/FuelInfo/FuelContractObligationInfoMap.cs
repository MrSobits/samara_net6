namespace Bars.GkhGji.Map.FuelInfo
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для Выполнение договорных обязательств по поставкам топлива
    /// </summary>
    public class FuelContractObligationInfoMap : JoinedSubClassMap<FuelContractObligationInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelContractObligationInfoMap()
            : base("Bars.GkhGji.Entities.FuelInfo.FuelContractObligationInfo", "GJI_FUEL_CONTRACT_OBLIG_INFO")
        {
        }

        /// <inheritdoc/>
        protected override void Map()
        {
            this.Property(x => x.CoalTotal, "Уголь Всего").Column("COAL_TOTAL");
            this.Property(x => x.CoalDirectContract, "Уголь в том числе по прямым договорам").Column("COAL_DIRECT_CONTRACT");
            this.Property(x => x.CoalIntermediator, "Уголь в том числе через посредников").Column("COAL_INTERMEDIATOR");
            this.Property(x => x.FirewoodTotal, "Дрова Всего").Column("FIREWOOD_TOTAL");
            this.Property(x => x.FirewoodDirectContract, "Дрова в том числе по прямым договорам").Column("FIREWOOD_DIRECT_CONTRACT");
            this.Property(x => x.FirewoodIntermediator, "Дрова в том числе через посредников").Column("FIREWOOD_INTERMEDIATOR");
            this.Property(x => x.MasutTotal, "Мазут Всего").Column("MASUT_TOTAL");
            this.Property(x => x.MasutDirectContract, "Мазут в том числе по прямым договорам").Column("MASUT_DIRECT_CONTRACT");
            this.Property(x => x.MasutIntermediator, "Мазут в том числе через посредников").Column("MASUT_INTERMEDIATOR");
            this.Property(x => x.GasTotal, "Газ Всего").Column("GAS_TOTAL");
            this.Property(x => x.GasDirectContract, "Газ в том числе по прямым договорам").Column("GAS_DIRECT_CONTRACT");
            this.Property(x => x.GasIntermediator, "Газ в том числе через посредников").Column("GAS_INTERMEDIATOR");
            this.Property(x => x.OtherTotal, "Другие виды топлива Всего").Column("OTHER_TOTAL");
            this.Property(x => x.OtherDirectContract, "Другие виды топлива в том числе по прямым договорам").Column("OTHER_DIRECT_CONTRACT");
            this.Property(x => x.OtherIntermediator, "Другие виды топлива в том числе через посредников").Column("OTHER_INTERMEDIATOR");
            this.Property(x => x.PeatTotal, "Торф Всего").Column("PEAT_TOTAL");
            this.Property(x => x.PeatDirectContract, "Торф в том числе по прямым договорам").Column("PEAT_DIRECT_CONTRACT");
            this.Property(x => x.PeatIntermediator, "Торф в том числе через посредников").Column("PEAT_INTERMEDIATOR");
            this.Property(x => x.LiquefiedGasTotal, "Сжиженный газ Всего").Column("LIQUEFIED_GAS_TOTAL");
            this.Property(x => x.LiquefiedGasDirectContract, "Сжиженный газ в том числе по прямым договорам").Column("LIQUEFIED_GAS_DIRECT_CONTRACT");
            this.Property(x => x.LiquefiedGasIntermediator, "Сжиженный газ в том числе через посредников").Column("LIQUEFIED_GAS_INTERMEDIATOR");
            this.Property(x => x.WoodWasteTotal, "Древесные отходы Всего").Column("WOOD_WASTE_TOTAL");
            this.Property(x => x.WoodWasteDirectContract, "Древесные отходы в том числе по прямым договорам").Column("WOOD_WASTE_DIRECT_CONTRACT");
            this.Property(x => x.WoodWasteIntermediator, "Древесные отходы в том числе через посредников").Column("WOOD_WASTE_INTERMEDIATOR");
            this.Property(x => x.DieselTotal, "Дизельное топливо Всего").Column("DIESEL_TOTAL");
            this.Property(x => x.DieselDirectContract, "Дизельное топливо в том числе по прямым договорам").Column("DIESEL_DIRECT_CONTRACT");
            this.Property(x => x.DieselIntermediator, "Дизельное топливо в том числе через посредников").Column("DIESEL_INTERMEDIATOR");
            this.Property(x => x.ElectricTotal, "Эл. энергия Всего").Column("ELECTRIC_TOTAL");
            this.Property(x => x.ElectricDirectContract, "Эл. энергия в том числе по прямым договорам").Column("ELECTRIC_DIRECT_CONTRACT");
            this.Property(x => x.ElectricIntermediator, "Эл. энергия в том числе через посредников").Column("ELECTRIC_INTERMEDIATOR");
        }
    }
}