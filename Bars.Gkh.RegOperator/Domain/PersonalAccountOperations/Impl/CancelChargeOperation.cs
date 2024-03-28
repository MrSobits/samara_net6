namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Dto;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using DomainModelServices;
    using Entities;
    using Gkh.Entities;
    using Repository;

    /// <summary>
    /// Отмена начислений
    /// </summary>
    [Obsolete("Не используется, работает неверно!")]
    public class CancelChargeOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public static string Key
        {
            get { return "CancelChargeOperation"; }
        }

        /// <summary>
        /// Код
        /// </summary>
        public override string Code
        {
            get { return CancelChargeOperation.Key; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Отмена начислений"; }
        }
        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            var persAccRepo = this.Container.ResolveRepository<BasePersonalAccount>();
            var chargePeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var persAccChargeDomain = this.Container.ResolveDomain<PersonalAccountCharge>();
            var moneyOperationDomain = this.Container.ResolveDomain<MoneyOperation>();
            var transferDomain = this.Container.ResolveDomain<Transfer>();
            var realObjRepo = this.Container.ResolveRepository<RealityObject>();


            var periodSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var roChargeAccOperDomain = this.Container.ResolveDomain<RealityObjectChargeAccountOperation>();

            var fileManager = this.Container.Resolve<IFileManager>();
            var updater = this.Container.Resolve<IRealityObjectAccountUpdater>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var chargePeriodService = this.Container.Resolve<IChargePeriodRepository>();

            var persAccChargesToSave = new List<PersonalAccountCharge>();
            var moneyOperationsToSave = new List<MoneyOperation>();
            var transfersToSave = new List<Transfer>();
            var periodSummariesToSave = new List<PersonalAccountPeriodSummary>();

            var historyFactory = this.Container.Resolve<IPersonalAccountHistoryCreator>();

            try
            {
                var modifiedRecords = baseParams.Params.GetAs<List<ModifiedRecord>>("modifRecs");
                var periodId = baseParams.Params.GetAs<long>("chargePeriodId");
                var chargePeriod = chargePeriodDomain.Get(periodId);
                var currChargePeriod = chargePeriodService.GetCurrentPeriod(false);

                if (chargePeriod == null)
                {
                    return new BaseDataResult(false, "Не найден выбранный период");
                }

                if (!baseParams.Files.ContainsKey("Document"))
                {
                    return new BaseDataResult(false, "Необходимо приложить файл");
                }

                var documentInfo = fileManager.SaveFile(baseParams.Files["Document"]);

                var chargeIds = modifiedRecords.Select(y => y.Id).ToArray();
                var realObjectIds = persAccChargeDomain.GetAll()
                    .Where(x => chargeIds.Contains(x.Id))
                    .Select(x => x.BasePersonalAccount.Room.RealityObject.Id)
                    .Distinct()
                    .ToArray();
                var realObjQuery = realObjRepo.GetAll().Where(x => realObjectIds.Contains(x.Id));

                var persAccIds = persAccChargeDomain.GetAll()
                                .Where(x => chargeIds.Contains(x.Id))
                                .Select(x => x.BasePersonalAccount.Id)
                                .Distinct()
                                .ToArray();

                var roChargeAccOperations = roChargeAccOperDomain.GetAll()
                        .Where(x => realObjectIds.Contains(x.Account.RealityObject.Id) && x.Period.Id == currChargePeriod.Id)
                        .ToList();

                var chargesPerAccount = new List<AccountCharge>();

                foreach (var record in modifiedRecords)
                {
                    var charge = persAccChargeDomain.Get(record.Id);

                    if (charge == null)
                    {
                        return new BaseDataResult(false, "Не найдено начисление");
                    }

                    if (record.CancellationSum == 0)
                    {
                        continue;
                    }

                    if (record.CancellationSum - charge.ChargeTariff > 0.005M)
                    {
                        return new BaseDataResult(false, "Невозможно отменить сумму больше начисленной");
                    }

                    if (-0.005M < record.CancellationSum - charge.ChargeTariff &&
                        record.CancellationSum - charge.ChargeTariff <= 0.005M)
                    {
                        record.CancellationSum = charge.ChargeTariff;
                    }

                    var baseTariffCharge = charge.ChargeTariff - charge.OverPlus;
                    var baseTariffCancellation = record.CancellationSum;
                    var decisTariffCancellation = 0M;

                    if (record.CancellationSum > baseTariffCharge)
                    {
                        baseTariffCancellation = baseTariffCharge;
                        decisTariffCancellation = record.CancellationSum - baseTariffCharge;
                    }

                    var moneyOperation = new MoneyOperation(charge.Guid, currChargePeriod)
                    {
                        Amount = -record.CancellationSum,
                        Reason = "Отмена начислений",
                        OperationDate = currChargePeriod.StartDate,
                        Document = documentInfo
                    };

                    var persAcc = charge.BasePersonalAccount;
                    var baseTariffWallet = persAcc.BaseTariffWallet;
                    var decisTariffWallet = persAcc.DecisionTariffWallet;

                    var periodSummary = periodSummaryDomain.GetAll()
                        .FirstOrDefault(x => x.PersonalAccount.Id == persAcc.Id
                            && x.Period.Id == currChargePeriod.Id);
                    var roChargeAccOperation = roChargeAccOperations
                        .FirstOrDefault(x => x.Account.RealityObject.Id == persAcc.Room.RealityObject.Id);

                    if (periodSummary == null || roChargeAccOperation == null)
                    {
                        return new BaseDataResult(false, "Произошла ошибка при отмене начислений");
                    }

                    periodSummary.ChargeTariff -= record.CancellationSum;
                    periodSummary.ChargedByBaseTariff -= baseTariffCancellation;
                    roChargeAccOperation.ChargedTotal -= record.CancellationSum;

                    var accountCharge = chargesPerAccount.FirstOrDefault(c => c.Account.Id == persAcc.Id);
                    if (accountCharge == null)
                    {
                        accountCharge = new AccountCharge { Account = persAcc };
                        chargesPerAccount.Add(accountCharge);
                    }
                    accountCharge.Charge += record.CancellationSum;

                    var baseTariffCancellationTransfer =
                        new PersonalAccountChargeTransfer(persAcc, baseTariffWallet.WalletGuid, charge.Guid, -baseTariffCancellation, moneyOperation)
                        {
                            Reason = "Отмена начислений по базовому тарифу",
                            PaymentDate = currChargePeriod.StartDate,
                            OperationDate = currChargePeriod.StartDate
                        };

                    if (decisTariffCancellation > 0M)
                    {
                        var decisTariffCancellationTransfer =
                            new PersonalAccountChargeTransfer(persAcc, decisTariffWallet.WalletGuid, charge.Guid, -decisTariffCancellation, moneyOperation)
                            {
                                Reason = "Отмена начислений по тарифу решений",
                                PaymentDate = currChargePeriod.StartDate,
                                OperationDate = currChargePeriod.StartDate
                            };

                        transfersToSave.Add(decisTariffCancellationTransfer);
                    }

                    persAccChargesToSave.Add(charge);
                    moneyOperationsToSave.Add(moneyOperation);
                    transfersToSave.Add(baseTariffCancellationTransfer);
                    periodSummariesToSave.Add(periodSummary);
                }

                var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);
                var changes = chargesPerAccount.Select(accountCharge => historyFactory.CreateChange(
                    accountCharge.Account,
                    PersonalAccountChangeType.ChargeUndo,
                    "Отмена начисления за период {0} на сумму {1}".FormatUsing(chargePeriod.Name, accountCharge.Charge),
                    accountCharge.Charge.ToString(),
                    accountCharge.Charge.ToString(),
                    DateTime.Now,
                    changeInfo.Document,
                    changeInfo.Reason)).ToList();

                var session = sessionProvider.GetCurrentSession();
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        persAccChargesToSave.ForEach(session.SaveOrUpdate);
                        moneyOperationsToSave.ForEach(session.SaveOrUpdate);
                        transfersToSave.ForEach(session.SaveOrUpdate);
                        periodSummariesToSave.ForEach(session.SaveOrUpdate);
                        roChargeAccOperations.ForEach(session.SaveOrUpdate);
                        changes.ForEach(session.SaveOrUpdate);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult(false, e.Message);
                    }
                }

                // TODO обновлять не все, а только нужные поля, как это сделано для отмены пени
                // после этого вернуть DatabaseMutexContext
                updater.UpdateBalance(realObjQuery, persAccIds);

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(persAccRepo);
                this.Container.Release(chargePeriodDomain);
                this.Container.Release(persAccChargeDomain);
                this.Container.Release(moneyOperationDomain);
                this.Container.Release(transferDomain);
                this.Container.Release(realObjRepo);
                this.Container.Release(periodSummaryDomain);
                this.Container.Release(roChargeAccOperDomain);
                this.Container.Release(fileManager);
                this.Container.Release(updater);
                this.Container.Release(sessionProvider);
            }
        }
    }

    class AccountCharge
    {
        public BasePersonalAccount Account;
        public decimal Charge;
    }
}