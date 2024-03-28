namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Оплата штрафов в постановлении ГЖИ
    /// </summary>
    public class ResolutionPayFine : BaseGkhEntity
    {
        /// <summary>
        /// Постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// сумма штрафа
        /// </summary>
        public virtual decimal? Amount { get; set; }

        /// <summary>
        /// Тип документа оплаты штрафа
        /// </summary>
        public virtual TypeDocumentPaidGji TypeDocumentPaid { get; set; }

        /// <summary>
        /// Код из Гис программы
        /// </summary>
        public virtual string GisUip { get; set; }
    }
}