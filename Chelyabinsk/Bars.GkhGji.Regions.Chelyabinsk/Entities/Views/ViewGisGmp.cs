namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;

    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;

    /*
     * Данная вьюха прденазначена для реестра гис гмп    
     */
    public class ViewGisGmp : PersistentObject
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Тип запросов оплат
        /// </summary>
        public virtual GisGmpPaymentsType GisGmpPaymentsType { get; set; }

        /// <summary>
        /// ФИО инспектора
        /// </summary>
        public virtual string Inspector { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// идентификатор плательщика
        /// </summary>
        public virtual string AltPayerIdentifier { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public virtual string UIN { get; set; }

        /// <summary>
        /// Назначение платежа (штраф, пени)
        /// </summary>
        public virtual string BillFor { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public virtual decimal TotalAmount { get; set; }

        /// <summary>
        /// оплачено
        /// </summary>
        public virtual decimal PaymentsAmount { get; set; }

        /// <summary>
        /// Сквитировано
        /// </summary>
        public virtual YesNo Reconciled { get; set; }

        /// <summary>
        /// Тип начисления
        /// </summary>
        public virtual GisGmpChargeType GisGmpChargeType { get; set; }

        /// <summary>
        /// Номер в СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        public virtual DateTime CalcDate { get; set; }
    }
}