namespace Bars.Gkh.RegOperator.DomainEvent.Handlers.PersonalAccountPayment
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;

    using Castle.Windsor;

    using Domain;
    using Domain.AggregationRoots;
    using Domain.ValueObjects;

    using Entities;
    using Entities.Wallet;

    using Events.PersonalAccountPayment.UndoPayment;

    using FastMember;

    using Infrastructure;
    using Enums;

    /// <summary>
    /// Обработчик событий отмен оплат лс на дом
    /// </summary>
    public class RealtyObjectUndoPaymentHandler :
        IDomainEventHandler<PersonalAccountTariffUndoEvent>,
        IDomainEventHandler<PersonalAccountDecisionUndoEvent>,
        IDomainEventHandler<PersonalAccountPenaltyUndoEvent>,
        IDomainEventHandler<PersonalAccountPreviousWorkUndoEvent>,
        IDomainEventHandler<PersonalAccountRentUndoEvent>,
        IDomainEventHandler<PersonalAccountSocialSupportUndoEvent>,
        IDomainEventHandler<PersonalAccountAccumulatedFundUndoEvent>,
        IDomainEventHandler<PersAccRestructAmicableAgreementUndoEvent>,

        IRealtyObjectPaymentEventHandler<PersonalAccountTariffUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountDecisionUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountPreviousWorkUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountRentUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountSocialSupportUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountAccumulatedFundUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersAccRestructAmicableAgreementUndoEvent>
    {
        private static readonly TypeAccessor accessor = TypeAccessor.Create(typeof(RealityObjectPaymentAccount));

        private readonly IRealtyObjectPaymentSession session;
        private readonly IPersonalAccountRecalcEventManager recalcManager;
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="session">Провайдер сессий</param>
        /// <param name="container">Контейнер</param>
        /// <param name="recalcManager">Менеджер перерасчётов</param>
        public RealtyObjectUndoPaymentHandler(
            IRealtyObjectPaymentSession session,
            IWindsorContainer container,
            IPersonalAccountRecalcEventManager recalcManager)
        {
            this.session = session;
            this.container = container;
            this.recalcManager = recalcManager;
        }

        #region IDomainEventHandler members

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountTariffUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Transfer.PaymentDate, RecalcEventType.CancelPayment, "Отмена оплаты");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountDecisionUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Transfer.PaymentDate, RecalcEventType.CancelPayment, "Отмена оплаты");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPenaltyUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            this.recalcManager.CreatePenaltyEvent(args.Account, args.Transfer.PaymentDate, RecalcEventType.CancelPayment, "Отмена оплаты");
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPreviousWorkUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountRentUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountSocialSupportUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountAccumulatedFundUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersAccRestructAmicableAgreementUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        #endregion

        #region IRealtyObjectPaymentEventHandler

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountTariffUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.BaseTariffPaymentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountDecisionUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.DecisionPaymentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPenaltyUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.PenaltyPaymentWallet, true);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPreviousWorkUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.PreviosWorkPaymentWallet);
        }

        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountRentUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.RentWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountSocialSupportUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.SocialSupportWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountAccumulatedFundUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.SocialSupportWallet);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersAccRestructAmicableAgreementUndoEvent args)
        {
            this.ExecuteInternal(root, args, x => x.RestructAmicableAgreementWallet);
        }

        private void ExecuteInternal(
            RealtyObjectPaymentRoot root,
            PersonalAccountPaymentUndoEvent args,
            Expression<Func<RealityObjectPaymentAccount, Wallet>> walletSelector,
            bool isPenalty = false)
        {
            var member = (PropertyInfo)((MemberExpression)walletSelector.Body).Member;
            var wallet = RealtyObjectUndoPaymentHandler.accessor[root.PaymentAccount, member.Name].To<Wallet>();

            try
            {
                var transfer = root.PaymentAccount.UndoPayment(
                wallet,
                new MoneyStream(
                    args.Transfer.TargetGuid, // Отправляем деньги туда же, куда оригинальная отмена
                    args.Operation,
                    DateTime.Now,
                    args.Transfer.Amount)
                {
                    OriginalTransfer = args.Transfer,
                    Description = args.Reason,
                    OriginatorName = args.Account.PersonalAccountNum,
                    IsAffect = args.Transfer.IsAffect
                },
                false);

                root.AddTransfer(transfer);
                root.ChargeAccount.UndoPayment(args.Transfer.Amount, isPenalty);
            }
            catch (ValidationException)
            {
                throw new ValidationException(
                        $"Недостаточно средств на счете оплат дома для отмены оплаты.\r\n" +
                        $"Адрес: {root.PaymentAccount.RealityObject.Address}.\r\n" +
                        $"Баланс кошелька ({wallet.GetWalletFullName(this.container, root.PaymentAccount, true)}).",
                        typeof(RealityObjectPaymentAccount),
                        null);
            }
            
        }

        #endregion
    }
}