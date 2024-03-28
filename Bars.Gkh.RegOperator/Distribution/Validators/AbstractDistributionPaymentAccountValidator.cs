namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Валидатор для проверки соответствия р/с получателя банковской выписки
    /// </summary>
    public abstract class AbstractDistributionPaymentAccountValidator<TDistribution> : IDistributionValidator 
        where TDistribution : IDistribution
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="CalcAccountRealityObject"/>
        /// </summary>
        public IDomainService<CalcAccountRealityObject> CalcAccountRealityObjectDomain { get; set; }

        /// <inheritdoc />
        public string Code => typeof(TDistribution).Name;

        /// <inheritdoc />
        public bool IsMandatory => false;

        public bool IsApply => false;

        /// <inheritdoc />
        public IDataResult Validate(BaseParams baseParams)
        {
            var distributables = DistributionProviderImpl.GetDistributables(baseParams).Cast<BankAccountStatement>();
            var distribution = this.Container.Resolve<IDistribution>(this.Code);

            using (this.Container.Using(distribution))
            {
                foreach (var distributable in distributables)
                {
                    // проверка только для приходных выписок
                    if (distributable.MoneyDirection != MoneyDirection.Income)
                    {
                        continue;
                    }

                    var paymentAccountNumber = distributable.RecipientAccountNum;
                    var realityObjects = this.ExtractRealityObjectIds(baseParams, distribution);
                    var paymentAccountNumDict = this.CalcAccountRealityObjectDomain.GetAll()
                        .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive) || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                        .WhereContains(y => y.RealityObject.Id, realityObjects)
                        .Where(y => y.Account.DateOpen <= DateTime.Today)
                        .Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
                        .Where(
                            y => (y.Account.TypeAccount == TypeCalcAccount.Special)
                                || (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today
                                    && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
                        .Select(y => new
                        {
                            RoId = y.RealityObject.Id,
                            y.Account.DateOpen,
                            AccountNumber = y.Account.AccountNumber ?? ((RegopCalcAccount)y.Account).ContragentCreditOrg.SettlementAccount
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.DateOpen).First().AccountNumber);

                    if (realityObjects.Any(x => paymentAccountNumDict.Get(x) != paymentAccountNumber))
                    {
                        return BaseDataResult.Error("Выявлено несоответствие р/с получателя");
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Извлечь дома из параметров распределения
        /// </summary>
        /// <param name="baseParams">Параметры распределения</param>
        /// <param name="distribution">Распределение</param>
        /// <returns>Массив домов</returns>
        protected abstract long[] ExtractRealityObjectIds(BaseParams baseParams, IDistribution distribution);
    }
}