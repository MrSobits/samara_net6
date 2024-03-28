namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;

    public class RequestsGisuReport : BasePrintForm
    {
        private long programCrId;
        private long[] manOrgId;
        private long[] roId;
        private long[] municipalityIds;
        private long finanseSourceId;
        private DateTime startDate;

        public IDomainService<RealityObject> RoService { get; set; }
        public IDomainService<FinanceSourceResource> FinSourceResService { get; set; }
        public IDomainService<BuildContract> BuildContractService { get; set; }
        public IDomainService<TransferCtr> TransferCtrService { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgBaseContractRoService { get; set; }
        public IDomainService<PerformedWorkAct> PerformedWorkActService { get; set; }
        public IDomainService<ContractRfObject> ContractRfObjService { get; set; }
        public IDomainService<ObjectCr> ObjectCrService { get; set; }
        public IDomainService<RealityObjectChargeAccountOperation> RoChargeAccOpService { get; set; }

        public RequestsGisuReport()
            : base(new ReportTemplateBinary(Properties.Resources.GisuRequestsReport))
        {
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            roId = baseParams.Params.GetAs("roId", string.Empty).ToLongArray();

            manOrgId = baseParams.Params.GetAs("manOrgId", string.Empty).ToLongArray();

            programCrId = baseParams.Params.GetAs<long>("programCrId");

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            finanseSourceId = baseParams.Params.GetAs<int>("financeSourceId").ToLong();
        }

        public override string Name { get { return "Отчет по заявкам в ГИСУ"; } }

        public override string Desciption { get { return "Отчет по заявкам в ГИСУ"; } }

        public override string GroupName { get { return "Региональный фонд"; } }

        public override string ParamsController { get { return "B4.controller.report.RequestsGisuReport"; } }

        public override string RequiredPermission { get { return "Reports.GkhRegOp.RequestsGisuReport"; } }

        public override void PrepareReport(ReportParams reportParams)
        {
            var allObjectCrRoIds = ObjectCrService.GetAll()
                .WhereIf(programCrId != 0, x => x.ProgramCr.Id == programCrId)
                .Select(x => x.RealityObject.Id);

            var allRoQuery = ContractRfObjService.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(roId.Any(), x => roId.Contains(x.RealityObject.Id))
                .WhereIf(manOrgId.Any(), x => manOrgId.Contains(x.ContractRf.ManagingOrganization.Id))
                .Where(x => allObjectCrRoIds.Contains(x.RealityObject.Id))
                .Select(x => x.RealityObject);


            //var typeContractBuildDict = new Dictionary<TypeContractBuild, string>
            //{
            //    { TypeContractBuild.Device, TypeContractBuild.Device.GetEnumMeta().Display },
            //    { TypeContractBuild.Lift, TypeContractBuild.Lift.GetEnumMeta().Display }
            //};


            //var allRoQuery = RoService.GetAll()
            //    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Municipality.Id))
            //    .WhereIf(RoId.Any(), x => RoId.Contains(x.Id));

            var allRo = allRoQuery.ToList();

            var allRes = FinSourceResService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                .WhereIf(programCrId != 0, x => x.ObjectCr.ProgramCr.Id == programCrId)
                .WhereIf(finanseSourceId != 0, x => x.FinanceSource.Id == finanseSourceId)
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    BudgetMu = arg.Sum(z => z.BudgetMu),
                    OwnerResource = arg.Sum(z => z.OwnerResource),
                    BudgetSubject = arg.Sum(z => z.BudgetSubject),
                    FundResource = arg.Sum(z => z.FundResource),
                    BudgetMuIncome = arg.Sum(z => z.BudgetMuIncome),
                    BudgetSubjectIncome = arg.Sum(z => z.BudgetSubjectIncome),
                    FundResourceIncome = arg.Sum(z => z.FundResourceIncome)
                });

            var allManOrgs = ManOrgBaseContractRoService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.RealityObject.Id))
                .Where(x => x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(
                    x =>
                        !x.ManOrgContract.EndDate.HasValue ||
                        x.ManOrgContract.EndDate.GetValueOrDefault() > DateTime.Now)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    baseContract = arg.FirstOrDefault()
                });

            var allBuildContracts =
                BuildContractService.GetAll()
                    .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                     .GroupBy(x => x.ObjectCr.RealityObject.Id)
                    .ToDictionary(x => x.Key, arg => new
                    {
                        BudgetMu = arg.Sum(z => z.BudgetMo),
                        OwnerResource = arg.Sum(z => z.OwnerMeans),
                        BudgetSubject = arg.Sum(z => z.BudgetSubject),
                        FundResource = arg.Sum(z => z.FundMeans)
                    });

            var smrBuildContracts =
                BuildContractService.GetAll()
                    .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                    .Where(x => x.TypeContractBuild == TypeContractBuild.Smr && x.TypeContractBuild != TypeContractBuild.NotDefined)
                    .WhereIf(programCrId != 0, x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNum,
                        x.DocumentDateFrom,
                        x.Builder,
                        x.OwnerMeans,
                        x.BudgetMo,
                        x.FundMeans,
                        x.BudgetSubject
                    })
                    .ToList();

            var typeBuildContracts =
                BuildContractService.GetAll()
                    .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                    .Where(x => x.TypeContractBuild == TypeContractBuild.Device || x.TypeContractBuild == TypeContractBuild.Lift)
                    .WhereIf(programCrId != 0, x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNum,
                        x.DocumentDateFrom,
                        x.Builder,
                        x.OwnerMeans,
                        x.BudgetMo,
                        x.FundMeans,
                        x.BudgetSubject,
                        x.TypeContractBuild
                    })
                    .ToList();

            var allTransferCtrs = TransferCtrService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                .WhereIf(finanseSourceId != 0, x => x.FinSource.Id == finanseSourceId)
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    BudgetMu = arg.Sum(z => z.BudgetMu),
                    OwnerResource = arg.Sum(z => z.OwnerResource),
                    BudgetSubject = arg.Sum(z => z.BudgetSubject),
                    FundResource = arg.Sum(z => z.FundResource),
                });

            var contractTransferCtrs = TransferCtrService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                .WhereIf(finanseSourceId != 0, x => x.FinSource.Id == finanseSourceId)
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    contractSum = arg.GroupBy(z => z.Contract.Id).ToDictionary(y => y.Key, arg2 => new
                    {
                        BudgetMu = arg2.Sum(r => r.BudgetMu),
                        OwnerResource = arg2.Sum(r => r.OwnerResource),
                        FundResource = arg2.Sum(r => r.FundResource),
                        BudgetSubject = arg2.Sum(r => r.BudgetSubject)
                    })
                });

            var contractTransferCtrsPaid = TransferCtrService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                .WhereIf(finanseSourceId != 0, x => x.FinSource.Id == finanseSourceId)
                .Where(x => x.State.Name == "Оплаченое ТОДК")
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    contractSum = arg.GroupBy(z => z.Contract.Id).ToDictionary(y => y.Key, arg2 => new
                    {
                        BudgetMu = arg2.Sum(r => r.BudgetMu),
                        OwnerResource = arg2.Sum(r => r.OwnerResource),
                        FundResource = arg2.Sum(r => r.FundResource),
                        BudgetSubject = arg2.Sum(r => r.BudgetSubject)
                    })
                });


            var allTransferCtrsPaid = TransferCtrService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                .WhereIf(finanseSourceId != 0, x => x.FinSource.Id == finanseSourceId)
                .Where(x => x.State.Name == "Оплаченое ТОДК")
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    BudgetMu = arg.Sum(z => z.BudgetMu),
                    OwnerResource = arg.Sum(z => z.OwnerResource),
                    BudgetSubject = arg.Sum(z => z.BudgetSubject),
                    FundResource = arg.Sum(z => z.FundResource),
                });

            var allPerformedActs = PerformedWorkActService.GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.ObjectCr.RealityObject.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    Sum = arg.Sum(z => z.Sum)
                });


            var roChargedTotalDict = RoChargeAccOpService.GetAll()
                .Where(x => x.Period.StartDate < startDate)
                .GroupBy(x => x.Account.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    ChargedTotal = arg.Sum(z => z.ChargedTotal)
                });

            var isRoInCrProgramAfter2014 = ObjectCrService
                .GetAll()
                .Where(x => allRoQuery.Select(y => y.Id).Contains(x.RealityObject.Id))
                .Where(x => x.ProgramCr.Period.DateStart.Year >= 2014)
                .Select(x => x.RealityObject.Id);

            var roOwnerResourceDict = TransferCtrService.GetAll()
                .Where(x => isRoInCrProgramAfter2014.Contains(x.ObjectCr.RealityObject.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, arg => new
                {
                    OwnerResource = arg.Sum(z => z.OwnerResource)
                });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRo");

            foreach (var ro in allRo)
            {
                section.ДобавитьСтроку();

                section["НаименованиеМО"] = ro.Municipality.Name;
                section["АдресДома"] = ro.Address;
                section["НаименованиеУО"] = string.Empty;

                if (allManOrgs.ContainsKey(ro.Id))
                {
                    section["НаименованиеУО"] =
                        allManOrgs[ro.Id].baseContract.ReturnSafe(x => x.ManOrgContract)
                            .ReturnSafe(x => x.ManagingOrganization)
                            .ReturnSafe(x => x.Contragent)
                            .ReturnSafe(x => x.Name) ?? string.Empty;
                }

                if (allRes.ContainsKey(ro.Id))
                {

                    section["ЛимитРФ"] = allRes[ro.Id].FundResource;
                    section["ЛимитРТ"] = allRes[ro.Id].BudgetSubject;
                    section["ЛимитМО"] = allRes[ro.Id].BudgetMu;
                    section["ЛимитСС"] = allRes[ro.Id].OwnerResource;
                    section["ЛимитВсего"] = allRes[ro.Id].FundResource + allRes[ro.Id].BudgetSubject +
                                            allRes[ro.Id].BudgetMu + allRes[ro.Id].OwnerResource;

                    section["ФинансированиеРФ"] = allRes[ro.Id].FundResourceIncome;
                    section["ФинансированиеРТ"] = allRes[ro.Id].BudgetSubjectIncome;
                    section["ФинансированиеМО"] = allRes[ro.Id].BudgetMuIncome;
                    section["ФинансированиеСС"] = roChargedTotalDict.ContainsKey(ro.Id)
                                                ? roChargedTotalDict[ro.Id].ChargedTotal -
                                                  (roOwnerResourceDict.ContainsKey(ro.Id) ? roOwnerResourceDict[ro.Id].OwnerResource : 0m)
                                                 : 0m;

                    section["ФинансированиеВсего"] = allRes[ro.Id].FundResourceIncome +
                                                     allRes[ro.Id].BudgetSubjectIncome + allRes[ro.Id].BudgetMuIncome;
                }

                if (allBuildContracts.ContainsKey(ro.Id))
                {
                    section["СуммаДоговораРФ"] = allBuildContracts[ro.Id].FundResource;
                    section["СуммаДоговораРТ"] = allBuildContracts[ro.Id].BudgetSubject;
                    section["СуммаДоговораМО"] = allBuildContracts[ro.Id].BudgetMu;
                    section["СуммаДоговораСС"] = allBuildContracts[ro.Id].OwnerResource;
                    section["СуммаДоговораВсего"] = allBuildContracts[ro.Id].FundResource +
                                                    allBuildContracts[ro.Id].BudgetSubject +
                                                    allBuildContracts[ro.Id].BudgetMu +
                                                    allBuildContracts[ro.Id].OwnerResource;
                }


                if (allTransferCtrs.ContainsKey(ro.Id))
                {
                    section["ЗаявкиРФ"] = allTransferCtrs[ro.Id].FundResource;
                    section["ЗаявкиРТ"] = allTransferCtrs[ro.Id].BudgetSubject;
                    section["ЗаявкиМО"] = allTransferCtrs[ro.Id].BudgetMu;
                    section["ЗаявкиСС"] = allTransferCtrs[ro.Id].OwnerResource;
                    section["ЗаявкиВсего"] = allTransferCtrs[ro.Id].FundResource + allTransferCtrs[ro.Id].BudgetSubject +
                                             allTransferCtrs[ro.Id].BudgetMu + allTransferCtrs[ro.Id].OwnerResource;
                }


                if (allTransferCtrsPaid.ContainsKey(ro.Id))
                {
                    section["ОплатаРФ"] = allTransferCtrsPaid[ro.Id].FundResource;
                    section["ОплатаРТ"] = allTransferCtrsPaid[ro.Id].BudgetSubject;
                    section["ОплатаМО"] = allTransferCtrsPaid[ro.Id].BudgetMu;
                    section["ОплатаСС"] = allTransferCtrsPaid[ro.Id].OwnerResource;
                    section["ОплатаВсего"] = allTransferCtrsPaid[ro.Id].FundResource +
                                             allTransferCtrsPaid[ro.Id].BudgetSubject +
                                             allTransferCtrsPaid[ro.Id].BudgetMu +
                                             allTransferCtrsPaid[ro.Id].OwnerResource;

                    section["ОстатокВсего"] =
                        section["ОстатокРФ"] =
                            allBuildContracts[ro.Id].FundResource - allTransferCtrsPaid[ro.Id].FundResource;
                    section["ОстатокРТ"] = allBuildContracts[ro.Id].BudgetSubject -
                                           allTransferCtrsPaid[ro.Id].BudgetSubject;
                    section["ОстатокМО"] = allBuildContracts[ro.Id].BudgetMu - allTransferCtrsPaid[ro.Id].BudgetMu;
                    section["ОстатокСС"] = allBuildContracts[ro.Id].OwnerResource -
                                           allTransferCtrsPaid[ro.Id].OwnerResource;
                }

                var sectionContract = section.ДобавитьСекцию("sectionContract");

                foreach (var contract in smrBuildContracts)
                {
                    sectionContract.ДобавитьСтроку();

                    sectionContract["НомерДоговора"] = contract.DocumentNum;
                    sectionContract["ДатаДоговора"] = contract.DocumentDateFrom;
                    sectionContract["Подрядчик"] = contract.Builder.ReturnSafe(x => x.Contragent).ReturnSafe(x => x.Name) ?? string.Empty;

                    sectionContract["СуммаДоговораРФ"] = contract.FundMeans;
                    sectionContract["СуммаДоговораРТ"] = contract.BudgetSubject;
                    sectionContract["СуммаДоговораМО"] = contract.BudgetMo;
                    sectionContract["СуммаДоговораСС"] = contract.OwnerMeans;

                    var allContract = contract.FundMeans + contract.BudgetSubject + contract.BudgetMo +
                                      contract.OwnerMeans;

                    sectionContract["СуммаДоговораВсего"] = allContract;

                    if (allPerformedActs.ContainsKey(ro.Id))
                    {
                        sectionContract["СуммаВсегоПоДоговоруКс2"] = allPerformedActs[ro.Id].Sum;
                    }


                    if (contractTransferCtrs.ContainsKey(ro.Id) &&
                        contractTransferCtrs[ro.Id].contractSum.ContainsKey(contract.Id))
                    {
                        sectionContract["СуммаПоЗаявкамДоговораРФ"] = contractTransferCtrs[ro.Id].contractSum[contract.Id].FundResource;
                        sectionContract["СуммаВсегоПоЗаявкамДоговораРТ"] = contractTransferCtrs[ro.Id].contractSum[contract.Id].BudgetSubject;
                        sectionContract["СуммаВсегоПоЗаявкамДоговораМО"] = contractTransferCtrs[ro.Id].contractSum[contract.Id].BudgetMu;
                        sectionContract["СуммаВсегоПоЗаявкамДоговораСС"] = contractTransferCtrs[ro.Id].contractSum[contract.Id].OwnerResource;
                        sectionContract["СуммаВсегоПоЗаявкамДоговора"] =
                            contractTransferCtrs[ro.Id].contractSum[contract.Id].FundResource +
                            contractTransferCtrs[ro.Id].contractSum[contract.Id].BudgetSubject +
                            contractTransferCtrs[ro.Id].contractSum[contract.Id].BudgetMu +
                            contractTransferCtrs[ro.Id].contractSum[contract.Id].OwnerResource;
                    }

                    if (contractTransferCtrsPaid.ContainsKey(ro.Id) &&
                        contractTransferCtrsPaid[ro.Id].contractSum.ContainsKey(contract.Id))
                    {
                        sectionContract["ОплатаПоДоговоруРФ"] = contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].FundResource;
                        sectionContract["ОплатаПоДоговоруРТ"] = contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].BudgetSubject;
                        sectionContract["ОплатаПоДоговоруМО"] = contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].BudgetMu;
                        sectionContract["ОплатаПоДоговоруСС"] = contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].OwnerResource;

                        var paidAllContract = contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].FundResource +
                            contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].BudgetSubject +
                            contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].BudgetMu +
                            contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].OwnerResource;

                        sectionContract["ОплатаВсегоПоДоговору"] = paidAllContract;


                        sectionContract["ОстатокВсегоПоДоговору"] = allContract - paidAllContract;
                        sectionContract["ОстатокПоДоговоруРФ"] = contract.FundMeans - contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].FundResource;
                        sectionContract["ОстатокПоДоговоруРТ"] = contract.BudgetSubject - contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].BudgetSubject;
                        sectionContract["ОстатокПоДоговоруМО"] = contract.BudgetMo - contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].BudgetMu;
                        sectionContract["ОстатокПоДоговоруСС"] = contract.OwnerMeans - contractTransferCtrsPaid[ro.Id].contractSum[contract.Id].OwnerResource;
                    }
                }

                var i = 1;
                //var curBuildContracts =
                //        typeBuildContracts.Where(x => x.TypeContractBuild == tp.TypeContractBuild).ToList();

                var sectionElement = section.ДобавитьСекцию("sectionElement");

                foreach (var contract in typeBuildContracts)
                {
                    sectionElement.ДобавитьСтроку();

                    sectionElement["НомерДоговораЭлемента"] = contract.DocumentNum;
                    sectionElement["ДатаДоговораЭлемента"] = contract.DocumentDateFrom;
                    sectionElement["ПодрядчикЭлемента"] = contract.Builder.ReturnSafe(x => x.Contragent).ReturnSafe(x => x.Name) ?? string.Empty;

                    sectionElement["номерЭлемента"] = i;
                    sectionElement["Элемент"] = contract.TypeContractBuild.GetEnumMeta().Display;


                    sectionElement["СуммаДоговораЭлементаРФ"] = contract.FundMeans;
                    sectionElement["СуммаДоговораЭлементаРТ"] = contract.BudgetSubject;
                    sectionElement["СуммаДоговораЭлементаМО"] = contract.BudgetMo;
                    sectionElement["СуммаДоговораЭлементаСС"] = contract.OwnerMeans;

                    sectionElement["СуммаДоговораЭлементаРФ"] = contract.FundMeans + contract.BudgetSubject +
                                                                  contract.BudgetMo + contract.OwnerMeans;

                    i++;
                }
            }
        }
    }
}