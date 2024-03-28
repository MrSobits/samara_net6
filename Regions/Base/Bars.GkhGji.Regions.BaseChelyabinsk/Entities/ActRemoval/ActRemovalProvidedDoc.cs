namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Предоставленные документы акта проверки предписания ГЖИ
    /// </summary>
    public class ActRemovalProvidedDoc : BaseEntity
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
        /// Aкт проверки предписания
        /// </summary>
        public virtual ActRemoval ActRemoval { get; set; }
    }
}