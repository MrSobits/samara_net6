namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для сущности "Жилой дом расчетного счета"
    /// </summary>
    public class CalcAccountRealityObjectService : ICalcAccountRealityObjectService
    {
        /// <summary>
        /// Windsor-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод возвращающий список домов, которые можно добавить
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListRobjectToAdd(BaseParams baseParams)
        {
            var domain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var realityObjectDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var roQuery = roDomain.GetAll()

                    // не показываем дома если они уже хотябы раз добавлены в счет по задаче 43420
                    .Where(
                        y => !domain.GetAll().Any(
                            x => x.Account.TypeAccount != TypeCalcAccount.Special
                                && x.RealityObject.Id == y.Id
                                && (!x.DateEnd.HasValue || x.DateEnd >= DateTime.Now)))
                    .Select(x => x.Id);

                var roIds = roQuery.Distinct().ToArray();

                roIds = realityObjectDecisionsService.GetRobjectsFundFormation(roQuery)
                    .Where(x => x.Value.FirstOrDefault() != null && x.Value.FirstOrDefault().Item2 == CrFundFormationDecisionType.RegOpAccount)
                    .Select(x => x.Key)
                    .ToArray();

                var data = roDomain.GetAll()
                    .Where(x => roIds.Contains(x.Id))
                    .Select(x => new {x.Id, x.Address, Municipality = x.Municipality.Name})
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(roDomain);
                this.Container.Release(realityObjectDecisionsService);
            }
        }

        /// <summary>
        /// Метод массового создания сущностей "Жилой дом расчетного счета"
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult MassCreate(BaseParams baseParams)
        {
            var regopcalcDomain = this.Container.ResolveDomain<RegopCalcAccount>();
            var domain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var realityObjectDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();

            try
            {
                var roIds = baseParams.Params.GetAs("roIds", new long[0]);
                var accId = baseParams.Params.GetAs<long>("accId");

                var regopAccount = regopcalcDomain.GetAll()
                    .Where(x => x.Id == accId)
                    .Select(x => new {x.DateOpen, x.DateClose})
                    .FirstOrDefault();

                if (regopAccount == null)
                {
                    return new BaseDataResult();
                }

                var now = DateTime.Now;

                long[] ids = roIds;
                var existing =
                    domain.GetAll()
                        .Where(x => ids.Contains(x.RealityObject.Id))
                        .WhereIf(regopAccount.DateClose.HasValue, x => x.Account.DateOpen <= regopAccount.DateClose)
                        .Where(x => !x.Account.DateClose.HasValue || x.Account.DateClose >= regopAccount.DateOpen)
                        .Where(x => !(x.Account is SpecialCalcAccount) || ((SpecialCalcAccount) x.Account).IsActive)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value > now)
                        .Select(x => x.RealityObject.Id)
                        .ToHashSet();

                if (existing.Count > 0)
                {
                    return BaseDataResult.Error("Один из выбранных домов уже обслуживается другим расчетным счетом");
                }

                roIds = roIds.Distinct().ToArray();
                var fundFormationDecisions = realityObjectDecisionsService.GetRobjectsFundFormationForRecalc(roIds);
                if (fundFormationDecisions.Any(x => x.Value.IsEmpty() || x.Value.Any(y => y.Item2 != CrFundFormationDecisionType.RegOpAccount)))
                {
                    return BaseDataResult.Error("У одного из выбранных домов отсутствует протокол решения о переходе на счет регионального оператора");
                }

                if (fundFormationDecisions.Any(x => x.Value.Any(y => !y.Item1.IsValid())))
                {
                    return BaseDataResult.Error("У одного из выбранных домов не указана дата вступления в силу протокола решения");
                }

                this.Container.InTransaction(
                    () =>
                        roIds.ForEach(
                            x => domain.Save(
                                new CalcAccountRealityObject
                                {
                                    DateStart = fundFormationDecisions.Get(x).Max(y => y.Item1),
                                    RealityObject = new RealityObject {Id = x},
                                    Account = new CalcAccount {Id = accId}
                                })));

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(regopcalcDomain);
                this.Container.Release(domain);
                this.Container.Release(realityObjectDecisionsService);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForRegop(BaseParams baseParams)
        {
            var regopId = baseParams.Params.GetAsId("regopId");
            var accountId = baseParams.Params.GetAsId("accId");
            var typeAccount = baseParams.Params.GetAs<TypeCalcAccount>("typeAccount");

            var calcAccountRealityObjectDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var regOperatorDomain = this.Container.ResolveDomain<RegOperator>();
            using (this.Container.Using(calcAccountRealityObjectDomain, regOperatorDomain))
            {
                return calcAccountRealityObjectDomain.GetAll()
                    .Where(x => regOperatorDomain.GetAll().Where(y => y.Id == regopId).Any(y => y.Contragent == x.Account.AccountOwner))
                    .Where(x => x.Account.TypeAccount == typeAccount)
                    .WhereIf(typeAccount == TypeCalcAccount.Regoperator, x => x.Account.Id == accountId)
                    .Where(x => x.DateStart >= DateTime.MinValue)
                    .Select(
                        x => new
                        {
                            Municipality = x.RealityObject.Municipality.Name,
                            x.RealityObject.Address,
                            x.Account.AccountNumber,
                            x.DateStart,
                            x.DateEnd
                        })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}