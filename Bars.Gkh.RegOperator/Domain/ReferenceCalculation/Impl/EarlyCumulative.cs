namespace Bars.Gkh.RegOperator.Domain.ReferenceCalculation.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Domain.ReferenceCalculation;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    public class EarlyCumulative : ReferenceCalculationService
    {
        public override DebtCalc DebtCalc => DebtCalc.Early;

        public override PenaltyCharge PenaltyCharge => PenaltyCharge.CumulativeTotal;

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
                    ? lawsuit.IsLimitationOfActions
                        ? "Задолженность начинается с даты применения срока исковой давности"
                        : "Задолженность начинается с даты открытия лицевого счёта."
                    : "";
                foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
                {
                    remainingPayment += referenceCalculation.TariffCharged;
                    totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                    referenceCalculation.TarifDebt = totalDebt;
                    if (remainingPayment >= 0 && !debtStarted)
                    {
                        ChargePeriod lastPaymentPeriod = PeriodDomain.Get(referenceCalculation.PeriodId);
                        if (totalPayment == 0)
                        {
                            lawsuit.DebtStartDate = debtStartDate;
                        }
                        else
                        {
                            //Рассчет даты начала задолженности
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

                            lawsuit.DebtStartDate = lawsuit.DebtStartDate > debtStartDate ? debtStartDate : lawsuit.DebtStartDate;
                            lawsuit.DebtEndDate = lawsuit.DebtEndDate < endDate ? endDate : lawsuit.DebtEndDate;
                        }

                        if (message == "")
                        {
                            if (remainingPayment == 0)
                                message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
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
                                lawsuit.Description = message;
                            }
                        }
                        debtStarted = true;
                    }
                    RefCalcDomain.Save(referenceCalculation);
                }

                //считаем эталонные пени
                var refcalcList = RefCalcDomain.GetAll().Where(x => x.Lawsuit == lawsuit).OrderBy(x => x.PeriodId).ToList();

                foreach (LawsuitReferenceCalculation referenceCalculation in refcalcList)
                {
                    var currentPeriod = chargePeriods.FirstOrDefault(x => x.Id == referenceCalculation.PeriodId.Id);
                    var penaltyPaymentsInPeriod = persAccAllPenaltyTransfers.Where(x => x.ChargePeriod.Id == referenceCalculation.PeriodId.Id).ToList();
                    if (penaltyPaymentsInPeriod.Count > 0)
                    {
                        referenceCalculation.PenaltyPayment = penaltyPaymentsInPeriod.Sum(x => x.Amount);
                        referenceCalculation.PenaltyPaymentDate = penaltyPaymentsInPeriod.Max(x => x.PaymentDate.ToString("dd.MM.yyyy"));
                    }
                    if (referenceCalculation.TarifDebt > 0)
                    {
                        ChargePenalty(referenceCalculation, referenceCalculation.TarifDebt, refundRateList, currentPeriod, docDate);
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

        private void ChargePenalty(LawsuitReferenceCalculation referenceCalculation, decimal debtBaseTariff, List<PaymentPenalties> refundRateList, ChargePeriod currentPeriod, DateTime docdate)
        {
            var rateList = refundRateList.OrderByDescending(x => x.DateStart)
                .Where(x => x.DateStart <= docdate)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= docdate).OrderBy(x => x.DateStart).FirstOrDefault();
            var realRate = refundRateList.OrderByDescending(x => x.DateStart)
                .Where(x => x.DateStart <= currentPeriod.StartDate)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= currentPeriod.StartDate).OrderBy(x => x.DateStart).FirstOrDefault();
            if (rateList != null && realRate != null)
            {
                var daysCount = (currentPeriod.EndDate.Value - currentPeriod.StartDate).TotalDays + 1;
                var bankRate = realRate.Percentage > 0 ? rateList.Percentage / 300 : 0;
                var penalty = debtBaseTariff * (int)daysCount * bankRate / 100;
                referenceCalculation.Penalties = Decimal.Round(penalty, 2);
                if (bankRate == 0)
                {
                    referenceCalculation.AccrualPenalties = "Мораторий на начисление пени, или пени не начислялась";
                }
                else
                {
                    referenceCalculation.AccrualPenalties = $"Задолженность за {currentPeriod.Name} составляет {decimal.Round(debtBaseTariff, 2)} руб. Ставка {decimal.Round(rateList.Percentage, 2)}%. Количество дней просрочки {(int)daysCount}.";
                    referenceCalculation.AccrualPenaltiesFormula = $"{decimal.Round(debtBaseTariff, 2)} x {(int)daysCount} x 1/300 x {decimal.Round(rateList.Percentage, 2)}%.";
                }
            }
            else
            {
                referenceCalculation.AccrualPenalties = "Мораторий на начисление пени, или пени не начислялась";
            }

            RefCalcDomain.Update(referenceCalculation);
        }
    }
}
