namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.ReferenceCalculation.Impl
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

    public class DebtorLateCumylative : DebtorReferenceCalculationService
    {
        public override DebtCalc DebtCalc => DebtCalc.Late;

        public override PenaltyCharge PenaltyCharge => PenaltyCharge.CumulativeTotal;

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

            //проставляем оплаты
            foreach (var сhargeTransfer in persAccAllChargeTransfers)
            {
                var previousPeriod = chargePeriods.Where(x => x.Id < сhargeTransfer.ChargePeriod.Id).LastOrDefault() ??
                    chargePeriods.Where(x => x.Id == сhargeTransfer.ChargePeriod.Id).FirstOrDefault();

                foreach (var referenceCalculation in refCalc)
                {
                    if (referenceCalculation.PeriodId == previousPeriod.Id)
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
            DateTime debtStartDate = openDate;
            message = totalPayment == 0
                ? debtor.UseCustomDate
                    ? "Задолженность начинается с даты применения срока исковой давности"
                    : "Задолженность начинается с даты открытия лицевого счёта."
                : "";

            var debtCalc = new List<DebtorReferenceCalculation>();
            var debtStarted = false;

            foreach (var referenceCalculation in refCalc)
            {
                totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                referenceCalculation.TarifDebt = totalDebt;

                if (totalDebt > 0)
                {
                    if (!debtStarted)
                    {
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

                        debtor.DebtStartDate = debtor.DebtStartDate > debtStartDate ? debtStartDate : debtor.DebtStartDate;
                        debtor.DebtEndDate = debtor.DebtEndDate < endDate ? endDate : debtor.DebtEndDate;

                        if (message == "")
                        {
                            if (referenceCalculation.TarifDebt == referenceCalculation.TariffCharged)
                            {
                                message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                            }
                            else
                            {
                                message = $"С {debtStartDate.ToShortDateString()} задолженность {referenceCalculation.TarifDebt.ToString(CultureInfo.InvariantCulture)}.";
                                referenceCalculation.Description = message;
                            }
                        }

                        debtStarted = true;
                    }

                    referenceCalculation.TarifDebtPay = referenceCalculation.TariffCharged;
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

            foreach (var referenceCalculation in refCalc)
            {
                RefCalcDomain.Save(referenceCalculation);
            }



            //считаем эталонные пени

            foreach (DebtorReferenceCalculation referenceCalculation in refCalc)
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
                    ChargePenalty(referenceCalculation, referenceCalculation.TarifDebt, refundRateList, currentPeriod, docDate);
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

        private void ChargePenalty(DebtorReferenceCalculation referenceCalculation, decimal debtBaseTariff, List<PaymentPenalties> refundRateList, ChargePeriod currentPeriod, DateTime docdate)
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