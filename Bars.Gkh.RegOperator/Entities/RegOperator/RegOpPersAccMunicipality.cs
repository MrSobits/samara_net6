namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    /// <summary>
    /// Лицевые счета по муниципальным образованиям
    /// </summary>
    public class RegOpPersAccMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Региональный оператор
        /// </summary>
        public virtual RegOperator RegOperator { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual string PersAccountNum { get; set; }

        /// <summary>
        /// ФИО собственника
        /// </summary>
        public virtual string OwnerFio { get; set; }

        /// <summary>
        /// Оплачено взносов 
        /// </summary>
        public virtual decimal? PaidContributions { get; set; }

        /// <summary>
        /// Начислено взносов 
        /// </summary>
        public virtual decimal? CreditContributions { get; set; }

        /// <summary>
        /// Начислено пени
        /// </summary>
        public virtual decimal? CreditPenalty { get; set; }

        /// <summary>
        /// Оплачено пени 
        /// </summary>
        public virtual decimal? PaidPenalty { get; set; }

        /// <summary>
        /// Сумма субсидии из МБ (местный бюджет),руб
        /// </summary>
        public virtual decimal? SubsidySumLocalBud { get; set; }

        /// <summary>
        /// Сумма субсидии из БС (бюджет субъекта),руб
        /// </summary>
        public virtual decimal? SubsidySumSubjBud { get; set; }

        /// <summary>
        /// Сумма субсидии из ФБ (федеральный бюджет),руб
        /// </summary>
        public virtual decimal? SubsidySumFedBud { get; set; }

        /// <summary>
        /// Сумма заимствования, руб
        /// </summary>
        public virtual decimal? SumAdopt { get; set; }

        /// <summary>
        /// Погашенная сумма заимствования,руб
        /// </summary>
        public virtual decimal? RepaySumAdopt { get; set; }
    }
}