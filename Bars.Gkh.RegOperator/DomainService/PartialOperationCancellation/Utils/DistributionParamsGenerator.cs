namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Класс хелпер для банковских выписок
    /// </summary>
    public static class DistributionParamsGenerator
    {
        /// <summary>
        /// Сформировать параметры для распределения средств
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="distribution">Распределение</param>
        /// <param name="distributionSum">Суммы распределения</param>
        /// <returns></returns>
        public static IDistributionArgs GetArgs(
            IDistributable distributable, 
            IDistribution distribution, 
            IDictionary<ITransferOwner, decimal> distributionSum)
        {
            if (distribution is AbstractPersonalAccountDistribution && distributable.MoneyDirection == MoneyDirection.Income)
            {
                return new DistributionByAccountsArgs(
                    distributable,
                    DistributeOn.Charges,
                    distributionSum
                        .Select(x => new DistributionByAccountsArgs.ByPersAccountRecord((BasePersonalAccount)x.Key, x.Value, 0))
                        .ToArray());
            }

            throw new NotSupportedException("Данный вид распределения не поддерживается");
        }
    }
}