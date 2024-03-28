namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Оплата штрафов в постановлении Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorPayFine : BaseGkhEntity
    {
        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public virtual ResolutionRospotrebnadzor Resolution { get; set; }

        /// <summary>
        /// Тип документа оплаты штрафа
        /// </summary>
        public virtual TypeDocumentPaidGji TypeDocumentPaid { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public virtual decimal? Amount { get; set; }

        /// <summary>
        /// Код из Гис программы
        /// </summary>
        public virtual string GisUip { get; set; }
    }
}