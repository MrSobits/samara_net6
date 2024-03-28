namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction
{
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    /// <summary>
    /// Модель получения "Нарушение действия акта проверки"
    /// </summary>
    public class ActCheckActionViolationGet : BaseActCheckActionViolation
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }
    
    /// <summary>
    /// Модель создания "Нарушение действия акта проверки"
    /// </summary>
    public class ActCheckActionViolationCreate : BaseActCheckActionViolation
    {
    }
    
    /// <summary>
    /// Модель обновления "Нарушение действия акта проверки"
    /// </summary>
    public class ActCheckActionViolationUpdate : BaseActCheckActionViolation, INestedEntityId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [OnlyExistsEntity(typeof(ActCheckActionViolation))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Нарушение действия акта проверки"
    /// </summary>
    public class BaseActCheckActionViolation
    {
        /// <summary>
        /// Уникальный идентификатор нарушения
        /// </summary>
        [RequiredExistsEntity(typeof(ViolationGji))]
        public long? ViolationId { get; set; }

        /// <summary>
        /// Пояснение контролируемого лица
        /// </summary>
        public string Explanation { get; set; }
    }
}