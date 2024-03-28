namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Protocol
{
    using System;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения "Информация о выявленном нарушении"
    /// </summary>
    public class ProtocolViolationGet : BaseProtocolViolation
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор дома, который проверялся в рамках проверки
        /// </summary>
        internal long? AddressId { get; set; }
    }

    /// <summary>
    /// Модель создания "Информация о выявленном нарушении"
    /// </summary>
    public class ProtocolViolationCreate : BaseProtocolViolation
    {
    }

    /// <summary>
    /// Модель обновления "Информация о выявленном нарушении"
    /// </summary>
    public class ProtocolViolationUpdate : BaseProtocolViolation, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(ProtocolViolation))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Информация о выявленном нарушении"
    /// </summary>
    public class BaseProtocolViolation
    {
        /// <summary>
        /// Уникальный идентификатор нарушения
        /// </summary>
        [RequiredExistsEntity(typeof(ViolationGji))]
        public long? ViolationId { get; set; }

        /// <summary>
        /// Дата нарушения
        /// </summary>
        public DateTime? DateViolation { get; set; }

        /// <summary>
        /// Статья закона (Идентификатор справочника)
        /// </summary>
        [OnlyExistsEntity(typeof(ArticleLawGji))]
        public long? LawId { get; set; }
    }
}