namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Предоставленные документы акта проверки ГЖИ
    /// </summary>
    public class ActCheckProvidedDoc : BaseEntity
    {
        /// <summary>
        /// Предоставляемый документа
        /// </summary>
        public virtual ProvidedDocGji ProvidedDoc { get; set; }

        /// <summary>
        /// Дата предосталвения
        /// </summary>
        public virtual DateTime? DateProvided { get; set; }

        /// <summary>
        /// Факт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }
    }
}