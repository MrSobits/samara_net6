namespace Bars.Gkh.RegOperator.Entities.Import
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Предупреждение про ЛС при импорте в закрытый период
    /// </summary>
    public class AccountWarningInClosedPeriodsImport : WarningInClosedPeriodsImport
    {
        /// <summary>
        /// Внешний номер ЛС
        /// </summary>
        public virtual string ExternalNumber { get; set; }

        /// <summary>
        /// Внешний идентификатор РКЦ
        /// </summary>
        public virtual string ExternalRkcId { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>        
        public virtual string Address { get; set; }

        /// <summary>
        /// Обработана
        /// </summary>        
        public virtual YesNo IsProcessed { get; set; }

        /// <summary>
        /// Может быть сопоставлена автоматически
        /// </summary>
        public virtual YesNo IsCanAutoCompared { get; set; }

        /// <summary>
        /// Идентификатор ЛС, заданный при сопоставлении (автоматическом или ручном)
        /// </summary>
        public virtual long? ComparingAccountId { get; set; }

        /// <summary>
        /// Информация по сопоставлению (автоматическом или ручном): номер ЛС - адрес - ФИО
        /// </summary>
        public virtual string ComparingInfo { get; set; }
    }
}
