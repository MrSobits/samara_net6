namespace Bars.Gkh.Gis.Entities.Register.MultipleAnalysis
{
    using System;
    using B4.DataAccess;
    using Enum;
    using RealEstate.GisRealEstateType;

    /// <summary>
    /// Шаблон отчета об анализе
    /// </summary>
    public class MultipleAnalysisTemplate : BaseEntity
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual GisRealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Условие
        /// </summary>
        public virtual GisTypeCondition TypeCondition { get; set; }

        /// <summary>
        /// День формирования
        /// </summary>
        public virtual short FormDay { get; set; }

        /// <summary>
        /// Электонный адрес
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Дата последнего формирования
        /// </summary>
        public virtual DateTime? LastFormDate { get; set; }

        /// <summary>
        /// Guid муниципального района
        /// </summary>
        public virtual string MunicipalAreaGuid { get; set; }

        /// <summary>
        /// Guid населенного пункта
        /// </summary>
        public virtual string SettlementGuid { get; set; }

        /// <summary>
        /// Guid улицы
        /// </summary>
        public virtual string StreetGuid { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual DateTime? MonthYear { get; set; }
    }
}