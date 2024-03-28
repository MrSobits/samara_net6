namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Информация о проверке
    /// </summary>
    public class InspectionInfo
    {
        /// <summary>
        /// Уникальный идентификатор проверки ГЖИ
        /// </summary>
        public long InspectionId { get; set; }

        /// <summary>
        /// Информация о связанных документах
        /// </summary>
        public IEnumerable<RelatedDocumentInfo> RelatedDocuments { get; set; }
    }
}