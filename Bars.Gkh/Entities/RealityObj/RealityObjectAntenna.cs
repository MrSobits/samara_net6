namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Система коллективного приема телевидения (СКПТ)
    /// </summary>
    public class RealityObjectAntenna : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Наличие в составе общедомового имущества СКПТ
        /// </summary>
        public virtual YesNoNotSet Availability { get; set; }

        /// <summary>
        /// Работоспособность СКПТ
        /// </summary>
        public virtual YesNoMinus Workability { get; set; }

        /// <summary>
        /// Диапазон
        /// </summary>
        public virtual AntennaRange Range { get; set; }

        /// <summary>
        /// Частота от
        /// </summary>
        public virtual decimal FrequencyFrom { get; set; }

        /// <summary>
        /// Частота до
        /// </summary>
        public virtual decimal FrequencyTo { get; set; }

        /// <summary>
        /// Количество квартир, подключенных к СКПТ
        /// </summary>
        public virtual int NumberApartments { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Причины отсутствия СКПТ
        /// </summary>
        public virtual AntennaReason Reason { get; set; }
    }
}