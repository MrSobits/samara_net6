namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Data.Common;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using B4.IoC;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Utils;

    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using DomainService;

    using Castle.Windsor;
    using DataResult;
    using Entities;
    using Enums;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;

    using RealityObjectAccount;

    /// <summary>
    /// Сервис рассчета счетов реестр домов регионального оператора
    /// </summary>
    public class RegopCalcAccountService : IRegopCalcAccountService
    {
        /// <summary>
        /// Контейнер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получение счетов реестр домов регионального оператора
        /// </summary>
        /// <param name="ro"></param>
        /// <returns></returns>
        public RegopCalcAccount GetRegopAccount(RealityObject ro)
        {
            var domain = this.Container.ResolveDomain<CalcAccountRealityObject>();

            using (this.Container.Using(domain))
            {
                return
                    domain.GetAll()
                        .Where(x => x.RealityObject.Id == ro.Id)
                        .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                        .Where(x => x.Account.DateOpen <= DateTime.Today)
                        .Where(x => !x.Account.DateClose.HasValue || x.Account.DateClose >= DateTime.Today)
                        .Where(x => x.DateStart <= DateTime.Today && (!x.DateEnd.HasValue || x.DateEnd >= DateTime.Today))
                        .OrderByDescending(x => x.Account.DateOpen)
                        .Select(x => x.Account)
                        .FirstOrDefault() as RegopCalcAccount;
            }
        }

        /// <summary>
        /// Отображения суммы счетов реестр домов регионального оператора
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListRegister(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var query = this.GetProxyQueryable(baseParams).Filter(loadParams, this.Container);

            return new ListSummaryResult(
                query.Order(loadParams).Paging(loadParams).ToList(),
                query.Count(),
                new
                    {
                        ChargeTotal = query.SafeSum(x => x.ChargeTotal),
                        PaidTotal = query.SafeSum(x => x.PaidTotal),
                        Debt = query.SafeSum(x => x.Debt),
                        Saldo = query.SafeSum(x => x.Saldo)
                    });

        }

        /// <summary>
        /// Формирования выборки 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IQueryable<RegopCalcAccountRoProxy> GetProxyQueryable(BaseParams baseParams)
        {
            /*
             * Вообщем этот метод получает дома Рег оператора по следующим условиям
                - Если дом находится в счете Рег Оператора
                - Если на доме есть актуальное решение (На Счете рег Оператора) но его еще не добавили в Счет рег оператора
                Если дом есть в счете рег оператора, то показываем номер, иначе номер пуст
             */

            var domain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var chaaccroDomain = this.Container.ResolveDomain<RealityObjectChargeAccount>();
            var ropayService = this.Container.Resolve<IRealityObjectPaymentService>();
            var roDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var chaccoperDomain = this.Container.Resolve<IDomainService<RealityObjectChargeAccountOperation>>();
            var crFundDecisDomain = this.Container.Resolve<IDomainService<CrFundFormationDecision>>();
            var paymentAccDomain = this.Container.ResolveDomain<RealityObjectPaymentAccount>();
            var calcAccRoDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            
            using (this.Container.Using(domain, chaaccroDomain, ropayService, roDecisionsService, roDomain, crFundDecisDomain))
            {
                var roQuery = calcAccRoDomain.GetAll()
                    .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                    .Where(x => x.DateStart <= DateTime.Today && (!x.DateEnd.HasValue || x.DateEnd >= DateTime.Today));

                var paymentAccounts = paymentAccDomain.GetAll()
                    .Where(x => roQuery.Any(z => z.RealityObject.Id == x.RealityObject.Id))
                    .Select(x => new {x.RealityObject.Id, x.DebtTotal, x.CreditTotal})
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Sum(x => (decimal?)(x.DebtTotal - x.CreditTotal))
                              .GetValueOrDefault());

                var paidTotal = chaccoperDomain.GetAll()
                    .Select(x => new
                                {
                                    isClosed = x.Period.IsClosed, 
                                    startDate = x.Period.StartDate, 
                                    x.ChargedPenalty, 
                                    x.PaidTotal, 
                                    x.PaidPenalty, 
                                    x.ChargedTotal, 
                                    x.SaldoIn, 
                                    x.SaldoOut, 
                                    RoId = x.Account.RealityObject.Id
                                })
                    .AsEnumerable()
                    .OrderBy(x => x.startDate)
                    .GroupBy(x => x.RoId)
                    .ToDictionary(
                        x => x.Key,
                        y => 
                            {
                                var lastOrDefault = y.LastOrDefault(x => x.isClosed);
                                return lastOrDefault != null
                                    ? new
                                        {
                                            PaidTotal = y.SafeSum(x => x.PaidTotal + x.PaidPenalty), 
                                            ChargeTotal = y.SafeSum(x => x.ChargedTotal),
                                            saldoOut = lastOrDefault.SaldoOut
                                        }
                                    : null;
                            });

                var dictAccounts = domain.GetAll()
                    .Where(x => (x.Account.TypeAccount == TypeCalcAccount.Regoperator))
                    .Select(x => new
                                {
                                    AccountId = x.Account.Id,
                                    x.Account.AccountNumber,
                                    RoId = x.RealityObject.Id,
                                    AccountOwner = x.Account.AccountOwner.Name
                                })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, xy => xy.First());

                return roQuery
                    .Select(
                        x => new
                        {
                            Municipality =
                                x.RealityObject.Municipality.ParentMo == null
                                    ? x.RealityObject.Municipality.Name
                                    : x.RealityObject.Municipality.ParentMo.Name,
                            Settlement =
                                x.RealityObject.MoSettlement != null
                                    ? x.RealityObject.MoSettlement.Name
                                    : (x.RealityObject.Municipality.ParentMo != null ? x.RealityObject.Municipality.Name : ""),
                            x.RealityObject.Address,
                            RoId = x.RealityObject.Id
                        })
                    .AsEnumerable()
                    .DistinctBy(x => x.RoId)
                    .Select(
                        x => new RegopCalcAccountRoProxy
                        {
                            AccountId = dictAccounts.Get(x.RoId).Return(y => y.AccountId),
                            AccountNumber = dictAccounts.Get(x.RoId).Return(y => y.AccountNumber) ?? string.Empty,
                            Municipality = x.Municipality,
                            MoSettlement = x.Settlement,
                            Address = x.Address,
                            AccountOwner = dictAccounts.Get(x.RoId).Return(y => y.AccountOwner),
                            RealityObjectId = x.RoId,
                            ChargeTotal = paidTotal.Get(x.RoId).Return(y => y.ChargeTotal),
                            PaidTotal = paidTotal.Get(x.RoId).Return(y => y.PaidTotal),
                            Debt = paidTotal.Get(x.RoId).Return(y => y.ChargeTotal - y.PaidTotal),
                            Saldo = paymentAccounts.Get(x.RoId).Return(y => y),
                        })
                    .AsQueryable();
            }
        }

        public IDataResult ListByRegop(BaseParams baseParams)
        {
            var regopId = baseParams.Params.GetAsId("regopId");

            var calcAccountDomain = this.Container.ResolveDomain<RegopCalcAccount>();
            var regOperatorDomain = this.Container.ResolveDomain<RegOperator>();
            using (this.Container.Using(calcAccountDomain, regOperatorDomain))
            {
                return calcAccountDomain.GetAll()
                    .Where(x => regOperatorDomain.GetAll().Where(y => y.Id == regopId).Any(y => y.Contragent == x.AccountOwner))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.AccountNumber
                        })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}