namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Раздел 1. Поставка, расход и остатки топлива
    /// </summary>
    public class FuelAmountInfo : BaseFuelInfo
    {
        /// <summary>
        /// Уголь
        /// Всего
        /// </summary>
        public virtual decimal? CoalTotal { get; set; }

        /// <summary>
        /// Уголь
        /// в том числе основное
        /// </summary>
        public virtual decimal? CoalPrimary { get; set; }

        /// <summary>
        /// Уголь
        /// в том числе резервное
        /// </summary>
        public virtual decimal? CoalReserve { get; set; }

        /// <summary>
        /// Дрова
        /// Всего
        /// </summary>
        public virtual decimal? FirewoodTotal { get; set; }

        /// <summary>
        /// Дрова
        /// в том числе основное
        /// </summary>
        public virtual decimal? FirewoodPrimary { get; set; }

        /// <summary>
        /// Дрова
        /// в том числе резервное
        /// </summary>
        public virtual decimal? FirewoodReserve { get; set; }

        /// <summary>
        /// Мазут 
        /// Всего
        /// </summary>
        public virtual decimal? MasutTotal { get; set; }

        /// <summary>
        /// Мазут 
        /// в том числе основное
        /// </summary>
        public virtual decimal? MasutPrimary { get; set; }

        /// <summary>
        /// Мазут 
        /// в том числе резервное
        /// </summary>
        public virtual decimal? MasutReserve { get; set; }

        /// <summary>
        /// Газ
        /// Всего
        /// </summary>
        public virtual decimal? GasTotal { get; set; }

        /// <summary>
        /// Газ
        /// в том числе основное
        /// </summary>
        public virtual decimal? GasPrimary { get; set; }

        /// <summary>
        /// Газ
        /// в том числе резервное
        /// </summary>
        public virtual decimal? GasReserve { get; set; }

        /// <summary>
        /// Другие виды топлива
        /// Всего
        /// </summary>
        public virtual decimal? OtherTotal { get; set; }

        /// <summary>
        /// Другие виды топлива
        /// в том числе основное
        /// </summary>
        public virtual decimal? OtherPrimary { get; set; }

        /// <summary>
        /// Другие виды топлива
        /// в том числе резервное
        /// </summary>
        public virtual decimal? OtherReserve { get; set; }

        /// <summary>
        /// Торф
        /// Всего
        /// </summary>
        public virtual decimal? PeatTotal { get; set; }

        /// <summary>
        /// Торф
        /// в том числе основное
        /// </summary>
        public virtual decimal? PeatPrimary { get; set; }

        /// <summary>
        /// Торф
        /// в том числе резервное
        /// </summary>
        public virtual decimal? PeatReserve { get; set; }

        /// <summary>
        /// Сжиженный газ
        /// Всего
        /// </summary>
        public virtual decimal? LiquefiedGasTotal { get; set; }

        /// <summary>
        /// Сжиженный газ
        /// в том числе основное
        /// </summary>
        public virtual decimal? LiquefiedGasPrimary { get; set; }

        /// <summary>
        /// Сжиженный газ
        /// в том числе резервное
        /// </summary>
        public virtual decimal? LiquefiedGasReserve { get; set; }

        /// <summary>
        /// Древесные отходы
        /// Всего
        /// </summary>
        public virtual decimal? WoodWasteTotal { get; set; }

        /// <summary>
        /// Древесные отходы
        /// в том числе основное
        /// </summary>
        public virtual decimal? WoodWastePrimary { get; set; }

        /// <summary>
        /// Древесные отходы
        /// в том числе резервное
        /// </summary>
        public virtual decimal? WoodWasteReserve { get; set; }

        /// <summary>
        /// Дизельное топливо
        /// Всего
        /// </summary>
        public virtual decimal? DieselTotal { get; set; }

        /// <summary>
        /// Дизельное топливо
        /// в том числе основное
        /// </summary>
        public virtual decimal? DieselPrimary { get; set; }

        /// <summary>
        /// Дизельное топливо
        /// в том числе резервное
        /// </summary>
        public virtual decimal? DieselReserve { get; set; }

        /// <summary>
        /// Эл. энергия
        /// Всего
        /// </summary>
        public virtual decimal? ElectricTotal { get; set; }

        /// <summary>
        /// Эл. энергия
        /// в том числе основное
        /// </summary>
        public virtual decimal? ElectricPrimary { get; set; }

        /// <summary>
        /// Эл. энергия
        /// в том числе резервное
        /// </summary>
        public virtual decimal? ElectricReserve { get; set; }
    }
}