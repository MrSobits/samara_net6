namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    using DomainModelServices;
    using Enums;
    using Overhaul.Entities;

    /// <summary>
    /// Расчетный счет
    /// </summary>
    public class CalcAccount : BaseImportableEntity, ITransferParty 
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CalcAccount()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Кредитная организация
        /// </summary>
        public virtual CreditOrg CreditOrg { get; set; }

        /// <summary>
        /// Владелец счета (контрагент)
        /// </summary>
        public virtual Contragent AccountOwner { get; set; }

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        public virtual DateTime DateOpen { get; set; }

        /// <summary>
        /// Дата закрытия счета
        /// </summary>
        public virtual DateTime? DateClose { get; set; }

        /// <summary>
        /// Тип счета
        /// </summary>
        public virtual TypeCalcAccount TypeAccount { get; set; }
        
        /// <inheritdoc />
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Тип владельца счета
        /// </summary>
        public virtual TypeOwnerCalcAccount TypeOwner { get; set; }

        /// <summary>
        /// Итого по расходу
        /// </summary>
        public virtual decimal TotalOut { get; set; }

        /// <summary>
        /// Итого по приходу 
        /// </summary>
        public virtual decimal TotalIn { get; set; }

        /// <summary>
        /// Сальдо по счету
        /// </summary>
        public virtual decimal Balance { get; set; }

        /// <summary>
        /// Входящий баланс
        /// </summary>
        public virtual decimal BalanceIn { get; set; }

        /// <summary>
        /// Исходящий баланс
        /// </summary>
        public virtual decimal BalanceOut { get; set; }

        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        public virtual string TransferGuid { get; protected set; }
    }
}