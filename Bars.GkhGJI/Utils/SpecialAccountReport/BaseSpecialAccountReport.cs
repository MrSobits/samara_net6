namespace Bars.GkhGji
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.GkhGji.Enums;

    public class BaseSpecialAccountReport : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Банк
        /// </summary>
        public virtual CreditOrg CreditOrg { get; set; }

        /// <summary>
        /// Ответственный исполнитель
        /// </summary>
        public virtual string Executor { get; set; }

        /// <summary>
        /// Сведения о сертификате
        /// </summary>
        public virtual string Sertificate { get; set; }

        // <summary>
        /// Месяц
        /// </summary>
        public virtual MonthEnums MonthEnums { get; set; }

        // <summary>
        /// Год
        /// </summary>
        public virtual YearEnums YearEnums { get; set; }

        /// <summary>
        /// Подписанный XML файл
        /// </summary>
        public virtual FileInfo SignedXMLFile { get; set; }
    }
}