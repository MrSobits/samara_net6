namespace Bars.GkhCr.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Дополнительные параметры Объекта КР
    /// </summary>
    public class AdditionalParameters : BaseImportableEntity
    {
        /// <summary>
        /// Объект КР
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Дата поступления запроса в КТС
        /// </summary>
        public virtual DateTime? RequestKtsDate { get; set; }

        /// <summary>
        /// Дата выдачи технических условий
        /// </summary>
        public virtual DateTime? TechConditionKtsDate { get; set; }

        /// <summary>
        /// Технические условия выданы (кому)
        /// </summary>
        public virtual string TechConditionKtsRecipient { get; set; }

        /// <summary>
        /// Дата поступления запроса в МУП Водоканал
        /// </summary>
        public virtual DateTime? RequestVodokanalDate { get; set; }

        /// <summary>
        /// Дата выдачи технических условий
        /// </summary>
        public virtual DateTime? TechConditionVodokanalDate { get; set; }

        /// <summary>
        /// Технические условия выданы (кому)
        /// </summary>
        public virtual string TechConditionVodokanalRecipient { get; set; }

        /// <summary>
        /// Дата поступления проекта на согласование
        /// </summary>
        public virtual DateTime? EntryForApprovalDate { get; set; }

        /// <summary>
        /// Дата согласования проекта в КТС
        /// </summary>
        public virtual DateTime? ApprovalKtsDate { get; set; }

        /// <summary>
        /// Дата согласования проекта в МУП Водоканал
        /// </summary>
        public virtual DateTime? ApprovalVodokanalDate { get; set; }

        /// <summary>
        /// Процент монтажа проекта
        /// </summary>
        public virtual decimal? InstallationPercentage { get; set; }

        /// <summary>
        /// Статус приемки объекта Заказчиком
        /// </summary>
        public virtual AcceptType? ClientAccepted { get; set; }

        /// <summary>
        /// Дата изменения статуса приемки объекта заказчиком
        /// </summary>
        public virtual DateTime? ClientAcceptedChangeDate { get; set; }

        /// <summary>
        /// Статус приемки объекта инспектором Ростехнадзора
        /// </summary>
        public virtual AcceptType? InspectorAccepted { get; set; }

        /// <summary>
        /// Дата изменения статуса приемки объекта инспектором
        /// </summary>
        public virtual DateTime? InspectorAcceptedChangeDate { get; set; }
    }
}