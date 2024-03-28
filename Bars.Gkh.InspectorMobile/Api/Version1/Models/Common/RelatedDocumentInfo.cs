namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Информация о связных документах
    /// </summary>
    public class RelatedDocumentInfo
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual TypeDocumentGji DocumentType { get; set; }
    }
}