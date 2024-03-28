namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;

    /// <summary>
    /// Сервис получения <see cref="ContragentRschetProxy"/>
    /// </summary>
    public class ContragentRschetSelectorService : BaseProxySelectorService<ContragentRschetProxy>
    {
        public static readonly Lazy<long> RegopContragentId = new Lazy<long>(ContragentRschetSelectorService.InitRegopId);

        /// <inheritdoc />
        protected override ICollection<ContragentRschetProxy> GetAdditionalCache()
        {
            var regopCalcAccountRepository = this.Container.ResolveRepository<RegopCalcAccount>();
            var specialCalcAccountRepository = this.Container.ResolveRepository<SpecialCalcAccount>();

            using (this.Container.Using(regopCalcAccountRepository, specialCalcAccountRepository))
            {
                return ContragentRschetSelectorService.GetProxies(regopCalcAccountRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds),
                        specialCalcAccountRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds))
                    .ToList();
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, ContragentRschetProxy> GetCache()
        {
            var regopCalcAccountRepository = this.Container.ResolveRepository<RegopCalcAccount>();
            var specialCalcAccountRepository = this.Container.ResolveRepository<SpecialCalcAccount>();

            using (this.Container.Using(regopCalcAccountRepository, specialCalcAccountRepository))
            {
                var regopCalcAccountQuery = this.FilterService.FilterByContragent(regopCalcAccountRepository.GetAll(), x => x.AccountOwner);
                var specialCalcAccountQuery = this.FilterService.FilterByContragent(specialCalcAccountRepository.GetAll(), x => x.AccountOwner);

                return ContragentRschetSelectorService.GetProxies(regopCalcAccountQuery, specialCalcAccountQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        public static IList<ContragentRschetProxy> GetProxies(IQueryable<RegopCalcAccount> regopCalcQuery,
            IQueryable<SpecialCalcAccount> specialCalcQuery)
        {
            var regopContragentId = ContragentRschetSelectorService.RegopContragentId.Value;
            var regopCalcAccounts = regopCalcQuery
                .Select(x => new
                {
                    x.Id,
                    x.ContragentCreditOrg.SettlementAccount,
                    BankContragentId = x.CreditOrg.GetNullableId(),
                    x.CreditOrg.CorrAccount,
                    ContragentId = x.AccountOwner.GetNullableId(),
                    x.DateOpen,
                    x.ObjectCreateDate,
                    x.DateClose,

                    IsFilial = false,
                    CalcAccountId = x.Id
                })
                .AsEnumerable()
                .Select(x => new ContragentRschetProxy
                {
                    Id = x.Id,
                    SettlementAccount = x.SettlementAccount,
                    BankContragentId = x.BankContragentId,
                    ContragentId = x.ContragentId,
                    CorrAccount = x.CorrAccount,
                    OpenDate = x.DateOpen.IsValid() ? x.DateOpen : x.ObjectCreateDate,
                    CloseDate = x.DateClose,

                    IsFilial = false,
                    CalcAccountId = x.Id,
                    IsRegopAccount = x.ContragentId == regopContragentId,
                    RegopAccountType = x.ContragentId == regopContragentId ? 1 : default(int?)
                })
                .ToList();

            var specialCalcAccounts = specialCalcQuery
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    x.Id,
                    SettlementAccount = x.AccountNumber,
                    BankContragentId = x.CreditOrg.GetNullableId(),
                    x.CreditOrg.CorrAccount,
                    ContragentId = x.AccountOwner.GetNullableId(),
                    x.DateOpen,
                    x.ObjectCreateDate,
                    x.DateClose,

                    IsFilial = false,
                    CalcAccountId = x.Id
                })
                .AsEnumerable()
                .Select(x => new ContragentRschetProxy
                {
                    Id = x.Id,
                    SettlementAccount = x.SettlementAccount,
                    BankContragentId = x.BankContragentId,
                    ContragentId = x.ContragentId,
                    CorrAccount = x.CorrAccount,
                    OpenDate = x.DateOpen.IsValid() ? x.DateOpen : x.ObjectCreateDate,
                    CloseDate = x.DateClose,

                    IsFilial = false,
                    CalcAccountId = x.Id,
                    IsRegopAccount = x.ContragentId == regopContragentId,
                    RegopAccountType = x.ContragentId == regopContragentId ? 1 : default(int?)
                })
                .ToList();

            return regopCalcAccounts
                .Union(specialCalcAccounts)
                .ToList();
        }

        private static long InitRegopId()
        {
            var container = ApplicationContext.Current.Container;
            var regOpRepository = container.ResolveRepository<RegOperator>();
            using (container.Using(regOpRepository))
            {
                return regOpRepository.GetAll()
                    .Select(x => x.Contragent.GetNullableId())
                    .FirstOrDefault() ?? -1;
            }
        }
    }
}