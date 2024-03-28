namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.MotivationPresentation
{
    using System.Collections.Generic;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель для получения результата проведения проверки для документа "Мотивированное представление"
    /// </summary>
    public class MotivationPresentationEventResultGet : BaseMotivationPresentationEventResult
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Модель для создания результата проведения проверки для документа "Мотивированное представление"
    /// </summary>
    public class MotivationPresentationEventResultCreate : BaseMotivationPresentationEventResult
    {
    }

    /// <summary>
    /// Модель для обновления результата проведения проверки для документа "Мотивированное представление"
    /// </summary>
    public class MotivationPresentationEventResultUpdate : BaseMotivationPresentationEventResult
    {
    }

    /// <summary>
    /// Базовая модель результатов проведения проверки для документа "Мотивированное представление"
    /// </summary>
    public abstract class BaseMotivationPresentationEventResult
    {
        /// <summary>
        /// Адрес дома, по которому указаны результаты проверки
        /// </summary>
        [RequiredExistsEntity(typeof(RealityObject))]
        public long? AddressId { get; set; }

        /// <summary>
        /// Выявленные нарушения
        /// </summary>
        [RequiredExistsEntity(typeof(ViolationGji))]
        public IEnumerable<long> Violations { get; set; }
    }
}