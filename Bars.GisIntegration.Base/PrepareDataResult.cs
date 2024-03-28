namespace Bars.GisIntegration.Base
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Результат подготовки данных
    /// </summary>
    public class PrepareDataResult
    {
        /// <summary>
        /// Результат валидации данных перед формированием пакетов
        /// </summary>
        public List<ValidateObjectResult> ValidateResult { get; set; }

        /// <summary>
        /// Сформированные пакеты
        /// </summary>
        public List<RisPackage> Packages { get; set; }

        /// <summary>
        /// Результат загрузки вложений
        /// </summary>
        public List<UploadAttachmentResult> UploadAttachmentsResult { get; set; }
    }
}
