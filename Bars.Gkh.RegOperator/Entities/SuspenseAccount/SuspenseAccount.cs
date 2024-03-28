namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.FileStorage;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using DomainModelServices;

    using Enums;

    using Exceptions;

    using Newtonsoft.Json;

    using ValueObjects;

    /// <summary>
    /// Счет невыясненных сумм
    /// </summary>
    public class SuspenseAccount : BaseImportableEntity, IMoneyOperationSource, IDistributable
    {
        private readonly IList<MoneyOperation> operations;

        /// <summary>
        /// NH
        /// </summary>
        public SuspenseAccount()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
            this.operations = new List<MoneyOperation>();
            this.DistributeState = DistributionState.NotDistributed;
        }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime DateReceipt { get; set; }

        /// <summary>
        /// Дата распределения
        /// </summary>
        public virtual DateTime? DistributionDate { get; set; }

        /// <summary>
        /// Состояние (распределен/не распределен)
        /// </summary>
        public virtual DistributionState DistributeState { get; set; }

        /// <summary>
        /// Тип платежа
        /// </summary>
        public virtual SuspenseAccountTypePayment SuspenseAccountTypePayment { get; set; }

        /// <summary>
        /// Направление движения средств (приход/расход)
        /// </summary>
        public virtual MoneyDirection MoneyDirection { get; set; }

        /// <summary>
        /// Рас.счет получателя 
        /// </summary>
        public virtual string AccountBeneficiary { get; set; }

        /// <summary>
        /// Сумма 
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// взыскан РОСП
        /// </summary>
        public virtual bool IsROSP { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        public virtual decimal RemainSum { get; set; }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public virtual string DetailsOfPayment { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Создать операцию
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CreateOperation(DynamicDictionary parameters, ChargePeriod period)
        {
            return this.CreateOperation(period);
        }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Источник распределения
        /// </summary>
        DistributionSource IDistributable.Source
        {
            get { return DistributionSource.SuspenseAccount; }
        }

        /// <summary>
        /// Код распределения (не пуст если SuspenseAccountStatus != NotDistributed )
        /// </summary>
        public virtual string DistributionCode { get; set; }

        /// <summary>
        /// На этот гуид будут вязаться различные операции, происходящие при распределении.
        /// Например, возврат займа дома
        /// </summary>
        public virtual string TransferGuid { get; protected set; }
        

        /// <summary>
        /// Операции, проведенные на НВС
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MoneyOperation> Operations
        {
            get { return this.operations; }
        }

        /// <summary>
        /// Создание операции по переводу денег
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            var newOper = new MoneyOperation(this.TransferGuid, period);
            this.operations.Add(newOper);
            return newOper;
        }

        /// <summary>
        /// Возвращаем операции
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<MoneyOperation> GetOperations()
        {
            return this.Operations.Where(x => !x.IsCancelled && x.CanceledOperation == null);
        }

        /// <summary>
        /// Отменить текущую активную операцию
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CancelOperation(MoneyOperation operation, ChargePeriod period)
        {
            if (operation == null)
            {
                throw new MoneyOperationException("Opened operation does not exists.");
            }

            var op = operation.Cancel(period);
            this.operations.Add(op);

            return op;
        }
    }
}