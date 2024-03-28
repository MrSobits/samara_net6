namespace Bars.Gkh.RegOperator.Domain.ReferenceCalculation.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Domain.ReferenceCalculation;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    public class LateMonthly : ReferenceCalculationService
    {
        public override DebtCalc DebtCalc => DebtCalc.Late;

        public override PenaltyCharge PenaltyCharge => PenaltyCharge.Monthly;

        public override IDataResult CalculateReferencePayments(Lawsuit lawsuit, List<ClaimWorkAccountDetail> claimWorkAccountDetailList, List<long> transfers)
        {
            //Удаляем старый рассчет
            ClearReferenceCalculation(lawsuit.Id);

            bool hasCancelledCharges = false;

            DateTime docDate = lawsuit.DocumentDate ?? DateTime.MinValue;
            var lastPeriod = GetLastReferenceCalculationPeriod(docDate);

            var refundRateList = PayPenDomain.GetAll().Where(x => x.DecisionType == Gkh.Enums.Decisions.CrFundFormationDecisionType.RegOpAccount).ToList();

            lawsuit.DebtSum = 0;
            lawsuit.DebtBaseTariffSum = 0;
            lawsuit.DebtDecisionTariffSum = 0;
            lawsuit.PenaltyDebt = 0;

            //устанвливаем дату начала задолжености на конец последнего периода.Далее отодвигаем назад вовремени в зависимоcти от ситуации на ЛС должника
            //концом задолжености считаем конец последнего пеоиода 
            lawsuit.DebtStartDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            lawsuit.DebtEndDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            var message = "";

            foreach (var claimWorkAccountDetail in claimWorkAccountDetailList)
            {
                //если применен срок исковой давности то начинаем от него
                DateTime openDate = lawsuit.IsLimitationOfActions ? lawsuit.DateLimitationOfActions.Value : claimWorkAccountDetail.PersonalAccount.OpenDate;
                DateTime endDate = lastPeriod.EndDate ?? DateTime.MinValue;

                var chargePeriods = PeriodDomain.GetAll()
                    .Where(x => x.EndDate >= openDate && x.EndDate <= endDate)
                    .OrderBy(x => x.StartDate)
                    .ToList();

                decimal roomArea = claimWorkAccountDetail.PersonalAccount.Room.Area;
                decimal areaShare = claimWorkAccountDetail.PersonalAccount.AreaShare;
                string accountNumber = claimWorkAccountDetail.PersonalAccount.PersonalAccountNum;
                long persAccId = claimWorkAccountDetail.PersonalAccount.Id;

                //получаем размер взноса за кр
                var paysize = PaySizeDomain.GetAll()
                    .Where(x => x.Municipality != null)
                    .Where(x => x.Value != null)
                    .Where(x => x.Municipality == claimWorkAccountDetail.PersonalAccount.Room.RealityObject.Municipality)
                    .ToList();

                Dictionary<ChargePeriod, decimal> paysizeByPeriod = new Dictionary<ChargePeriod, decimal>();

                var decisions = DecisionDomain.GetAll()
                    .Where(x => x.Protocol.RealityObject == claimWorkAccountDetail.PersonalAccount.Room.RealityObject)
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
                var refCalc = new List<LawsuitReferenceCalculation>();

                var allHistory = EntityLogLightDomain.GetAll()
                      .Where(x => x.ClassName == "BasePersonalAccount" && x.ParameterName == "area_share")
                      .Where(x => x.EntityId == claimWorkAccountDetail.PersonalAccount.Id)
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
                        new LawsuitReferenceCalculation
                        {
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            ObjectVersion = 0,
                            PeriodId = period,
                            AccountNumber = accountNumber,
                            AreaShare = areaShareByPeriod,
                            Lawsuit = lawsuit,
                            BaseTariff = paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0,
                            PersonalAccountId = claimWorkAccountDetail.PersonalAccount,
                            RoomArea = roomArea,
                            TariffCharged = firstPeriod
                                ? CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod, openDate)
                                : CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod)
                        });
                    firstPeriod = false;
                }

                refCalc.OrderBy(x => x.PeriodId);

                //Все трансферы
                var persAccAllTransfers = PaymentDomain.GetAll()
                    .WhereIf(transfers.Count > 0, x => !transfers.Contains(x.Id))
                    .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                    .Where(x => x.Operation.IsCancelled != true)
                    .Where(x => x.PaymentDate >= claimWorkAccountDetail.PersonalAccount.OpenDate)
                    .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации

                List<PersonalAccountChargeTransfer> persAccPenaltyChargesList = new List<PersonalAccountChargeTransfer>();

                decimal persAccPenaltyCharges = 0;

                //отмены начислений 
                var canceledCharge = ChargeDomain.GetAll()
                    .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                    .WhereIf(lawsuit.IsLimitationOfActions, x => x.OperationDate > lawsuit.DateLimitationOfActions)
                    .Where(x => x.ChargePeriod.Id <= lastPeriod.Id)
                    .Where(x => x.Reason == "Отмена начислений по базовому тарифу")
                    .FirstOrDefault();

                if (canceledCharge != null)
                {
                    hasCancelledCharges = true;
                }

                //Оплаты
                var persAccAllChargeTransfers = persAccAllTransfers
                    .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
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
                    .OrderBy(x => x.PaymentDate)
                    .ToList();

                var persAccAllPenaltyTransfers = persAccAllTransfers
                    .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
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
                    })
                    .OrderBy(x => x.PaymentDate)
                    .ToList();

                decimal penaltyPayed = persAccAllPenaltyTransfers.Count > 0 ? persAccAllPenaltyTransfers.Sum(x => x.Amount) : 0;
                decimal totalCharged = refCalc.Sum(x => x.TariffCharged);
                decimal totalPayment = persAccAllChargeTransfers.Sum(x => x.Amount);
                decimal resultDebt = totalCharged - totalPayment;

                persAccAllChargeTransfers = persAccAllChargeTransfers.OrderBy(x => x.Id).ToList();

                //проставляем оплаты
                foreach (var сhargeTransfer in persAccAllChargeTransfers)
                {
                    var previousPeriod = chargePeriods.Where(x => x.Id < сhargeTransfer.ChargePeriod.Id).LastOrDefault() ??
                        chargePeriods.Where(x => x.Id == сhargeTransfer.ChargePeriod.Id).FirstOrDefault();

                    foreach (var referenceCalculation in refCalc)
                    {
                        if (referenceCalculation.PeriodId.Id == previousPeriod.Id)
                        {
                            if (referenceCalculation.PaymentDate.IsEmpty())
                            {
                                referenceCalculation.PaymentDate = сhargeTransfer.PaymentDate.ToShortDateString();
                            }
                            referenceCalculation.TarifPayment += сhargeTransfer.Amount;
                            if (сhargeTransfer.Reason == "Возврат взносов на КР")
                            {
                                referenceCalculation.Description = "Возврат взносов на КР";
                            }
                        }
                    }
                }

                decimal totalDebt = 0;

                var debtCalc = new List<LawsuitReferenceCalculation>();

                foreach (var referenceCalculation in refCalc)
                {
                    totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                    referenceCalculation.TarifDebt = totalDebt;

                    if (totalDebt > 0 || referenceCalculation.TarifPayment > 0)
                    {
                        referenceCalculation.TarifDebtPay = referenceCalculation.TariffCharged > totalDebt ? totalDebt : referenceCalculation.TariffCharged;
                        debtCalc.Add(referenceCalculation);

                        if (referenceCalculation.TarifPayment > 0)
                        {
                            var pay = referenceCalculation.TarifPayment;

                            if (pay >= referenceCalculation.TarifDebtPay)
                            {
                                pay -= referenceCalculation.TarifDebtPay;
                                referenceCalculation.TarifDebtPay = 0;
                                debtCalc.Remove(referenceCalculation);

                                while (pay > 0 && debtCalc.Any())
                                {
                                    if (pay >= debtCalc.Last().TarifDebtPay)
                                    {
                                        pay -= debtCalc.Last().TarifDebtPay;
                                        debtCalc.Last().TarifDebtPay = 0;
                                        debtCalc.Remove(debtCalc.Last());
                                    }
                                    else
                                    {
                                        debtCalc.Last().TarifDebtPay -= pay;
                                        pay = 0;
                                    }
                                }
                            }
                            else
                            {
                                debtCalc.Last().TarifDebtPay -= pay;
                            }
                        }
                    }
                }

                var firstDebtRefCalc = refCalc.FirstOrDefault(x => x.TarifDebtPay > 0);
                if (firstDebtRefCalc != null)
                {
                    if (firstDebtRefCalc == refCalc.First() && firstDebtRefCalc.TarifDebtPay == firstDebtRefCalc.TariffCharged)
                    {
                        lawsuit.DebtStartDate = openDate;
                        message = lawsuit.IsLimitationOfActions
                            ? "Задолженность начинается с даты применения срока исковой давности"
                            : "Задолженность начинается с даты открытия лицевого счёта.";
                        firstDebtRefCalc.Description = message;
                        lawsuit.Description = message;
                    }
                    else
                    {
                        ChargePeriod lastPaymentPeriod = PeriodDomain.Get(firstDebtRefCalc.PeriodId);

                        //Рассчет даты начала задолженности
                        var debtStartDate = CalculateDebtStartDate(
                            firstDebtRefCalc.BaseTariff,
                            roomArea,
                            firstDebtRefCalc.AreaShare,
                            firstDebtRefCalc.TarifDebtPay,
                            lastPaymentPeriod.StartDate);

                        // Фикс несостыковки дат открытия аккаунта и начала задолженности из-за округлений
                        // (дата начала может быть указана на 1 день раньше из-за десятых-сотых копеек долга,
                        // если такое происходит и итоговая дата раньше даты открытия - заменяем на дату открытия)
                        debtStartDate = debtStartDate < openDate ? openDate : debtStartDate;

                        lawsuit.DebtStartDate = debtStartDate;
                        lawsuit.DebtEndDate = endDate;

                        if (firstDebtRefCalc.TarifDebtPay == firstDebtRefCalc.TariffCharged)
                        {
                            message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                            firstDebtRefCalc.Description = message;
                            lawsuit.Description = message;
                        }
                        else
                        {
                            message = $"С {debtStartDate.ToShortDateString()} задолженность {firstDebtRefCalc.TarifDebt.ToString(CultureInfo.InvariantCulture)}.";
                            firstDebtRefCalc.Description = message;
                            lawsuit.Description = message;
                        }
                    }
                }

                foreach (var referenceCalculation in refCalc)
                {
                    RefCalcDomain.Save(referenceCalculation);
                }

                /*foreach (var referenceCalculation in refCalc)
                {
                    totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                    referenceCalculation.TarifDebt = totalDebt;

                    if (totalDebt > 0)
                    {
                        referenceCalculation.TarifDebtPay = referenceCalculation.TariffCharged;
                        debtCalc.Add(referenceCalculation);
                    }

                    if (referenceCalculation.TarifPayment > 0)
                    {
                        var pay = referenceCalculation.TarifPayment;
                        var j = debtCalc.Count - 2;
                        while (j > -1 && pay > 0)
                        {
                            if (pay >= debtCalc[j].TarifDebtPay)
                            {
                                pay -= debtCalc[j].TarifDebtPay;

                                var calc = refCalc.Where(x => x.PeriodId == debtCalc[j].PeriodId)
                                    .FirstOrDefault();

                                refCalc[refCalc.IndexOf(calc)].TarifDebtPay = 0;

                                debtCalc.RemoveAt(j);
                            }
                            else
                            {
                                debtCalc[j].TarifDebtPay -= pay;

                                var calc = refCalc.Where(x => x.PeriodId == debtCalc[j].PeriodId)
                                    .FirstOrDefault();

                                refCalc[refCalc.IndexOf(calc)].TarifDebtPay = debtCalc[j].TarifDebtPay;

                                pay = 0;
                            }
                            j--;
                        }
                        if (pay > 0)
                            if (pay > debtCalc[0].TarifDebtPay)
                            {
                                debtCalc.RemoveAt(0);

                                referenceCalculation.TarifDebtPay = 0;
                            }
                            else
                            {
                                debtCalc[0].TarifDebtPay -= pay;

                                referenceCalculation.TarifDebtPay = debtCalc[0].TarifDebtPay;
                            }

                    }
                    RefCalcDomain.Save(referenceCalculation);
                }

                debtCalc.OrderBy(x => x.PeriodId);

                if (debtCalc.IsNotEmpty())
                {
                    var referenceCalculation = debtCalc.First();

                    //Рассчет даты начала задолженности
                    ChargePeriod lastPaymentPeriod = this.PeriodDomain.Get(referenceCalculation.PeriodId);
                    debtStartDate = CalculateDebtStartDate(
                        referenceCalculation.BaseTariff,
                        roomArea,
                        referenceCalculation.AreaShare,
                        referenceCalculation.TarifDebtPay,
                        lastPaymentPeriod.StartDate);

                    // Фикс несостыковки дат открытия аккаунта и начала задолженности из-за округлений
                    // (дата начала может быть указана на 1 день раньше из-за десятых-сотых копеек долга,
                    // если такое происходит и итоговая дата раньше даты открытия - заменяем на дату открытия)
                    debtStartDate = debtStartDate < openDate ? openDate : debtStartDate;

                    lawsuit.DebtStartDate = lawsuit.DebtStartDate > debtStartDate ? debtStartDate : lawsuit.DebtStartDate;
                    lawsuit.DebtEndDate = lawsuit.DebtEndDate < endDate ? endDate : lawsuit.DebtEndDate;

                    if (message == "")
                    {
                        if (referenceCalculation.TarifDebtPay == referenceCalculation.TariffCharged)
                            message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                        else
                        {
                            message = $"С {debtStartDate.ToShortDateString()} задолженность {referenceCalculation.TarifDebtPay.ToString(CultureInfo.InvariantCulture)}.";
                            referenceCalculation.Description = message;
                            lawsuit.Description = message;
                        }
                    }
                    this.RefCalcDomain.Save(referenceCalculation);
                }*/

                //считаем эталонные пени

                foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
                {
                    var currentPeriod = chargePeriods.FirstOrDefault(x => x.Id == referenceCalculation.PeriodId.Id);
                    var penaltyPaymentsInPeriod = persAccAllPenaltyTransfers.Where(x => x.ChargePeriod.Id == referenceCalculation.PeriodId.Id).ToList();
                    if (penaltyPaymentsInPeriod.Count > 0)
                    {
                        referenceCalculation.PenaltyPayment = penaltyPaymentsInPeriod.Sum(x => x.Amount);
                        referenceCalculation.PenaltyPaymentDate = penaltyPaymentsInPeriod.Max(x => x.PaymentDate.ToString("dd.MM.yyyy"));
                    }
                    if (referenceCalculation.TarifDebtPay > 0)
                    {
                        ChargeMonthlyPenalty(referenceCalculation, referenceCalculation.TarifDebtPay, refundRateList, currentPeriod, docDate);
                    }
                }

                persAccPenaltyCharges = RefCalcDomain.GetAll().Where(x => x.Lawsuit == lawsuit).Sum(x => x.Penalties);

                var sumsBySummaries = GetSummarySums(claimWorkAccountDetail, lastPeriod);

                lawsuit.DebtBaseTariffSum += resultDebt < 0 ? 0 : resultDebt;
                lawsuit.DebtDecisionTariffSum += sumsBySummaries.decisionDebtSum < 0 ? 0 : sumsBySummaries.decisionDebtSum;
                lawsuit.DebtSum += resultDebt + sumsBySummaries.decisionDebtSum < 0 ? 0 : resultDebt + sumsBySummaries.decisionDebtSum;
                lawsuit.PenaltyDebt += (persAccPenaltyCharges - penaltyPayed);

            }

            LawsuitDomain.Update(lawsuit);
            if (hasCancelledCharges)
            {
                message += " Внимание!!!! на ЛС есть отмены начислений. Возможно списание исковой давности";
            }

            return new BaseDataResult(
                new
                {
                    lawsuit.DebtDecisionTariffSum,
                    lawsuit.DebtBaseTariffSum,
                    lawsuit.DebtSum,
                    lawsuit.Description,
                    dateStartDebt = lawsuit.DebtStartDate,
                    message,
                    lawsuit.DebtCalcMethod,
                    lawsuit.DebtEndDate,
                    PenaltyDebt = lawsuit.PenaltyDebt ?? 0
                });
        }

        private void ChargeMonthlyPenalty(LawsuitReferenceCalculation referenceCalculation, decimal debtBaseTariff, List<PaymentPenalties> refundRateList, ChargePeriod currentPeriod, DateTime docdate)
        {
            //дата начала отсчета пени - 10 число месяца, следующего за месяцем, в котором образовалась задолженность
            DateTime startDate = new DateTime();
            if (currentPeriod.StartDate.Month != 12)
                startDate = new DateTime(currentPeriod.StartDate.Year, currentPeriod.StartDate.Month + 1, 10);
            else
                startDate = new DateTime(currentPeriod.StartDate.Year + 1, 1, 10);

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
                        morDayCount += (rate.DateEnd - rate.DateStart).Value.Days;

                    if(payRate.Percentage == 0)
                        morDayCount += (payRate.DateEnd - payRate.DateStart).Value.Days;

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