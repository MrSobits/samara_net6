namespace Bars.GisIntegration.Base.Tasks.SendData
{
    using System;

    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Сериализуемый результат обработки объекта
    /// Сохраняется в BLOB в результате обработки пакета
    /// </summary>
    [Serializable]
    public class SerializableObjectProcessingResult
    {
        /// <summary>
        /// Конструктор сериализуемого результата обработки объекта
        /// </summary>
        /// <param name="objectProcessingResult">результат обработки объекта</param>
        public SerializableObjectProcessingResult(ObjectProcessingResult objectProcessingResult)
        {
            this.RisId = objectProcessingResult.RisId;
            this.ExternalId = objectProcessingResult.ExternalId;
            this.GisId = objectProcessingResult.GisId;
            this.Description = objectProcessingResult.Description;
            this.State = objectProcessingResult.State;
            this.Message = objectProcessingResult.Message;
        }

        /// <summary>
        /// Идентификатор сущности Ris
        /// </summary>
        public long RisId { get; set; }

        /// <summary>
        /// Идентификатор сущности сторонней системы
        /// </summary>
        public long ExternalId { get; set; }

        /// <summary>
        /// Идентификатор сущности Gis
        /// </summary>
        public string GisId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public ObjectProcessingState State { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
