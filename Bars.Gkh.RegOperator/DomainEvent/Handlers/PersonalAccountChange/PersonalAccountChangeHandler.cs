namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using System;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Security;
    using B4.Utils;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Utils;

    using DomainModelServices;
    using DomainModelServices.PersonalAccount;

    using Entities.PersonalAccount;

    using Enums;

    using Events.PersonalAccount;

    using Gkh.Entities;

    public class PersonalAccountChangeHandler :
        IDomainEventHandler<PersonalAccountChangeOwnerEvent>,
        IDomainEventHandler<PersonalAccountChangeAreaShareEvent>,
        IDomainEventHandler<PersonalAccountChangeDateOpenEvent>,
        IDomainEventHandler<PersonalAccountBalanceChangeEvent>
    {
        private readonly IRepository<User> userRepo;
        private readonly IUserIdentity userIdentity;
        private readonly IPersonalAccountHistoryCreator historyCreator;
        private readonly IDomainService<EntityLogLight> logDomain;
        private readonly IDomainService<PersonalAccountChange> changeDomain;
        private readonly IDomainService<AccountOwnershipHistory> ownershipHistoryDomain;
        private readonly IPersonalAccountRecalcEventManager recalcEventManager;

        public PersonalAccountChangeHandler(
            IDomainService<EntityLogLight> logDomain,
            IUserIdentity userIdentity,
            IRepository<User> userRepo,
            IPersonalAccountHistoryCreator historyCreator, 
            IDomainService<PersonalAccountChange> changeDomain,
            IDomainService<AccountOwnershipHistory> ownershipHistoryDomain,
            IPersonalAccountRecalcEventManager recalcEventManager)
        {
            this.logDomain = logDomain;
            this.userRepo = userRepo;
            this.historyCreator = historyCreator;
            this.changeDomain = changeDomain;
            this.ownershipHistoryDomain = ownershipHistoryDomain;
            this.recalcEventManager = recalcEventManager;
            this.userIdentity = userIdentity;
        }

        public void Handle(PersonalAccountChangeOwnerEvent args)
        {
            this.changeDomain.Save(
                this.historyCreator
                    .CreateChange(args.Account,
                        PersonalAccountChangeType.OwnerChange,
                        string.Format("Смена абонента ЛС с \"{0}\" на \"{1}\"",
                            args.OldOwner.Return(x => x.Name),
                            args.NewOwner.Return(x => x.Name)),
                        args.NewOwner.Return(x => x.Id).ToStr(),
                        args.OldOwner.Return(x => x.Id).ToStr(),
                        args.ChangeInfo.DateActual,
                        args.ChangeInfo.Document,
                        args.ChangeInfo.Reason));

            this.ownershipHistoryDomain.Save(
                new AccountOwnershipHistory(args.Account, args.NewOwner, args.ChangeInfo.DateActual));                
        }

        public void Handle(PersonalAccountChangeAreaShareEvent args)
        {
            var login = this.userRepo.Get(this.userIdentity.UserId).Return(u => u.Login);

            this.logDomain.Save(new EntityLogLight
            {
                ClassName = "BasePersonalAccount",
                EntityId = args.Account.Id,
                PropertyName = "AreaShare",
                PropertyValue = args.NewAreaShare.ToStr(),
                DateActualChange = args.DateActual,
                DateApplied = DateTime.UtcNow,
                Document = args.Document,
                ParameterName = "area_share",
                User = login.IsEmpty() ? "anonymous" : login
            });
        }

        public void Handle(PersonalAccountChangeDateOpenEvent args)
        {
            var login = this.userRepo.Get(this.userIdentity.UserId).Return(u => u.Login);

            if (args.OldDateOpen < args.NewDateOpen)
            {
                this.recalcEventManager.CreateChargeEvent(args.Account, args.OldDateOpen, RecalcEventType.ChangeOpenDate, "Смена даты открытия");
            }

            this.logDomain.Save(new EntityLogLight
            {
                ClassName = "BasePersonalAccount",
                EntityId = args.Account.Id,
                PropertyName = "OpenDate",
                PropertyValue = args.NewDateOpen.ToStr(),
                DateActualChange = args.DateActual,
                DateApplied = DateTime.UtcNow,
                Document = null,
                ParameterName = "account_open_date",
                User = login.IsEmpty() ? "anonymous" : login
            });
        }

        public void Handle(PersonalAccountBalanceChangeEvent args)
        {
            this.changeDomain.Save(
                this.historyCreator
                    .CreateChange(args.Account,
                        PersonalAccountChangeType.SaldoChange,
                        string.Format(
                            "Ручное изменение сальдо {2} с {0} на {1}",
                            args.OldValue.RegopRoundDecimal(2),
                            args.NewValue.RegopRoundDecimal(2),
                            args.WalletType.GetDisplayName().ToLower()),
                        args.NewValue.RegopRoundDecimal(2).ToStr(),
                        args.OldValue.RegopRoundDecimal(2).ToStr(),
                        null,
                        args.BaseDoc,
                        args.Reason));
        }
    }
}