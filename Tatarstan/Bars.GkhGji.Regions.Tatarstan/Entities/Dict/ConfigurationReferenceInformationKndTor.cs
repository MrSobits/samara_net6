namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Конфигурация справочной информации ТОР КНД
    /// </summary>
    public class ConfigurationReferenceInformationKndTor : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Идентификатор из ТОР
        /// </summary>
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Тип справочника
        /// </summary>
        public virtual DictTypes Type { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}
