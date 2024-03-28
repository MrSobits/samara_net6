namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.GkhDi.Enums;

    using Gkh.Entities;

    /// <summary>
    /// Базовая услуга
    /// </summary>
    public class BaseService : BaseGkhEntity
    {
        /// <summary>
        /// Шаблонная услуга
        /// </summary>
        public virtual TemplateService TemplateService { get; set; }

        /// <summary>
        /// Раскрытие информации объекта недвижимости
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Текущий поставщик услуги
        /// </summary>
        public virtual Contragent Provider { get; set; }

        /// <summary>
        /// Доход, полученный за предоставление услуги
        /// </summary>
        public virtual decimal? Profit { get; set; }

        /// <summary>
        /// Единица измерения (Копируется с шаблонной услуги, но если есть галочка изменяемое, то есть возможность редактировать в самой услуге)
        /// При создании копируется значение из шаблонной улуги
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Храним сумму тарифа для потребителей с максимальной датой. Из него потом дергаем инфу в реестр
        /// </summary>
        public virtual decimal? TariffForConsumers { get; set; }

        /// <summary>
        /// Храним тип енума тарифа для потребителей с максимальной датой. Из него потом дергаем инфу в реестр
        /// </summary>
        public virtual TariffIsSetForDi TariffIsSetForDi { get; set; }

        /// <summary>
        /// Храним дату тарифа для потребителей с максимальной датой. Из него потом дергаем инфу в реестр
        /// </summary>
        public virtual DateTime? DateStartTariff { get; set; }

        /// <summary>
        /// Наличие планово-предупредительных работ
        /// </summary>
        public virtual YesNoNotSet ScheduledPreventiveMaintanance { get; set; }
    }
}
