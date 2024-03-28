namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Отмененные начисления
    /// </summary>
    public class CancelCharge : BaseImportableEntity
    {
        /// <summary>
        /// .ctor NH
        /// </summary>
        public CancelCharge()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="account">Лс</param>
        /// <param name="chargeOperation">Операция отмены начислений</param>
        /// <param name="period">Период, за который отменяем начисления</param>
        /// <param name="cancelType">Тип отмененного начисления (по базовому, по тарифу решений, пени)</param>
        public CancelCharge(ChargeOperationBase chargeOperation, BasePersonalAccount account, ChargePeriod period, decimal cancelSum, CancelType cancelType)
        {
            this.PersonalAccount = account;
            this.ChargeOperation = chargeOperation;
            this.CancelPeriod = period;
            this.CancelType = cancelType;
            this.CancelSum = cancelSum;
        }

        /// <summary>
        /// Операция отмены начислений
        /// </summary>
        public virtual ChargeOperationBase ChargeOperation { get; set; }

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Период, за который отменяем начисления
        /// </summary>
        public virtual ChargePeriod CancelPeriod { get; set; }


        /// <summary>
        /// Тип отмененного начисления (по базовому, по тарифу решений, пени)
        /// </summary>
        public virtual CancelType CancelType { get; set; }

        /// <summary>
        /// Сумма отмененного начисления
        /// </summary>
        public virtual decimal CancelSum { get; set; }

    }
}
