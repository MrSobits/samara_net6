namespace Bars.Gkh.RegOperator.DomainEvent.Handlers.PersonalAccountPayment
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Domain;
    using Domain.AggregationRoots;
    using Domain.ValueObjects;
    using DomainModelServices.PersonalAccount;

    using Entities.Wallet;
    using Events.PersonalAccountPayment;
    using Events.PersonalAccountPayment.Payment;

    using FastMember;

    using Infrastructure;

    /// <summary>
    /// Обработчик событий оплат лс на дом
    /// </summary>
    public class RealtyObjectPaymentHandler :
        IDomainEventHandler<PersonalAccountPaymentByBaseTariffEvent>,
        IDomainEventHandler<PersonalAccountPaymentByDecisionEvent>,
        IDomainEventHandler<PersonalAccountPenaltyPaymentEvent>,
        IDomainEventHandler<PersonalAccountPreviousWorkPaymentEvent>,
        IDomainEventHandler<PersonalAccountRentPaymentEvent>,
        IDomainEventHandler<PersonalAccountSocialSupportPaymentEvent>,
        IDomainEventHandler<PersonalAccountAccumulatedFundPaymentEvent>,
        IDomainEventHandler<PersAccRestructAmicableAgreementPaymentEvent>,

        IRealtyObjectPaymentEventHandler<PersonalAccountPaymentByBaseTariffEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountPaymentByDecisionEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyPaymentEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountPreviousWorkPaymentEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountRentPaymentEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountSocialSupportPaymentEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountAccumulatedFundPaymentEvent>,
        IRealtyObjectPaymentEventHandler<PersAccRestructAmicableAgreementPaymentEvent>
    {
        private static readonly TypeAccessor accessor = TypeAccessor.Create(typeof(RealityObjectPaymentAccount));

        private readonly IPersonalAccountRecalcEventManager recalcManager;
        private readonly IRealtyObjectPaymentSession session;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="session">Поставщик сессий</param>
        /// <param name="recalcManager">Менеджер перерасчётов</param>
        public RealtyObjectPaymentHandler(
            IRealtyObjectPaymentSession session,
            IPersonalAccountRecalcEventManager recalcManager)
        {
            this.session = session;
            this.recalcManager = recalcManager;
        }

        #region IDomainEventHandler members

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPaymentByBaseTariffEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPaymentByDecisionEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPenaltyPaymentEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPreviousWorkPaymentEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountRentPaymentEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountSocialSupportPaymentEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата");//???
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountAccumulatedFundPaymentEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersAccRestructAmicableAgreementPaymentEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Money.OperationFactDate, RecalcEventType.Payment, "Оплата"); 
        }

        #endregion

        #region IRealtyObjectPaymentEventHandler members

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPaymentByBaseTariffEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.BaseTariffPaymentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPaymentByDecisionEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.DecisionPaymentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPenaltyPaymentEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.PenaltyPaymentWallet, true);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPreviousWorkPaymentEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.PreviosWorkPaymentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountRentPaymentEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.RentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountSocialSupportPaymentEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.SocialSupportWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountAccumulatedFundPaymentEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.AccumulatedFundWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersAccRestructAmicableAgreementPaymentEvent args)
        {
            this.ExecuteInternal(root, args.Money, x => x.RestructAmicableAgreementWallet);
        }

        private void ExecuteInternal(
            RealtyObjectPaymentRoot root,
            MoneyStream money,
            Expression<Func<RealityObjectPaymentAccount, Wallet>> walletSelector,
            bool isPenalty = false)
        {
            var member = (PropertyInfo)((MemberExpression)walletSelector.Body).Member;
            var wallet = RealtyObjectPaymentHandler.accessor[root.PaymentAccount, member.Name].To<Wallet>();

            root.AddTransfer(root.PaymentAccount.StoreMoney(wallet, money, false));
            root.ChargeAccount.ApplyPayment(money.Amount, isPenalty);
        }

        #endregion
    }
}