namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Займ подрядчика
    /// </summary>
    public class BuilderLoan : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Заемщик
        /// </summary>
        public virtual Contragent Lender { get; set; }

        /// <summary>
        /// Сумма выданного
        /// </summary>
        public virtual decimal? Amount { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? DateIssue { get; set; }

        /// <summary>
        /// Плановая дата возврата
        /// </summary>
        public virtual DateTime? DatePlanReturn { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }
    }
}
