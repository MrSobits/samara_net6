namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Раздел 3. Выполнение договорных обязательств по поставкам топлива
    /// </summary>
    public class FuelContractObligationInfo : BaseFuelInfo
    {
        /// <summary>
        /// Уголь
        /// Всего
        /// </summary>
        public virtual decimal? CoalTotal { get; set; }

        /// <summary>
        /// Уголь
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? CoalDirectContract { get; set; }

        /// <summary>
        /// Уголь
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? CoalIntermediator { get; set; }

        /// <summary>
        /// Дрова
        /// Всего
        /// </summary>
        public virtual decimal? FirewoodTotal { get; set; }

        /// <summary>
        /// Дрова
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? FirewoodDirectContract { get; set; }

        /// <summary>
        /// Дрова
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? FirewoodIntermediator { get; set; }

        /// <summary>
        /// Мазут 
        /// Всего
        /// </summary>
        public virtual decimal? MasutTotal { get; set; }

        /// <summary>
        /// Мазут 
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? MasutDirectContract { get; set; }

        /// <summary>
        /// Мазут 
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? MasutIntermediator { get; set; }

        /// <summary>
        /// Газ
        /// Всего
        /// </summary>
        public virtual decimal? GasTotal { get; set; }

        /// <summary>
        /// Газ
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? GasDirectContract { get; set; }

        /// <summary>
        /// Газ
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? GasIntermediator { get; set; }

        /// <summary>
        /// Другие виды топлива
        /// Всего
        /// </summary>
        public virtual decimal? OtherTotal { get; set; }

        /// <summary>
        /// Другие виды топлива
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? OtherDirectContract { get; set; }

        /// <summary>
        /// Другие виды топлива
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? OtherIntermediator { get; set; }

        /// <summary>
        /// Торф
        /// Всего
        /// </summary>
        public virtual decimal? PeatTotal { get; set; }

        /// <summary>
        /// Торф
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? PeatDirectContract { get; set; }

        /// <summary>
        /// Торф
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? PeatIntermediator { get; set; }

        /// <summary>
        /// Сжиженный газ
        /// Всего
        /// </summary>
        public virtual decimal? LiquefiedGasTotal { get; set; }

        /// <summary>
        /// Сжиженный газ
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? LiquefiedGasDirectContract { get; set; }

        /// <summary>
        /// Сжиженный газ
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? LiquefiedGasIntermediator { get; set; }

        /// <summary>
        /// Древесные отходы
        /// Всего
        /// </summary>
        public virtual decimal? WoodWasteTotal { get; set; }

        /// <summary>
        /// Древесные отходы
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? WoodWasteDirectContract { get; set; }

        /// <summary>
        /// Древесные отходы
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? WoodWasteIntermediator { get; set; }

        /// <summary>
        /// Дизельное топливо
        /// Всего
        /// </summary>
        public virtual decimal? DieselTotal { get; set; }

        /// <summary>
        /// Дизельное топливо
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? DieselDirectContract { get; set; }

        /// <summary>
        /// Дизельное топливо
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? DieselIntermediator { get; set; }

        /// <summary>
        /// Эл. энергия
        /// Всего
        /// </summary>
        public virtual decimal? ElectricTotal { get; set; }

        /// <summary>
        /// Эл. энергия
        /// в том числе по прямым договорам
        /// </summary>
        public virtual decimal? ElectricDirectContract { get; set; }

        /// <summary>
        /// Эл. энергия
        /// в том числе через посредников
        /// </summary>
        public virtual decimal? ElectricIntermediator { get; set; }
    }
}