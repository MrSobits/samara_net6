namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Предоставленные документы акта без взаимодействия
    /// </summary>
    public class ActIsolatedProvidedDoc : BaseEntity
    {
        /// <summary>
        /// Предоставляемый документ
        /// </summary>
        public virtual ProvidedDocGji ProvidedDoc { get; set; }

        /// <summary>
        /// Дата предосталвения
        /// </summary>
        public virtual DateTime? DateProvided { get; set; }

        /// <summary>
        /// Факт проверки
        /// </summary>
        public virtual ActIsolated ActIsolated { get; set; }
    }
}