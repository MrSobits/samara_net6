namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck
{
    using System.Collections.Generic;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения "Результаты проведения проверки"
    /// </summary>
    public class ActCheckEventResultGet : BaseActCheckEventResult<ActCheckViolationGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Модель создания "Результаты проведения проверки"
    /// </summary>
    public class ActCheckEventResultCreate : BaseActCheckEventResult<ActCheckViolationCreate>
    {
    }

    /// <summary>
    /// Модель обновления "Результаты проведения проверки"
    /// </summary>
    public class ActCheckEventResultUpdate : BaseActCheckEventResult<ActCheckViolationUpdate>, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(ActCheckRealityObject))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Результаты проведения проверки"
    /// </summary>
    public class BaseActCheckEventResult<TViolations>
    {
        /// <summary>
        /// Адрес дома, по которому указаны результаты проверки
        /// </summary>
        [RequiredExistsEntity(typeof(RealityObject))]
        public long? AddressId { get; set; }

        /// <summary>
        /// Выявленные нарушения
        /// </summary>
        public IEnumerable<TViolations> Violations { get; set; }

        /// <summary>
        /// Общее описание выявленных нарушений
        /// </summary>
        public string DescriptionViolations { get; set; }
    }
}