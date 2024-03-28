namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Реестр платежных документов (за период)"
    /// </summary>
    class ExtendedInformationAboutTransferFunds : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        // идентификатор программы КР
        private int programCrId = 0;
        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MaxValue;
        private TypeFinanceGroup typeFinGroup;

        public ExtendedInformationAboutTransferFunds()
            : base(new ReportTemplateBinary(Properties.Resources.ExtendedInformationAboutTransferFunds))
        {
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ExtendedInformationAboutTransferFunds"; }
        }

        public override string Desciption
        {
            get { return "Информация о перечислении средств (расширенная)"; }
        }

        public override string GroupName
        {
            get { return "Финансирование"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ExtendedInformationAboutTransferFunds"; }
        }

        public override string Name
        {
            get { return "Информация о перечислении средств (расширенная)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();
            startDate = baseParams.Params["dateStart"].ToDateTime();

            var dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            endDate = dateEnd != DateTime.MinValue ? dateEnd : DateTime.Now;

            typeFinGroup = (TypeFinanceGroup)baseParams.Params["typeFinGroup"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {

            var serviceBasePaymentOrder = Container.Resolve<IDomainService<BasePaymentOrder>>();

            var enddate = new DateTime(this.endDate.Year, endDate.Month, 1);
            var endDateMinusOne = new DateTime(enddate.Year, enddate.Month - 1, enddate.Day);

            var paymentsByMuDict = serviceBasePaymentOrder.GetAll()
                .Where(x => x.BankStatement.TypeFinanceGroup == this.typeFinGroup) // Параметр Отчета
                .Where(x => x.BankStatement.ObjectCr.ProgramCr.Id == this.programCrId) // Параметр Отчета
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate >= this.startDate && x.DocumentDate <= enddate) // Параметр Отчета
                .Select(x => new
                {
                    x.TypePaymentOrder,
                    Sum = x.Sum.HasValue ? x.Sum.Value : decimal.Zero,  
                    x.RepeatSend,
                    x.TypeFinanceSource,
                    DocumentDate = x.DocumentDate.Value,
                    muId = x.BankStatement.ObjectCr.RealityObject.Municipality.Id,
                    muName = x.BankStatement.ObjectCr.RealityObject.Municipality.Name,
                })
                .ToList();

            var col3_18 = paymentsByMuDict
                .GroupBy(x => new { x.muId, x.muName })
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    {
                        var paymentOrdersIn = x.Where(y => y.TypePaymentOrder == TypePaymentOrder.In).ToList();
                        var paymentOrdersOut = x.Where(y => y.TypePaymentOrder == TypePaymentOrder.Out).ToList();

                        var sumTypeIn = Math.Abs(paymentOrdersIn.Sum(y => y.Sum));
                        var sumMinus = Math.Abs(paymentOrdersIn.Where(y => y.Sum < 0).Sum(y => y.Sum));
                        var sumRedirected = Math.Abs(paymentOrdersIn.Where(y => y.RepeatSend).Sum(y => y.Sum));

                        var sumTypeOut = Math.Abs(paymentOrdersOut.Sum(y => y.Sum));
                        var sumTypeOutMinus = Math.Abs(paymentOrdersOut.Where(y => y.Sum < 0).Sum(y => y.Sum));
                        var sumTypeOutMinusRedirected = Math.Abs(paymentOrdersOut.Where(y => y.RepeatSend && y.Sum < 0).Sum(y => y.Sum));

                        var sumSubjectAndPlace = paymentOrdersIn
                            .Where(y => y.TypeFinanceSource == TypeFinanceSource.SubjectBudgetFunds || y.TypeFinanceSource == TypeFinanceSource.PlaceBudgetFunds)
                            .Sum(y => y.Sum);

                        var financeSourceDict = paymentOrdersIn
                            .GroupBy(y => y.TypeFinanceSource)
                            .ToDictionary(
                                y => y.Key, 
                                y => new
                                {
                                    sumTypeInNoRedirected = Math.Abs(y.Where(z => !z.RepeatSend).Sum(z => z.Sum)),
                                    sumMinus = Math.Abs(y.Where(z => z.Sum < 0).Sum(z => z.Sum)),
                                    sumRedirected = Math.Abs(y.Where(z => z.RepeatSend).Sum(z => z.Sum)),
                                    sum = Math.Abs(y.Sum(z => z.Sum))
                                });

                        var returnBuildersByMonthsDict = paymentOrdersOut
                            .Where(y => y.Sum < 0)
                            .GroupBy(y => y.DocumentDate.Month)
                                    .ToDictionary(
                                        y => y.Key,
                                        y => new 
                                        {
                                            Sum = Math.Abs(y.Sum(z => z.Sum)),
                                            RepeatSum = Math.Abs(y.Where(z => z.RepeatSend).Sum(z => z.Sum))
                                        });

                        var returnLocalBudgetByMonthsDict = paymentOrdersIn
                            .Where(y => y.Sum < 0)
                            .Where(y => y.TypeFinanceSource == TypeFinanceSource.SubjectBudgetFunds || y.TypeFinanceSource == TypeFinanceSource.PlaceBudgetFunds)
                            .GroupBy(y => y.DocumentDate.Month)
                                .ToDictionary(
                                    y => y.Key,
                                    y => new 
                                    {
                                        Sum = Math.Abs(y.Sum(z => z.Sum)),
                                        RepeatSum = Math.Abs(y.Where(z => z.RepeatSend).Sum(z => z.Sum))
                                    });

                        return new
                        {
                            sumTypeIn,
                            sumMinus,
                            sumRedirected,
                            sumTypeOut,
                            sumTypeOutMinus,
                            sumTypeOutMinusRedirected,
                            sumSubjectAndPlace,
                            financeSourceDict,
                            returnBuildersByMonthsDict,
                            returnLocalBudgetByMonthsDict
                        };
                    });

            var col22_25 = paymentsByMuDict
                .Where(x => x.DocumentDate <= endDateMinusOne) // Параметр Отчета
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    {
                        var paymentOrdersIn = x.Where(y => y.TypePaymentOrder == TypePaymentOrder.In).ToList();
                        var paymentOrdersOut = x.Where(y => y.TypePaymentOrder == TypePaymentOrder.Out).ToList();

                        var sumSubjectAndPlace = Math.Abs(paymentOrdersIn
                            .Where(y => y.TypeFinanceSource == TypeFinanceSource.SubjectBudgetFunds || y.TypeFinanceSource == TypeFinanceSource.PlaceBudgetFunds)
                            .Sum(y => y.Sum));

                        var sumTypeOut = Math.Abs(paymentOrdersOut.Sum(y => y.Sum));

                        return new
                        {
                            sumSubjectAndPlace,
                            sumTypeOut
                        };
                    });

            var col23_26 = paymentsByMuDict
                .Where(x => x.DocumentDate.Year == endDateMinusOne.Year) // Параметр Отчета
                .Where(x => x.DocumentDate.Month == endDateMinusOne.Month) // Параметр Отчета
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    {
                        var paymentOrdersIn = x.Where(y => y.TypePaymentOrder == TypePaymentOrder.In).ToList();
                        var paymentOrdersOut = x.Where(y => y.TypePaymentOrder == TypePaymentOrder.Out).ToList();

                        var sumSubjectAndPlace = Math.Abs(paymentOrdersIn
                            .Where(y => y.TypeFinanceSource == TypeFinanceSource.SubjectBudgetFunds || y.TypeFinanceSource == TypeFinanceSource.PlaceBudgetFunds)
                            .Sum(y => y.Sum));

                        var sumTypeOut = Math.Abs(paymentOrdersOut.Sum(y => y.Sum));

                        return new
                        {
                            sumSubjectAndPlace,
                            sumTypeOut
                        };
                    });
            
            var count = 0;
            var count1 = 28;

            var buildersReturnMonthList = col3_18.Values.SelectMany(x => x.returnBuildersByMonthsDict.Keys).Distinct().OrderBy(x => x).ToList();

            var returnLocalBudgetMonthList = col3_18.Values.SelectMany(x => x.returnLocalBudgetByMonthsDict.Keys).Distinct().OrderBy(x => x).ToList();
            
            var fieldNames = new List<string>()
                {
                    "SummPeriodComingType",
                    "AllReturn",
                    "AllRedirected",
                    "FondComing",
                    "FondReturnFromTSJ",
                    "FondRedirected",
                    "FondComingWithRedirected",
                    "BudgetComing",
                    "BudgetReturnFromTSJ",
                    "BudgetRedirected",
                    "BudgetComingWithRedirected",
                    "LocalBudgetComing",
                    "LocalBudgetReturnFromTSJ",
                    "LocalBudgetRedirected",
                    "LocalBudgetComingWithRedirected",
                    "ComingForOwners",
                    "MBConsumptionAtEndPeriod",
                    "MBConsumptionAtEndPeriodMinusOne",
                    "MBConsumptionAtMonthEndPeriodMinusOne",
                    "TSJConsumptionAtEndPeriod",
                    "TSJConsumptionAtEndPeriodMinusOne",
                    "TSJConsumptionAtMonthEndPeriodMinusOne",
                    "ReturnedFromContractors",
                    "RedirectedContractors"
                };

            if (buildersReturnMonthList.Count > 0)
            {
                var columnReturn = reportParams.ComplexReportParams.ДобавитьСекцию("columnReturn");

                foreach (var month in buildersReturnMonthList)
                {
                    columnReturn.ДобавитьСтроку();
                    columnReturn["count1"] = ++count1;
                    columnReturn["count2"] = ++count1;
                    columnReturn["ContractorsMonthName"] = new DateTime(1, month, 1).ToString("MMMM");
                    columnReturn["ReturnedFromContractorsByMonth"] = string.Format("$ReturnedFromContractorsByMonth{0}$", month);
                    columnReturn["RedirectedContractorsByMonth"] = string.Format("$RedirectedContractorsByMonth{0}$", month);

                    columnReturn["TotalReturnedFromContractorsByMonth"] = string.Format("$TotalReturnedFromContractorsByMonth{0}$", month);
                    columnReturn["TotalRedirectedContractorsByMonth"] = string.Format("$TotalRedirectedContractorsByMonth{0}$", month);

                    fieldNames.Add("ReturnedFromContractorsByMonth" + month.ToStr());
                    fieldNames.Add("RedirectedContractorsByMonth" + month.ToStr());
                }
            }

            if (returnLocalBudgetMonthList.Count > 0)
            {
                var columnTotalReturn = reportParams.ComplexReportParams.ДобавитьСекцию("columnLocalBudget");

                foreach (var month in returnLocalBudgetMonthList)
                {
                    columnTotalReturn.ДобавитьСтроку();
                    columnTotalReturn["count3"] = ++count1;
                    columnTotalReturn["count4"] = ++count1;
                    columnTotalReturn["LocalBudgetMonthName"] = new DateTime(1, month, 1).ToString("MMMM");
                    columnTotalReturn["ReturnLocalBudgetByMonth"] = string.Format("$ReturnLocalBudgetByMonth{0}$", month);
                    columnTotalReturn["RedirectReturnLocalBudgetByMonth"] = string.Format("$RedirectReturnLocalBudgetByMonth{0}$", month);

                    columnTotalReturn["TotalReturnLocalBudgetByMonth"] = string.Format("$TotalReturnLocalBudgetByMonth{0}$", month);
                    columnTotalReturn["TotalRedirectReturnLocalBudgetByMonth"] = string.Format("$TotalRedirectReturnLocalBudgetByMonth{0}$", month);

                    fieldNames.Add("ReturnLocalBudgetByMonth" + month.ToStr());
                    fieldNames.Add("RedirectReturnLocalBudgetByMonth" + month.ToStr());
                }
            }

            var fieldDict = fieldNames.ToDictionary(x => x, x => new decimal?()); 
            var totalDict = fieldNames.ToDictionary(x => "Total" + x, x => new decimal());

            reportParams.SimpleReportParams["FirstDayOfEndPeriod"] = enddate.ToShortDateString();
            reportParams.SimpleReportParams["FirstDayOfEndPeriodMinusOne"] = endDateMinusOne.ToShortDateString();
            reportParams.SimpleReportParams["MonthOfEndPeriodMinusOne"] = endDateMinusOne.ToString("MMMM");

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var data in col3_18.OrderBy(x => x.Key.muName))
            {
                section.ДобавитьСтроку();

                section["Number"] = ++count;
                section["MuName"] = data.Key.muName;

                var value = data.Value;

                fieldDict.Keys.ToList().ForEach(x => fieldDict[x] = 0);

                fieldDict["SummPeriodComingType"] = value.sumTypeIn;
                fieldDict["AllReturn"] = value.sumMinus;
                fieldDict["AllRedirected"] = value.sumRedirected;
                
                if (value.financeSourceDict.ContainsKey(TypeFinanceSource.FederalFunds))
                {
                    var federalFunds = value.financeSourceDict[TypeFinanceSource.FederalFunds];
                    
                    fieldDict["FondComing"] = federalFunds.sumTypeInNoRedirected;
                    fieldDict["FondReturnFromTSJ"] = federalFunds.sumMinus;
                    fieldDict["FondRedirected"] = federalFunds.sumRedirected;
                    fieldDict["FondComingWithRedirected"] = federalFunds.sum;
                }

                if (value.financeSourceDict.ContainsKey(TypeFinanceSource.SubjectBudgetFunds))
                {
                    var subjectBudgetFunds = value.financeSourceDict[TypeFinanceSource.SubjectBudgetFunds];

                    fieldDict["BudgetComing"] = subjectBudgetFunds.sumTypeInNoRedirected;
                    fieldDict["BudgetReturnFromTSJ"] = subjectBudgetFunds.sumMinus;
                    fieldDict["BudgetRedirected"] = subjectBudgetFunds.sumRedirected;
                    fieldDict["BudgetComingWithRedirected"] = subjectBudgetFunds.sum;
                }
                
                if (value.financeSourceDict.ContainsKey(TypeFinanceSource.PlaceBudgetFunds))
                {
                    var placeBudgetFunds = value.financeSourceDict[TypeFinanceSource.PlaceBudgetFunds];

                    fieldDict["LocalBudgetComing"] = placeBudgetFunds.sumTypeInNoRedirected;
                    fieldDict["LocalBudgetReturnFromTSJ"] = placeBudgetFunds.sumMinus;
                    fieldDict["LocalBudgetRedirected"] = placeBudgetFunds.sumRedirected;
                    fieldDict["LocalBudgetComingWithRedirected"] = placeBudgetFunds.sum;
                }

                fieldDict["ComingForOwners"] = value.financeSourceDict.ContainsKey(TypeFinanceSource.OccupantFunds) ? value.financeSourceDict[TypeFinanceSource.OccupantFunds].sumTypeInNoRedirected : decimal.Zero;
                fieldDict["MBConsumptionAtEndPeriod"] = value.sumSubjectAndPlace;

                if (col22_25.ContainsKey(data.Key.muId))
                {
                    var columns_22_25 = col22_25[data.Key.muId];

                    fieldDict["MBConsumptionAtEndPeriodMinusOne"] = columns_22_25.sumSubjectAndPlace;
                    fieldDict["TSJConsumptionAtEndPeriodMinusOne"] = columns_22_25.sumTypeOut;
                }

                if (col23_26.ContainsKey(data.Key.muId))
                {
                    var columns_23_26 = col23_26[data.Key.muId];

                    fieldDict["MBConsumptionAtMonthEndPeriodMinusOne"] = columns_23_26.sumSubjectAndPlace;
                    fieldDict["TSJConsumptionAtMonthEndPeriodMinusOne"] = columns_23_26.sumTypeOut;
                }

                fieldDict["TSJConsumptionAtEndPeriod"] = value.sumTypeOut;
                fieldDict["ReturnedFromContractors"] = value.sumTypeOutMinus;
                fieldDict["RedirectedContractors"] = value.sumTypeOutMinusRedirected;

                var buildersMontsData = value.returnBuildersByMonthsDict;
                var localBudgetMontsData = value.returnLocalBudgetByMonthsDict;

                foreach (var month in buildersMontsData)
                {
                    fieldDict["ReturnedFromContractorsByMonth" + month.Key] = month.Value.Sum;
                    fieldDict["RedirectedContractorsByMonth" + month.Key] = month.Value.RepeatSum;
                }

                foreach (var month in localBudgetMontsData)
                {
                    fieldDict["ReturnLocalBudgetByMonth" + month.Key] = month.Value.Sum;
                    fieldDict["RedirectReturnLocalBudgetByMonth" + month.Key] = month.Value.RepeatSum;
                }
                
                foreach (var fieldName in fieldNames)
                {
                    section[fieldName] = fieldDict[fieldName].HasValue ? fieldDict[fieldName].Value.ToStr() : string.Empty;
                    totalDict["Total" + fieldName] += fieldDict[fieldName].HasValue ? fieldDict[fieldName].Value : decimal.Zero;
                }
            }

            totalDict.ForEach(field => reportParams.SimpleReportParams[field.Key] = field.Value);
        }
    }
}