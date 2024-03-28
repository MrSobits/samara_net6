namespace Bars.GkhGji.Entities
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Протокол органа муниципального жилищного контроля
    /// </summary>
    public class ProtocolMhc : DocumentGji
    {
        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Муниципальное образование (Орган прокуратуры, вынесший постановление)
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата поступления в ГЖИ
        /// </summary>
        public virtual DateTime? DateSupply { get; set; }
    }
}