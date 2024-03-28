namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl;

    using Enums;
    using Gkh.Entities;
    using Entities;

    /// <summary>
    /// Интерфейс для работы с оплатами
    /// </summary>
    public interface IRealityObjectPaymentService
    {
        /// <summary>
        /// Создать операцию оплаты
        /// </summary>
        /// <param name="ro">Объект недвижимости</param>
        /// <param name="sum">Сумма операции</param>
        /// <param name="opType"><see cref="PaymentOperationType">Тип операции</see></param>
        /// <param name="operationDate">Дата операции</param>
        /// <param name="paymentAccount">Счет оплат дома</param>
        /// <param name="subsidyAccount"></param>
        RealityObjectPaymentAccountOperation CreatePaymentOperation(
            RealityObject ro,
            decimal sum,
            PaymentOperationType opType,
            DateTime operationDate,
            RealityObjectPaymentAccount paymentAccount = null,
            RealityObjectSubsidyAccount subsidyAccount = null);

        /// <summary>
        /// Получение Dictionary с суммами раздела Дебет
        /// </summary>
        IDictionary<long, RealityObjectPaymentService.AccountOperationsInfo> GetDebetOperationsSum(
            long? mrId, long? moId, long[] roIds, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Получение Dictionary с суммами раздела Кредит
        /// </summary>
        IDictionary<long, RealityObjectPaymentService.AccountOperationsInfo> GetCreditOperationsSum(
            long? mrId, long? moId, long[] roIds, DateTime? startDate = null, DateTime? endDate = null);

        Dictionary<long, RealityObjectPaymentSummary> GetRobjectAccountSummary(IQueryable<RealityObject> query);
    }
}