namespace Bars.GkhRf.Report
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
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class PaymentCrMonthByRealObj : BasePrintForm
    {
        #region Входящие параметры
        private DateTime dateReport;
        private long[] municipalityIds;
        #endregion

        public IWindsorContainer Container { get; set; }

        public PaymentCrMonthByRealObj(): base(new ReportTemplateBinary(Properties.Resources.PaymentCrMonthByRealObj))
        {
        }

        public override string Name
        {
            get
            {
                return "Сведения за месяц об оплате КР (по домам)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения за месяц об оплате КР (по домам)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PaymentCrMonthByRealObj";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.PaymentCrMonthByRealObj";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            dateReport = baseParams.Params.GetAs("reportDate", DateTime.MinValue);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servicePaymentItem = Container.Resolve<IDomainService<PaymentItem>>();
            var serviceRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();

            var dateStartYear = new DateTime(dateReport.Year, 1, 1);
            var dateStartMonth = new DateTime(dateReport.Year, dateReport.Month, 1);
            var dateEndMonth = new DateTime(dateReport.Year, dateReport.Month, DateTime.DaysInMonth(dateReport.Year, dateReport.Month));
            
            var dictDataPaymentItem =
                servicePaymentItem.GetAll()
                                  .Where(x => x.ChargeDate >= dateStartMonth && x.ChargeDate <= dateEndMonth)
                                  .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Payment.RealityObject.Municipality.Id))
                                  .GroupBy(x => new { x.Payment.RealityObject.Id, x.TypePayment })
                                  .Select(x => new PaymentItemProxy
                                                {
                                                      RealityObjectId = x.Key.Id,
                                                      TypePayment = x.Key.TypePayment,
                                                      IncomeBalance = x.Sum(y => y.IncomeBalance).ToDecimal(),
                                                      ChargePopulation = x.Sum(y => y.ChargePopulation).ToDecimal(),
                                                      Recalculation = x.Sum(y => y.Recalculation).ToDecimal(),
                                                      PaidPopulation = x.Sum(y => y.PaidPopulation).ToDecimal(),
                                                      OutgoingBalance = x.Sum(y => y.OutgoingBalance).ToDecimal()
                                                  })
                                 .AsEnumerable()
                                 .GroupBy(x => x.RealityObjectId)
                                 .ToDictionary(
                                      x => x.Key, 
                                      x => x.ToDictionary(y => y.TypePayment));

            var dictDataRoForYear = servicePaymentItem.GetAll()
                     .Where(x => x.ChargeDate >= dateStartYear && x.ChargeDate <= dateStartMonth.AddMonths(1).AddDays(-1))
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Payment.RealityObject.Municipality.Id))
                     .GroupBy(x => x.Payment.RealityObject.Id)
                     .Select(x => new
                         {
                             RealityObjectId = x.Key,
                             ChargePopulation = x.Sum(y => y.ChargePopulation),
                             PaidPopulation = x.Sum(y => y.PaidPopulation),
                             OutgoingBalance = x.Sum(y => y.OutgoingBalance)
                         })
                     .ToDictionary(x => x.RealityObjectId);

            var roIdQuery = servicePaymentItem.GetAll()
                      .Where(x => x.ChargeDate >= dateStartMonth && x.ChargeDate <= dateEndMonth)
                      .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Payment.RealityObject.Municipality.Id))
                      .Select(x => x.Payment.RealityObject.Id);

            var dictMuInfo = serviceRealityObject.GetAll()
                .Where(x => roIdQuery.Contains(x.Id))
                .Select(x => new
                            {
                                x.Id,
                                address = x.Address,
                                MuName = x.Municipality.Name
                            })
                .AsEnumerable()
                .GroupBy(x => x.MuName)
                .ToDictionary(x => x.Key, x => x.ToList());

            //Списки кодов работ для отдельных записей TypePayment
            //BuildingCurrentRepair
            var listCodeWorktypePayment40 = new List<string> { "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            //SanitaryEngineeringRepair
            var listCodeWorktypePayment50 = new List<string> { "2", "3", "4" };
            //HeatingRepair
            var listCodeWorktypePayment60 = new List<string> { "1", "5" };

            var workCodes = listCodeWorktypePayment40;
            workCodes.AddRange(listCodeWorktypePayment50);
            workCodes.AddRange(listCodeWorktypePayment60);

            var dictListedInRfTypePayment = serviceTypeWorkCr.GetAll()
                                 .Where(x => roIdQuery.Contains(x.ObjectCr.RealityObject.Id))
                                 .Where(x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Close)
                                 .Where(x => workCodes.Contains(x.Work.Code))
                                 .Select(x => new { x.ObjectCr.RealityObject.Id, x.Work.Code })
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(
                                     x => x.Key,
                                     x =>
                                         {
                                             var typePayment40 = x.Select(y => y.Code).Intersect(listCodeWorktypePayment40).Any();
                                             var typePayment50 = x.Select(y => y.Code).Intersect(listCodeWorktypePayment50).Any();
                                             var typePayment60 = x.Select(y => y.Code).Intersect(listCodeWorktypePayment60).Any();
                                             var typePayment70 = x.Select(y => y.Code).Contains("6");

                                             var dict = new Dictionary<int, bool>();

                                             dict.Add(40, typePayment40);
                                             dict.Add(50, typePayment50);
                                             dict.Add(60, typePayment60);
                                             dict.Add(70, typePayment70);

                                             return dict;
                                         });

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("SectionMu");
            var sectionData = sectionMu.ДобавитьСекцию("SectionData");

            var num = 0;
            foreach (var item in dictMuInfo.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();

                var listRo = item.Value;

                sectionMu["MuName"] = item.Key;

                foreach (var ro in listRo.OrderBy(x => x.address))
                {
                    var roId = ro.Id;

                    sectionData.ДобавитьСтроку();

                    sectionData["num"] = ++num;
                    sectionData["Address"] = ro.address;

                    if (dictDataRoForYear.ContainsKey(roId))
                    {
                        sectionData["ChargeStartYear"] = dictDataRoForYear[roId].ChargePopulation;                        
                        sectionData["PaidStartYear"] = dictDataRoForYear[roId].PaidPopulation;
                        sectionData["OutgoingBalance"] = dictDataRoForYear[roId].OutgoingBalance;
                    }

                    sectionData["Month"] = dateReport.ToString("MMMM yyyy");

                    foreach (var data in dictDataPaymentItem[roId])
                    {
                        var type = (int)data.Key;
                        sectionData["IncomeBalance" + type] = data.Value.IncomeBalance;
                        sectionData["ChargePopulation" + type] = data.Value.ChargePopulation;
                        sectionData["Recalculation" + type] = data.Value.Recalculation;
                        sectionData["PaidPopulation" + type] = data.Value.PaidPopulation;
                        sectionData["OutgoingBalance" + type] = data.Value.OutgoingBalance;

                        if (dictListedInRfTypePayment.ContainsKey(roId) && (type == 40 || type == 50 || type == 60 || type == 70))
                        {
                            sectionData["ListedInRf" + type] = dictListedInRfTypePayment[roId][type]
                                                                   ? (data.Value.OutgoingBalance / 2)
                                                                   : 0;
                        }
                    }
                }

                var tmpDataYear = listRo.Where(x => dictDataRoForYear.ContainsKey(x.Id)).Select(x => dictDataRoForYear[x.Id]).ToList();
                sectionMu["SumChargeStartYear"] = tmpDataYear.Sum(x => x.ChargePopulation);
                sectionMu["SumPaidStartYear"] = tmpDataYear.Sum(x => x.PaidPopulation);
                sectionMu["SumOutgoingBalance"] = tmpDataYear.Sum(x => x.OutgoingBalance);
                sectionMu["Month"] = dateReport.ToString("MMMM yyyy");

                var tmpDataMonth = listRo.Where(x => dictDataPaymentItem.ContainsKey(x.Id))
                                        .SelectMany(x => dictDataPaymentItem[x.Id])
                                        .GroupBy(x => x.Value.TypePayment)
                                        .ToDictionary(
                                            x => x.Key,
                                            v => new {
                                                        IncomeBalance = v.Sum(x => x.Value.IncomeBalance),
                                                        ChargePopulation = v.Sum(x => x.Value.ChargePopulation),
                                                        Recalculation = v.Sum(x => x.Value.Recalculation),
                                                        PaidPopulation = v.Sum(x => x.Value.PaidPopulation),
                                                        OutgoingBalance = v.Sum(x => x.Value.OutgoingBalance)                                                        
                                                });

                foreach (var data in tmpDataMonth)
                {
                    var type = ((int)data.Key).ToString();
                    sectionMu["SumIncomeBalance" + type] = data.Value.IncomeBalance;
                    sectionMu["SumChargePopulation" + type] = data.Value.ChargePopulation;
                    sectionMu["SumRecalculation" + type] = data.Value.Recalculation;
                    sectionMu["SumPaidPopulation" + type] = data.Value.PaidPopulation;
                    sectionMu["SumOutgoingBalance" + type] = data.Value.OutgoingBalance;
                }

                var dictListedInRf = listRo.Where(x => dictListedInRfTypePayment.ContainsKey(x.Id))
                                          .SelectMany(
                                              x => dictDataPaymentItem[x.Id]
                                                  .Where( y => y.Value.TypePayment == TypePayment.BuildingCurrentRepair
                                                  || y.Value.TypePayment == TypePayment.SanitaryEngineeringRepair
                                                  || y.Value.TypePayment == TypePayment.HeatingRepair
                                                  || y.Value.TypePayment == TypePayment.ElectricalRepair))
                                          .GroupBy(x => x.Value.TypePayment)
                                          .ToDictionary(x => x.Key, v => v.Sum(x => x.Value.OutgoingBalance) / 2);

                foreach (var data in dictListedInRf)
                {
                    sectionMu["SumListedInRf" + (int)data.Key] = data.Value;
                }
            }

            var total = dictDataPaymentItem.Values.SelectMany(x => x)
                                   .GroupBy(x => x.Value.TypePayment)
                                   .ToDictionary(
                                       x => x.Key,
                                       v => new
                                           {
                                               IncomeBalance = v.Sum(x => x.Value.IncomeBalance),
                                               ChargePopulation = v.Sum(x => x.Value.ChargePopulation),
                                               Recalculation = v.Sum(x => x.Value.Recalculation),
                                               PaidPopulation = v.Sum(x => x.Value.PaidPopulation),
                                               OutgoingBalance = v.Sum(x => x.Value.OutgoingBalance)
                                           });

            reportParams.SimpleReportParams["TotalChargeStartYear"] = dictDataRoForYear.Values.Sum(x => x.ChargePopulation);
            reportParams.SimpleReportParams["TotalPaidStartYear"] = dictDataRoForYear.Values.Sum(x => x.PaidPopulation);
            reportParams.SimpleReportParams["TotalOutgoingBalance"] = dictDataRoForYear.Values.Sum(x => x.OutgoingBalance);
            reportParams.SimpleReportParams["Month"] = dateReport.ToString("MMMM yyyy");

            foreach (var data in total)
            {
                var type = ((int)data.Key).ToString();
                reportParams.SimpleReportParams["TotalIncomeBalance" + type] = data.Value.IncomeBalance;
                reportParams.SimpleReportParams["TotalChargePopulation" + type] = data.Value.ChargePopulation;
                reportParams.SimpleReportParams["TotalRecalculation" + type] = data.Value.Recalculation;
                reportParams.SimpleReportParams["TotalPaidPopulation" + type] = data.Value.PaidPopulation;
                reportParams.SimpleReportParams["TotalOutgoingBalance" + type] = data.Value.OutgoingBalance;
            }

            var dictListedInRf1 = dictDataPaymentItem.Values.SelectMany(x => x.Where(y => y.Value.TypePayment == TypePayment.BuildingCurrentRepair
                              || y.Value.TypePayment == TypePayment.SanitaryEngineeringRepair
                              || y.Value.TypePayment == TypePayment.HeatingRepair
                              || y.Value.TypePayment == TypePayment.ElectricalRepair))
                      .GroupBy(x => x.Value.TypePayment)
                      .ToDictionary(x => x.Key, v => v.Sum(x => x.Value.OutgoingBalance) / 2);

            foreach (var data in dictListedInRf1)
            {
                reportParams.SimpleReportParams["TotalListedInRf" + (int)data.Key] = data.Value;
            }
        }

        private class PaymentItemProxy
        {
            public long RealityObjectId;

            public TypePayment TypePayment;

            public decimal IncomeBalance;

            public decimal ChargePopulation;

            public decimal Recalculation;

            public decimal PaidPopulation;

            public decimal OutgoingBalance;
        }
    }
}