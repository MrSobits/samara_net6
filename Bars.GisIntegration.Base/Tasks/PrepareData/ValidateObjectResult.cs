namespace Bars.GisIntegration.Base.Tasks.PrepareData
{
    using System;

    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Результат валидации объекта
    /// </summary>
    [Serializable]
    public class ValidateObjectResult
    {
        /// <summary>
        /// Описание объекта
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Статус валидации
        /// </summary>
        public ObjectValidateState State { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
