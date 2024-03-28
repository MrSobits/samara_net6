namespace Bars.Gkh.RegOperator.Dto
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Трансфер между источником и получателем денег
    /// </summary>
    public class TransferDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Источник денег
        /// </summary>
        public string SourceGuid { get; protected set; }

        /// <summary>
        /// Получатель денег
        /// </summary>
        public string TargetGuid { get; protected set; }

        /// <summary>
        /// Коэффициент суммы у получателя
        /// </summary>
        public int TargetCoef { get; set; }

        /// <summary>
        /// Операция, в рамках которой проводился трансфер
        /// </summary>
        public MoneyOperationDto Operation { get; set; }

        /// <summary>
        /// Операция, в рамках которой проводился трансфер
        /// </summary>
        public long OperationId { get; set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal Amount { get; protected set; }

        /// <summary>
        /// Причина перевода
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Плательщик/получатель/основание
        /// </summary>
        public string OriginatorName { get; set; }

        /// <summary>
        /// Дата фактической оплаты. Важно знать, когда оплата садится задним числом
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Признак того, что трансфер является "транзитным".
        /// Т.е. сначала трансфер произошел на целевой кошелек, а затем просто закинулся на другой
        /// </summary>
        public bool IsInDirect { get; set; }

        /// <summary>
        /// Влияющий на баланс
        /// </summary>
        public bool IsAffect { get; set; }

        /// <summary>
        /// Является займом
        /// </summary>
        public bool IsLoan { get; set; }

        /// <summary>
        /// Является возвратом займа
        /// </summary>
        public bool IsReturnLoan { get; set; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public long ChargePeriodId { get; set; }

        /// <summary>
        /// Тип источника поступления
        /// </summary>
        public TypeTransferSource PaymentSource { get; set; }
    }
}