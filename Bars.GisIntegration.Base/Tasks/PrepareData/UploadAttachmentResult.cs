namespace Bars.GisIntegration.Base.Tasks.PrepareData
{
    using System;

    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Результат загрузки вложения
    /// </summary>
    [Serializable]
    public class UploadAttachmentResult
    {
        /// <summary>
        /// Статус загрузки
        /// </summary>
        public ObjectValidateState State { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Идентификатор вложения
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя вложения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание вложения
        /// </summary>
        public string Description { get; set; }
    }
}