using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    public class LongTermObjectLoan : BaseImportableEntity
    {
        /// <summary>
        /// Объект долгосрочной программы
        /// </summary>
        public virtual LongTermPrObject Object { get; set; }

        /// <summary>
        /// Объект долгосрочной программы, выдавший займ
        /// </summary>
        public virtual LongTermPrObject ObjectIssued { get; set; }

        /// <summary>
        /// Сумма займа
        /// </summary>
        public virtual decimal LoanAmount { get; set; }

        /// <summary>
        /// Дата займа
        /// </summary>
        public virtual DateTime DateIssue { get; set; }

        /// <summary>
        /// Дата погашения
        /// </summary>
        public virtual DateTime DateRepayment { get; set; }

        /// <summary>
        /// Периода займа (мес.)
        /// </summary>
        public virtual int PeriodLoan { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}