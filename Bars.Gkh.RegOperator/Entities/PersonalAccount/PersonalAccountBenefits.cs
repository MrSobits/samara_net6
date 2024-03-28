namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Информация по начисленным льготам
    /// </summary>
    public class PersonalAccountBenefits : BaseImportableEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }
    }
}