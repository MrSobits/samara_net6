namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Пара: коммунальная услуга и коммунальный ресурс договора с поставщиком ресурсов
    /// </summary>
    public class SupResContractServiceResource : BaseRisEntity
    {
        /// <summary>
        /// Договор с поставщиком ресурсов
        /// </summary>
        public virtual SupplyResourceContract Contract { get; set; }

        /// <summary>
        /// Код записи справочника "Вид коммунальной услуги" (реестровый номер 3)
        /// </summary>
        public virtual string ServiceTypeCode { get; set; }

        /// <summary>
        /// Идентификатор записи справочника "Вид коммунальной услуги" (реестровый номер 3)
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
        /// Дата начала поставки ресурса
        /// </summary>
        public virtual DateTime? StartSupplyDate { get; set; }

        /// <summary>
        /// Дата окончания поставки ресурса
        /// </summary>
        public virtual DateTime? EndSupplyDate { get; set; }
    }
}
