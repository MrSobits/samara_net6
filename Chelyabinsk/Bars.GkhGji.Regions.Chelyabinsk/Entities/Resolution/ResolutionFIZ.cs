namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Изменение в статье закона постановления ГЖИ
    /// </summary>
    public class ResolutionFiz : BaseGkhEntity
    {
        /// <summary>
        /// Постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// Тип документа физ лица
        /// </summary>
        public virtual FLDocType FLDocType { get; set; }

        /// <summary>
        /// Тип документа физ лица
        /// </summary>
        public virtual PhysicalPersonDocType PhysicalPersonDocType { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        public virtual String DocumentNumber { get; set; }

        /// <summary>
        /// Код плательщика
        /// </summary>
        public virtual String PayerCode { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        public virtual String DocumentSerial { get; set; }

        /// <summary>
        /// Является ли гражданином РФ
        /// </summary>
        public virtual Boolean IsRF { get; set; }
    }
}