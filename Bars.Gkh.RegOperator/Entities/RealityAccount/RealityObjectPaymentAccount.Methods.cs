namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;

    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Extenstions;

    using Domain.ValueObjects;
    using Enums;
    using Exceptions;

    using Gkh.Domain.CollectionExtensions;
    using Loan;
    using Refactor;
    using ValueObjects;
    using Wallet;

    /// <summary>
    /// Счет оплат дома
    /// </summary>
    public partial class RealityObjectPaymentAccount
    {
        /// <summary>Получить баланс</summary>
        /// <returns>Баланс</returns>
        public virtual decimal GetBalance()
        {
            return this.DebtTotal - this.CreditTotal;
        }

        /// <summary>
        /// Закинуть деньги на кошелек
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="money">Поток средств</param>
        /// <param name="needCheckWallet">Необходимость в проверке владельца кошелька</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer StoreMoney(IWallet wallet, MoneyStream money, bool needCheckWallet = true)
        {
            if (needCheckWallet && !this.IsOwner(wallet))
            {
                throw new WalletException("Кошелёк не принадлежит счёту оплаты дома");
            }

            this.DebtTotal += money.Amount;

            return wallet.StoreMoney(TransferBuilder.Create(this, money));
        }

        /// <summary>
        /// Снять деньги с кошелка
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="money">Поток средств</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer TakeMoney(IWallet wallet, MoneyStream money)
        {
            this.CreditTotal += money.Amount;

            return wallet.TakeMoney(TransferBuilder.Create(this, money));
        }

        /// <summary>
        /// Отменить оплату
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="money">Поток средств</param>
        /// <param name="needCheckWallet">Необходимость в проверке владельца кошелька</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer UndoPayment(IWallet wallet, MoneyStream money, bool needCheckWallet = true)
        {
            if (needCheckWallet && !this.IsOwner(wallet))
            {
                throw new WalletException("Кошелёк не принадлежит счёту оплаты дома");
            }

            this.DebtTotal -= money.Amount;

            money.IsAffect = true;

            return wallet.TakeMoney(TransferBuilder.Create(this, money));
        }

        /// <summary>
        /// Зарезервировать займ
        /// </summary>
        /// <param name="loan">Займ</param>
        /// <param name="loanSourceType">Источник займа</param>
        /// <param name="amount">Сумма</param>
        /// <returns>Кошелек займа</returns>
        public virtual RealityObjectLoanWallet ReserveLoan(
            RealityObjectLoan loan,
            TypeSourceLoan loanSourceType,
            decimal amount)
        {
            var wallet = this.GetWallet(loanSourceType);
            return loan.ReserveLoan(wallet, loanSourceType, amount);
        }

        /// <summary>
        /// Взять займ по типу у источника
        /// </summary>
        /// <param name="source">Источник займа</param>
        /// <param name="loanSourceType">Тип источника займа</param>
        /// <param name="amount">Сумма заимствования</param>
        /// <param name="operation">Операция</param>
        /// <param name="period">Период</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer TakeLoan(
            RegopCalcAccount source,
            TypeSourceLoan loanSourceType,
            decimal amount,
            MoneyOperation operation,
            ChargePeriod period)
        {
            var targetWallet = this.GetWallet(loanSourceType);

            var moneyStream = new MoneyStream(source, operation, DateTime.Now, amount)
            {
                Description = $"Взятие займа ({loanSourceType.GetEnumMeta().Display})",
                IsAffect = true
            };

            var tr = targetWallet.StoreMoney(TransferBuilder.Create(this, moneyStream));

            tr.IsLoan = true;

            this.Loan += amount;
            this.DebtTotal += amount;

            return tr;
        }

        /// <summary>
        /// Вернуть займ
        /// </summary>
        /// <param name="calcAccount">Расчетный счет</param>
        /// <param name="walletToReturnFrom">Кошелёк</param>
        /// <param name="amount">Сумма возврата</param>
        /// <param name="operation">Операция</param>
        /// <param name="period">Период</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer ReturnLoan(
            CalcAccount calcAccount,
            Wallet.Wallet walletToReturnFrom,
            decimal amount,
            MoneyOperation operation,
            ChargePeriod period)
        {
            if (walletToReturnFrom.Balance < amount)
            {                
             //   throw new WalletException("Недостаточно средств для возврата долга!");
            }

            var moneyStream = new MoneyStream(calcAccount, operation, DateTime.Now, amount)
            {
                Description = operation.Reason ?? "Возврат займа",
                IsAffect = true
            };

            var transfer = walletToReturnFrom.TakeMoney(TransferBuilder.Create(this, moneyStream));

            if (transfer != null)
            {
                transfer.IsReturnLoan = true;
            }

            this.Loan -= amount;
            this.DebtTotal -= amount;

            return transfer;
        }

        /// <summary>
        /// Заблокировать деньги на определенном кошельке
        /// </summary>
        /// <param name="wallet">Кошелёк</param>
        /// <param name="operation">Операция</param>
        /// <param name="amount">Сумма блокирования</param>
        /// <param name="targetGuid">Будущий получатель денег</param>
        /// <returns>Трансфер</returns>
        public virtual MoneyLock LockMoney(
            IWallet wallet,
            MoneyOperation operation,
            decimal amount,
            string targetGuid)
        {
            this.DebtTotal -= amount;
            this.MoneyLocked += amount;

            return wallet.LockMoney(operation, amount, targetGuid);
        }

        /// <summary>
        /// Снять блокировку денег на определенном кошельке
        /// </summary>
        /// <param name="wallet">Кошелек</param>
        /// <param name="operation">Операция</param>
        /// <param name="moneyLock">Блокировка</param>
        public virtual void UnlockMoney(
            IWallet wallet,
            MoneyOperation operation,
            MoneyLock moneyLock)
        {
            this.DebtTotal += moneyLock.Amount;
            this.MoneyLocked -= moneyLock.Amount;

            wallet.UnlockMoney(moneyLock, operation);
        }

        /// <summary>
        /// Метод возвращает дату последней операции
        /// </summary>
        /// <returns> Дата </returns>
        public virtual DateTime GetLastOperDate()
        {
            // TODO: refactor this!
            var listLastOperDatesByWallet = new List<DateTime>
            {
                this.GetLastOperDateByWallet(this.BaseTariffPaymentWallet),
                this.GetLastOperDateByWallet(this.DecisionPaymentWallet),
                this.GetLastOperDateByWallet(this.PenaltyPaymentWallet),
                this.GetLastOperDateByWallet(this.TargetSubsidyWallet),
                this.GetLastOperDateByWallet(this.FundSubsidyWallet),
                this.GetLastOperDateByWallet(this.RegionalSubsidyWallet),
                this.GetLastOperDateByWallet(this.StimulateSubsidyWallet),
                this.GetLastOperDateByWallet(this.OtherSourcesWallet),
                this.GetLastOperDateByWallet(this.BankPercentWallet),
            };

            return listLastOperDatesByWallet.SafeMax(x => x);
        }

        /// <summary>
        /// Метод возвращает список guid кошельков субсидий
        /// </summary>
        /// <returns> List guid кошельков </returns>
        public virtual List<string> GetSubsidyWalletGuids()
        {
            return new List<string>
            {
                this.FundSubsidyWallet.WalletGuid,
                this.RegionalSubsidyWallet.WalletGuid,
                this.StimulateSubsidyWallet.WalletGuid,
                this.TargetSubsidyWallet.WalletGuid
            };
        }

        /// <summary>
        /// Вернуть имя Mutex'а
        /// </summary>
        /// <returns>Строка</returns>
        public virtual string GetMutexName()
        {
            return $"Reality_Object_Payment_Account_{this.Id}";
        }

        private DateTime GetLastOperDateByWallet(Wallet.Wallet wallet)
        {
            var lastInDate = wallet.InTransfers.SafeMax(x => x.PaymentDate);
            var lastOutDate = wallet.OutTransfers.SafeMax(x => x.PaymentDate);

            return lastInDate > lastOutDate ? lastInDate : lastOutDate;
        }
    }
}
