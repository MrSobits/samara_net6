namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class RoCalcAccountFixAction : BaseExecutionAction
    {
        public override string Description => "Удаление дублей специальных счетов жилого дома. А так же фикс c типами расчетного счета";

        public override string Name => "Удаление дублей специальных счетов жилого дома";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var calcaccroDoman = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var specaccDomain = this.Container.ResolveDomain<SpecialCalcAccount>();
            var calcaccDoman = this.Container.ResolveDomain<CalcAccount>();
            var rodecService = this.Container.Resolve<IRealityObjectDecisionsService>();

            using (this.Container.Using(calcaccroDoman, specaccDomain, rodecService))
            {
                //список домов и спец. счетов
                //берутся только те дома, у которых несколько спец.счетов
                var objsHaveTwinSpec = calcaccroDoman.GetAll()
                    .ToList()
                    .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                    .Select(
                        x => new
                        {
                            RoId = x.RealityObject.Id,
                            x.Id,
                            AccountId = x.Account != null ? x.Account.Id : 0,
                            IsActive = (x.Account as SpecialCalcAccount) != null && (x.Account as SpecialCalcAccount).IsActive
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .Select(
                        x => new
                        {
                            RoId = x.Key,
                            Accounts = x
                                .Select(
                                    y => new
                                    {
                                        RoAccId = y.Id,
                                        y.AccountId,
                                        y.IsActive
                                    })
                                .OrderByDescending(y => y.IsActive)
                                .ThenByDescending(y => y.AccountId)
                                .ToArray()
                        })
                    .ToList();

                var robjects = objsHaveTwinSpec.Select(x => new RealityObject {Id = x.RoId}).ToList();

                var roIds = robjects.Select(x => x.Id).ToList();

                var crFundDecisions = rodecService.GetActualDecisionForCollection<CrFundFormationDecision>(robjects, true);
                var accOwnerDecisions = rodecService.GetActualDecisionForCollection<AccountOwnerDecision>(robjects, true);

                var robjectManOrg = this.Container.Resolve<IManagingOrgRealityObjectService>().GetAllActive(DateTime.Today)
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.ManOrgContract.StartDate,
                            x.ManOrgContract.ManagingOrganization.Contragent
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y
                            .OrderByDescending(x => x.StartDate)
                            .Select(x => x.Contragent)
                            .First());

                foreach (var obj in objsHaveTwinSpec)
                {
                    var roFundDec = crFundDecisions.FirstOrDefault(x => x.Key.Id == obj.RoId).Return(x => x.Value);

                    int skipCount = 0;

                    //если активное решение о способе формирования фонда кр - на спецсчете,
                    //то удаляем все, кроме последнего созданного и активного
                    if (roFundDec != null && roFundDec.Decision == CrFundFormationDecisionType.SpecialAccount)
                    {
                        skipCount = 1;
                    }

                    for (int i = 0; i < obj.Accounts.Length; i++)
                    {
                        var item = obj.Accounts[i];

                        var account = specaccDomain.Load(item.AccountId);

                        if (i < skipCount)
                        {
                            // если тип владельца счета Custom,
                            //то берем текущий договор управления и выдергиваем у него контрагента
                            if (accOwnerDecisions
                                .FirstOrDefault(x => x.Key.Id == obj.RoId)
                                .Return(x => x.Value)
                                .Return(x => x.DecisionType) == AccountOwnerDecisionType.Custom)
                            {
                                var owner = robjectManOrg.Get(obj.RoId);

                                if (owner == null)
                                {
                                    continue;
                                }

                                account.AccountOwner = owner;

                                try
                                {
                                    specaccDomain.Update(account);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                        }

                        this.Container.UsingForResolved<IDataTransaction>(
                            (c, tr) =>
                            {
                                try
                                {
                                    if (calcaccroDoman.Get(item.RoAccId) != null)
                                    {
                                        calcaccroDoman.Delete(item.RoAccId);
                                    }
                                    if (specaccDomain.Get(item.AccountId) != null)
                                    {
                                        specaccDomain.Delete(item.AccountId);
                                    }

                                    tr.Commit();
                                }
                                catch
                                {
                                    tr.Rollback();
                                }
                            });
                    }
                }

                this.Container.InTransaction(
                    () =>
                    {
                        var calcAccIds =
                            calcaccDoman.GetAll()
                                .Where(x => !specaccDomain.GetAll().Any(y => y.Id == x.Id))
                                .Select(x => x.Id)
                                .ToList();

                        foreach (var calcAccId in calcAccIds)
                        {
                            var rec = calcaccDoman.Load(calcAccId);
                            rec.TypeAccount = TypeCalcAccount.Regoperator;
                            calcaccDoman.Update(rec);
                        }
                    });

                this.Container.InTransaction(
                    () =>
                    {
                        var specAccIds =
                            specaccDomain.GetAll()
                                .Where(x => !calcaccroDoman.GetAll().Any(y => y.Account == x))
                                .Select(x => x.Id)
                                .ToList();

                        foreach (var id in specAccIds)
                        {
                            specaccDomain.Delete(id);
                        }
                    });
            }

            return new BaseDataResult();
        }
    }
}