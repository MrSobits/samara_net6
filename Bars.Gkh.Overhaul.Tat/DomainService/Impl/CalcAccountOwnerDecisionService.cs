namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using NHibernate.Linq;

    public class CalcAccountOwnerDecisionService : ICalcAccountOwnerDecisionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<SpecialCalcAccount> SpecialCalcAccountDomain { get; set; }
        public IDomainService<RegopCalcAccount> RegopCalcAccountDomain { get; set; }
        public IDomainService<CalcAccountRealityObject> CalcAccountRealityObjectDomain { get; set; }
        public IDomainService<CreditOrg> CreditOrgDomain { get; set; }
        public IDomainService<ContragentBankCreditOrg> ContragentBankCreditOrgDomain { get; set; }

        private IDictionary<string, CreditOrg> creditOrgCache = new Dictionary<string, CreditOrg>();
        private ISet<long> calcAccountRealityObjectCache = new HashSet<long>();

        public IDataResult SaveRegopCalcAccount(RegOpAccountDecision decision)
        {
            var contragentId = decision.RegOperator?.Contragent.Id ?? 0;
            var calcAccount = this.RegopCalcAccountDomain.GetAll()
                .Single(x => x.AccountOwner.Id == contragentId);
            
            if (calcAccount == null)
            {
                return BaseDataResult.Error("Не удалось определить счет регопа");
            }

            var toSave = new List<BaseEntity>();

            var calcRo = this.CalcAccountRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject.Id == decision.RealityObject.Id)
                .Where(x => x.Account.Id == calcAccount.Id)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();

            if (calcRo == null)
            {
                var newCalcAccRo = new CalcAccountRealityObject
                {
                    Account = calcAccount,
                    RealityObject = new RealityObject { Id = decision.RealityObject.Id },
                    DateStart = decision.ObjectCreateDate,
                };
                toSave.Add(newCalcAccRo);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, toSave, 5000, useStatelessSession: true);

            return new BaseDataResult();
        }

        public IDataResult SaveSpecialCalcAccount(SpecialAccountDecision decision)
        {
            var contragentId = decision.TypeOrganization == TypeOrganization.RegOperator
                ? decision.RegOperator?.Contragent.Id ?? 0
                : decision.ManagingOrganization?.Contragent.Id ?? 0;
            if (contragentId == 0)
            {
                return BaseDataResult.Error("Не указан владелец счета");
            }

            var calcAccount = this.SpecialCalcAccountDomain.GetAll()
                    .Where(x => x.AccountNumber == decision.AccountNumber)
                    .FirstOrDefault(x => x.AccountOwner.Id == contragentId)
                ?? this.GetSpecialCalcAccount(decision);
            
            var toSave = new List<BaseEntity>();
            if (calcAccount.CreditOrg.Id == 0)
            {
                toSave.Add(calcAccount.CreditOrg);
            }
            if (calcAccount.Id == 0)
            {
                toSave.Add(calcAccount);
            }
            
            var calcRo = this.CalcAccountRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject.Id == decision.RealityObject.Id)
                .Where(x => x.Account.Id == calcAccount.Id)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();

            if (calcRo == null)
            {
                var openDate = decision.OpenDate.GetValueOrDefault();
                var newCalcAccRo = new CalcAccountRealityObject
                {
                    Account = calcAccount,
                    RealityObject = new RealityObject { Id = decision.RealityObject.Id },
                    DateStart = openDate.IsValid() ? openDate : decision.ObjectCreateDate,
                    DateEnd = decision.CloseDate
                };
                toSave.Add(newCalcAccRo);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, toSave, 5000, useStatelessSession: true);

            return new BaseDataResult();
        }

        public IDataResult SaveRegopCalcAccount(IQueryable<RegOpAccountDecision> decisionQuery)
        {
            this.InitRegopCache();

            var regopRepos = this.Container.ResolveRepository<RegOperator>();
            using (this.Container.Using(regopRepos))
            {
                var contragentCreditOrgInfo = this.ContragentBankCreditOrgDomain.GetAll()
                    .Fetch(x => x.Contragent)
                    .Join(regopRepos.GetAll(),
                        x => x.Contragent,
                        x => x.Contragent,
                        (c, r) => c)
                    .Single();

                var regopCalcAccount = this.RegopCalcAccountDomain.GetAll()
                        .Where(x => x.ContragentCreditOrg.Id == contragentCreditOrgInfo.Id)
                        .SingleOrDefault(x => x.AccountOwner == contragentCreditOrgInfo.Contragent)
                    ?? new RegopCalcAccount
                    {
                        ContragentCreditOrg = contragentCreditOrgInfo,
                        AccountNumber = contragentCreditOrgInfo.SettlementAccount,
                        CreditOrg = contragentCreditOrgInfo.CreditOrg,
                        AccountOwner = contragentCreditOrgInfo.Contragent,
                        DateOpen = contragentCreditOrgInfo.ObjectCreateDate,
                        TypeAccount = TypeCalcAccount.Regoperator,
                        TypeOwner = TypeOwnerCalcAccount.Regoperator
                    };

                var toSave = new List<BaseEntity>();
                if (regopCalcAccount.Id == 0)
                {
                    toSave.Add(regopCalcAccount);
                }

                var roInfo = decisionQuery
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.ObjectCreateDate
                    })
                    .ToList();
                var roCalcAccCount = 0;
                foreach (var info in roInfo)
                {
                    if (!this.calcAccountRealityObjectCache.Contains(info.RoId))
                    {
                        var newCalcAccRo = new CalcAccountRealityObject
                        {
                            Account = regopCalcAccount,
                            RealityObject = new RealityObject { Id = info.RoId },
                            DateStart = info.ObjectCreateDate
                        };
                        toSave.Add(newCalcAccRo);
                        roCalcAccCount++;
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, toSave, 5000, useStatelessSession: true);

                return new BaseDataResult(toSave.Count)
                {
                    Message = $"Привязано домов к счету Регопа: {roCalcAccCount};"
                };
            }
        }

        public IDataResult SaveSpecialCalcAccount(IQueryable<SpecialAccountDecision> decisionQuery)
        {
            var query = decisionQuery
                .Where(x => (x.TypeOrganization == TypeOrganization.RegOperator && (long?) x.RegOperator.Id != null) 
                    || (x.TypeOrganization != TypeOrganization.RegOperator && (long?) x.ManagingOrganization.Id != null));

            this.InitCache(query);

            var newCreditOrgs = new HashSet<string>();
            var toSave = new List<BaseEntity>();
            var decisions = query
                .Fetch(x => x.RegOperator)
                .ThenFetch(x => x.Contragent)
                .Fetch(x => x.ManagingOrganization)
                .ThenFetch(x => x.Contragent)
                .Fetch(x => x.CreditOrg)
                .Where(y => !this.SpecialCalcAccountDomain.GetAll()
                    .Where(x => x.AccountNumber == y.AccountNumber)
                    .Any(x => x.AccountOwner.Id == (y.TypeOrganization == TypeOrganization.RegOperator
                        ? y.RegOperator.Contragent.Id
                        : y.ManagingOrganization.Contragent.Id)))
                .ToList();
            var calcAccCount = 0;
            var roCalcAccCount = 0;
            foreach (var decision in decisions)
            {
                var calcAccount = this.GetSpecialCalcAccount(decision);
                var key = this.creditOrgKey(calcAccount.CreditOrg);
                if (calcAccount.CreditOrg.Id == 0 && !newCreditOrgs.Contains(key))
                {
                    toSave.Add(calcAccount.CreditOrg);
                    newCreditOrgs.Add(key);
                }

                toSave.Add(calcAccount);
                calcAccCount++;
                if (!this.calcAccountRealityObjectCache.Contains(decision.Id))
                {
                    var openDate = decision.OpenDate.GetValueOrDefault();
                    var newCalcAccRo = new CalcAccountRealityObject
                    {
                        Account = calcAccount,
                        RealityObject = new RealityObject { Id = decision.RealityObject.Id },
                        DateStart = openDate.IsValid() ? openDate : decision.ObjectCreateDate,
                        DateEnd = decision.CloseDate
                    };
                    toSave.Add(newCalcAccRo);
                    roCalcAccCount++;
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, toSave, 5000, useStatelessSession: true);

            return new BaseDataResult(toSave.Count)
            {
                Message = $"Привязано домов к спец. счету: {roCalcAccCount};{Environment.NewLine}" +
                    $"Создано кредитных организаций: {newCreditOrgs.Count};{Environment.NewLine}" +
                    $"Создано спец счетов: {calcAccCount}"
            };
        }

        private SpecialCalcAccount GetSpecialCalcAccount(SpecialAccountDecision decision)
        {
            var creditOrg = this.GetCreditOrg(decision);

            var openDate = decision.OpenDate.GetValueOrDefault();
            return new SpecialCalcAccount
            {
                IsActive = !(decision.CloseDate.HasValue && decision.CloseDate < DateTime.Today),
                AccountNumber = decision.AccountNumber,
                CreditOrg = creditOrg,
                AccountOwner = decision.TypeOrganization == TypeOrganization.RegOperator
                    ? decision.RegOperator.Contragent
                    : decision.ManagingOrganization.Contragent,
                DateOpen = openDate.IsValid() ? openDate : decision.ObjectCreateDate,
                DateClose = decision.CloseDate,
                TypeAccount = TypeCalcAccount.Special,
                TypeOwner = decision.TypeOrganization == TypeOrganization.RegOperator
                    ? TypeOwnerCalcAccount.Regoperator
                    : TypeOwnerCalcAccount.Manorg
            };
        }

        private CreditOrg GetCreditOrg(SpecialAccountDecision entity)
        {
            var key = this.specialAccountKey(entity);
            if (this.creditOrgCache.ContainsKey(key))
            {
                return this.creditOrgCache[key];
            }

            var creditOrg = new CreditOrg
            {
                Parent = entity.CreditOrg,
                Name = entity.CreditOrg.Name,
                Inn = entity.Inn,
                Kpp = entity.Kpp,
                Ogrn = entity.Ogrn,
                Okpo = entity.Okpo,
                Bik = entity.Bik,
                CorrAccount = entity.CorrAccount
            };
            this.creditOrgCache.Add(key, creditOrg);

            return creditOrg;
        }

        private void InitRegopCache()
        {
            this.calcAccountRealityObjectCache = this.CalcAccountRealityObjectDomain.GetAll()
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                .Where(x => x.Account.TypeOwner == TypeOwnerCalcAccount.Regoperator)
                .Select(x => x.RealityObject.Id)
                .ToHashSet();
        }

        private void InitCache(IQueryable<SpecialAccountDecision> specialAccountQuery)
        {
            this.creditOrgCache = this.CreditOrgDomain.GetAll()
                .Where(x => specialAccountQuery
                    .Where(y => x.CorrAccount == y.CorrAccount)
                    .Where(y => x.Inn == y.Inn)
                    .Where(y => x.Kpp == y.Kpp)
                    .Where(y => x.Ogrn == y.Ogrn)
                    .Any(y => x.Bik == y.Bik))
                .AsEnumerable()
                .ToDictionary(this.creditOrgKey);

            this.calcAccountRealityObjectCache = specialAccountQuery
                .Where(y => this.CalcAccountRealityObjectDomain.GetAll()
                    .Any(x => x.RealityObject.Id == y.RealityObject.Id))
                .Select(x => x.Id)
                .ToHashSet();
        }

        private readonly Func<CreditOrg, string> creditOrgKey = x => $"{x.Inn}_{x.Kpp}_{x.Ogrn}_{x.Bik}|{x.CorrAccount}";

        private readonly Func<SpecialAccountDecision, string> specialAccountKey = x => $"{x.Inn}_{x.Kpp}_{x.Ogrn}_{x.Bik}|{x.CorrAccount}";
    }
}