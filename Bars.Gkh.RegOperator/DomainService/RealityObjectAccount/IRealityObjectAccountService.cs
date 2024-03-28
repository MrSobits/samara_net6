namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount
{
    using System.Collections.Generic;
    using B4;
    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Интерфейс для работы с данными по счетам МКД
    /// </summary>
    public interface IRealityObjectAccountService
    {
        /// <summary>
        /// Получить результат для отображения счета начислений
        /// </summary>
        IDataResult GetChargeAccountResult(BaseParams baseParams);

        /// <summary>
        /// Получить результат для отображения счета оплат
        /// </summary>
        IDataResult GetPaymentAccountResult(BaseParams baseParams);

        /// <summary>
        /// Получить результат для отображения счета расчета с поставщиками
        /// </summary>
        IDataResult GetSupplierAccountResult(BaseParams baseParams);

        RealityObjectPaymentAccount GetPaymentAccount(RealityObject robject);

        RealityObjectChargeAccount GetChargeAccount(RealityObject robject);

        RealityObjectSupplierAccount GetSupplierAccount(RealityObject robject);

        RealityObjectChargeAccountOperation GetLastClosedOperation(RealityObject robject);

        /// <summary>
        /// Получить результат для отображения счета оплат
        /// </summary>
        IDataResult GetSubsidyAccountResult(BaseParams baseParams);

        IDataResult GetPaymentAccountBySources(BaseParams baseParams);

        IEnumerable<RealityObjectChargeAccount> GetChargeAccounts(IEnumerable<RealityObject> realties);
    }
}