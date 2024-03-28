namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.Gkh.Modules.GkhDi.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Прочие услуги
    /// </summary>
    public class OtherService : BaseGkhEntity
    {
        /// <summary>
        /// Раскрытие информации объекта недвижимости
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        [Obsolete]
        public virtual string Name { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Ед. измерения (строка).
        /// </summary>
        [Obsolete("Использовать свойство UnitMeasure")]
        public virtual string UnitMeasureStr { get; set; }

        /// <summary>
        /// Тариф.
        /// </summary>
        [Obsolete("Использовать IDomainService<TariffForConsumersOtherService> для получения необходимого тарифа")]
        public virtual decimal? Tariff { get; set; }

        /// <summary>
        /// Поставщик.
        /// </summary>
        [Obsolete("Использовать IDomainService<ProviderOtherService> для получения необходимого поставщика")]
        public virtual string Provider { get; set; }

        /// <summary>
        /// Шаблонная услуга.
        /// </summary>
        public virtual TemplateOtherService TemplateOtherService { get; set; }

        /// <summary>
        /// Единица измерения.
        /// Для разных домов могут быть разные единицы измерения одной услуги, отличные от указанных в справочнике TemplateOtherService.
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}
