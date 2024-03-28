namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.FileStorage;
    using B4.Modules.States;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using Domain;
    using DomainModelServices;
    using Loan;
    using ValueObjects;
    using Enums;
    using GkhCr.Entities;
    using Newtonsoft.Json;

    /// <summary>Займ дома</summary>
    public class RealityObjectLoan : BaseImportableEntity, IStatefulEntity, ITransferParty, IMoneyOperationSource
    {
        private readonly IList<MoneyOperation> operations;
        private IList<RealityObjectLoanWallet> loansFromWallets;

        /// <summary>NH</summary>
        protected RealityObjectLoan()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
            this.operations = new List<MoneyOperation>();
            this.LoanDate = DateTime.Now;
        }

        public RealityObjectLoan(
            RegopCalcAccount source, 
            RealityObjectPaymentAccount target,
            decimal loanSum,
            ProgramCr program,
            long documentNum) : this()
        {
            ArgumentChecker.NotNull(target, "Получатель займа не найден", "target");
            ArgumentChecker.NotNull(source, "Источник займа не найден", "source");
            ArgumentChecker.NotNull(program, "Программа КР не найдена", "program");

            if (loanSum <= 0)
            {
                throw new ArgumentException("Сумма займа должна быть больше 0");
            }

            this.LoanTaker = target;
            this.LoanSum = loanSum;
            this.ProgramCr = program;
            this.Source = source;
        }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObjectPaymentAccount LoanTaker { get; protected set; }

        /// <summary>
        /// Источник займа (расчетный счет регопа)
        /// </summary>
        public virtual RegopCalcAccount Source { get; protected set; }

        /// <summary>
        /// Дата займа
        /// </summary>
        public virtual DateTime LoanDate { get; set; }

        /// <summary>
        /// Дата операции внутри периода
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Планируемая количество месяцев на возврат займа
        /// </summary>
        public virtual int PlanLoanMonthCount { get; set; }

        /// <summary>
        /// Фактическая дата возврата
        /// </summary>
        public virtual DateTime? FactEndDate { get; set; }

        /// <summary>
        /// Программа кап.ремонта
        /// </summary>
        public virtual ProgramCr ProgramCr { get; protected set; }

        /// <summary>
        /// Сумма займа
        /// </summary>
        public virtual decimal LoanSum { get; protected set; }

        /// <summary>
        /// Погашенная сумма займа
        /// </summary>
        public virtual decimal LoanReturnedSum { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Document { get; set; }
        
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual long DocumentNum { get; set; }

        /// <summary>
        /// Список ООИ
        /// </summary>
        public virtual string CommonEstateObjectNames { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Проведенные операции
        /// </summary>
        [JsonIgnore]
        public virtual IList<MoneyOperation> Operations
        {
            get
            {
                return this.operations;
            }

            protected set
            {
                this.operations.Clear();
                if (value != null)
                {
                    foreach (var operation in value)
                    {
                        this.operations.Add(operation);
                    }
                }
            }
        }

        /// <summary>
        /// Детализация займа по кошелькам
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<RealityObjectLoanWallet> LoansFromWallets
        {
            get
            {
                return this.loansFromWallets ?? (this.loansFromWallets = new List<RealityObjectLoanWallet>());
            }
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns>
        /// <see cref="MoneyOperation"/>
        /// </returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            var newOper = new MoneyOperation(this.TransferGuid, period)
            {
                Reason = "Взятие займа"
            };

            this.Operations.Add(newOper);

            return newOper;
        }
        
        /// <summary>
        /// Зафиксировать операцию взятия займа
        /// </summary>
        /// <returns>Список проведенных трансферов</returns>
        public virtual List<Transfer> TakeLoan(ChargePeriod period)
        {
            if (this.Operations.Any())
            {
                throw new ValidationException("Операция займа уже производилась");
            }

            this.OperationDate = period.GetCurrentInPeriodDate();

            var transfers = new List<Transfer>();
            var operation = this.CreateOperation(period);

            foreach (var loan in this.LoansFromWallets)
            {
                var transfer = this.LoanTaker.TakeLoan(this.Source,
                    loan.TypeSourceLoan,
                    loan.Sum,
                    operation,
                    period);

                if (transfer != null)
                {
                    transfers.Add(transfer);
                }
            }

            return transfers;
        }

        public virtual RealityObjectLoanWallet ReserveLoan(Wallet.Wallet target, TypeSourceLoan typeSource, decimal sum)
        {
            var rec = new RealityObjectLoanWallet(this, target, typeSource, sum);

            this.loansFromWallets.Add(rec);

            return rec;
        }
    }
}

