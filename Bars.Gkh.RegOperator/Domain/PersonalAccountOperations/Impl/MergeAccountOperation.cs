namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;
    
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using DomainService.PersonalAccount;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;

    [Obsolete("use PersonalAccountMerger")]
    public class MergeAccountOperation : PersonalAccountOperationBase
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> AccountSummaryDomain { get; set; }
        public IPersonalAccountOperationService PersonalAccountOperationService { get; set; }
        public IVersionedEntityService VersionService { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
        
        public override string PermissionKey
        {
            get { return "GkhRegOp.PersonalAccount.Registry.Action.Merge"; }
        }

        public static string Key
        {
            get { return "MergeAccountOperation"; }
        }

        public override string Code
        {
            get { return Key; }
        }

        public override string Name
        {
            get { return "Слияние"; }
        }

        public override IDataResult Execute(BaseParams baseParams)
        {
            var proxy = baseParams.Params.ReadClass<AccountShareProxy>();

            #region Validation
            if (proxy == null || proxy.Accounts == null || !proxy.Accounts.Any())
            {
                return new BaseDataResult(false, "Нет данных для слияния!");
            }

            var rooms = proxy.Accounts.Select(x => x.Room.Return(y => y.Id)).Distinct().ToList();

            if (!rooms.Any() || rooms.Count() > 1)
            {
                return new BaseDataResult(false, "Слияние может проводится для лицевых счетов только одного помещения!");
            }

            var room = proxy.Accounts.Select(x => x.Room).First();

            var modifedAccountsIds = proxy.Accounts.Select(x => x.BasePersonalAccount.Id).ToList();

            var persistedAccounts =
                this.PersonalAccountDomain.GetAll()
                    .Where(x => !x.State.FinalState && x.Room.Id == room.Id).ToList();

            var persistedAccountsShare =
                persistedAccounts.Where(x => !modifedAccountsIds.Contains(x.Id)).SafeSum(x => x.AreaShare);

            var newShare = proxy.Accounts.SafeSum(x => x.NewShare);

            if (newShare + persistedAccountsShare > 1)
            {
                return new BaseDataResult(false, "Суммарное значение долей лицевых счетов в одном помещении не может быть больше 1 (единицы)!");
            }

            if (proxy.Accounts.All(x => x.NewShare != 0))
            {
                return new BaseDataResult(false, "При слиянии лицевых счетов, должен быть лицевой счет с нулевой долей собственности!");
            }
            #endregion Validation

            var modifiedRecords = persistedAccounts.Where(x => modifedAccountsIds.Contains(x.Id)).ToList();

            /*
             * 1) Для закрытх ЛС получаем входящее сальдо
             * 2) Делим входящее сальдо между всеми незакрытми
             * 3) Закрываем закрытые (у кого новая доля == 0)
             * 4) Версионируем долю
             * 5) Обновляем незакрытые
             */

            // 1)
            var closed =
                modifiedRecords
                    .Where(x => proxy.Accounts.Any(a => a.NewShare == 0 && a.BasePersonalAccount.Id == x.Id))
                    .ToList();

            var closedIds = closed.Select(x => x.Id).ToList();

            var currentPeriod = this.ChargePeriodRepo.GetCurrentPeriod();

            // 2)
            decimal totalSaldoOut =
                currentPeriod == null
                    ? 0
                    : this.AccountSummaryDomain.GetAll()
                        .Where(x => closedIds.Contains(x.PersonalAccount.Id) && x.Period.Id == currentPeriod.Id)
                        .SafeSum(x => x.SaldoIn + x.ChargeTariff + x.RecalcByBaseTariff + x.Penalty - x.PenaltyPayment - x.TariffPayment); // TODO fix recalc

            var nonClosed = modifiedRecords.Except(closed).ToList();
            var nonClosedIds = nonClosed.Select(x => x.Id).ToList();

            var summaries =
                this.AccountSummaryDomain.GetAll()
                    .Where(x => nonClosedIds.Contains(x.PersonalAccount.Id))
                    .Where(x => x.Period.Id == currentPeriod.Id)
                    .ToList()
                    .ToDictionary(x => x.PersonalAccount.Id);

            var proxyDict = proxy.Accounts.ToDictionary(x => x.BasePersonalAccount.Id);

            this.Container.InTransaction(() =>
            {
                var saldoShare = totalSaldoOut / nonClosed.Count();
                foreach (var x in closed)
                {
                    x.AreaShare = 0;
                    var result = this.PersonalAccountOperationService.CloseAccount(x, PersonalAccountChangeType.MergeAndClose, null, () => "Закрытие ЛС в связи со слиянием");

                    if (!result.Success)
                    {
                        throw new Exception(result.Message);
                    }
                }

                // 4), 5)
                nonClosed.ForEach(x =>
                {
                    var summary = summaries.ContainsKey(x.Id) ? summaries[x.Id] : null;
                    if (summary != null)
                    {
                        summary.SaldoIn += saldoShare;
                        this.AccountSummaryDomain.Update(summary);
                    }

                    var modified = proxyDict[x.Id];

                    x.AreaShare = modified.NewShare;

                    if (modified.Modified)
                    {
                        var bParams = new BaseParams
                        {
                            Params = new DynamicDictionary
                            {
                                {"className", typeof (BasePersonalAccount).Name},
                                {"propertyName", "AreaShare"},
                                {"value", modified.NewShare},
                                {"entityId", x.Id},
                                {"factDate", modified.ActualChangeDate}
                            },
                            Files = new Dictionary<string, FileData>()
                        };

                        this.VersionService.SaveParameterVersion(bParams);
                    }

                    this.PersonalAccountOperationService.CreateAccountHistory(
                        x,
                        PersonalAccountChangeType.MergeAndSaldoChange,
                        () => "Изменение сальдо и доли собственности в связи со слиянием ЛС");

                    this.PersonalAccountDomain.Update(x);
                });
            });

            return new BaseDataResult("Слияние прошло успешно");
        }

        private class AccountShareProxy
        {
            public List<AccountProxy> Accounts { get; set; }
        }

        private class AccountProxy
        {
            public BasePersonalAccount BasePersonalAccount { get; set; }

            public Room Room { get; set; }

            public decimal NewShare { get; set; }

            public bool Modified { get; set; }

            public DateTime ActualChangeDate { get; set; }
        }
    }
}