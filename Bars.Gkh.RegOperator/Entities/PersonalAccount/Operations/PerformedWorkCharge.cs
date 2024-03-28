namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Операция по зачету средств
    /// </summary>
    public class PerformedWorkCharge : BaseImportableEntity
    {
		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="chargeOperation">Базовая операция</param>
		/// <param name="period">Период, на который садится зачет средств</param>
		/// <param name="sum">Сумма начисления</param>
		/// <param name="active">Флаг активности зачёта средств</param>
		public PerformedWorkCharge(ChargeOperationBase chargeOperation, ChargePeriod period, decimal sum, bool active)
        {
            this.ChargeOperation = chargeOperation;
            this.ChargePeriod = period;
            this.Sum = sum;
            this.Active = active;
        }

        /// <summary>
        /// .ctor NH
        /// </summary>
        protected PerformedWorkCharge()
        {
        }
        
        /// <summary>
        /// Базовая операция
        /// </summary>
        public virtual ChargeOperationBase ChargeOperation { get; set; }

        /// <summary>
        /// Период, на который садится зачет средств
        /// </summary>
        public virtual ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// Тип кошелька, на которую пошло распределение
        /// </summary>
        public virtual WalletType DistributeType { get; set; }

        /// <summary>
        /// Сумма начисления
        /// </summary>
        public virtual decimal Sum { get; set; }

		/// <summary>
		/// Флаг активности зачёта средств
		/// </summary>
		public virtual bool Active { get; set; }
    }
}
