﻿namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.ReferenceCalculation.Impl
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class DebtorEarlyMonthly : DebtorReferenceCalculationService
    {
        public override DebtCalc DebtCalc => DebtCalc.Early;

        public override PenaltyCharge PenaltyCharge => PenaltyCharge.Monthly;

        public override IDataResult CalculateReferencePayments(AgentPIRDebtor debtor, List<long> transfers)
        {
            //Удаляем старый рассчет
            ClearReferenceCalculation(debtor.Id);

            bool hasCancelledCharges = false;

            DateTime docDate = DateTime.Now;
            var lastPeriod = GetLastReferenceCalculationPeriod(docDate);

            var refundRateList = PayPenDomain.GetAll().Where(x => x.DecisionType == Gkh.Enums.Decisions.CrFundFormationDecisionType.RegOpAccount).ToList();
            var persAcc = PersAccDomain.Get(debtor.BasePersonalAccount.Id);

            debtor.DebtBaseTariff = 0;
            debtor.PenaltyDebt = 0;

            //устанвливаем дату начала задолжености на конец последнего периода.Далее отодвигаем назад вовремени в зависимоcти от ситуации на ЛС должника
            //концом задолжености считаем конец последнего пеоиода 
            debtor.DebtStartDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            debtor.DebtEndDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            var message = "";


            //если применен срок исковой давности то начинаем от него
            DateTime openDate = debtor.UseCustomDate ? debtor.CustomDate.Value : debtor.BasePersonalAccount.OpenDate;
            DateTime endDate = lastPeriod.EndDate ?? DateTime.Now;

            var chargePeriods = PeriodDomain.GetAll()
                .Where(x => x.EndDate >= openDate && x.EndDate <= endDate)
                .OrderBy(x => x.StartDate)
                .ToList();

            decimal roomArea = persAcc.Room.Area;
            decimal areaShare = persAcc.AreaShare;
            string accountNumber = persAcc.PersonalAccountNum;
            long persAccId = persAcc.Id;

            //получаем размер взноса за кр
            var paysize = PaySizeDomain.GetAll()
                .Where(x => x.Municipality != null)
                .Where(x => x.Value != null)
                .Where(x => x.Municipality == persAcc.Room.RealityObject.Municipality)
                .ToList();

            Dictionary<ChargePeriod, decimal> paysizeByPeriod = new Dictionary<ChargePeriod, decimal>();

            var decisions = DecisionDomain.GetAll()
                .Where(x => x.Protocol.RealityObject == persAcc.Room.RealityObject)
                .ToList();

            chargePeriods.ForEach(x =>
            {
                if (decisions != null)
                {
                    foreach (var mfdh in decisions)
                    {
                        foreach (var pmf in mfdh.Decision)
                        {
                            if (pmf.Value > 0 && pmf.From == x.StartDate && (!pmf.To.HasValue || pmf.To.Value >= x.EndDate))
                            {
                                if (!paysizeByPeriod.ContainsKey(x))
                                {
                                    paysizeByPeriod.Add(x, pmf.Value);
                                }
                            }
                        }
                    }
                }

                if (!paysizeByPeriod.ContainsKey(x))
                {
                    foreach (var psr in paysize)
                    {
                        if (psr.Paysize.DateStart <= x.StartDate && (psr.Paysize.DateEnd >= x.EndDate || psr.Paysize.DateEnd == null))
                        {
                            decimal? paySizeVal = psr.Value;
                            paysizeByPeriod.Add(x, paySizeVal ?? 0);
                        }
                    }
                }

            });

            var firstPeriod = true;
            var refCalc = new List<DebtorReferenceCalculation>();

            var allHistory = EntityLogLightDomain.GetAll()
                  .Where(x => x.ClassName == "BasePersonalAccount" && x.ParameterName == "area_share")
                  .Where(x => x.EntityId == persAcc.Id)
                  .ToList();

            //исключаем более ранние изменения с той же датой начала действия
            var filteredHistory = allHistory
                .GroupBy(x => new { x.EntityId, x.DateActualChange })
                .ToDictionary(x => x.Key)
                .Select(x => x.Value.OrderByDescending(u => u.DateApplied).FirstOrDefault())
                .ToList();

            foreach (ChargePeriod period in chargePeriods)
            {
                var share = filteredHistory.Where(x => x.DateActualChange <= period.EndDate).OrderByDescending(x => x.DateApplied)
                    .ThenByDescending(x => x.Id).FirstOrDefault();
                var areaShareByPeriod = share != null ? Convert.ToDecimal(share.PropertyValue.Replace('.', ',')) : areaShare;
                refCalc.Add(
                    new DebtorReferenceCalculation
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 0,
                        PeriodId = period.Id,
                        AccountNumber = accountNumber,
                        AreaShare = areaShareByPeriod,
                        AgentPIRDebtor = debtor,
                        Penalties = 0,
                        BaseTariff = paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0,
                        PersonalAccountId = persAccId,
                        RoomArea = roomArea,
                        TariffCharged = firstPeriod
                            ? CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod, openDate)
                            : CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod)
                    });
                firstPeriod = false;
            }

            //Все трансферы
            var persAccAllTransfers = PaymentDomain.GetAll()
                .WhereIf(transfers.Count > 0, x => !transfers.Contains(x.Id))
                .Where(x => x.Owner.Id == persAcc.Id)
                .Where(x => x.Operation.IsCancelled != true)
                .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации

            List<PersonalAccountChargeTransfer> persAccPenaltyChargesList = new List<PersonalAccountChargeTransfer>();

            decimal persAccPenaltyCharges = 0;

            //отмены начислений 
            var canceledCharge = ChargeDomain.GetAll()
                .Where(x => x.Owner.Id == persAcc.Id)
                .WhereIf(debtor.UseCustomDate, x => x.OperationDate > debtor.CustomDate)
                .Where(x => x.ChargePeriod.Id <= lastPeriod.Id)
                .Where(x => x.Reason == "Отмена начислений по базовому тарифу")
                .FirstOrDefault();

            if (canceledCharge != null)
            {
                hasCancelledCharges = true;
            }

            //Оплаты
            var persAccAllChargeTransfers = persAccAllTransfers
                .WhereIf(debtor.UseCustomDate, x => x.PaymentDate > debtor.CustomDate)
                .Where(
                    x => x.Reason == "Оплата по базовому тарифу" ||
                         x.Reason == "Возврат взносов на КР" ||
                         x.Reason == "Оплата по тарифу решения")
                .Select(x => new
                {
                    x.Id,
                    Amount = x.Reason == "Возврат взносов на КР" ? x.Amount * (-1) : x.Amount,
                    x.ChargePeriod,
                    x.IsAffect,
                    x.IsInDirect,
                    x.IsLoan,
                    x.IsReturnLoan,
                    x.Owner,
                    x.PaymentDate,
                    x.Reason
                })
                .ToList();

            var persAccAllPenaltyTransfers = persAccAllTransfers
                .WhereIf(debtor.UseCustomDate, x => x.PaymentDate > debtor.CustomDate)
                .Where(
                    x => x.Reason == "Оплата пени" ||
                        x.Reason == "Возврат оплаты пени")
                .Select(x => new
                {
                    x.Id,
                    Amount = x.Reason == "Возврат оплаты пени" ? x.Amount * (-1) : x.Amount,
                    x.ChargePeriod,
                    x.IsAffect,
                    x.IsInDirect,
                    x.IsLoan,
                    x.IsReturnLoan,
                    x.Owner,
                    x.PaymentDate,
                    x.Reason
                }).ToList();

            decimal penaltyPayed = persAccAllPenaltyTransfers.Count > 0 ? persAccAllPenaltyTransfers.Sum(x => x.Amount) : 0;
            decimal totalCharged = refCalc.Sum(x => x.TariffCharged);
            decimal totalPayment = persAccAllChargeTransfers.Sum(x => x.Amount);
            decimal resultDebt = totalCharged - totalPayment;

            persAccAllChargeTransfers = persAccAllChargeTransfers.OrderBy(x => x.Id).ToList();

            //Расставляем даты и оплаты по месяцам
            for (var chargeIndex = 0;
            chargeIndex < persAccAllChargeTransfers.Count && chargeIndex < refCalc.Count;
            chargeIndex++)
            {
                refCalc[chargeIndex].PaymentDate = persAccAllChargeTransfers[chargeIndex].PaymentDate.ToShortDateString();
                refCalc[chargeIndex].TarifPayment = persAccAllChargeTransfers[chargeIndex].Amount;
                if (persAccAllChargeTransfers[chargeIndex].Reason == "Возврат взносов на КР")
                {
                    refCalc[chargeIndex].Description = "Возврат взносов на КР";
                }

            }

            //Инвертируем оплаты для отображения
            decimal remainingPayment = totalPayment * -1;
            decimal totalDebt = 0;
            var debtStarted = false;
            DateTime debtStartDate = openDate;
            message = totalPayment == 0
                ? debtor.UseCustomDate
                    ? "Задолженность начинается с даты применения срока исковой давности"
                    : "Задолженность начинается с даты открытия лицевого счёта."
                : "";
            foreach (DebtorReferenceCalculation referenceCalculation in refCalc)
            {
                remainingPayment += referenceCalculation.TariffCharged;
                totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                referenceCalculation.TarifDebt = totalDebt;
                if (remainingPayment >= 0 && !debtStarted)
                {
                    //Рассчет даты начала задолженности
                    ChargePeriod lastPaymentPeriod = PeriodDomain.Get(referenceCalculation.PeriodId);
                    debtStartDate = CalculateDebtStartDate(
                        referenceCalculation.BaseTariff,
                        roomArea,
                        referenceCalculation.AreaShare,
                        remainingPayment,
                        lastPaymentPeriod.StartDate);

                    // Фикс несостыковки дат открытия аккаунта и начала задолженности из-за округлений
                    // (дата начала может быть указана на 1 день раньше из-за десятых-сотых копеек долга,
                    // если такое происходит и итоговая дата раньше даты открытия - заменяем на дату открытия)
                    debtStartDate = debtStartDate < openDate ? openDate : debtStartDate;

                    debtor.DebtStartDate = debtor.DebtStartDate > debtStartDate ? debtStartDate : debtor.DebtStartDate;
                    debtor.DebtEndDate = debtor.DebtEndDate < endDate ? endDate : debtor.DebtEndDate;

                    if (message == "")
                    {
                        if (remainingPayment == 0)
                        {
                            message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                        }
                        else
                        {
                            switch (debtStartDate.Day)
                            {
                                //Разный текст примечания в зависимости от дня начала задолженности
                                case 1:
                                    message = $"За предыдущий месяц переплата {referenceCalculation.TariffCharged - remainingPayment}.";
                                    break;
                                case 2:
                                    message = $"За {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                    break;
                                default:
                                    message =
                                        $"С {lastPaymentPeriod.StartDate.ToShortDateString()} " +
                                        $"по {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                    break;
                            }

                            message += $" С {debtStartDate.ToShortDateString()} задолженность {remainingPayment.ToString(CultureInfo.InvariantCulture)}.";
                            referenceCalculation.Description = message;
                        }
                    }
                    debtStarted = true;
                }
                RefCalcDomain.Save(referenceCalculation);
            }

            //считаем эталонные пени
            var refcalcList = RefCalcDomain.GetAll().Where(x => x.AgentPIRDebtor == debtor).OrderBy(x => x.PeriodId).ToList();

            foreach (DebtorReferenceCalculation referenceCalculation in refcalcList)
            {
                var currentPeriod = chargePeriods.FirstOrDefault(x => x.Id == referenceCalculation.PeriodId);
                var penaltyPaymentsInPeriod = persAccAllPenaltyTransfers.Where(x => x.ChargePeriod.Id == referenceCalculation.PeriodId).ToList();
                if (penaltyPaymentsInPeriod.Count > 0)
                {
                    referenceCalculation.PenaltyPayment = penaltyPaymentsInPeriod.Sum(x => x.Amount);
                    referenceCalculation.PenaltyPaymentDate = penaltyPaymentsInPeriod.Max(x => x.PaymentDate.ToString("dd.MM.yyyy"));
                }
                if (referenceCalculation.TarifDebt > 0)
                {
                    ChargeMonthlyPenalty(referenceCalculation, referenceCalculation.TarifDebt, refundRateList, currentPeriod, docDate);
                }
            }
            persAccPenaltyCharges = RefCalcDomain.GetAll().Where(x => x.AgentPIRDebtor == debtor).Sum(x => x.Penalties.Value);

            debtor.DebtBaseTariff += resultDebt < 0 ? 0 : resultDebt;
            debtor.PenaltyDebt += (persAccPenaltyCharges - penaltyPayed);



            DebtorDomain.Update(debtor);
            if (hasCancelledCharges)
            {
                message += " Внимание!!!! на ЛС есть отмены начислений. Возможно списание исковой давности";
            }

            return new BaseDataResult(
                new
                {
                    DebtBaseTariffSum = debtor.DebtBaseTariff,
                    dateStartDebt = debtor.DebtStartDate,
                    message,
                    debtor.DebtEndDate,
                    debtor.PenaltyDebt
                });
        }

        private void ChargeMonthlyPenalty(DebtorReferenceCalculation referenceCalculation, decimal debtBaseTariff, List<PaymentPenalties> refundRateList, ChargePeriod currentPeriod, DateTime docdate)
        {
            //дата начала отсчета пени - 10 число месяца, следующего за месяцем, в котором образовалась задолженность
            DateTime startDate = new DateTime();
            if (currentPeriod.StartDate.Month != 12)
            {
                startDate = new DateTime(currentPeriod.StartDate.Year, currentPeriod.StartDate.Month + 1, 10);
            }
            else
            {
                startDate = new DateTime(currentPeriod.StartDate.Year + 1, 1, 10);
            }

            var rateList = new List<PaymentPenalties>();

            //справочник пени на момент перерассчета
            var nowRate = refundRateList
                .Where(x => x.DateStart <= docdate)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= docdate)
                .OrderBy(x => x.DateStart)
                .FirstOrDefault();

            //справочник пени на момент начала отсчета пени
            var realRate = refundRateList
                .Where(x => x.DateStart <= currentPeriod.StartDate)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= currentPeriod.StartDate)
                .OrderBy(x => x.DateStart)
                .FirstOrDefault();

            //справочник пени на момент начала отсчета пени
            var payRate = refundRateList
                .Where(x => x.DateStart <= startDate)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= startDate)
                .OrderBy(x => x.DateStart)
                .FirstOrDefault();

            //не наложен мораторий на период начала отсчета и на период перерасчета
            if (realRate.Percentage != 0 && nowRate.Percentage != 0)
            {
                var bankRate = nowRate.Percentage / 300;
                var totalDayCount = 0;

                //период начала не совпадает с периодом звсп
                if (realRate.DateStart != nowRate.DateStart)
                {
                    var dayCount = (docdate - startDate).Days;

                    rateList = refundRateList
                    .Where(x => x.DateStart >= startDate)
                    .Where(x => x.DateEnd <= nowRate.DateStart)
                    .Where(x => x.Percentage == 0)
                    .ToList();

                    var morDayCount = 0;
                    foreach (var rate in rateList)
                    {
                        morDayCount += (rate.DateEnd - rate.DateStart).Value.Days;
                    }

                    if (payRate.Percentage == 0)
                    {
                        morDayCount += (payRate.DateEnd - payRate.DateStart).Value.Days;
                    }

                    //количество дней между началом действия справочника на момент перерсчета и датой звсп
                    var nowDayCount = (docdate - nowRate.DateStart).Days;

                    totalDayCount = dayCount - morDayCount + nowDayCount;
                    var penalty = debtBaseTariff * totalDayCount * bankRate / 100;
                    referenceCalculation.Penalties = decimal.Round(penalty, 2);
                }
                else
                {
                    totalDayCount = (docdate - startDate).Days;
                    var penalty = debtBaseTariff * totalDayCount * bankRate / 100;
                    referenceCalculation.Penalties = decimal.Round(penalty, 2);
                }

                referenceCalculation.AccrualPenalties = $"Задолженность за {currentPeriod.Name} составляет {decimal.Round(debtBaseTariff, 2)} руб. Ставка {decimal.Round(nowRate.Percentage, 2)}%. Количество дней просрочки {totalDayCount}.";
                referenceCalculation.AccrualPenaltiesFormula = $"{decimal.Round(debtBaseTariff, 2)} x {totalDayCount} x 1/300 x {decimal.Round(realRate.Percentage, 2)}%.";
            }
            else
            {
                referenceCalculation.Penalties = 0;
                referenceCalculation.AccrualPenalties = "Мораторий на начисление пени, или пени не начислялась";
            }

            RefCalcDomain.Update(referenceCalculation);
        }
    }
}