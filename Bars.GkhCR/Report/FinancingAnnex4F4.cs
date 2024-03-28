namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Приложение 4 к отчету о расходовании средств Фонда (форма 4)
    /// </summary>
    public class FinancingAnnex4F4 : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;
        private DateTime reportDate = DateTime.MinValue;
        private TypeFinanceGroup typeFinanceGroup;

        public FinancingAnnex4F4()
            : base(new ReportTemplateBinary(Properties.Resources.Financing_Annex4_F4))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.FinancingAnnex4F4"; }
        }

        public override string Desciption
        {
            get { return "Приложение 4 к отчету о расходовании средств Фонда (форма 4)"; }
        }

        public override string GroupName
        {
            get { return "Финансирование"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.FinancingAnnex4F4"; }
        }

        public override string Name
        {
            get { return "Приложение 4 к отчету о расходовании средств Фонда (форма 4)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();
            this.typeFinanceGroup = baseParams.Params["typeFinanceGroup"].ToInt() != 0 ? (TypeFinanceGroup)baseParams.Params["typeFinanceGroup"].ToInt() : TypeFinanceGroup.Other;
            var dateReport = baseParams.Params["reportDate"].ToDateTime();
            this.reportDate = dateReport != DateTime.MinValue ? dateReport : DateTime.Now;
            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                                  ? baseParams.Params["municipalityIds"].ToString()
                                  : string.Empty;
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var startMonth = this.reportDate.AddDays(-(reportDate.Day - 1));
            var endMonth = startMonth.AddMonths(1).AddDays(-1);
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Get(programCrId);
            var realObjIdByPersonalAccount = this.Container.Resolve<IDomainService<PersonalAccount>>()
                .GetAll()
                .Where(x => x.FinanceGroup == typeFinanceGroup && x.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => x.ObjectCr.RealityObject.Id)
                .ToList();

            var bankStatementOutList = this.Container.Resolve<IDomainService<PaymentOrderOut>>()
                .GetAll()
                .Where(x => x.TypePaymentOrder == TypePaymentOrder.Out && x.PayerContragent != null && x.ReceiverContragent != null && x.BankStatement.ObjectCr != null)
                .Where(x => x.DocumentDate >= startMonth && x.DocumentDate <= endMonth)
                .Where(x => x.BankStatement.TypeFinanceGroup == typeFinanceGroup)
                .Where(x => x.BankStatement.Period.Id == programCr.Period.Id)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.BankStatement.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                                  {
                                      RealityObjectId = x.BankStatement.ObjectCr.RealityObject.Id,
                                      x.BidDate,
                                      x.BidNum,
                                      x.DocumentDate,
                                      x.DocumentNum,
                                      SumIn = (decimal?)0,
                                      SumOut = x.Sum,
                                      RedirectFundsIn = (decimal?)0,
                                      RedirectFundsOut = x.RedirectFunds,
                                      x.RepeatSend,
                                      x.PayPurpose,
                                      x.TypeFinanceSource,
                                      PayerContragentName = x.PayerContragent.Name,
                                      ReceiverContragentName = x.ReceiverContragent.Name,
                                      BankStatementNum = x.BankStatement.DocumentNum,
                                      MunicipalityName = x.BankStatement.ObjectCr.RealityObject.Municipality.Name,
                                      x.BankStatement.ObjectCr.RealityObject.Address,
                                      MunicipalityGroup = x.BankStatement.ObjectCr.RealityObject.Municipality.Group
                                  })
                    .OrderBy(x => x.MunicipalityName)
                    .ThenBy(x => x.Address)
                    .ThenBy(x => x.DocumentDate)
                    .ThenBy(x => x.DocumentNum)
                    .AsEnumerable()
                    .Where(x => realObjIdByPersonalAccount.Contains(x.RealityObjectId))
                    .ToList();

            var bankStatementInList = this.Container.Resolve<IDomainService<PaymentOrderIn>>()
                .GetAll()
                .Where(x => x.TypePaymentOrder == TypePaymentOrder.In && x.PayerContragent != null && x.ReceiverContragent != null && x.BankStatement.ObjectCr != null)
                .Where(x => x.DocumentDate >= startMonth && x.DocumentDate <= endMonth)
                .Where(x => x.BankStatement.TypeFinanceGroup == typeFinanceGroup)
                .Where(x => x.BankStatement.Period.Id == programCr.Period.Id)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.BankStatement.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                                  {
                                      RealityObjectId = x.BankStatement.ObjectCr.RealityObject.Id,
                                      x.BidDate,
                                      x.BidNum,
                                      x.DocumentDate,
                                      x.DocumentNum,
                                      SumIn = x.Sum,
                                      SumOut = (decimal?)0,
                                      RedirectFundsIn = x.RedirectFunds,
                                      RedirectFundsOut = (decimal?)0,
                                      x.RepeatSend,
                                      x.PayPurpose,
                                      x.TypeFinanceSource,
                                      PayerContragentName = x.PayerContragent.Name,
                                      ReceiverContragentName = x.ReceiverContragent.Name,
                                      BankStatementNum = x.BankStatement.DocumentNum,
                                      MunicipalityName = x.BankStatement.ObjectCr.RealityObject.Municipality.Name,
                                      x.BankStatement.ObjectCr.RealityObject.Address,
                                      MunicipalityGroup = x.BankStatement.ObjectCr.RealityObject.Municipality.Group
                                  })
                    .OrderBy(x => x.MunicipalityName)
                    .ThenBy(x => x.Address)
                    .ThenBy(x => x.DocumentDate)
                    .ThenBy(x => x.DocumentNum)
                    .AsEnumerable()
                    .Where(x => realObjIdByPersonalAccount.Contains(x.RealityObjectId))
                    .ToList();

            var result = bankStatementOutList;
            result.AddRange(bankStatementInList);
            var dataByMunicipality = result.Where(x => x.SumIn <= 0 && x.TypeFinanceSource != TypeFinanceSource.OccupantFunds)
                            .GroupBy(x => x.MunicipalityName)
                            .ToDictionary(x => x.Key, y => y.ToList());

            reportParams.SimpleReportParams["год"] = reportDate.ToString("yyyy");
            reportParams.SimpleReportParams["месяц"] = reportDate.ToString("MMMM");

            var municipalityNum = 0;
            var bidNum = 0;

            var grandTotalSumRedirect = 0m;
            var grandTotalSumReturn = 0m;
            var grandTotalSum = 0m;
            var grandTotalSumReturnBuilder = 0m;

            foreach (var municipality in dataByMunicipality)
            {
                var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияРайоны"); 
                sectionMu.ДобавитьСтроку();

                sectionMu["НомРайона"] = ++municipalityNum;
                sectionMu["МунРайон"] = municipality.Key;

                var dataByManorg = municipality.Value.GroupBy(x => x.SumOut == 0 ? x.ReceiverContragentName : x.PayerContragentName).ToDictionary(x => x.Key, y => y.ToList());
                var totalSumRedirect = 0m;
                var totalSumReturn = 0m;
                var totalSum = 0m;
                var totalSumReturnBuilder = 0m;

                foreach (var manorg in dataByManorg)
                {
                    foreach (var document in manorg.Value)
                    {
                        if (document.SumOut == 0m)
                        {
                            if (document.TypeFinanceSource == TypeFinanceSource.PlaceBudgetFunds
                                || document.TypeFinanceSource == TypeFinanceSource.FederalFunds
                                || document.TypeFinanceSource == TypeFinanceSource.SubjectBudgetFunds)
                            {
                                var sumByAbs = Math.Abs(document.SumIn.HasValue ? document.SumIn.Value : 0m);
                                var sectionDocReturn = sectionMu.ДобавитьСекцию("СекцияУпрОрг");
                                sectionDocReturn.ДобавитьСтроку();

                                sectionDocReturn["УпрОрг"] = manorg.Key;
                                sectionDocReturn["НомерВыписки"] = ++bidNum;
                                sectionDocReturn["Адрес"] = document.Address;
                                sectionDocReturn["ДатаНомерВозврат"] = string.Format("{0} от {1}", document.BidNum, document.BidDate.HasValue ? document.BidDate.Value.ToShortDateString() : string.Empty);
                                sectionDocReturn["СуммаВозврат"] = sumByAbs == 0m ? string.Empty : sumByAbs.ToStr();

                                totalSumReturn += sumByAbs;
                            }

                            continue;
                        }

                        var sectionDoc = sectionMu.ДобавитьСекцию("СекцияУпрОрг");
                        sectionDoc.ДобавитьСтроку();

                        sectionDoc["УпрОрг"] = manorg.Key;
                        sectionDoc["НомерВыписки"] = ++bidNum;
                        sectionDoc["Адрес"] = document.Address;

                        if (document.SumOut > 0)
                        {
                            sectionDoc["ДатаНомер"] = string.Format("{0} от {1}", document.DocumentNum, document.DocumentDate.HasValue ? document.DocumentDate.Value.ToShortDateString() : string.Empty);
                            sectionDoc["Сумма"] = document.SumOut == 0 ? string.Empty : document.SumOut.ToStr();
                            sectionDoc["СуммаПовторно"] = document.RedirectFundsOut == 0 ? string.Empty : document.RedirectFundsOut.ToStr();

                            totalSum += document.SumOut.HasValue ? document.SumOut.Value : 0m;
                            totalSumRedirect += document.RedirectFundsOut.HasValue ? document.RedirectFundsOut.Value : 0m;

                            continue;
                        }

                        var redirectSum = Math.Abs(document.SumOut.HasValue ? document.SumOut.Value : 0m);

                        sectionDoc["ДатаНомерВозврат"] = string.Format("{0} от {1}", document.DocumentNum, document.DocumentDate.HasValue ? document.DocumentDate.Value.ToShortDateString() : string.Empty);
                        sectionDoc["СуммаВозврат1"] = redirectSum == 0m ? string.Empty : redirectSum.ToStr();

                        totalSumReturnBuilder += redirectSum;
                    }
                }
                sectionMu["СуммаВсего"] = totalSum == 0 ? string.Empty : totalSum.ToStr();
                sectionMu["СуммаПовторноВсего"] = totalSumRedirect == 0 ? string.Empty : totalSumRedirect.ToStr();
                sectionMu["СуммаВозвратВсего"] = totalSumReturn == 0 ? string.Empty : totalSumReturn.ToStr();
                sectionMu["СуммаВозврат1Всего"] = totalSumReturnBuilder == 0 ? string.Empty : totalSumReturnBuilder.ToStr();

                grandTotalSum += totalSum;
                grandTotalSumRedirect += totalSumRedirect;
                grandTotalSumReturn += totalSumReturn;
                grandTotalSumReturnBuilder += totalSumReturnBuilder;
            }

            reportParams.SimpleReportParams["СуммаВсегоВсего"] = grandTotalSum == 0 ? string.Empty : grandTotalSum.ToStr();
            reportParams.SimpleReportParams["СуммаПовторноВсегоВсего"] = grandTotalSumRedirect == 0 ? string.Empty : grandTotalSumRedirect.ToStr();
            reportParams.SimpleReportParams["СуммаВозвратВсегоВсего"] = grandTotalSumReturn == 0 ? string.Empty : grandTotalSumReturn.ToStr();
            reportParams.SimpleReportParams["СуммаВозврат1ВсегоВсего"] = grandTotalSumReturnBuilder == 0 ? string.Empty : grandTotalSumReturnBuilder.ToStr();
        }
    }
}