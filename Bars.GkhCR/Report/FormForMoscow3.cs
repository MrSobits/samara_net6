namespace Bars.GkhCr.Report
{
	using B4.Modules.Reports;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Domain.CollectionExtensions;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Enums;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Bars.GkhCr.Report.Helper;
	using Castle.Windsor;
	using System;
	using System.Collections.Generic;
	using System.Linq;

    /// <summary>
    /// Форма для Москвы 3
    /// </summary>
    public class FormForMoscow3 : BasePrintForm
    {
        #region параметры отчета

        private int programCrId = 0;
        private DateTime reportDate = DateTime.MinValue;
        private List<long> municipalityIds = new List<long>();

        #endregion

        #region Properties

        public FormForMoscow3(ReportTemplateBinary template = null):
            base(template ?? new ReportTemplateBinary(Properties.Resources.FormForMoscow3))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Форма для Москвы 3";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Форма для Москвы 3";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты КР";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FormForMoscow3";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.FormForMoscow3";
            }
        }

        #endregion Properties

        protected Func<decimal?, string> AsString = value => (value.HasValue && value.Value != 0) ? value.ToString() : string.Empty;

        protected virtual Dictionary<string, string> GetWorkGroups()
        {
            var workGroups = Enumerable.Range(1, 17).Select(x => x.ToStr()).ToDictionary(x => x, x => x);

            workGroups["15"] = "14";
            workGroups["17"] = "16";
            workGroups["18"] = "18";
            workGroups["29"] = "29";
            workGroups["30"] = "30";
            workGroups["1018"] = "1018";
            workGroups["1019"] = "1018";

            return workGroups;
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            this.municipalityIds.Clear();

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) 
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToList() 
                : new List<long>();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (this.programCrId <= 0)
            {
                throw new Exception("Укажите параметр \"Программа\"");
            }

            if (this.reportDate == DateTime.MinValue || this.reportDate == null)
            {
                throw new Exception("Не указан параметр \"Дата отчета\"");
            }

            // Финансирование по 185-ФЗ
            var financeSourceId = this.Container.Resolve<IDomainService<FinanceSource>>().GetAll()
                .Where(x => x.Code == "1")
                .Select(x => x.Id)
                .FirstOrDefault();

            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { id = x.Id, name = x.Name })
                .AsEnumerable()
                .OrderBy(x => x.name)
                .ToDictionary(x => x.id, x => x.name);

            var typeWorksByFinanceSource = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id));

            var objectCrIds = typeWorksByFinanceSource.Select(x => x.ObjectCr.Id);
            var realObjIds = typeWorksByFinanceSource.Select(x => x.ObjectCr.RealityObject.Id);

            var realtyObjByMunicipal = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => realObjIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    municipalityId = x.Municipality.Id,
                    realtyFederalNumber = x.FederalNum,
                    muFederalNumber = x.Municipality.FederalNumber,
                    x.Address,
                    DateCommissioning = x.DateCommissioning.HasValue ? x.DateCommissioning.Value.Year.ToStr() : string.Empty,
                    CapitalGroupName = x.CapitalGroup.Name,
                    DateLastOverhaul = x.DateLastOverhaul.HasValue ? x.DateLastOverhaul.Value.Year.ToStr() : string.Empty,
                    x.AreaMkd,
                    x.AreaLivingNotLivingMkd,
                    x.AreaLiving,
                    x.AreaLivingOwned,
                    x.TypeHouse, // TODO проверить
                    x.NumberLiving,
                    x.MaximumFloors,
                    WallMaterialName = x.WallMaterial.Name,
                    x.NumberEntrances
                })
                .AsEnumerable()
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Address));

            var objectCrInfoDict = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Select(x => new { x.RealityObject.Id, x.FederalNumber })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.FederalNumber).FirstOrDefault());

            var manOrgContractQuery = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= this.reportDate)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= this.reportDate)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Where(x => realObjIds.Contains(x.RealityObject.Id));

            var contragentIdsQuery = manOrgContractQuery.Select(x => x.ManOrgContract.ManagingOrganization.Contragent.Id);

            var manOrgContract = manOrgContractQuery
                .Select(x => new
                {
                    x.Id,
                    realtyObjId = x.RealityObject.Id,
                    contragentId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                    manageOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    manageOrgMailingAddress = x.ManOrgContract.ManagingOrganization.Contragent.MailingAddress,
                    manageOrgPhone = x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                    manageOrgEmail = x.ManOrgContract.ManagingOrganization.Contragent.Email,
                    typeContractManOrgRealObj = x.ManOrgContract.TypeContractManOrgRealObj,
                    typeManagement = x.ManOrgContract.ManagingOrganization.TypeManagement,
                    isTSJ = x.ManOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ
                });

            var manageOrgByRealtyObj = manOrgContract.AsEnumerable().GroupBy(x => x.realtyObjId).ToDictionary(x => x.Key);

            var contragentFio = this.Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                .Where(x => contragentIdsQuery.Contains(x.Contragent.Id))
                .Where(x => x.Position.Code == "3" || x.Position.Code == "4" || x.Position.Code == "1")
                .Where(x => x.DateEndWork == null || x.DateEndWork.Value > this.reportDate)
                .Select(x => new { contragentId = x.Contragent.Id, x.Name, x.Surname, x.Patronymic, x.Position.Code })
                .AsEnumerable()
                .OrderByDescending(x => x.Code)
                .GroupBy(x => x.contragentId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Surname + " " + y.Name + " " + y.Patronymic).FirstOrDefault());

            var financeSourceByRealtyObj = this.Container.Resolve<IDomainService<FinanceSourceResource>>()
                .GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .Where(x => realObjIds.Contains(x.ObjectCr.RealityObject.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    fundResource = x.Sum(z => z.FundResource),
                    budgetSubject = x.Sum(z => z.BudgetSubject),
                    budgetMu = x.Sum(z => z.BudgetMu),
                    ownerResource = x.Sum(z => z.OwnerResource)
                })
                .ToDictionary(x => x.Key);

            var protocolCrDict = this.Container.Resolve<IDomainService<ProtocolCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .Where(x => realObjIds.Contains(x.ObjectCr.RealityObject.Id))
                .Select(x => new
                {
                    realtyObjId = x.ObjectCr.RealityObject.Id,
                    x.TypeDocumentCr,
                    x.DateFrom,
                    x.DocumentNum,
                    x.CountVote
                })
                .AsEnumerable()
                .GroupBy(x => x.realtyObjId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.TypeDocumentCr)
                          .ToDictionary(
                              y => y.Key.Key,
                              y => new
                              {
                                  DateFrom = y.Select(z => z.DateFrom ?? new DateTime(1, 1, 1)).FirstOrDefault(),
                                  DocNum = y.Select(z => z.DocumentNum).FirstOrDefault(),
                                  CountVote = y.Select(z => z.CountVote ?? decimal.Zero).FirstOrDefault(),
                              }));

            var workGroups = GetWorkGroups();

            var typeWorksDataByObjectCrDict = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    typeWorkId = x.Id,
                    objectCrId = x.ObjectCr.Id,
                    realtyObjId = x.ObjectCr.RealityObject.Id,
                    workCode = x.Work.Code,
                    typeWork = x.Work.TypeWork,
                    volume = x.Volume,
                    costSum = x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.realtyObjId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(z => new
                               {
                                   z.workCode,
                                   z.typeWork,
                                   volume = (z.volume ?? 0).RoundDecimal(2),
                                   costSum = (z.costSum ?? 0).RoundDecimal(2)
                               })
                          .ToList());

            var typeWorksByRealtyObj = typeWorksDataByObjectCrDict.ToDictionary(
                x => x.Key,
                x =>
                {
                    var distributedDict = x.Value
                        .Select(y => new SumDistributorTypeWorkProxy
                        {
                            WorkCode = y.workCode,
                            TypeWork = y.typeWork,
                            Sum = y.costSum
                        })
                        .ToList()
                        .GetDistrubutedList(workGroups.Keys.ToList())
                        .GroupBy(y => workGroups[y.WorkCode])
                        .ToDictionary(y => y.Key, y => y.Sum(z => z.Sum));

                    var valuesByWork = x.Value
                        .Where(y => y.typeWork == TypeWork.Work)
                        .Where(y => workGroups.ContainsKey(y.workCode))
                        .GroupBy(y => workGroups[y.workCode])
                        .ToDictionary(
                             y => y.Key,
                             y => new TypeWorkProxy
                                    {
                                        costSum = distributedDict.ContainsKey(y.Key) ? distributedDict[y.Key] : 0, 
                                        volume = y.Sum(z => z.volume)
                                    });

                    x.Value
                        .Where(y => y.typeWork == TypeWork.Service)
                        .Where(y => workGroups.ContainsKey(y.workCode))
                        .GroupBy(y => workGroups[y.workCode])
                        .ForEach(y =>
                                {
                                    var proxy = new TypeWorkProxy
                                                    {
                                                        costSum = y.Sum(z => z.costSum),
                                                        volume = y.Sum(z => z.volume),
                                                    };

                                    valuesByWork[y.Key] = proxy;
                                });

                    var complexWork = new TypeWorkProxy();

                    Action<string> addToComplexWork = key =>
                    {
                        if (valuesByWork.ContainsKey(key))
                        {
                            complexWork.costSum += valuesByWork[key].costSum;
                            complexWork.volume += valuesByWork[key].volume;
                        }
                    };

                    addToComplexWork("2");
                    addToComplexWork("3");

                    valuesByWork["2and3"] = complexWork;

                    return valuesByWork;
                });
            
            var sumDict = new Dictionary<string, decimal>();
            
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("секцияМо");

            foreach (var municipal in municipalityDict)
            {
                if (!realtyObjByMunicipal.ContainsKey(municipal.Key))
                {
                    continue;
                }

                var realtyObjects = realtyObjByMunicipal[municipal.Key];

                Action<string, decimal?> addToSum = (x, y) =>
                    {
                        if (y.HasValue && y.Value != 0)
                        {
                            sectionMo["col" + x] = y;
                            this.AddToSumAndWriteToReport(sumDict, x, y.Value);
                        }
                    };

                foreach (var realtyObject in realtyObjects)
                {
                    sectionMo.ДобавитьСтроку();

                    sectionMo["col2"] = realtyObject.muFederalNumber;
                    sectionMo["col3"] = municipal.Value;

                    sectionMo["col5"] = objectCrInfoDict.ContainsKey(realtyObject.Id) ? objectCrInfoDict[realtyObject.Id] : string.Empty;

                    sectionMo["col7"] = realtyObject.Address;
                    sectionMo["col8"] = realtyObject.DateCommissioning;
                    sectionMo["col9"] = realtyObject.DateLastOverhaul;
                    sectionMo["col10"] = realtyObject.CapitalGroupName;

                    addToSum("11", realtyObject.AreaMkd);
                    addToSum("12", realtyObject.AreaLivingNotLivingMkd);
                    addToSum("13", realtyObject.AreaLiving);
                    addToSum("14", realtyObject.AreaLivingOwned);
                    addToSum("17", realtyObject.NumberLiving);

                    sectionMo["col69"] = realtyObject.WallMaterialName;

                    addToSum("70", realtyObject.MaximumFloors);
                    addToSum("71", realtyObject.NumberEntrances);

                    sectionMo["col37"] = 11000 / 1000;

                    if (manageOrgByRealtyObj.ContainsKey(realtyObject.Id))
                    {
                        var manOrg = manageOrgByRealtyObj[realtyObject.Id];

                        var manOrgTsj = manOrg.FirstOrDefault(x => x.isTSJ);
                        var manOrgOther = manOrg.FirstOrDefault(x => !x.isTSJ);
                        var manOrgDirect = manOrg.FirstOrDefault(x => x.typeContractManOrgRealObj == TypeContractManOrg.DirectManag);

                        if (manOrgTsj != null)
                        {
                            sectionMo["col18"] = manOrgTsj.manageOrgName;

                            if (contragentFio.ContainsKey(manOrgTsj.contragentId))
                            {
                                sectionMo["col19"] = contragentFio[manOrgTsj.contragentId];
                            }

                            sectionMo["col20"] = manOrgTsj.manageOrgMailingAddress;
                            sectionMo["col21"] = manOrgTsj.manageOrgPhone;
                            sectionMo["col22"] = manOrgTsj.manageOrgEmail;
                            sectionMo["col73"] = manOrgTsj.manageOrgName;
                            if (manOrgTsj.typeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                            {
                                sectionMo["col72"] = manOrgTsj.typeManagement.GetEnumMeta().Display;
                                sectionMo["col73"] = manOrgTsj.manageOrgName;
                            }
                        }

                        if (manOrgOther != null && (manOrgTsj == null || manOrgTsj.typeContractManOrgRealObj == TypeContractManOrg.DirectManag))
                        {
                            sectionMo["col72"] = manOrgOther.typeManagement.GetEnumMeta().Display;
                            sectionMo["col73"] = manOrgOther.manageOrgName;

                            sectionMo["col18"] = manOrgOther.manageOrgName;
                            if (contragentFio.ContainsKey(manOrgOther.contragentId))
                            {
                                sectionMo["col19"] = contragentFio[manOrgOther.contragentId];
                            }

                            sectionMo["col20"] = manOrgOther.manageOrgMailingAddress;
                            sectionMo["col21"] = manOrgOther.manageOrgPhone;
                            sectionMo["col22"] = manOrgOther.manageOrgEmail;
                            sectionMo["col73"] = manOrgOther.manageOrgName;
                        }

                        if (manOrgDirect != null)
                        {
                            sectionMo["col72"] = manOrgDirect.typeManagement.GetEnumMeta().Display;
                        }
                    }

                    // Виды работ
                    if (typeWorksByRealtyObj.ContainsKey(realtyObject.Id))
                    {
                        var works = typeWorksByRealtyObj[realtyObject.Id];
                           
                        foreach (var workProxy in works)
                        {
                            sectionMo["volume" + workProxy.Key] = AsString(workProxy.Value.volume);
                            sectionMo["costSum" + workProxy.Key] = AsString(workProxy.Value.costSum);
                        }

                        var costSum48 = this.FillSpecificFields(sectionMo, works, sumDict);
                        addToSum("48", costSum48);

                        var col46 = works
                            .Where(x => new List<string> { "1", "2", "3", "4", "5", "6" }.Contains(x.Key))
                            .SafeSum(x => x.Value.costSum) + costSum48;

                        addToSum("46", col46);

                        Func<string, bool> sumGreaterThanZero = key => works.ContainsKey(key) && works[key].costSum != 0;

                        var listWorksToTest = new List<string> { "1", "2and3", "4", "5", "6", "12", "13", "16" };

                        sectionMo["col36"] = listWorksToTest.All(sumGreaterThanZero) ? "Да" : "Нет";
                    }

                    if (financeSourceByRealtyObj.ContainsKey(realtyObject.Id))
                    {
                        var financeSource = financeSourceByRealtyObj[realtyObject.Id];
                        addToSum("38", financeSource.fundResource);
                        addToSum("39", financeSource.budgetSubject);
                        addToSum("40", financeSource.budgetMu);
                        addToSum("41", financeSource.ownerResource);
                    }

                    if (protocolCrDict.ContainsKey(realtyObject.Id))
                    {
                        var protocolCrByType = protocolCrDict[realtyObject.Id];
                        foreach (var protocolCr in protocolCrByType)
                        {
                            switch (protocolCr.Key)
                            {
                                case TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey:
                                    sectionMo["col42"] = protocolCr.Value.DateFrom;
                                    sectionMo["col75"] = protocolCr.Value.DocNum;
                                    sectionMo["col76"] = protocolCr.Value.DateFrom.ToShortDateString();
                                    break;
                                case TypeDocumentCrLocalizer.ProtocolCompleteCrKey:
                                    sectionMo["col43"] = protocolCr.Value.DateFrom;
                                    break;
                                case TypeDocumentCrLocalizer.ProtocolNeedCrKey:
                                    var countVote = protocolCr.Value.CountVote;
                                    var countPercent = (countVote / realtyObject.AreaLivingNotLivingMkd) * 100;

                                    addToSum("15", countVote);
                                    addToSum("16", countPercent);
                                    break;
                            }
                        }
                    }
                }

                var resultDict = workGroups.Values.Distinct().ToDictionary(x => x, x => new TypeWorkProxy { volume = 0 });
                typeWorksByRealtyObj.ForEach(x => x.Value.ForEach(
                    y =>
                        {
                            if (resultDict.ContainsKey(y.Key))
                            {
                                resultDict[y.Key].costSum += y.Value.costSum;
                                resultDict[y.Key].volume += y.Value.volume;
                            }
                            else
                            {
                                resultDict[y.Key] = new TypeWorkProxy { costSum = y.Value.costSum, volume = y.Value.volume };
                            }
                        }));

                foreach (var result in resultDict)
                {
                    reportParams.SimpleReportParams["SummaryCost" + result.Key] = AsString(result.Value.costSum);
                    reportParams.SimpleReportParams["SummaryVolume" + result.Key] = AsString(result.Value.volume);
                }

                foreach (var pair in sumDict)
                {
                    reportParams.SimpleReportParams["sum" + pair.Key] = AsString(pair.Value);
                }
            }
        }

        protected void AddToSumAndWriteToReport(Dictionary<string, decimal> sumDict, string key, decimal value)
        {
            if (sumDict.ContainsKey(key))
            {
                sumDict[key] += value;
            }
            else
            {
                sumDict[key] = value;
            }
        }
        
        protected virtual decimal FillSpecificFields(Section section, Dictionary<string, TypeWorkProxy> workDict, Dictionary<string, decimal> sumDict)
        {
            var workType29 = workDict.FirstOrDefault(x => x.Key == "29");
            var workType7Sum = workDict.ContainsKey("7") ? workDict["7"].costSum : 0;
            var workType8Sum = workDict.ContainsKey("8") ? workDict["8"].costSum : 0;

            var volume29B7 = workType29.Value != null ? workType7Sum > 100000 ? workType29.Value.volume : 0 : 0;
            var costSum29B7 = workType29.Value != null ? workType7Sum > 100000 ? workType29.Value.costSum : 0 : 0;
            var volume29B8 = workType29.Value != null ? workType8Sum > 100000 ? workType29.Value.volume : 0 : 0;
            var costSum29B8 = workType29.Value != null ? workType8Sum > 100000 ? workType29.Value.costSum : 0 : 0;

            Action<string, decimal?> addToSum = (x, y) =>
            {
                if (y.HasValue && y.Value != 0)
                {
                    section["col" + x] = y;
                    this.AddToSumAndWriteToReport(sumDict, x, y.Value);
                }
            };

            addToSum("59", volume29B7);
            addToSum("60", costSum29B7);
            addToSum("61", volume29B8);
            addToSum("62", costSum29B8);

            var work9 = workDict.FirstOrDefault(x => x.Key == "9");
            var work10 = workDict.FirstOrDefault(x => x.Key == "10");
            var work11 = workDict.FirstOrDefault(x => x.Key == "11");
            var costSum9 = work9.Value != null ? work9.Value.costSum : 0;
            var costSum10 = work10.Value != null ? work10.Value.costSum : 0;
            var costSum11 = work11.Value != null ? work11.Value.costSum : 0;

            var costSum48 = (workType7Sum > 100000 ? workType7Sum : costSum29B7) +
                            (workType8Sum > 100000 ? workType8Sum : costSum29B8) +
                             costSum9 + costSum10 + costSum11;

            section["col48"] = AsString(costSum48);

            var complex69 = ((workType7Sum < 100000 || workType8Sum < 100000 ? 1 : 0) * costSum9 * costSum10 * costSum11) == 0
                                ? "Нет"
                                : "Да";
            section["complex69"] = complex69;

            return costSum48;
        }

        protected sealed class TypeWorkProxy
        {
            public decimal? volume;

            public decimal costSum;
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}