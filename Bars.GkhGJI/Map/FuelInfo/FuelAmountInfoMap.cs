namespace Bars.GkhGji.Map.FuelInfo
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Мапиинг для Поставка, расход и остатки топлива
    /// </summary>
    public class FuelAmountInfoMap : JoinedSubClassMap<FuelAmountInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelAmountInfoMap()
            : base("Bars.GkhGji.Entities.FuelInfo.FuelAmountInfo", "GJI_FUEL_AMOUNT_INFO")
        {
        }

        ///<inheritdoc/>
        protected override void Map()
        {
            this.Property(x => x.CoalTotal, "Уголь Всего").Column("COAL_TOTAL");
            this.Property(x => x.CoalPrimary, "Уголь в том числе основное").Column("COAL_PRIMARY");
            this.Property(x => x.CoalReserve, "Уголь в том числе резервное").Column("COAL_RESERVE");
            this.Property(x => x.FirewoodTotal, "Дрова Всего").Column("FIREWOOD_TOTAL");
            this.Property(x => x.FirewoodPrimary, "Дрова в том числе основное").Column("FIREWOOD_PRIMARY");
            this.Property(x => x.FirewoodReserve, "Дрова в том числе резервное").Column("FIREWOOD_RESERVE");
            this.Property(x => x.MasutTotal, "Мазут Всего").Column("MASUT_TOTAL");
            this.Property(x => x.MasutPrimary, "Мазут в том числе основное").Column("MASUT_PRIMARY");
            this.Property(x => x.MasutReserve, "Мазут в том числе резервное").Column("MASUT_RESERVE");
            this.Property(x => x.GasTotal, "Газ Всего").Column("GAS_TOTAL");
            this.Property(x => x.GasPrimary, "Газ в том числе основное").Column("GAS_PRIMARY");
            this.Property(x => x.GasReserve, "Газ в том числе резервное").Column("GAS_RESERVE");
            this.Property(x => x.OtherTotal, "Другие виды топлива Всего").Column("OTHER_TOTAL");
            this.Property(x => x.OtherPrimary, "Другие виды топлива в том числе основное").Column("OTHER_PRIMARY");
            this.Property(x => x.OtherReserve, "Другие виды топлива в том числе резервное").Column("OTHER_RESERVE");
            this.Property(x => x.PeatTotal, "Торф Всего").Column("PEAT_TOTAL");
            this.Property(x => x.PeatPrimary, "Торф в том числе основное").Column("PEAT_PRIMARY");
            this.Property(x => x.PeatReserve, "Торф в том числе резервное").Column("PEAT_RESERVE");
            this.Property(x => x.LiquefiedGasTotal, "Сжиженный газ Всего").Column("LIQUEFIED_GAS_TOTAL");
            this.Property(x => x.LiquefiedGasPrimary, "Сжиженный газ в том числе основное").Column("LIQUEFIED_GAS_PRIMARY");
            this.Property(x => x.LiquefiedGasReserve, "Сжиженный газ в том числе резервное").Column("LIQUEFIED_GAS_RESERVE");
            this.Property(x => x.WoodWasteTotal, "Древесные отходы Всего").Column("WOOD_WASTE_TOTAL");
            this.Property(x => x.WoodWastePrimary, "Древесные отходы в том числе основное").Column("WOOD_WASTE_PRIMARY");
            this.Property(x => x.WoodWasteReserve, "Древесные отходы в том числе резервное").Column("WOOD_WASTE_RESERVE");
            this.Property(x => x.DieselTotal, "Дизельное топливо Всего").Column("DIESEL_TOTAL");
            this.Property(x => x.DieselPrimary, "Дизельное топливо в том числе основное").Column("DIESEL_PRIMARY");
            this.Property(x => x.DieselReserve, "Дизельное топливо в том числе резервное").Column("DIESEL_RESERVE");
            this.Property(x => x.ElectricTotal, "Эл. энергия Всего").Column("ELECTRIC_TOTAL");
            this.Property(x => x.ElectricPrimary, "Эл. энергия в том числе основное").Column("ELECTRIC_PRIMARY");
            this.Property(x => x.ElectricReserve, "Эл. энергия в том числе резервное").Column("ELECTRIC_RESERVE");
        }
    }
}