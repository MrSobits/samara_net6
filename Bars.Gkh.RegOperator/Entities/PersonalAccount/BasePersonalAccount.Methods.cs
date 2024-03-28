namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using B4.Modules.FileStorage;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Utils;

    using Domain;
    using Domain.ValueObjects;

    using DomainEvent;
    using DomainEvent.Events.PersonalAccount;

    using DomainModelServices;

    using Enums;

    using Exceptions;

    using ValueObjects;

    /// <summary>
    /// Лицевой счет
    /// </summary>
    public partial class BasePersonalAccount
    {
        #region Property changes
        /// <summary>
        /// Установить значение доли собственности
        /// </summary>
        /// <param name="specification">
        /// Спецификация помещения
        /// </param>
        /// <param name="newAreaShare">
        /// Новая доля собственности
        /// </param>
        /// <param name="dateActual">
        /// Дата актуальности
        /// </param>
        /// <param name="fileInfo">
        /// The file Info.
        /// </param>
        public virtual void SetAreaShare(IRoomAreaShareSpecification specification, decimal newAreaShare, DateTime? dateActual, FileInfo fileInfo)
        {
            ArgumentChecker.NotNull(specification, nameof(specification));

            if (!specification.ValidateAreaShare(this, this.Room, newAreaShare, dateActual))
            {
                throw new ArgumentException("Общая доля собственности в помещении превышает максимальное значение");
            }

            var oldAreaShare = this.AreaShare;

            this.AreaShare = newAreaShare;

            DomainEvents.Raise(new PersonalAccountChangeAreaShareEvent(this, newAreaShare, oldAreaShare, dateActual ?? DateTime.Now, fileInfo));
        }

        /// <summary>
        /// Установить новую дату открытия счета
        /// </summary>
        /// <param name="newDateOpen">Новая дата открытия ЛС</param>
        public virtual void SetDateOpen(DateTime newDateOpen)
        {
            var oldDateOpen = this.OpenDate;

            this.OpenDate = newDateOpen;

            DomainEvents.Raise(new PersonalAccountChangeDateOpenEvent(this, this.OpenDate, oldDateOpen, DateTime.Now));
        }

        /// <summary>
        /// Установить нового собственника ЛС
        /// </summary>
        /// <param name="newOwner">Новый собственник ЛС</param>
        /// <param name="changeInfo">Информация об изменении</param>
        public virtual void SetOwner(PersonalAccountOwner newOwner, PersonalAccountChangeInfo changeInfo)
        {
            ArgumentChecker.NotNull(newOwner, nameof(newOwner));

            if (this.AccountOwner != null && this.AccountOwner.Id == newOwner.Id)
            {
                throw new ArgumentException("Новый собственник ЛС соответствует текущему собственнику. Установка нового собственника ЛС не удалась");
            }

            var oldOwner = this.AccountOwner;

            this.AccountOwner = newOwner;

            changeInfo.DateActual = changeInfo.DateActual == default(DateTime) ? DateTime.Now : changeInfo.DateActual;

            DomainEvents.Raise(new PersonalAccountChangeOwnerEvent(this, newOwner, oldOwner, changeInfo));
            DomainEvents.Raise(new PersonalAccountChangeOwnerDtoEvent(this, newOwner, oldOwner));
        }

        /// <summary>
        /// Применение распределения зачета средств за ранее выполненные работы
        /// </summary>
        public virtual Transfer ApplyPerformedWorkDistribution(decimal amount, string reason, DateTime operationDate, FileInfo document, ChargePeriod period)
        {
            var summary = this.GetOpenedPeriodSummary();

            var bc = summary.ApplyPerformedWorkDistribution(amount, reason, operationDate, document);

            this.TariffChargeBalance -= amount;

            var moneyOperation = new MoneyOperation(bc.TransferGuid, period, document)
            {
                Amount = amount
            };

            var moneyStream = new MoneyStream(bc, moneyOperation, bc.OperationDate, amount)
            {
                Description = "Зачет средств за выполненные работы",
                IsAffect = false
            };
            moneyOperation.Reason = "Зачет средств за выполненные работы";

            var transfer = this.BaseTariffWallet.TakeMoney(TransferBuilder.Create(this, moneyStream));
            transfer.TargetCoef = -1;

            return transfer;
        }

        /// <summary>
        /// Применение отложенного распределения зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="chargeSource">Распределение</param>
        /// <param name="amount">Сумма распределения</param>
        /// <param name="walletType">Тип кошелька</param>
        /// <param name="moneyOperation">Операция</param>
        /// <param name="parameters">Параметры распределения зачета средств на ЛС</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer ApplyPerfWorkDeferredDistribution(PerformedWorkChargeSource chargeSource, decimal amount, WalletType walletType, MoneyOperation moneyOperation, PerformedWorkDistributionParams parameters)
        {
            if (parameters.ApplyPeriodSummary)
            {
                var summary = this.GetOpenedPeriodSummary();

                if (walletType == WalletType.BaseTariffWallet)
                {
                    summary.PerformedWorkChargedBase += amount;
                }
                else if (walletType == WalletType.DecisionTariffWallet)
                {
                    summary.PerformedWorkChargedDecision += amount;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(walletType));
                }
                summary.RecalcSaldoOut();
            }

            // если распределяем в открытом периоде, то трансферы создавать не надо
            if (!parameters.CreateTransfer)
            {
                return null;
            }

            var moneyStream = new MoneyStream(chargeSource, moneyOperation, DateTime.UtcNow, amount)
            {
                Description = moneyOperation.Reason
            };

            var transfer = this.GetMainWallet(walletType).TakeMoney(TransferBuilder.Create(this, moneyStream));

            transfer.TargetCoef = -1;

            return transfer;
        }

        /// <summary>
        ///  Метод ручного изменения пени
        /// </summary>
        /// <param name="userPenalty"></param>
        /// <param name="debetPenalty"></param>
        /// <param name="reason"></param>
        /// <param name="file"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual Transfer ChangePenalty(decimal userPenalty, decimal debetPenalty, string reason, FileInfo file, ChargePeriod period)
        {
            // поулчаем открытый период 
            var summary = this.GetOpenedPeriodSummary();

            // Получаем изменение пеней
            var penaltyChange = summary.ChangePenalty(userPenalty, debetPenalty, reason, file);

            // получаем дельту изменения пеней
            var penaltyDelta = penaltyChange.NewValue - penaltyChange.CurrentValue;

            // учитываем дельту на балансе пеней для ЛС
            this.PenaltyChargeBalance += penaltyDelta;

            var moneyStream = new MoneyStream(penaltyChange, penaltyChange.CreateOperation(period), DateTime.Now, penaltyDelta)
            {
                Description = "Установка/изменение пени"
            };

            return this.PenaltyWallet.TakeMoney(TransferBuilder.Create(this, moneyStream)); 
        }

        /// <summary>
        ///  Метод ручного изменения пени
        /// </summary>
        /// <returns></returns>
        public virtual List<Transfer> ChangeSaldoMass(
            SaldoChangeSource source,
            decimal saldoByBaseTariffDelta, 
            decimal saldoByDecisionTariffDelta, 
            decimal saldoByPenaltyDelta)
        {
            // получаем открытый период 
            var summary = this.GetOpenedPeriodSummary();
            summary.ChangeSaldo(saldoByBaseTariffDelta, saldoByDecisionTariffDelta, saldoByPenaltyDelta);

            // учитываем дельту на балансе по типу платежа
            this.PenaltyChargeBalance += saldoByPenaltyDelta;
            this.TariffChargeBalance += saldoByBaseTariffDelta;
            this.DecisionChargeBalance += saldoByDecisionTariffDelta;

            var result = new List<Transfer>();

            if (saldoByBaseTariffDelta != 0)
            {
                result.Add(
                    this.BaseTariffWallet.TakeMoney(
                        TransferBuilder.Create(
                            this,
                            new MoneyStream(source, source.Operation, DateTime.Now, saldoByBaseTariffDelta)
                            {
                                Description = "Установка/изменение сальдо по базовому тарифу"
                            })));
            }

            if (saldoByDecisionTariffDelta != 0)
            {
                result.Add(
                    this.DecisionTariffWallet.TakeMoney(
                        TransferBuilder.Create(
                            this,
                        new MoneyStream(source, source.Operation, DateTime.Now, saldoByDecisionTariffDelta)
                        {
                            Description = "Установка/изменение сальдо по тарифу решения"
                        })));
            }

            if (saldoByPenaltyDelta != 0)
            {
                result.Add(
                    this.PenaltyWallet.TakeMoney(
                        TransferBuilder.Create(
                            this,
                        new MoneyStream(source, source.Operation, DateTime.Now, saldoByPenaltyDelta)
                        {
                            Description = "Установка/изменение пени"
                        })));
            }

            DomainEvents.Raise(new PersonalAccountSaldoChangeMassEvent(this, saldoByBaseTariffDelta, saldoByDecisionTariffDelta, saldoByPenaltyDelta));
            return result;
        }
        #endregion Property changes

        #region Charge

        /// <summary>
        /// Создание расчета по ЛС
        /// </summary>
        /// <param name="period">Период, за который необходимо провести начисление</param>
        /// <param name="packet">Пакет начислений, в рамках которого проводится расчет</param>
        /// <param name="factory">Фабрика для получения реализаций калькуляторов начислений, перерасчета и пеней</param>
        /// <returns>Начисление <seealso cref="UnacceptedCharge"/></returns>
        public virtual UnacceptedCharge CreateCharge(IPeriod period, UnacceptedChargePacket packet, IChargeCalculationImplFactory factory)
        {
            ArgumentChecker.NotNull(period, nameof(period));
            ArgumentChecker.NotNull(packet, nameof(packet));
            ArgumentChecker.NotNull(factory, nameof(factory));

            var charge = new UnacceptedCharge(this, packet);

            charge.Calculate(period, factory);

            return charge;
        }

        /// <summary>
        /// Создание расчета по ЛС до указанной даты
        /// </summary>
        /// <param name="period">Период, за который необходимо провести начисление</param>
        /// <param name="packet">Пакет начислений, в рамках которого проводится расчет</param>
        /// <param name="factory">Фабрика для получения реализаций калькуляторов начислений, перерасчета и пеней</param>
        /// <param name="calcDate">Дата, до которой будет произведен расчет</param>
        /// <returns>Начисление <seealso cref="UnacceptedCharge"/></returns>
        public virtual UnacceptedCharge CreateCharge(IPeriod period, UnacceptedChargePacket packet, IChargeCalculationImplFactory factory, DateTime calcDate)
        {
            var oldCloseDate = this.CloseDate;

            try
            {
                this.CloseDate = calcDate.Date;
                return this.CreateCharge(period, packet, factory);
            }
            finally
            {
                this.CloseDate = oldCloseDate;
            }
        }

        public virtual List<Transfer> ApplyCharge(PersonalAccountCharge charge, MoneyOperation moneyOperation)
        {
            ArgumentChecker.NotNull(charge, nameof(charge));

            this.TariffChargeBalance += charge.ChargeTariff + charge.RecalcByBaseTariff - charge.OverPlus;
            this.DecisionChargeBalance += charge.OverPlus + charge.RecalcByDecisionTariff;
            this.PenaltyChargeBalance += charge.Penalty + charge.RecalcPenalty;

            var transfers = new List<Transfer>();

            var btTransfer = this.BaseTariffWallet.TakeMoney(TransferBuilder.Create(this, new MoneyStream(charge, moneyOperation, charge.ChargeDate, charge.ChargeTariff - charge.OverPlus)));

            var recalcTransfer = this.BaseTariffWallet.TakeMoney(TransferBuilder.Create(this, new MoneyStream(charge, moneyOperation, charge.ChargeDate, charge.RecalcByBaseTariff)));

            var dtTransfer = this.DecisionTariffWallet.TakeMoney(TransferBuilder.Create(this, new MoneyStream(charge, moneyOperation, charge.ChargeDate, charge.OverPlus)));

            var dtRecalcTransfer = this.DecisionTariffWallet.TakeMoney(TransferBuilder.Create(this, new MoneyStream(charge, moneyOperation, charge.ChargeDate, charge.RecalcByDecisionTariff)));

            var pTransfer = this.PenaltyWallet.TakeMoney(TransferBuilder.Create(this, new MoneyStream(charge, moneyOperation, charge.ChargeDate, charge.Penalty)));

            var pRecalcTransfer = this.PenaltyWallet.TakeMoney(TransferBuilder.Create(this, new MoneyStream(charge, moneyOperation, charge.ChargeDate, charge.RecalcPenalty)));

            if (btTransfer != null)
            {
                btTransfer.Reason = "Начисление по базовому тарифу";
                transfers.Add(btTransfer);
            }

            if (recalcTransfer != null)
            {
                recalcTransfer.Reason = "Перерасчет по базовому тарифу";
                transfers.Add(recalcTransfer);
            }

            if (dtTransfer != null)
            {
                dtTransfer.Reason = "Начисление по тарифу решения";
                transfers.Add(dtTransfer);
            }

            if (dtRecalcTransfer != null)
            {
                dtRecalcTransfer.Reason = "Перерасчет по тарифу решения";
                transfers.Add(dtRecalcTransfer);
            }

            if (pTransfer != null)
            {
                pTransfer.Reason = "Начисление пени";
                transfers.Add(pTransfer);
            }

            if (pRecalcTransfer != null)
            {
                pRecalcTransfer.Reason = "Перерасчет пени";
                transfers.Add(pRecalcTransfer);
            }

            return transfers;
        }

        /// <summary>
        /// Метод отменяет начисления
        /// </summary>
        /// <param name="charge">Источник отмены</param>
        /// <param name="operation">Операция</param>
        /// <param name="info">Информация об отмене</param>
        /// <param name="chargePeriod">Период</param>
        /// <param name="changeInfo">Информация об отмене по суммам</param>
        /// <returns></returns>
        public virtual List<Transfer> UndoCharge(
            ChargeOperationBase charge, 
            MoneyOperation operation,
            PersonalAccountCancelChargeInfo info,
            ChargePeriod chargePeriod, 
            PersonalAccountChangeInfo changeInfo)
        {
            var summary = this.GetOpenedPeriodSummary();

            summary.UndoCharge(
                info.BaseTariffSum,
                info.DecisionTariffSum,
                info.PenaltySum,
                info.BaseTariffChange,
                info.DecisionTariffChange,
                info.PenaltyChange);

            var transfers = new List<Transfer>();

            var btTransfer = this.BaseTariffWallet.TakeMoney(
                TransferBuilder.Create(
                    this,
                    new MoneyStream(charge, operation, chargePeriod.StartDate, info.BaseTariffSum)
                    {
                        Description = "Отмена начислений по базовому тарифу"
                    }));

            if (btTransfer != null)
            {
                transfers.Add(btTransfer);
            }

            var dtTransfer = this.DecisionTariffWallet.TakeMoney(
                TransferBuilder.Create(
                    this,
                    new MoneyStream(charge, operation, chargePeriod.StartDate, info.DecisionTariffSum)
                    {
                        Description = "Отмена начислений по тарифу решений"
                    }));

            if (dtTransfer != null)
            {
                transfers.Add(dtTransfer);
            }

            var pTransfer = this.PenaltyWallet.TakeMoney(
                TransferBuilder.Create(
                    this,
                    new MoneyStream(charge, operation, chargePeriod.StartDate, info.PenaltySum)
                    {
                        Description = "Отмена начисления пени"
                    }));

            if (pTransfer != null)
            {
                transfers.Add(pTransfer);
            }

            var btChangeTransfer = this.BaseTariffWallet.TakeMoney(
                TransferBuilder.Create(
                    this,
                    new MoneyStream(charge, operation, chargePeriod.StartDate, info.BaseTariffChange)
                    {
                        Description = "Отмена ручной корректировки по базовому тарифу"
                    }));

            if (btChangeTransfer != null)
            {
                transfers.Add(btChangeTransfer);
            }

            var dtChangeTransfer = this.DecisionTariffWallet.TakeMoney(
                TransferBuilder.Create(
                    this,
                    new MoneyStream(charge, operation, chargePeriod.StartDate, info.DecisionTariffChange)
                    {
                        Description = "Отмена ручной корректировки по тарифу решений"
                    }));

            if (dtChangeTransfer != null)
            {
                transfers.Add(dtChangeTransfer);
            }

            var pChangeTransfer = this.PenaltyWallet.TakeMoney(
                TransferBuilder.Create(
                    this,
                    new MoneyStream(charge, operation, chargePeriod.StartDate, info.PenaltyChange)
                    {
                        Description = "Отмена ручной корректировки пени"
                    }));

            if (pChangeTransfer != null)
            {
                transfers.Add(pChangeTransfer);
            }

            transfers.ForEach(x => x.TargetCoef = -1);

            //по базовому
            this.TariffChargeBalance -= info.CancelBaseTariffSum;
            
            //тариф
            this.DecisionChargeBalance -= info.CancelDecisionTariffSum;

            //пени
            this.PenaltyChargeBalance -= info.CancelPenaltySum;

            DomainEvents.Raise(
                new PersonalAccountChargeUndoEvent(
                    this, 
                    info.CancelBaseTariffSum, 
                    info.CancelDecisionTariffSum, 
                    info.CancelPenaltySum,  
                    changeInfo)
                {
                    Operation = operation
                });
         
            return transfers;
        }

        /// <summary>
        /// Отмена начисления пени
        /// </summary>
        /// <param name="charge">Начисление, содержащее пеню</param>
        /// <param name="operation">Операция, в рамках которой происходит отмена</param>
        /// <param name="newPenalty">Новое значение пени</param>
        /// <returns>Трансфер отмены</returns>
        public virtual Transfer UndoPenalty(PersonalAccountCharge charge, MoneyOperation operation, decimal newPenalty, PersonalAccountChangeInfo changeInfo)
        {
            Transfer transfer = null;

            if (newPenalty != 0)
            {
                var summary = this.GetOpenedPeriodSummary();
                summary.UndoPreviousPenalty(newPenalty);

                transfer = this.PenaltyWallet.StoreMoney(
                    TransferBuilder.Create(
                        this,
                        new MoneyStream(charge, operation, DateTime.UtcNow, newPenalty)
                        {
                            Description = "Отмена начисления пени"
                        }));

                transfer.TargetCoef = -1;
            }

            var oldPenaltyBalance = this.PenaltyChargeBalance;
            this.PenaltyChargeBalance -= newPenalty;

            DomainEvents.Raise(new PersonalAccountPenaltyChargeUndoEvent(this, oldPenaltyBalance, this.PenaltyChargeBalance, changeInfo) { Operation = operation });

            return transfer;
        }

        #endregion

        #region Payments

        /// <summary>Провести оплату по лицевому счету</summary>
        /// <param name="command">Стратегия разделения денег по базовому и тарифу решения</param>
        /// <param name="payment">Поток денег</param>
        /// <param name="type"></param>
        /// <param name="mode">Режим выполнения операции</param>
        /// <param name="reserve">Суммы, которые не нужно распределять (заранее заданы)</param>
        /// <returns>Трансферы</returns>
        public virtual List<Transfer> ApplyPayment(
            IPersonalAccountPaymentCommand command,
            MoneyStream payment,
            AmountDistributionType type = AmountDistributionType.Tariff,
            ExecutionMode mode = ExecutionMode.Sequential,
            AccountDistributionMoneyReserve reserve = null)
        {
            var result = command.Execute(this, payment, type, reserve);

            if (mode == ExecutionMode.Bulk)
            {
                return result.Transfers;
            }

            this.TariffChargeBalance -= result.DistributionResult.ByBaseTariff + result.DistributionResult.BySocSupport;
            this.DecisionChargeBalance -= result.DistributionResult.ByDecisionTariff;
            this.PenaltyChargeBalance -= result.DistributionResult.ByPenalty;
           
            var summary = this.GetOpenedPeriodSummary();

            summary.ApplyPayment(
                result.DistributionResult.ByBaseTariff + result.DistributionResult.BySocSupport, // в карточке ЛС в поле оплачено по базовому тарифу должна учесться сумма по соц.поддержке
                result.DistributionResult.ByDecisionTariff,
                result.DistributionResult.ByPenalty);

            return result.Transfers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="refund"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public virtual IEnumerable<Transfer> ApplyRefund(IPersonalAccountRefundCommand command, MoneyStream refund, ExecutionMode mode = ExecutionMode.Sequential)
        {
            ArgumentChecker.NotNull(refund, "payment");

            if (refund.Amount <= 0)
            {
                throw new RefundException("Сумма возврата должна быть положительной.");
            }

            if (command.GetAccountBalance(this) < refund.Amount)
            {
                throw new RefundException($"На счету номер {this.PersonalAccountNum} недостаточно денег для возврата.");
            }

            refund.IsAffect = true;
            var result = command.Execute(this, refund, mode);

            if (mode == ExecutionMode.Bulk)
            {
                return result.Transfers;
            }

            this.TariffChargeBalance += result.RefundFromBaseTariffWallet + result.RefundFromSocSuppWallet;
            this.DecisionChargeBalance += result.RefundFromDecisionTariffWallet;
            this.PenaltyChargeBalance += result.RefundFromPenaltyWallet;

            var summary = this.GetOpenedPeriodSummary();
            summary.ApplyRefund(result.RefundFromBaseTariffWallet + result.RefundFromSocSuppWallet, result.RefundFromDecisionTariffWallet, result.RefundFromPenaltyWallet);

            return result.Transfers;
        }

        /// <summary>Отменить операцию возврата по ЛС</summary>
        /// <param name="command">Стратегия отмены возврата</param>
        /// <param name="period">Период начисления</param>
        /// <param name="cancelOperation">Операция, в рамках которой осуществляется возврат</param>
        /// <param name="cancelDate">Дата отмены возврата, если null, то садится числом подтверждения</param>
        /// <returns>Трансферы</returns>
        public virtual IEnumerable<Transfer> UndoRefund(IPersonalAccountRefundCommand command, MoneyOperation cancelOperation, ChargePeriod period, DateTime? cancelDate)
        {
            var result = command.Undo(this, cancelOperation, period, cancelDate);

            this.TariffChargeBalance -= result.RefundFromBaseTariffWallet + result.RefundFromSocSuppWallet;
            this.DecisionChargeBalance -= result.RefundFromDecisionTariffWallet;
            this.PenaltyChargeBalance -= result.RefundFromPenaltyWallet;

            var summary = this.GetOpenedPeriodSummary();

            summary.ApplyPayment(result.RefundFromBaseTariffWallet + result.RefundFromSocSuppWallet, result.RefundFromDecisionTariffWallet, result.RefundFromPenaltyWallet);

            return result.Transfers;
        }

        /// <summary>Отменить операцию оплаты по ЛС</summary>
        /// <param name="command">Стратегия отмены оплаты</param>
        /// <param name="period">Период начисления</param>
        /// <param name="newOperation">Операция, в рамках которой осуществляется оплата</param>
        /// <param name="cancelDate">Дата отмены оплаты, если null, то садится числом подтверждения</param>
        /// <returns>Трансферы</returns>
        public virtual List<Transfer> UndoPayment(IPersonalAccountPaymentCommand command, ChargePeriod period, MoneyOperation newOperation, DateTime? cancelDate)
        {
            ArgumentChecker.NotNull(newOperation.CanceledOperation, nameof(newOperation));

            var result = command.Undo(this, newOperation, period, cancelDate);

            this.TariffChargeBalance += result.UndoByBaseTariff + result.UndoBySocSupport;
            this.DecisionChargeBalance += result.UndoByDecisionTariff;
            this.PenaltyChargeBalance += result.UndoPenalty;

            var summary = this.GetOpenedPeriodSummary();

            summary.ApplyPayment(
                -result.UndoByBaseTariff - result.UndoBySocSupport,
                -result.UndoByDecisionTariff,
                -result.UndoPenalty);

            return result.Transfers;
        }

        #endregion Payments

        #region Direct money moves

        /// <summary>
        /// Перенести средства с одного кошелька на другой
        /// </summary>
        /// <param name="fromWallet">Кошелек-источник</param>
        /// <param name="targetAccount">Целевой аккаунт</param>
        /// <param name="toWallet">Целевой кошелек</param>
        /// <param name="operation">Операция</param>
        /// <param name="source">Объект, через который производится операция</param>
        /// <param name="amount">Сумма</param>
        /// <param name="operationDate">Дата операции</param>
        /// <returns>Тарнсфер</returns>
        public virtual IList<Transfer> MoveMoney(
            Wallet.Wallet fromWallet,
            BasePersonalAccount targetAccount,
            Wallet.Wallet toWallet, 
            MoneyOperation operation, 
            ITransferParty source,
            decimal amount,
            DateTime operationDate)
        {
            var sourceBuilder = TransferBuilder.Create(this, new MoneyStream(source, operation, operationDate, amount));
            var targeBuilder = TransferBuilder.Create(targetAccount, new MoneyStream(source, operation, operationDate, amount));

            var transfers = fromWallet.MoveToAnotherAccount(sourceBuilder, targeBuilder, toWallet);

            if (transfers.IsNotEmpty())
            {
                foreach (var transfer in transfers)
                {
                    transfer.IsInDirect = true;
                }
            }

            return transfers;
        }

        /// <summary>
        /// Перемещение задолжности по базовому тарифу
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="amount"></param>
        public virtual void MoveBaseTariffCharge(BasePersonalAccount recipient, decimal amount)
        {
            ArgumentChecker.NotNull(recipient, nameof(recipient));

            this.TariffChargeBalance -= amount;
            recipient.TariffChargeBalance += amount;

            if (amount < 0)
            {
                this.GetOpenedPeriodSummary().ApplyPayment(amount, 0m, 0m);
                recipient.GetOpenedPeriodSummary().ApplyPayment(-amount, 0m, 0m);
            }
            else
            {
                this.GetOpenedPeriodSummary().ApplyCharge(-amount, 0m, 0m, 0m, 0m, 0m);
                recipient.GetOpenedPeriodSummary().ApplyCharge(amount, 0m, 0m, 0m, 0m, 0m);
            }
        }

        /// <summary>
        /// Перемещение задолженности по тарифу решения
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="amount"></param>
        public virtual void MoveDecisionTariffCharge(BasePersonalAccount recipient, decimal amount)
        {
            ArgumentChecker.NotNull(recipient, nameof(recipient));

            this.DecisionChargeBalance -= amount;
            recipient.DecisionChargeBalance += amount;

            if (amount < 0)
            {
                this.GetOpenedPeriodSummary().ApplyPayment(0m, amount, 0m);
                recipient.GetOpenedPeriodSummary().ApplyPayment(0m, -amount, 0m);
            }
            else
            {
                this.GetOpenedPeriodSummary().ApplyCharge(0m, -amount, 0m, 0m, 0m, 0m);
                recipient.GetOpenedPeriodSummary().ApplyCharge(0m, amount, 0m, 0m, 0m, 0m);
            }
        }

        /// <summary>
        /// Перемещение задолженности по пеням
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="amount"></param>
        public virtual void MovePenaltyCharge(BasePersonalAccount recipient, decimal amount)
        {
            ArgumentChecker.NotNull(recipient, nameof(recipient));

            this.PenaltyChargeBalance -= amount;
            recipient.PenaltyChargeBalance += amount;

            if (amount < 0)
            {
                this.GetOpenedPeriodSummary().ApplyPayment(0m, 0m, amount);
                recipient.GetOpenedPeriodSummary().ApplyPayment(0m, 0m, -amount);
            }
            else
            {
                this.GetOpenedPeriodSummary().ApplyCharge(0m, 0m, -amount, 0m, 0m, 0m);
                recipient.GetOpenedPeriodSummary().ApplyCharge(0m, 0m, amount, 0m, 0m, 0m);
            }
        }

        #endregion

        /// <summary>
        /// Произвести распределение
        /// </summary>
        /// <param name="distributionParams">
        /// The distribution Params.
        /// </param>
        /// <returns>
        /// The <see cref="AccountDistributionResult"/>.
        /// </returns>
        public virtual AccountDistributionResult Distribute(AccountDistributionParams distributionParams)
        {
            var amount = distributionParams.Amount.RegopRoundDecimal(2);

            var reserved = distributionParams.Reserved;

            var distributeAmount = amount - reserved.Sum;

            var type = distributionParams.DistributionType;

            var byBaseTariff = 0m;
            var byDecisionTariff = 0m;
            var byPenalty = 0m;

            var openedPeriodSummary = this.GetOpenedPeriodSummary();

            this.TariffChargeBalance = (openedPeriodSummary.BaseTariffDebt + openedPeriodSummary.GetBaseTariffDebt()).RegopRoundDecimal(2);
            this.PenaltyChargeBalance = (openedPeriodSummary.PenaltyDebt + openedPeriodSummary.GetPenaltyDebt()).RegopRoundDecimal(2);
            this.DecisionChargeBalance = (openedPeriodSummary.DecisionTariffDebt + openedPeriodSummary.GetDecisionTariffDebt()).RegopRoundDecimal(2);

            // если есть задолженность по базовому тарифу, то гасим ее
            if (type.HasFlag(AmountDistributionType.Tariff) && distributeAmount > 0 && this.TariffChargeBalance > 0)
            {
                // если задолженность больше оплаты
                byBaseTariff = this.TariffChargeBalance > distributeAmount

                    // то вся сумма ушла на эту задолженность
                    ? distributeAmount

                    // иначе только сумма, покрывающая задолженность
                    : this.TariffChargeBalance;

                // вычитаем сумму, которая ушла на погашение задолженности
                distributeAmount -= byBaseTariff;
            }

            // если оплата распределена не полностью к этому моменту и есть задолженность сверх базового тарифа, то гасим эту задолженность
            if (type.HasFlag(AmountDistributionType.Tariff) && distributeAmount > 0 && this.DecisionChargeBalance > 0)
            {
                byDecisionTariff = this.DecisionChargeBalance > distributeAmount
                    ? distributeAmount
                    : this.DecisionChargeBalance;

                distributeAmount -= byDecisionTariff;
            }           

            // если оплата распределена не полностью к этому моменту и есть задолженность по пени, то гасим эту задолженность
            if (type.HasFlag(AmountDistributionType.Penalty) && distributeAmount > 0 && this.PenaltyChargeBalance > 0)
            {
                byPenalty = this.PenaltyChargeBalance > distributeAmount
                    ? distributeAmount
                    : this.PenaltyChargeBalance;

                distributeAmount -= byPenalty;
            }

            // остаток оплаты кидаем на базовый тариф
            byBaseTariff += distributeAmount;

            return new AccountDistributionResult
            {
                ByBaseTariff = byBaseTariff + reserved.ByBaseTariff,
                ByDecisionTariff = byDecisionTariff + reserved.ByDecisionTariff,
                ByPenalty = byPenalty + reserved.ByPenalty
            };
        }

        /// <summary>
        /// Установить дату закрытия
        /// </summary>
        /// <param name="date">Дата закрытия</param>
        /// <param name="raiseEvent">Вызвать событие изменения даты закрытия для перерасчёта</param>
        public virtual void SetCloseDate(DateTime date, bool raiseEvent = true)
        {
            this.CloseDate = date;

            if (raiseEvent)
            {
                DomainEvents.Raise(new PersonalAccountCloseEvent(this, date));
            }
        }

        #region State getters

        /// <summary>
        /// Статус закрыт
        /// </summary>
        /// <returns>true, если имя статуса содержит "закрыт"</returns>
        public virtual bool IsClosed()
        {
            return this.State != null && (this.State.Code == "2" || this.State.Code == "3");
        }

        /// <summary>
        /// Статус закрыт с долгом
        /// </summary>
        /// <returns>true, если имя статуса содержит "закрыт с долгом"</returns>
        public virtual bool IsClosedWithCredit()
        {
            return this.State != null && this.State.Code == "3";
        }

        /// <summary>
        /// Метод устанавливает свойству CurrenPeriodSummary значение для исключения запросов ко всем саммари
        /// </summary>
        /// <param name="summary">Детализация ЛС в периоде</param>
        public virtual void SetOpenedPeriodSummary(PersonalAccountPeriodSummary summary)
        {
            if (summary != null && !summary.Period.IsClosed)
            {
                this.OpenedPeriodSummary = summary;
            }
        }

        /// <summary>
        /// Метод получения текущего открытого период саммари для ЛС
        /// </summary>
        public virtual PersonalAccountPeriodSummary GetOpenedPeriodSummary()
        {
            if (this.OpenedPeriodSummary != null && !this.OpenedPeriodSummary.Period.IsClosed)
            {
                return this.OpenedPeriodSummary;
            }

            var summary = this.Summaries.FirstOrDefault(x => !x.Period.IsClosed);

            if (summary == null)
            {
                throw new InvalidOperationException("Period Summary does not exists.");
            }

            this.SetOpenedPeriodSummary(summary);

            return summary;
        }

        /// <summary>
        /// Метод возвращает список guid основных кошельков
        /// </summary>
        /// <returns> Дата </returns>
        public virtual List<string> GetMainWalletGuids()
        {
            var listWallets = new List<string>
            {
                this.BaseTariffWallet.WalletGuid,
                this.DecisionTariffWallet.WalletGuid,
                this.PenaltyWallet.WalletGuid
            };

            return listWallets;
        }

        /// <summary>
        /// Метод возвращает список основных кошельков
        /// </summary>
        public virtual List<Wallet.Wallet> GetMainWallets()
        {
            var listWallets = new List<Wallet.Wallet>
            {
                this.BaseTariffWallet,
                this.DecisionTariffWallet,
                this.PenaltyWallet
            };

            return listWallets;
        }

        /// <summary>
        /// Метод возвращает кошелек по типу
        /// </summary>
        /// <param name="walletType">Тип кошелька</param>
        /// <returns>Кошелек</returns>
        public virtual Wallet.Wallet GetMainWallet(WalletType walletType)
        {
            switch (walletType)
            {
                case WalletType.BaseTariffWallet:
                    return this.BaseTariffWallet;

                case WalletType.DecisionTariffWallet:
                    return this.DecisionTariffWallet;

                case WalletType.PenaltyWallet:
                    return this.PenaltyWallet;

                default:
                    throw new ArgumentOutOfRangeException(nameof(walletType), walletType, null);
            }
        }

        /// <summary>
        /// Метод возвращает 3 основных кошелька в словаре с типом
        /// </summary>
        /// <returns>Кошелек</returns>
        public virtual IDictionary<WalletType, Wallet.Wallet> GetMainWalletsDict()
        {
            return new Dictionary<WalletType, Wallet.Wallet>
            {
                { WalletType.BaseTariffWallet, this.GetMainWallet(WalletType.BaseTariffWallet) },
                { WalletType.DecisionTariffWallet, this.GetMainWallet(WalletType.DecisionTariffWallet) },
                { WalletType.PenaltyWallet, this.GetMainWallet(WalletType.PenaltyWallet) }
            };
        }
        #endregion State getters

        /// <summary>
        /// Метод создания трансфера
        /// </summary>
        /// <param name="sourceGuid">Источник</param>
        /// <param name="targetGuid">Целевой гуид</param>
        /// <param name="moneyStream">Поток средств</param>
        /// <returns>Трансфер</returns>
        protected override Transfer CreateTransferInternal(string sourceGuid, string targetGuid, MoneyStream moneyStream)
        {
            // если пришёл трансфер начисления
            if (moneyStream.Source is IChargeOriginator)
            {
                return new PersonalAccountChargeTransfer(this, sourceGuid, targetGuid, moneyStream.Amount, moneyStream.Operation);
            }

            return new PersonalAccountPaymentTransfer(this, sourceGuid, targetGuid, moneyStream.Amount, moneyStream.Operation);
        }

        /// <inheritdoc />
        public override WalletOwnerType TransferOwnerType => WalletOwnerType.BasePersonalAccount;

        /// <inheritdoc />
        public override string GetDescription()
        {
            return this.PersonalAccountNum;
        }
    }
    
    public class AccountDistributionResult
    {
        public decimal ByBaseTariff { get; set; }

        public decimal ByDecisionTariff { get; set; }

        public decimal ByPenalty { get; set; }

        public decimal BySocSupport { get; set; }
    }

    public class AccountDistributionParams
    {
        public AccountDistributionParams() { }

        public AccountDistributionParams(decimal amount) : this (amount, AmountDistributionType.Tariff)
        {
            
        }

        public AccountDistributionParams(decimal amount, AmountDistributionType type, AccountDistributionMoneyReserve reserved = null)
        {
            this.Amount = amount;
            this.DistributionType = type;
            this.Reserved = reserved ?? new AccountDistributionMoneyReserve();

            if (this.Amount < this.Reserved.Sum)
            {
                throw new ValidationException("Сумма зарезервированных средств не может превышать сумму распределения");
            }
        }

        public decimal Amount { get; set; }

        public AmountDistributionType DistributionType { get; set; }
        public AccountDistributionMoneyReserve Reserved { get; set; }
    }

    /// <summary>
    /// Класс для указания конкретных сумм для распределения 
    /// </summary>
    public class AccountDistributionMoneyReserve
    {
        public decimal ByBaseTariff { get; set; }

        public decimal ByDecisionTariff { get; set; }

        public decimal ByPenalty { get; set; }

        public decimal BySocSupport { get; set; }

        public decimal Sum => this.ByBaseTariff + this.ByDecisionTariff + this.ByPenalty + this.BySocSupport;
    }
}