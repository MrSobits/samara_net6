namespace Bars.Gkh.RegOperator.Entities.Loan
{
    using System;
    using System.Linq;
    
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using Domain;

    using Enums;

    using Gkh.Domain.CollectionExtensions;

    using ValueObjects;

    using Wallet;

    /// <summary>Связь кошельков домов для займа</summary>
    public class RealityObjectLoanWallet : BaseImportableEntity
    {
        /// <summary>
        /// For NH
        /// </summary>
        protected RealityObjectLoanWallet()
        {

        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="loan">Займ</param>
        /// <param name="wallet">Кошелек получателя</param>
        /// <param name="typeSourceLoan">Тип истоника займа</param>
        /// <param name="sum">Сумма</param>
        public RealityObjectLoanWallet(RealityObjectLoan loan, Wallet wallet, TypeSourceLoan typeSourceLoan, decimal sum)
        {
            ArgumentChecker.NotNull(loan, "loan");
            ArgumentChecker.NotNull(wallet, "wallet");
            ArgumentChecker.ValidEnumerationValue(typeSourceLoan, "typeSourceLoan");

            if (sum <= 0)
            {
                throw new ArgumentException("Сумма займа должна быть больше 0");
            }

            this.Loan = loan;
            this.Wallet = wallet;
            this.TypeSourceLoan = typeSourceLoan;
            this.Sum = sum;
        }

        /// <summary>
        /// Займ
        /// </summary>
        public virtual RealityObjectLoan Loan { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public virtual Wallet Wallet { get; set; }

        /// <summary>
        /// Тип источника
        /// </summary>
        public virtual TypeSourceLoan TypeSourceLoan { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Погашенная сумма
        /// </summary>
        public virtual decimal ReturnedSum { get; set; }

        /// <summary>Возврат долга</summary>
        /// <param name="wallet">Кошелек с которого списывается долг</param>
        /// <param name="period"></param>
        /// <param name="mode">Тип запуска</param>
        /// <returns>The <see cref="Transfer"/>.</returns>
        public virtual Transfer Repayment(
            Wallet wallet,
            ChargePeriod period,
            ExecutionMode mode = ExecutionMode.Sequential)
        {
            if (this.Sum == this.ReturnedSum)
            {
                return null;
            }

            //var inTransfers = wallet.InTransfers
            //     .Where(x => x.OperationDate > this.Loan.OperationDate || x.ObjectCreateDate.AddDays(1) > this.Loan.OperationDate)
            //        .Where(x => !x.IsLoan).ToList();

            //var outTransfers = wallet.RealityObjectOutTransfers
            //       .Where(x => x.OperationDate > this.Loan.OperationDate)
            //       .Where(x => x.CopyTransfer == null).ToList();

            //var inTransfersSum = wallet.InTransfers
            //    .Where(x => x.OperationDate > this.Loan.OperationDate || x.ObjectCreateDate.AddDays(1) > this.Loan.OperationDate)
            //       .Where(x => !x.IsLoan).SafeSum(x => x.Amount);

            //var outTransfersSum = wallet.RealityObjectOutTransfers
            //       .Where(x => x.OperationDate > this.Loan.OperationDate.AddDays(1))
            //       .Where(x => x.CopyTransfer == null).SafeSum(x => x.Amount);

            //var balance =
            //    wallet.InTransfers
            //        .Where(x => !x.IsLoan)
            //        //привязка к дате взятия займа заменяется на дату операци / или дату создания трансфера (реального распределения оплаты)
            //        // если оплата распределена после взятия займа, она в любом случае попадет под сумму к возврату, если не было создано расходных операций
            //        // прибавляем день к дате займа чтобы операции одного дня с займом учитывались в оплатах и не учитывались в расходах
            //        .Where(x => x.OperationDate > this.Loan.OperationDate || x.ObjectCreateDate.AddDays(1) > this.Loan.OperationDate)
            //        .SafeSum(x => x.Amount)
            //    - wallet.RealityObjectOutTransfers
            //        .Where(x => x.CopyTransfer == null)
            //      .Where(x => x.OperationDate > this.Loan.OperationDate.AddDays(1))
            //        .SafeSum(x => x.Amount);

            // Баланс по лс решено считать за весь период
            var balance =
                wallet.InTransfers
                    //    .Where(x => !x.IsLoan)
                    //    .Where(x => x.OperationDate > this.Loan.OperationDate)
                    .SafeSum(x => x.Amount)
                - wallet.RealityObjectOutTransfers
                    //     .Where(x => x.CopyTransfer == null)
                    //     .Where(x => x.OperationDate > this.Loan.OperationDate)
                    .SafeSum(x => x.Amount);

            var debt = this.Sum - this.ReturnedSum;

            var sumReturned = Math.Min(balance, debt);

            if (sumReturned <= 0)
            {
                return null;
            }

            var operation = this.Loan.CreateOperation(period);

            operation.Reason = "Возврат займа ({0})".FormatUsing(this.TypeSourceLoan.GetEnumMeta().Display);
            var transfer = this.Loan.LoanTaker.ReturnLoan(this.Loan.Source,
                wallet,
                sumReturned,
                operation,
                period);

            this.ReturnedSum += sumReturned;

            return transfer;
        }
    }
}