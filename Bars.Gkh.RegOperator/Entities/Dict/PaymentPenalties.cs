namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Bars.Gkh.Entities;

    using Gkh.Enums.Decisions;
    using Newtonsoft.Json;

    /// <summary>
    /// Справочник расчетов пеней
    /// </summary>
    public class PaymentPenalties : BaseImportableEntity
    {
        public PaymentPenalties()
        {
            _excludes = new List<PaymentPenaltiesExcludePersAcc>();
            DecisionType = CrFundFormationDecisionType.Unknown;
        }

        /// <summary>
        /// Количество дней
        /// </summary>
        public virtual int Days { get; set; }

        /// <summary>
        /// Ставка рефинансирования, %
        /// </summary>
        public virtual decimal Percentage { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Способ формирования
        /// </summary>
        public virtual CrFundFormationDecisionType DecisionType { get; set; }

        /// <summary>
        /// Исключения по ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PaymentPenaltiesExcludePersAcc> Excludes
        {
            get { return _excludes ?? (_excludes = new List<PaymentPenaltiesExcludePersAcc>()); }
        }

        private IList<PaymentPenaltiesExcludePersAcc> _excludes;
    }
}