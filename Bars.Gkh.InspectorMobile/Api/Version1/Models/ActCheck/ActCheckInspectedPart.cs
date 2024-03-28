namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck
{
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения "Инспектируемая часть"
    /// </summary>
    public class ActCheckInspectedPartGet : BaseActCheckInspectedPart
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Модель создания "Инспектируемая часть"
    /// </summary>
    public class ActCheckInspectedPartCreate : BaseActCheckInspectedPart
    {
    }

    /// <summary>
    /// Модель создания "Инспектируемая часть"
    /// </summary>
    public class ActCheckInspectedPartUpdate : BaseActCheckInspectedPart, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(ActCheckInspectedPart))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Инспектируемая часть"
    /// </summary>
    public class BaseActCheckInspectedPart
    {
        /// <summary>
        /// Уникальный идентификатор инспектируемой части
        /// </summary>
        [RequiredExistsEntity(typeof(InspectedPartGji))]
        public long? InspectedPartId { get; set; }

        /// <summary>
        /// Характер и местоположение
        /// </summary>
        public string CharacterLocation { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}