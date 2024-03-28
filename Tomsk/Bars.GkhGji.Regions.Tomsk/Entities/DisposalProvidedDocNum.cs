namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Дата для Предоставляемых документов
    /// </summary>
    public class DisposalProvidedDocNum : BaseEntity
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual long ProvideDocumentsNum { get; set; }

    }
}
