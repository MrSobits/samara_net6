namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    public class PersAccChangesRestoreAction : BaseExecutionAction
    {
        private readonly IWindsorContainer _container;

        public PersAccChangesRestoreAction(IWindsorContainer container)
        {
            this._container = container;
        }

        private BaseDataResult Execute()
        {
            var domain = this._container.ResolveRepository<PersonalAccountChange>();
            var ownerDomain = this._container.ResolveRepository<PersonalAccountOwner>();

            var changesHasNoValues = domain.GetAll()
                .Where(x => x.OldValue == null || x.NewValue == null)
                .Where(
                    x => x.ChangeType == PersonalAccountChangeType.OwnerChange
                        || x.ChangeType == PersonalAccountChangeType.PenaltyChange
                        || x.ChangeType == PersonalAccountChangeType.SaldoChange);

            var owners = ownerDomain.GetAll()
                .Where(x => x.Name != null)
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var changeOwnerSearchPattern = new Regex("^Смена абонента ЛС с \"(.+)\" на \"(.+)\"$");
            var changeSaldoSearchPattern = new Regex(@"^Ручное изменение сальдо с (-?\d+\,?\d*) на (-?\d+\,?\d*)$");
            var changePenaltySearchPattern = new Regex(@"^Ручное изменение пени с (-?\d+\,?\d*) на (-?\d+\,?\d*)$");

            var listForUpdate = new List<PersonalAccountChange>();

            foreach (var change in changesHasNoValues)
            {
                switch (change.ChangeType)
                {
                    case PersonalAccountChangeType.OwnerChange:
                        if (changeOwnerSearchPattern.IsMatch(change.Description))
                        {
                            var match = changeOwnerSearchPattern.Match(change.Description);

                            var oldName = match.Groups[1].Value;
                            var newName = match.Groups[2].Value;

                            var oldOwner = owners.ContainsKey(oldName) && owners[oldName].Length == 1
                                ? owners[oldName][0].Id.ToStr()
                                : null;

                            var newOwner = owners.ContainsKey(newName) && owners[newName].Length == 1
                                ? owners[newName][0].Id.ToStr()
                                : null;

                            change.NewValue = newOwner;
                            change.OldValue = oldOwner;

                            listForUpdate.Add(change);
                        }
                        break;

                    case PersonalAccountChangeType.SaldoChange:
                        if (changeSaldoSearchPattern.IsMatch(change.Description))
                        {
                            var match = changeSaldoSearchPattern.Match(change.Description);

                            var oldValue = match.Groups[1].Value;
                            var newValue = match.Groups[2].Value;

                            if (change.OldValue.IsEmpty())
                            {
                                change.OldValue = oldValue;
                            }

                            if (change.NewValue.IsEmpty())
                            {
                                change.NewValue = newValue;
                            }

                            listForUpdate.Add(change);
                        }
                        break;

                    case PersonalAccountChangeType.PenaltyChange:
                        if (changePenaltySearchPattern.IsMatch(change.Description))
                        {
                            var match = changePenaltySearchPattern.Match(change.Description);

                            var oldValue = match.Groups[1].Value;
                            var newValue = match.Groups[2].Value;

                            if (change.OldValue.IsEmpty())
                            {
                                change.OldValue = oldValue;
                            }

                            if (change.NewValue.IsEmpty())
                            {
                                change.NewValue = newValue;
                            }

                            listForUpdate.Add(change);
                        }
                        break;
                }
            }

            TransactionHelper.InsertInManyTransactions(this._container, listForUpdate);

            this.CreateFirstOwnerChange();

            this.CreateFirstOwnerChangeIfOldValueExists();

            return new BaseDataResult();
        }

        private void CreateFirstOwnerChange()
        {
            var accountDomain = this._container.ResolveDomain<BasePersonalAccount>();
            var changeDomain = this._container.ResolveDomain<PersonalAccountChange>();
            var chargePeriodRepo = this._container.Resolve<IChargePeriodRepository>();

            var accounts = accountDomain.GetAll()
                .Where(
                    z => !changeDomain.GetAll()
                        .Where(x => x.ChangeType == PersonalAccountChangeType.OwnerChange)
                        .Any(x => x.PersonalAccount.Id == z.Id));

            var itemsToCreate = new List<PersonalAccountChange>();

            foreach (var account in accounts)
            {
                var change = new PersonalAccountChange(
                    account,
                    string.Format(
                        "Смена абонента ЛС с \"Неизвестно\" на \"{0}\"",
                        account.AccountOwner.Name),
                    PersonalAccountChangeType.OwnerChange,
                    account.ObjectCreateDate,
                    account.OpenDate,
                    "admin",
                    account.AccountOwner.Id.ToStr(),
                    "0",
                    chargePeriodRepo.GetPeriodByDate(account.ObjectCreateDate));
                itemsToCreate.Add(change);
            }

            TransactionHelper.InsertInManyTransactions(this._container, itemsToCreate, 1000, true, true);
        }

        private void CreateFirstOwnerChangeIfOldValueExists()
        {
            var accountDomain = this._container.ResolveDomain<BasePersonalAccount>();
            var changeDomain = this._container.ResolveDomain<PersonalAccountChange>();
            var ownersDomain = this._container.ResolveDomain<PersonalAccountOwner>();
            var chargePeriodRepo = this._container.Resolve<IChargePeriodRepository>();

            var changes = changeDomain.GetAll()
                .Where(x => x.ChangeType == PersonalAccountChangeType.OwnerChange)
                .Where(x => x.OldValue != null)
                .Where(x => x.OldValue != "0")
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var accounts = accountDomain.GetAll()
                .Where(
                    z => !changeDomain.GetAll()
                        .Where(x => x.PersonalAccount.Id == z.Id)
                        .Any(x => x.OldValue == "0"));

            var itemsToCreate = new List<PersonalAccountChange>();

            var owners = ownersDomain.GetAll().ToDictionary(x => x.Id);

            foreach (var account in accounts)
            {
                if (!changes.ContainsKey(account.Id))
                {
                    continue;
                }

                var change = changes[account.Id].OrderByDescending(x => x.ObjectCreateDate).FirstOrDefault();

                changes.Remove(account.Id);

                if (change == null)
                {
                    continue;
                }

                var owner = owners.Get(change.OldValue.ToLong());

                if (owner == null)
                {
                    continue;
                }

                var item = new PersonalAccountChange(
                    account,
                    string.Format("Смена абонента ЛС с \"Неизвестно\" на \"{0}\"", owner.Name),
                    PersonalAccountChangeType.OwnerChange,
                    account.ObjectCreateDate,
                    account.OpenDate,
                    "admin",
                    owner.Id.ToStr(),
                    "0",
                    chargePeriodRepo.GetPeriodByDate(account.ObjectCreateDate));

                itemsToCreate.Add(item);
            }

            TransactionHelper.InsertInManyTransactions(this._container, itemsToCreate, 1000, true, true);
        }

        #region Properties
        /// <summary>
        /// Код для регистрации
        /// </summary>
        /// <summary>
        /// Тип хранимого параметра
        /// </summary>
        public override string Description => "Проставление значений в истории лс. Проставляет значения историю изменений собственников";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Проставление значений в истории лс";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;
        #endregion Properties
    }
}