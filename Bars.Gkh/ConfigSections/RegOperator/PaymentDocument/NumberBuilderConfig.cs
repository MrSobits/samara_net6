namespace Bars.Gkh.ConfigSections.RegOperator.PaymentDocument
{
    using Bars.Gkh.Config;
    using Gkh.Enums;

    /// <summary>
    /// Настройки для формирования конфига для формирования номера документа
    /// </summary>
    public class NumberBuilderConfig : IGkhConfigSection
    {
        /// <summary>
        /// Наименование параметра (тип параметра)
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Количество символов 
        /// </summary>
        public virtual int SymbolsCount { get; set; }

        /// <summary>
        /// Расположения символов в значении
        /// </summary>
        public virtual SymbolsLocation SymbolsLocation { get; set; }

        /// <summary>
        /// Порядок в номере
        /// </summary>
        public virtual int Order { get; set; }
    }
}