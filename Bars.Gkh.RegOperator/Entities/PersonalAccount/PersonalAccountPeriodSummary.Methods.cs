namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Utils;

    public partial class PersonalAccountPeriodSummary
    {
        /// <summary>
        /// Применить оплату
        /// </summary>
        /// <param name="baseTariffPayment"></param>
        /// <param name="decisionTariffPayment"></param>
        /// <param name="penaltyPayment"></param>
        /// <param name="updateSaldo">Флаг обновления сальдо</param>
        public virtual void ApplyPayment(decimal baseTariffPayment, decimal decisionTariffPayment, decimal penaltyPayment, bool updateSaldo = true)
        {
            this.TariffPayment += baseTariffPayment;
            this.TariffDecisionPayment += decisionTariffPayment;
            this.PenaltyPayment += penaltyPayment;

            if (updateSaldo)
            {
                this.RecalcSaldoOut();
            }
        }

        /// <summary>
        /// Применить возврат
        /// </summary>
        public virtual void ApplyRefund(decimal baseTariffRefund, decimal decisionTariffRefund, decimal penaltyRefund)
        {
            this.TariffPayment -= baseTariffRefund;
            this.TariffDecisionPayment -= decisionTariffRefund;
            this.PenaltyPayment -= penaltyRefund;

            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Применить начисление
        /// </summary>
        public virtual void ApplyCharge(
            decimal baseTariff,
            decimal decisionTariff,
            decimal penalty,
            decimal recalcByPenalty,
            decimal recalcByTariff,
            decimal recalByDecision)
        {
            this.ChargedByBaseTariff += baseTariff;
            this.ChargeTariff += decisionTariff + baseTariff;
            this.Penalty += penalty;
            this.RecalcByBaseTariff += recalcByTariff;
            this.RecalcByDecisionTariff += recalByDecision;
            this.RecalcByPenalty += recalcByPenalty;

            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Применить начисление
        /// </summary>
        public virtual void ApplyCharge(PersonalAccountCharge charge)
        {
            this.ApplyCharge(
                charge.ChargeTariff - charge.OverPlus,
                charge.OverPlus,
                charge.Penalty,
                charge.RecalcPenalty,
                charge.RecalcByBaseTariff,
                charge.RecalcByDecisionTariff);
        }

        /// <summary>
        /// Отменить начисление
        /// </summary>
        public virtual void UndoCharge(PersonalAccountCharge charge)
        {
            this.UndoCharge(
                charge.ChargeTariff - charge.OverPlus,
                charge.OverPlus,
                charge.Penalty,
                charge.RecalcPenalty,
                charge.RecalcByBaseTariff,
                charge.RecalcByDecisionTariff);
        }

        /// <summary>
        /// Отмена начислений
        /// </summary>
        public virtual void UndoCharge(
            decimal baseTariff,
            decimal decisionTariff,
            decimal penalty,
            decimal baseTariffChange,
            decimal decisionTariffChange,
            decimal penaltyChange)
        {
            this.Penalty -= penalty;
            this.ChargedByBaseTariff -= baseTariff;
            this.ChargeTariff -= baseTariff + decisionTariff;

            this.PenaltyChange -= penaltyChange;
            this.BaseTariffChange -= baseTariffChange;
            this.DecisionTariffChange -= decisionTariffChange;

            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Уменьшение входящего сальдо
        /// </summary>
        /// <param name="newPenalty">Число, на которое уменьшаем</param>
        public virtual void UndoPreviousPenalty(decimal newPenalty)
        {
            this.Penalty -= newPenalty;
            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Обновить баланс
        /// </summary>
        /// <param name="byBaseTariff"></param>
        /// <param name="byDecisionTariff"></param>
        /// <param name="penaltyMoney"></param>
        public virtual void UpdateBalance(decimal byBaseTariff, decimal byDecisionTariff, decimal penaltyMoney)
        {
            this.TariffPayment = byBaseTariff;
            this.TariffDecisionPayment = byDecisionTariff;
            this.PenaltyPayment = penaltyMoney;

            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Метод изменения пени для конкретного периода
        /// penalty - новое значение ввел пользователь
        /// debetPenalty - значение дебетового пени (Начислено - Оплачено) на тот момент когда меняют пени в ручную
        /// </summary>
        public virtual PenaltyChange ChangePenalty(decimal penalty, decimal debetPenalty, string reason, FileInfo file)
        {
            // Фиксируем изменение пеней и исходящее сальдо
            var penaltyDelta = penalty - debetPenalty;

            // Если старое и новое значение равны, то ничего не делаем
            if (penaltyDelta == 0)
            {
                throw new InvalidOperationException("Новое значение пени равно исходному");
            }

            var change = new PenaltyChange
            {
                Account = this.PersonalAccount,
                AccountPeriodSummary = this,
                NewValue = penalty,
                CurrentValue = debetPenalty,
                Reason = reason,
                Document = file
            };

            // Фиксируем в коллекции
            this.penaltyChanges.Add(change);

            this.PenaltyChange += penaltyDelta;

            this.RecalcSaldoOut();

            return change;
        }

        /// <summary>
        /// Метод изменения пени для конкретного периода
        /// penalty - новое значение ввел пользователь
        /// debetPenalty - значение дебетового пени (Начислено - Оплачено) на тот момент когда меняют пени в ручную
        /// </summary>
        public virtual void ChangeSaldo(decimal saldoByBaseTariffDelta, decimal saldoByDecisionTariffDelta, decimal saldoByPenaltyDelta)
        {
            // меняем начислено по типу
            this.BaseTariffChange += saldoByBaseTariffDelta;
            this.DecisionTariffChange += saldoByDecisionTariffDelta;
            this.PenaltyChange += saldoByPenaltyDelta;

            // пересчитываем исх сальдо
            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Применить операцию установки/изменения сальдо
        /// </summary>
        /// <param name="newValue">
        /// Новое значение
        /// </param>
        /// <param name="reason">
        /// Причина
        /// </param>
        /// <param name="document">
        /// Документ
        /// </param>
        /// <returns>
        /// The <see cref="PeriodSummaryBalanceChange" />.
        /// </returns>
        public virtual PeriodSummaryBalanceChange ChangeSaldo(decimal newValue, string reason, FileInfo document)
        {
            var oldValue = this.SaldoOut.RegopRoundDecimal(2) - (this.PenaltyDebt + this.GetPenaltyDebt());

                //метод ChangeSaldo мы уже вызываем из текущего открытого периода GetOpenedPeriodSummary();
            var diff = newValue - oldValue;

            if (diff == 0m)
            {
                throw new InvalidOperationException("Новое сальдо равно исходному сальдо");
            }

            this.BaseTariffChange += diff;

            var daysInMonth = DateTime.DaysInMonth(this.Period.StartDate.Year, this.Period.StartDate.Month);
            var now = DateTime.Now;
            var day = Math.Min(now.Day, daysInMonth);

            var changeDate = new DateTime(this.Period.StartDate.Year, this.Period.StartDate.Month, day);

            var change = new PeriodSummaryBalanceChange(this, changeDate, oldValue, newValue, document, reason);

            this.balanceChanges.Add(change);

            this.RecalcSaldoOut();

            return change;
        }

        /// <summary>
        /// Применение распределения зачета средств за ранее выполненные работы
        /// </summary>
        public virtual PeriodSummaryBalanceChange ApplyPerformedWorkDistribution(decimal amount, string reason, DateTime operationDate, FileInfo document)
        {
            var oldValue = this.SaldoOut;

            this.PerformedWorkChargedBase += amount;

            // TODO: fix in future
            var change = new PeriodSummaryBalanceChange(this, operationDate, oldValue, oldValue - amount, document, reason);
            this.balanceChanges.Add(change);

            this.RecalcSaldoOut();

            return change;
        }

        /// <summary>
        /// Пересчитать исходящее сальдо
        /// </summary>
        /// <returns></returns>
        public virtual void RecalcSaldoOut()
        {
            this.SaldoOut =
                (this.SaldoIn + this.GetTotalCharge() + this.GetTotalChange() - this.GetTotalPayment() - this.GetTotalPerformedWorkCharge())
                    .RegopRoundDecimal(2);
        }

        /// <summary>
        /// Подсчет суммы по дебету за период
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetTotalCharge()
        {
            return this.ChargeTariff + this.Penalty + this.RecalcByBaseTariff + this.RecalcByDecisionTariff + this.RecalcByPenalty;
        }

        /// <summary>
        /// Подсчёт суммы изменений сальдо по периоду
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetTotalChange()
        {
            return this.BaseTariffChange + this.DecisionTariffChange + this.PenaltyChange;
        }

        /// <summary>
        /// Подсчет суммы перерасчета по периоду
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetTotalRecalc()
        {
            return this.RecalcByBaseTariff + this.RecalcByDecisionTariff + this.RecalcByPenalty;
        }

        /// <summary>
        /// Подсчет суммы по кредиту за период
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetTotalPayment()
        {
            return this.TariffPayment + this.TariffDecisionPayment + this.PenaltyPayment;
        }

        /// <summary>
        /// Получить сумму начислений
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetChargedByTariff()
        {
            return this.GetChargedByBaseTariff() + this.GetChargedByDecisionTariff();
        }

        /// <summary>
        /// Получить сумму начислений по базовому тарифу
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetChargedByBaseTariff()
        {
            return this.ChargedByBaseTariff + this.RecalcByBaseTariff;
        }

        /// <summary>
        /// Получить сумму начислений по тарифу решений
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetChargedByDecisionTariff()
        {
            return this.ChargeTariff - this.ChargedByBaseTariff + this.RecalcByDecisionTariff;
        }

        /// <summary>
        /// Получить сумму начислений по пени
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetChargedByPenalty()
        {
            return this.Penalty + this.RecalcByPenalty;
        }

        /// <summary>
        /// Получить задолженность по пени
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetPenaltyDebt()
        {
            return this.Penalty + this.RecalcByPenalty + this.PenaltyChange - this.PenaltyPayment;
        }

        /// <summary>
        /// Получить задолженность по тарифу решения
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetDecisionTariffDebt()
        {
            return this.GetChargedByDecisionTariff() + this.DecisionTariffChange - this.TariffDecisionPayment;
        }

        /// <summary>
        /// Получить задолженность по базовому тарифу
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetBaseTariffDebt()
        {
            return this.GetChargedByBaseTariff() + this.BaseTariffChange - this.TariffPayment;
        }

        /// <summary>
        /// Получить оплаты
        /// </summary>
        public virtual decimal GetTariffPayment()
        {
            return this.TariffPayment + this.TariffDecisionPayment;
        }

        /// <summary>
        /// Получить задолженность
        /// </summary>
        public virtual decimal GetEndTariffDebt()
        {
            return this.BaseTariffDebt
                + this.DecisionTariffDebt
                + this.GetChargedByBaseTariff()
                + this.GetChargedByDecisionTariff();
        }

        /// <summary>
        /// Получить сумму зачетов за ранее выполненные работы
        /// </summary>
        public virtual decimal GetTotalPerformedWorkCharge()
        {
            return this.PerformedWorkChargedBase + this.PerformedWorkChargedDecision;
        }
    }
}