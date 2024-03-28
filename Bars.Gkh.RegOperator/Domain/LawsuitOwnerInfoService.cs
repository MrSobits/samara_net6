namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor;

    using Castle.Windsor;

    using Entities.Owner;

    using Gkh.Modules.ClaimWork.Entities;

    using Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using System.Globalization;

    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.RegOperator.Domain.ReferenceCalculation;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Сервис "Собственник в исковом заявлении"
    /// </summary>
    public class LawsuitOwnerInfoService : ILawsuitOwnerInfoService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<LawsuitOwnerInfo> LawsuitOwnerInfoDomain { get; set; }

        public IDomainService<PersonalAccountOwnerInformation> PersonalAccountOwnerInformationDomain { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<Lawsuit> LawsuitDomain { get; set; }

        public IDomainService<ClaimWorkAccountDetail> AccountDetailDomain { get; set; }

        public IDebtPeriodCalcService DebtPeriodCalcService { get; set; }

        private IDomainService<ChargePeriod> PeriodDomain { get; set; }

        private IDomainService<LawsuitReferenceCalculation> RefCalcDomain { get; set; }
        
        private IGkhConfigProvider ConfigProvider { get; set; }
        
        
        public IDataResult CalcLegalWithReferenceCalc(long docId, List<long> transfers)
        {
            ConfigProvider = this.Container.Resolve<IGkhConfigProvider>();
            var paymentConfig = this.ConfigProvider.Get<ClaimWorkConfig>().Common.General.DebtCalc;
            var penaltyConfig = this.ConfigProvider.Get<ClaimWorkConfig>().Common.General.PenaltyCharge;

            Lawsuit lawsuit = this.LawsuitDomain.Get(docId);
            List<ClaimWorkAccountDetail> claimWorkAccountDetail = this.AccountDetailDomain.GetAll()
                .Where(x => x.ClaimWork == lawsuit.ClaimWork)
                .ToList();

            var allCalcs = this.Container.ResolveAll<IReferenceCalculationService>();
            var calcer = allCalcs.FirstOrDefault(x => x.DebtCalc == paymentConfig && x.PenaltyCharge == penaltyConfig);
            return calcer.CalculateReferencePayments(lawsuit, claimWorkAccountDetail, transfers);
        }
        
        public IDataResult GetDebtStartDateCalculate(BaseParams baseParams)
        {
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();
            //Проверяем документ
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }
            this.ClearReferenceCalculation(docId);

            string message = "";

            DocumentClw documentClw = this.Container.Resolve<IDomainService<DocumentClw>>().Get(docId);
            try
            {
                Lawsuit lawsuitCO = this.LawsuitDomain.GetAll()
                    .FirstOrDefault(x => x.ClaimWork == documentClw.ClaimWork && x.DocumentType == Gkh.Modules.ClaimWork.Enums.ClaimWorkDocumentType.CourtOrderClaim);
                Lawsuit lawsuit = this.LawsuitDomain.Get(docId);

                RefCalcDomain.GetAll()
                    .Where(x => x.Lawsuit == lawsuitCO).ToList()
                    .ForEach(x =>
                    {
                        LawsuitReferenceCalculation lrc = new LawsuitReferenceCalculation
                        {
                            Lawsuit = lawsuit,
                            AccountNumber = x.AccountNumber,
                            AreaShare = x.AreaShare,
                            BaseTariff = x.BaseTariff,
                            Description = x.Description,
                            PaymentDate = x.PaymentDate,
                            PeriodId = x.PeriodId,
                            PersonalAccountId = x.PersonalAccountId,
                            RoomArea = x.RoomArea,
                            TarifDebt = x.TarifDebt,
                            TariffCharged = x.TariffCharged,
                            TarifPayment = x.TarifPayment,
                            ObjectCreateDate = x.ObjectCreateDate,
                            ObjectEditDate = x.ObjectEditDate,
                            ObjectVersion = x.ObjectVersion
                        };
                        RefCalcDomain.Save(lrc);

                    });

                return new BaseDataResult(
                new
                {
                    lawsuitCO.DebtDecisionTariffSum,
                    lawsuitCO.DebtBaseTariffSum,
                    lawsuitCO.DebtSum,
                    lawsuitCO.Description,
                    lawsuitCO.DebtCalcMethod,
                    dateStartDebt = lawsuitCO.DebtStartDate,
                    lawsuitCO.DebtEndDate,
                    lawsuitCO.PenaltyDebt,
                    message
                });
            }
            catch (Exception e)
            {
                message = "При переносе данных произошла ошибка: " + e.Message;
                return new BaseDataResult(
              new
              {

                  DebtDecisionTariffSum = 0,
                  DebtBaseTariffSum = 0,
                  DebtSum = 0,
                  Description = 0,
                  DebtCalcMethod = 10,
                  dateStartDebt = DateTime.MinValue,
                  DebtEndDate = DateTime.MinValue,
                  PenaltyDebt = 0,
                  message
              });
            }

        }
    

        /// <summary>
        /// Получение по Лс собственников для искового заявления
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Собственники в исковом заявлении</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var lawsuitId = baseParams.Params.GetAs<long>("Lawsuit");
            var documentDate = baseParams.Params.GetAs("DocumentDate", DateTime.MinValue);

            var loadParam = baseParams.GetLoadParam();

            var claimWorkIds = this.LawsuitDomain.GetAll()
                .Where(x => x.Id == lawsuitId)
                .Select(x => x.ClaimWork.Id);

            var accountDetailQuery = this.AccountDetailDomain.GetAll()
                .Where(x => claimWorkIds.Contains(x.ClaimWork.Id));

            var data = this.PersonalAccountOwnerInformationDomain.GetAll()
                .Where(x => accountDetailQuery.Any(y => y.PersonalAccount.Id == x.BasePersonalAccount.Id))
                .Where(x => !x.EndDate.HasValue || x.EndDate.Value.Date >= documentDate.Date)
                .Select(x => new
                {
                    x.Id,
                    x.Owner,
                    x.AreaShare,
                    x.EndDate
                });

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }

        /// <inheritdoc />
        public IDataResult DebtCalculate(BaseParams baseParams)
        {
            var selectedIds = baseParams.Params.GetAs("ids", new long[0]);
            var lawsuitOwnerInfoList = this.LawsuitOwnerInfoDomain.GetAll()
                .WhereContains(x => x.Id, selectedIds)
                .ToList();

            if (lawsuitOwnerInfoList.IsEmpty())
            {
                return BaseDataResult.Error("Не найдена информация о переданных собственниках");
            }

            this.DebtPeriodCalcService.Calculate(lawsuitOwnerInfoList.Select(x => x.PersonalAccount.Id).Distinct());

            foreach (var lawsuitOwnerInfo in lawsuitOwnerInfoList)
            {
                this.CalculateProcess(lawsuitOwnerInfo);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, lawsuitOwnerInfoList, useStatelessSession: true);

            return new BaseDataResult(selectedIds);
        }

        public IDataResult DebtStartDateCalculate(BaseParams baseParams)
        {
            var documentClwService = this.Container.Resolve<IDomainService<DocumentClw>>();
            var paysizeRecordDomain = Container.ResolveDomain<PaysizeRecord>();
            var decisionDomain = Container.ResolveDomain<MonthlyFeeAmountDecHistory>();
            var entityLogLightDomain = this.Container.ResolveDomain<EntityLogLight>();
            var personalAccountPaymentTransferDomain = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>();
            var lawsuitRepository = this.Container.ResolveRepository(typeof(Lawsuit));
            this.PeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();

            try
            {
                //Проверяем документ
                var docId = baseParams.Params.GetAs<long>("docId");
                if (docId == 0)
                {
                    return BaseDataResult.Error("Не найдена информация о документе");
                }

                //Удаляем старый рассчет
                this.ClearReferenceCalculation(docId);
                DocumentClw documentClw = documentClwService.Get(docId);
                DateTime docDate = documentClw.DocumentDate ?? DateTime.MinValue;

                ClaimWorkAccountDetail claimWorkAccountDetail = this.AccountDetailDomain.GetAll()
                    .FirstOrDefault(x => x.ClaimWork == documentClw.ClaimWork);

                ChargePeriod lastPeriod = this.GetLastReferenceCalculationPeriod(docDate);

                DateTime openDate = claimWorkAccountDetail.PersonalAccount.OpenDate;
                DateTime endDate = lastPeriod.EndDate ?? DateTime.MinValue;

                var chargePeriods = this.PeriodDomain.GetAll()
                    .Where(x => x.EndDate >= openDate && x.EndDate <= endDate)
                    .OrderBy(x => x.StartDate)
                    .ToList();

                decimal roomArea = claimWorkAccountDetail.PersonalAccount.Room.Area;
                decimal areaShare = claimWorkAccountDetail.PersonalAccount.AreaShare;
                string accountNumber = claimWorkAccountDetail.PersonalAccount.PersonalAccountNum;
                Lawsuit lawsuit = this.LawsuitDomain.Get(docId);

                //получаем размер взноса за кр
                var paysize = paysizeRecordDomain.GetAll()
                    .Where(x => x.Municipality != null)
                    .Where(x => x.Value != null)
                    .Where(x => x.Municipality == claimWorkAccountDetail.PersonalAccount.Room.RealityObject.Municipality)
                    .ToList();

                Dictionary<ChargePeriod, decimal> paysizeByPeriod = new Dictionary<ChargePeriod, decimal>();

                var decisions = decisionDomain.GetAll()
                    .Where(x => x.Protocol.RealityObject == claimWorkAccountDetail.PersonalAccount.Room.RealityObject)
                    .ToList();

                chargePeriods.ForEach(x =>
                {
                    var pmf = decisions.SelectMany(y => y.Decision)
                        .Where(y => y.Value > 0 && y.From == x.StartDate && (!y.To.HasValue || y.To.Value >= x.EndDate))
                        .FirstOrDefault();
                    if (pmf != null)
                    {
                        paysizeByPeriod.Add(x, pmf.Value);
                    }

                    if (!paysizeByPeriod.ContainsKey(x))
                    {
                        foreach (PaysizeRecord psr in paysize)
                        {
                            if (psr.Paysize.DateStart <= x.StartDate && (psr.Paysize.DateEnd >= x.EndDate || psr.Paysize.DateEnd == null))
                            {
                                decimal? paySizeVal = psr.Value;
                                paysizeByPeriod.Add(x, paySizeVal.HasValue ? paySizeVal.Value : 0);
                            }
                        }
                    }
                });

                var firstPeriod = true;
                var refCalc = new List<LawsuitReferenceCalculation>();

                var allHistory = entityLogLightDomain.GetAll()
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
                    var share = filteredHistory.Where(x => x.DateActualChange <= period.EndDate).OrderByDescending(x => x.DateApplied).ThenByDescending(x => x.Id).FirstOrDefault();
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
                                ? LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod, openDate)
                                : LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod)
                        });
                    firstPeriod = false;
                }

                //Все трансферы
                var persAccAllTransfers = personalAccountPaymentTransferDomain.GetAll()
                    .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                    .Where(x => x.Operation.IsCancelled != true)
                    .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации

                //Оплаты
                var persAccAllChargeTransfers = persAccAllTransfers.Where(
                    x => x.Reason == "Оплата по базовому тарифу" ||
                        x.Reason == "Оплата пени" ||
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
                        }).ToList();

                //Отмены оплат
                var persAccAllReturnTransfers = persAccAllTransfers.Where(
                    x => x.Reason == "Отмена оплаты по базовому тарифу" ||
                        x.Reason == "Отмена оплаты по тарифу решения" ||
                        x.Reason == "Отмена оплаты пени" ||
                        x.Reason == "Возврат взносов на КР").ToList();
                decimal totalCharged = refCalc.Sum(x => x.TariffCharged);
                decimal totalPayment = persAccAllChargeTransfers.Sum(x => x.Amount);
                decimal resultDebt = totalCharged - totalPayment;

                //Расставляем даты и оплаты по месяцам
                persAccAllChargeTransfers = persAccAllChargeTransfers.OrderBy(x => x.Id).ToList();
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
                string message = totalPayment == 0 ? "Нет оплат. Задолженность начинается с даты открытия лицевого счёта." : "";
                foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
                {
                    remainingPayment = remainingPayment + referenceCalculation.TariffCharged;
                    totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                    referenceCalculation.TarifDebt = totalDebt;
                    if (remainingPayment >= 0 && !debtStarted)
                    {
                        //Рассчет даты начала задолженности
                        debtStartDate = LawsuitOwnerInfoService.CalculateDebtStartDate(
                            referenceCalculation.BaseTariff,
                            //  LawsuitOwnerInfoService.GetTariff(lastPaymentPeriod.StartDate),
                            roomArea,
                            referenceCalculation.AreaShare,
                            remainingPayment,
                            referenceCalculation.PeriodId.StartDate);

                        // Фикс несостыковки дат открытия аккаунта и начала задолженности из-за округлений
                        // (дата начала может быть указана на 1 день раньше из-за десятых-сотых копеек долга,
                        // если такое происходит и итоговая дата раньше даты открытия - заменяем на дату открытия)
                        debtStartDate = debtStartDate < openDate ? openDate : debtStartDate;

                        lawsuit.DebtStartDate = debtStartDate;
                        lawsuit.DebtEndDate = endDate;

                        if (message == "")
                        {
                            if (remainingPayment == 0)
                                message = $"Нет остаточных оплат за месяц. Задолженность начинается с {debtStartDate.ToShortDateString()}";
                            else
                            {
                                switch (debtStartDate.Day)
                                {
                                    //Разный текст примечания в зависимости от дня начала задолженности
                                    case 1:
                                        message = $"За предыдущий месяц переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString()}.";
                                        break;
                                    case 2:
                                        message = $"За {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                        break;
                                    default:
                                        message =
                                            $"С {referenceCalculation.PeriodId.StartDate.ToShortDateString()} " +
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

                    this.RefCalcDomain.Save(referenceCalculation);
                }

                //TODO: Учет тарифа решения
                lawsuit.DebtBaseTariffSum = resultDebt;
                lawsuit.DebtDecisionTariffSum = 0;
                lawsuit.DebtSum = resultDebt;

                lawsuitRepository.Evict(lawsuit);
                this.Container.Release(lawsuitRepository);
                //this.LawsuitDomain.Update(lawsuit);

                return new BaseDataResult(
                    new
                    {
                        lawsuit.DebtDecisionTariffSum,
                        lawsuit.DebtBaseTariffSum,
                        lawsuit.DebtSum,
                        Description = lawsuit.Description,
                        dateStartDebt = debtStartDate,
                        message,
                        lawsuit.DebtCalcMethod,
                        lawsuit.DebtEndDate
                    });
            }
            finally
            {
                this.Container.Release(documentClwService);
                this.Container.Release(paysizeRecordDomain);
                this.Container.Release(decisionDomain);
                this.Container.Release(entityLogLightDomain);
                this.Container.Release(personalAccountPaymentTransferDomain);
                this.Container.Release(lawsuitRepository);
            }
        }

        private void ClearReferenceCalculation(long docId)
        {
            var lawsuitReferenceCalculationDomain = this.Container.Resolve<IDomainService<LawsuitReferenceCalculation>>();
            var lawsuitDomain = this.Container.Resolve<IDomainService<Lawsuit>>();
            try
            {
                long lawsuitId = lawsuitDomain.Get(docId).Id;

                var currentCalc = lawsuitReferenceCalculationDomain.GetAll()
                    .Where(x => x.Lawsuit.Id == lawsuitId)
                    .Select(x => x.Id).ToList();

                foreach (long calcId in currentCalc)
                {
                    lawsuitReferenceCalculationDomain.Delete(calcId);
                }
            }
            finally
            {
                this.Container.Release(lawsuitReferenceCalculationDomain);
                this.Container.Release(lawsuitDomain);
            }
        }

        private ChargePeriod GetLastReferenceCalculationPeriod(DateTime documentDate)
        {
            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
            try
            {
                var lastPeriod = new ChargePeriod();

                if (documentDate != DateTime.MinValue && documentDate.Day >= 25)
                {
                    lastPeriod = periodDomain.GetAll()
                        .Where(x => x.EndDate.HasValue && x.EndDate < documentDate)
                        .OrderByDescending(x => x.StartDate)
                        .FirstOrDefault();
                }
                else if (documentDate != DateTime.MinValue)
                {
                    lastPeriod = periodDomain.GetAll()
                        .Where(x => x.EndDate.HasValue && x.EndDate < documentDate.AddMonths(-1))
                        .OrderByDescending(x => x.StartDate)
                        .FirstOrDefault();
                }

                this.Container.Release(periodDomain);
                return lastPeriod;
            }
            finally
            {
                this.Container.Release(periodDomain);
            }
        }

        private static decimal CalculateMonthCharge(decimal tariff, decimal area, decimal share, DateTime openDate = default(DateTime))
        {
            if (openDate == default(DateTime))
            {
                return (area * share * tariff).RoundDecimal(2);
            }

            int daysInMonth = DateTime.DaysInMonth(openDate.Year, openDate.Month);
            int daysCounted = (new DateTime(openDate.AddMonths(1).Year, openDate.AddMonths(1).Month, 1) - openDate).Days;
            return (area * share * tariff * decimal.Divide(daysCounted, daysInMonth)).RoundDecimal(2);
        }

        private static DateTime CalculateDebtStartDate(decimal tariff, decimal area, decimal share, decimal debt, DateTime month)
        {
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            decimal totalPayment = tariff * area * share;
            decimal day = 0;
            if (totalPayment > 0)
            {
                day = (totalPayment - debt) * daysInMonth / totalPayment;
            }
            else
            {
                day = 0;
            }

            DateTime resutDate = new DateTime(month.Year, month.Month, 1).AddDays((int)day);
            return resutDate;
        }

        private void CalculateProcess(LawsuitOwnerInfo lawsuitOwnerInfo)
        {
            this.SetDebtSum(lawsuitOwnerInfo);

            lawsuitOwnerInfo.DebtBaseTariffSum = this.Round(lawsuitOwnerInfo.DebtBaseTariffSum * lawsuitOwnerInfo.AreaShare);
            lawsuitOwnerInfo.DebtDecisionTariffSum = this.Round(lawsuitOwnerInfo.DebtDecisionTariffSum * lawsuitOwnerInfo.AreaShare);
            lawsuitOwnerInfo.PenaltyDebt = this.Round(lawsuitOwnerInfo.PenaltyDebt * lawsuitOwnerInfo.AreaShare);
        }

        private void SetDebtSum(LawsuitOwnerInfo lawsuitOwnerInfo)
        {
            List<DebtPeriodInfo> sumInfoList;
            if (!this.DebtPeriodCalcService.DebtDict.TryGetValue(lawsuitOwnerInfo.PersonalAccount.Id, out sumInfoList))
            {
                return;
            }

            var oldDebt = sumInfoList
                .FirstOrDefault(x => x.Period.Id == lawsuitOwnerInfo.StartPeriod.Id);

            var newDebt = sumInfoList
                .WhereIf(lawsuitOwnerInfo.EndPeriod.IsClosed, x => x.Period.StartDate > lawsuitOwnerInfo.EndPeriod.EndDate)
                .WhereIf(lawsuitOwnerInfo.EndPeriod.IsClosed, x => x.Period.StartDate < lawsuitOwnerInfo.EndPeriod.EndDate.Value.AddDays(3))
                .WhereIf(!lawsuitOwnerInfo.EndPeriod.IsClosed, x => x.Period.Id == lawsuitOwnerInfo.EndPeriod.Id)
                .FirstOrDefault();

            lawsuitOwnerInfo.DebtBaseTariffSum = newDebt.BaseTariffSum - oldDebt.BaseTariffSum;
            lawsuitOwnerInfo.DebtDecisionTariffSum = newDebt.DecisionTariffSum - oldDebt.DecisionTariffSum;
            lawsuitOwnerInfo.PenaltyDebt = newDebt.PenaltySum - oldDebt.PenaltySum;
        }

        private decimal Round(decimal val) => Math.Truncate(val * 100) / 100;
    }
}