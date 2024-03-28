namespace Bars.GkhGji.Entities
{
    using System;

    using Gkh.Entities;
    using Enums;

    /// <summary>
    /// Протокол прокуратуры
    /// </summary>
    public class ProtocolRSO : DocumentGji
    {
        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Тип РСО составившей протокол
        /// </summary>
        public virtual TypeSupplierProtocol TypeSupplierProtocol { get; set; }

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
        /// Газоснабжающая организация, вынесшая протокол
        /// </summary>
        public virtual Contragent GasSupplier { get; set; }

        /// <summary>
        /// Дата поступления в ГЖИ
        /// </summary>
        public virtual DateTime? DateSupply { get; set; }
    }
}