namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class AllocationFundsToPeopleInCrReport : BasePrintForm
    {
        public AllocationFundsToPeopleInCrReport()
            : base(new ReportTemplateBinary(Properties.Resources.AllocationFundsToPeopleInCr))
        {
        }

        public IWindsorContainer Container { get; set; }

        #region Входящие параметры
        private List<long> municipalityIds;
        private DateTime dateStart;
        private DateTime dateEnd;
        private long programCrId;
        #endregion

        public override string Name
        {
            get
            {
                return "Информация о распределении денежных средств граждан на капитальный ремонт жилых домов";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация о распределении денежных средств граждан на капитальный ремонт жилых домов";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Финансирование";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.AllocationFundsToPeopleInCr";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.AllocationFundsToPeopleInCr";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var strMunicipalityId = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = strMunicipalityId.Split(',').Select(x => x.ToLong()).ToList();
            dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            programCrId = baseParams.Params.GetAs<long>("programCrIds", 0);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var monthStartDate = new DateTime(dateStart.Year, dateStart.Month, 1);
            var monthEndDate = new DateTime(dateEnd.Year, dateEnd.Month, DateTime.DaysInMonth(dateEnd.Year, dateEnd.Month));

            var dictMuName =
                Container.Resolve<IDomainService<Municipality>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                         .Select(x => new { x.Id, x.Name })
                         .OrderBy(x => x.Name)
                         .ToDictionary(x => x.Id, v => v.Name);


            var realtyObjectQuery = Container.Resolve<IDomainService<Payment>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var realtyObjectIdsQuery = realtyObjectQuery.Select(x => x.RealityObject.Id);

            if (programCrId > 0) // задана программа
            {
                var realtyObjectIsObjectCrQuery = Container.Resolve<IDomainService<ObjectCr>>()
                         .GetAll()
                         .Where(x => x.ProgramCr.Id == programCrId)
                         .Select(x => x.RealityObject.Id);

                realtyObjectIdsQuery = realtyObjectQuery
                    .Where(x => realtyObjectIsObjectCrQuery.Contains(x.RealityObject.Id))
                    .Select(x => x.RealityObject.Id);
            }

            var realtyObjectByMuDict = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    municipalityId = x.Municipality.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.ToList());

            // для полей 4,5
            var paymentByRoDict = Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.Payment.RealityObject.Id))
                .Where(x => x.ChargeDate >= monthStartDate && x.ChargeDate <= monthEndDate)
                .Where(x => x.TypePayment == TypePayment.Cr || x.TypePayment == TypePayment.HireRegFund || x.TypePayment == TypePayment.Cr185)
                .GroupBy(x => x.Payment.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    ChargePopulation = x.Sum(y => y.ChargePopulation) + x.Sum(y => y.Recalculation),
                    PaidPopulation = x.Sum(y => y.PaidPopulation)
                })
                .ToDictionary(x => x.Key, x => new { x.ChargePopulation, x.PaidPopulation });

            // для полей 6
            var transferRfByRoDict = Container.Resolve<IDomainService<TransferRfRecObj>>().GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => x.TransferRfRecord.TransferDate >= monthStartDate && x.TransferRfRecord.TransferDate <= monthEndDate)
                .Where(x => x.TransferRfRecord.State.Name == "В работе ГИСУ")
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.Sum,
                    manOrgName = x.TransferRfRecord.TransferRf.ContractRf.ManagingOrganization.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .Select(x => new
                {
                    x.Key,
                    sum = x.Sum(y => y.Sum),
                    manOrgNames = string.Join(", ", x.Select(y => y.manOrgName).Distinct().Where(y => !string.IsNullOrEmpty(y)))
                })
                .ToDictionary(x => x.Key);

            // для поля 7
            var paymentOrderInByRoDict = Container.Resolve<IDomainService<PaymentOrderIn>>().GetAll()
                .Where(x => x.TypeFinanceSource == TypeFinanceSource.OccupantFunds)
                .Where(x => realtyObjectIdsQuery.Contains(x.BankStatement.ObjectCr.RealityObject.Id))
                .Where(x => x.DocumentDate >= dateStart && x.DocumentDate <= dateEnd)
                .GroupBy(x => x.BankStatement.ObjectCr.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    sum = x.Sum(y => y.Sum)
                })
                .ToDictionary(x => x.Key, x => x.sum);

            // для поля 9
            var programsByRoDict = Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => realtyObjectIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => x.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active || x.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Complete)
                .Select(x => new { realityObjectId = x.RealityObject.Id, programName = x.ProgramCr.Name })
                .AsEnumerable()
                .GroupBy(x => x.realityObjectId)
                .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.programName)));


            foreach (var mu in dictMuName)
            {
                var num = 0;
                var muId = mu.Key;

                var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("SectionMu");
                sectionMu.ДобавитьСтроку();
                sectionMu["Мо"] = dictMuName[muId];

                if (!realtyObjectByMuDict.ContainsKey(muId))
                {
                    continue;
                }

                var sectionData = sectionMu.ДобавитьСекцию("SectionData");
                var listRo = realtyObjectByMuDict[muId];

                foreach (var item in listRo.OrderBy(x => x.Address))
                {
                    sectionData.ДобавитьСтроку();
                    sectionData["Номер"] = ++num;
                    sectionData["Адрес"] = item.Address;

                    var roId = item.Id;

                    if (paymentByRoDict.ContainsKey(roId))
                    {
                        sectionData["НачисленоЖителям"] = paymentByRoDict[roId].ChargePopulation;
                        sectionData["ОплаченоЖителями"] = paymentByRoDict[roId].PaidPopulation;
                    }

                    sectionData["УО"] = transferRfByRoDict.ContainsKey(roId)
                                        ? transferRfByRoDict[roId].manOrgNames
                                        : string.Empty;

                    var sumTransferRfByRoDict = transferRfByRoDict.ContainsKey(roId) ? transferRfByRoDict[roId].sum : 0M;
                    sectionData["ПеречисленоГКУГИСУ"] = sumTransferRfByRoDict;

                    var sumPaymentOrderInByRoDict = paymentOrderInByRoDict.ContainsKey(roId)
                                                        ? paymentOrderInByRoDict[roId]
                                                        : 0M;
                    sectionData["Расходы"] = sumPaymentOrderInByRoDict;

                    sectionData["ОстДенСредствГКУГИСУ"] = sumTransferRfByRoDict - sumPaymentOrderInByRoDict;

                    sectionData["ПрограммаКр"] = programsByRoDict.ContainsKey(roId)
                                                     ? programsByRoDict[roId]
                                                     : string.Empty;
                }

                var tmpPaymentByRo = listRo.Where(x => paymentByRoDict.ContainsKey(x.Id)).Select(x => paymentByRoDict[x.Id]).ToList();
                sectionMu["ИтогоМо1"] = tmpPaymentByRo.Sum(x => x.ChargePopulation);
                sectionMu["ИтогоМо2"] = tmpPaymentByRo.Sum(x => x.PaidPopulation);

                var tmpTransferRfByRo = listRo.Where(x => transferRfByRoDict.ContainsKey(x.Id)).Select(x => transferRfByRoDict[x.Id]).ToList().Sum(x => x.sum);
                sectionMu["ИтогоМо3"] = tmpTransferRfByRo;

                var tmpPaymentOrderInByRo = listRo.Where(x => paymentOrderInByRoDict.ContainsKey(x.Id)).Select(x => paymentOrderInByRoDict[x.Id]).ToList().Sum(x => x);
                sectionMu["ИтогоМо4"] = tmpPaymentOrderInByRo;

                sectionMu["ИтогоМо5"] = tmpTransferRfByRo - tmpPaymentOrderInByRo;
            }

            reportParams.SimpleReportParams["Итого1"] = paymentByRoDict.Values.Sum(x => x.ChargePopulation);
            reportParams.SimpleReportParams["Итого2"] = paymentByRoDict.Values.Sum(x => x.PaidPopulation);
            var tmpTransferRfByRoTotal = transferRfByRoDict.Values.Sum(x => x.sum);
            reportParams.SimpleReportParams["Итого3"] = tmpTransferRfByRoTotal;
            var tmpPaymentOrderInByRoTotal = paymentOrderInByRoDict.Values.Sum(x => x);
            reportParams.SimpleReportParams["Итого4"] = tmpPaymentOrderInByRoTotal;
            reportParams.SimpleReportParams["Итого5"] = tmpTransferRfByRoTotal - tmpPaymentOrderInByRoTotal;

        }
    }
}