namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Репозиторий денежных средств дома
    /// </summary>
    public interface IRealtyObjectMoneyRepository
    {
        /// <summary>
        /// Метод возвращает трансферы дебета
        /// </summary>
        /// <param name="paymentAccounts">Счета оплат домов</param>
        /// <returns>Трансферы</returns>
        IQueryable<TransferDto> GetDebtTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts);

        /// <summary>
        /// Метод возвращает трансферы дебета
        /// </summary>
        /// <param name="guids">Идентификаторы кошельков домов</param>
        /// <returns>Трансферы</returns>
        ICollection<TransferDto> GetDebtTransfers(ICollection<string> guids);

        /// <summary>
        /// Метод возвращает трансферы кредита
        /// </summary>
        /// <param name="paymentAccounts">Счета оплат домов</param>
        /// <returns>Трансферы</returns>
        IQueryable<TransferDto> GetCreditTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts);

        /// <summary>
        /// Метод возвращает трансферы дебета
        /// </summary>
        /// <param name="guids">Идентификаторы кошельков домов</param>
        /// <returns>Трансферы</returns>
        ICollection<TransferDto> GetCreditTransfers(ICollection<string> guids);

        /// <summary>
        /// Получить идентификаторы кошельков 
        /// </summary>
        ISet<string> GetWalletGuids(ICollection<RealityObjectPaymentAccount> paymentAccounts);

        /// <summary>
        /// Метод возвращает балансы домов
        /// </summary>
        /// <param name="realityObjects">Дома</param>
        /// <returns>Балансы домов</returns>
        [Obsolete("Не актуальная информация")]
        IQueryable<RealtyObjectBalanceDto> GetRealtyBalances(IQueryable<RealityObject> realityObjects);

        /// <summary>
        /// Метод возвращает трансферы субсидий
        /// </summary>
        /// <param name="paymentAccounts">Счета оплат домов</param>
        /// <returns>Трансферы</returns>
        IQueryable<TransferDto> GetSubsidyTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts);

        /// <summary>
        /// Метод возвращает трансферы дебета
        /// </summary>
        /// <param name="paymentAccount">Счет оплат дома</param>
        /// <returns>Трансферы</returns>
        IQueryable<TransferDto> GetSubsidyTransfers(RealityObjectPaymentAccount paymentAccount);

        /// <summary>
        /// Метод возвращает информацию по займам дома
        /// </summary>
        /// <param name="realityObjects">Дома</param>
        /// <param name="anyOperations">В конечном статусе</param>
        /// <returns>Трансферы</returns>
        IQueryable<RealityObjectLoanDto> GetRealityObjectLoanSum(IQueryable<RealityObject> realityObjects, bool anyOperations = true);
    }

    /// <summary>
    /// Dto-объект займ дома
    /// </summary>
    public class RealityObjectLoanDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long RealityObjectId { get; set; }

        /// <summary>
        /// Сумма займов
        /// </summary>
        public decimal LoanSum { get; set; }

        /// <summary>
        /// Сумма возвратов займов
        /// </summary>
        public decimal LoanReturnedSum { get; set; }
    }

    /// <summary>
    /// Dto-объект трансфера
    /// </summary>
    public class TransferDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор владельца трансфера
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// Причина (наименование операции)
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Плательщик/получатель/основание
        /// </summary>
        public string OriginatorName { get; set; }

        /// <summary>
        /// Сумма опирации
        /// </summary>
        public decimal Amount { get; set; }
    }

    public class RealtyObjectBalanceDto
    {
        public long RealtyObjectId { get; set; }

        public decimal Debt { get; set; }

        public decimal Credit { get; set; }

        public decimal Lock { get; set; }

        public decimal Loan { get; set; }
    }
}