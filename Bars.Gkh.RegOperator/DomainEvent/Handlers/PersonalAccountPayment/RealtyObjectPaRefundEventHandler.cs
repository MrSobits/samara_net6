namespace Bars.Gkh.RegOperator.DomainEvent.Handlers.PersonalAccountPayment
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using AutoMapper;

    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using Domain;
    using Domain.AggregationRoots;
    using Domain.ValueObjects;

    using Events.PersonalAccountRefund;

    using FastMember;

    using Infrastructure;

    /// <summary>
    /// Обработчик событий возврата средств
    /// </summary>
    public class RealtyObjectPaRefundEventHandler :
        IDomainEventHandler<PersonalAccountTariffRefundEvent>,
        IDomainEventHandler<PersonalAccountDecisionRefundEvent>,
        IDomainEventHandler<PersonalAccountPenaltyRefundEvent>,

        IRealtyObjectPaymentEventHandler<PersonalAccountTariffRefundEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountDecisionRefundEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyRefundEvent>
    {
        private static readonly TypeAccessor accessor = TypeAccessor.Create(typeof(RealityObjectPaymentAccount));

        private readonly IRealtyObjectPaymentSession session;
        private readonly IWindsorContainer container;

        private readonly IMapper mapper;
        
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="session">Поставщик сессий</param>
        public RealtyObjectPaRefundEventHandler(IWindsorContainer container, IRealtyObjectPaymentSession session, IMapper mapper)
        {
            this.container = container;
            this.session = session;
            this.mapper = mapper;
        }

        #region IDomainEventHandler members

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountTariffRefundEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            // TODO: когда добавиться вызво перерасчета пени, убрать фильтрацию в BankDocumentImportService.AcceptImportedPayment
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountDecisionRefundEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            // TODO: когда добавиться вызво перерасчета пени, убрать фильтрацию в BankDocumentImportService.AcceptImportedPayment
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPenaltyRefundEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
            // TODO: когда добавиться вызво перерасчета пени, убрать фильтрацию в BankDocumentImportService.AcceptImportedPayment
        }

        #endregion

        #region IRealtyObjectPaymentEventHandler members

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountTariffRefundEvent args)
        {
            this.ExecuteInternal(x => x.BaseTariffPaymentWallet, root, args);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountDecisionRefundEvent args)
        {
            this.ExecuteInternal(x => x.DecisionPaymentWallet, root, args);
        }

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPenaltyRefundEvent args)
        {
            this.ExecuteInternal(x => x.PenaltyPaymentWallet, root, args);
        }

        private void ExecuteInternal(
            Expression<Func<RealityObjectPaymentAccount, Wallet>> walletSelector,
            RealtyObjectPaymentRoot root,
            PersonalAccountRefundEvent args)
        {
            var money = this.mapper.Map<MoneyStream>(args.Refund);

            var member = (PropertyInfo)((MemberExpression)walletSelector.Body).Member;
            var wallet = RealtyObjectPaRefundEventHandler.accessor[root.PaymentAccount, member.Name].To<Wallet>();

            try
            {
                
                root.AddTransfer(root.PaymentAccount.TakeMoney(wallet, money));
                root.ChargeAccount.UndoPayment(money.Amount, wallet.WalletType == WalletType.PenaltyWallet);
            }
            catch (ValidationException)
            {
                throw new ValidationException(
                        $"Недостаточно средств на счете оплат дома для возврата средств.\r\n" +
                        $"Адрес: {root.PaymentAccount.RealityObject.Address}.\r\n" +
                        $"Баланс кошелька ({wallet.GetWalletFullName(this.container, root.PaymentAccount, true)}): {wallet.Balance}.",
                        typeof(RealityObjectPaymentAccount),
                        null);
            }
        }

        #endregion
    }
}