namespace Bars.Gkh.Gis.Entities.Register.ServiceSubsidyRegister
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Entities.Dicts;

    using LoadedFileRegister;

    /// <summary>
    /// Субсидии по жильцам
    /// </summary>
    public class ServiceSubsidyRegister : BaseEntity
    {
        /// <summary>
        /// ПСС
        /// </summary>
        public virtual string Pss { get; set; }

        /// <summary>
        /// Расчетный месяц
        /// </summary>
        public virtual DateTime CalculationMonth { get; set; }

        /// <summary>
        /// Лицевой счет управляющей компании
        /// </summary>
        public virtual long ManagementOrganizationAccount { get; set; }
        
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }

        /// <summary>
        /// Начисленная сумма субсидий - льгот получателя
        /// </summary>
        public virtual double AccruedBenefitSum { get; set; }

        /// <summary>
        /// Начисленная сумма субсидий - ЕДВ получателя
        /// </summary>
        public virtual double AccruedEdvSum { get; set; }

        /// <summary>
        /// Начисленная перерасчетом сумма субсидий - льгот получателя
        /// </summary>
        public virtual double RecalculatedBenefitSum { get; set; }

        /// <summary>
        /// Начисленная перерасчетом сумма субсидий - ЕДВ получателя
        /// </summary>
        public virtual double RecalculatedEdvSum { get; set; }

        /// <summary>
        /// Подразделение организации
        /// </summary>
        public virtual string OrganizationUnit { get; set; }

        /// <summary>
        /// Загруженный файл
        /// </summary>
        public virtual LoadedFileRegister LoadedFile { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Идентификатор ЛС
        /// </summary>
        public virtual long PersonalAccountId { get; set; }
    }
}
