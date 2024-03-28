namespace Bars.GisIntegration.Base.Tasks.PrepareData
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Сериализуемый результат подготовки данных
    /// </summary>
    [Serializable]
    public class SerializablePrepareDataResult
    {
        /// <summary>
        /// Результат валидации данных перед формированием пакетов
        /// </summary>
        public List<ValidateObjectResult> ValidateResult { get; set; }

        /// <summary>
        /// Результат загрузки файлов
        /// </summary>
        public List<UploadAttachmentResult> UploadResult { get; set; }

        /// <summary>
        /// Идентификаторы сформированных пакетов
        /// </summary>
        public List<long> PackageIds { get; set; }
    }
}
