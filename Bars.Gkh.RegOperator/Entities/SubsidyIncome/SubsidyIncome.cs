namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
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
    /// Реестр субсидий
    /// </summary>
    public class SubsidyIncome : BaseImportableEntity, IMoneyOperationSource, IDistributable
    {
        private readonly IList<MoneyOperation> operations;

        /// <summary>
        /// NH
        /// </summary>
        public SubsidyIncome()
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
        /// Направление движения средств (приход/расход)
        /// </summary>
        public virtual MoneyDirection MoneyDirection { get; set; }

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
        /// Документ-основание
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Источник распределения
        /// </summary>
        DistributionSource IDistributable.Source
        {
            get { return DistributionSource.SubsidyIncome; }
        }

        /// <summary>
        /// Коды распределения (не пуст если SuspenseAccountStatus != NotDistributed )
        /// </summary>
        public virtual string DistributionCode { get; set; }

        /// <summary>
        /// Предполагаемые коды распределения
        /// </summary>
        public virtual string TypeSubsidyDistr { get; set; }

        /// <summary>
        /// На этот гуид будут вязаться различные операции, происходящие при распределении.
        /// Например, возврат займа дома
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Количество записей
        /// </summary>
        public virtual int DetailsCount { get; set; }

        /// <summary>
        /// Банковская выписка
        /// </summary>
        public virtual BankAccountStatement BankAccountStatement { get; set; }

        /// <summary>
        /// Определение домов ЛС
        /// </summary>
        public virtual SubsidyIncomeDefineType SubsidyIncomeDefineType { get; set; }

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
        public virtual MoneyOperation CreateOperation(DynamicDictionary dictionary, ChargePeriod period)
        {
            return this.CreateOperation(period);
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            var newOper = new MoneyOperation(this.TransferGuid, period);
            this.operations.Add(newOper);

            // Сделано для того, чтобы  OperationDate отличался от предыдущей операции, так как нам нужен порядок операций
            Thread.Sleep(1000);

            newOper.OperationDate = DateTime.Now;

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