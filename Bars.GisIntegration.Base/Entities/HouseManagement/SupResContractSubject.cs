namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Предмет договора с поставщиком ресурсов
    /// </summary>
    public class SupResContractSubject : BaseRisEntity
    {
        /// <summary>
        /// Договор с поставщиком ресурсов
        /// </summary>
        public virtual SupplyResourceContract Contract { get; set; }

        /// <summary>
        /// Код НСИ "Вид коммунальной услуги" (реестровый номер 3)
        /// </summary>
        public virtual string ServiceTypeCode { get; set; }

        /// <summary>
        /// Идентификатор НСИ "Вид коммунальной услуги" (реестровый номер 3)
        /// </summary>
        public virtual string ServiceTypeGuid { get; set; }

        /// <summary>
        /// Код записи справочника "Тарифицируемый ресурс" (реестровый номер 239)
        /// </summary>
        public virtual string MunicipalResourceCode { get; set; }

        /// <summary>
        /// Идентификатор записи справочника "Тарифицируемый ресурс" (реестровый номер 239)
        /// </summary>
        public virtual string MunicipalResourceGuid { get; set; }

        /// <summary>
        /// Тип системы теплоснабжения
        /// </summary>
        public virtual HeatingSystemType? HeatingSystemType { get; set; }

        /// <summary>
        /// Схема присоединения
        /// </summary>
        public virtual ConnectionSchemeType? ConnectionSchemeType { get; set; }

        /// <summary>
        /// Дата начала поставки ресурса
        /// </summary>
        public virtual DateTime? StartSupplyDate { get; set; }

        /// <summary>
        /// Дата окончания поставки ресурса
        /// </summary>
        public virtual DateTime? EndSupplyDate { get; set; }

        /// <summary>
        /// Плановый объем подачи за год
        /// </summary>
        public virtual decimal? PlannedVolume { get; set; }

        /// <summary>
        /// Плановый объем подачи за год - Единица измерения
        /// </summary>
        public virtual string Unit { get; set; }

        /// <summary>
        /// Плановый объем подачи за год - Режим подачи
        /// </summary>
        public virtual string FeedingMode { get; set; }
    }
}
