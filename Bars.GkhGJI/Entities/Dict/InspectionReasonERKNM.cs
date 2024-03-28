namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    public class InspectionReasonERKNM : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual ERKNMDocumentType ERKNMDocumentType { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string ERKNMId { get; set; }

    }
}
