namespace Bars.GkhCrTp.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4.Modules.Reports;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по программе КР
    /// </summary>
    public class ProgramCrReport : BasePrintForm
    {
        // идентификаторы муниципальных образований
        private long[] municipalityIds;
        private long programCrId;
        private long dopProgramCrId;
        private DateTime reportDate;
        private Dictionary<long, string> dateLastOverhaulByRoDict;
        private Dictionary<long, MunicipalityInfoProxy> municipalityInfoDict;
        private Dictionary<long, string> finSourcesDict;
        private List<string> workCodes;

        /// <summary>
        /// Конструктор отчёта
        /// </summary>
        public ProgramCrReport() : base(new ReportTemplateBinary(Properties.Resource.ProgramCrReport))
        {
        }

        /// <summary>
        /// Windsor-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Необходимые разрешения
        /// </summary>
        public override string RequiredPermission
        {
            get { return "Reports.CR.ProgramCr"; }
        }

        /// <summary>
        /// Наименование отчёта
        /// </summary>
        public override string Name
        {
            get { return "Отчет по программе КР"; }
        }

        /// <summary>
        /// Описание отчёта
        /// </summary>
        public override string Desciption
        {
            get { return "Отчет по программе КР"; }
        }

        /// <summary>
        /// Наименование группы
        /// </summary>
        public override string GroupName
        {
            get { return "Отчеты кап.ремонта"; }
        }

        /// <summary>
        /// Наименование контроллера параметров
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.ProgramCrReport"; }
        }

        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();
            this.dopProgramCrId = baseParams.Params["dopProgram"].ToInt();
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            var m = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        /// <summary>
        /// Генератор отчёта
        /// </summary>
        public override string ReportGenerator { get; set; }
        
        /// <summary>
        /// Подготовка отчёта
        /// </summary>
        /// <param name="reportParams">Параметры отчёта</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Год"] = this.Container.Resolve<IDomainService<ProgramCr>>().GetAll()
                 .Where(x => x.Id == this.programCrId)
                 .Select(x => x.Period.DateStart.Year.ToStr())
                 .FirstOrDefault();

            this.municipalityInfoDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                .ToDictionary(x => x.Id, x => new MunicipalityInfoProxy { Name = x.Name, Group = x.Group });

            var objectsCrList = this.GetRealtyObjInfo();
            var objectsCrDict = objectsCrList.GroupBy(x => x.MunicipalityId)
                .ToDictionary(x => x.Key, x => x.Select(y => y).ToList());
            var manOrgRealityObj = this.GetManOrgRealityObj();
            var managerManOrg = this.GetManagerInfo();
            var municipalityList = this.GetMunicipalitys();

            var finSourceResourceData = this.GetFinSourceResourceData();
            var objCrFinSourceResourceDict = this.GetCrObjFinSourceResourceDictByCrObj(finSourceResourceData);
            var objCrFinSourceResourceDictMuTotal = this.GetCrObjFinSourceResourceDictByMu(finSourceResourceData);
            var objCrFinSourceResourceDictMuGrTotal = this.GetCrObjFinSourceResourceDictByMuGr(finSourceResourceData);
            var objCrFinSourceResourceDictTotal = this.GetCrObjFinSourceResourceDictTotal(finSourceResourceData);

            var typeWorkCrData = this.GetTypeWorkData();
            var objCrWorksData = this.GetWorksInfoByCrObj(typeWorkCrData);
            var objCrWorksDataMuTotal = this.GetWorksInfoByMu(typeWorkCrData);
            var objCrWorksDataMuGrTotal = this.GetWorksInfoByMuGr(typeWorkCrData);
            var objCrWorksDataTotal = this.GetWorksInfoTotal(typeWorkCrData);
            var programsLimit = this.GetProgramLimitsDict();
            this.dateLastOverhaulByRoDict = this.GetDateLastOverhaulByRo();

            // итоговые значения по мун. образованию ст. 20-25
            var objCrFeaturesByMu = objectsCrList
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                x => x.Key,
                x =>
                new RealtyObjectFeatureValues
                    {
                        AreaMkd = x.Select(y => y.AreaMkd).Sum(),
                        AreaLiving = x.Select(y => y.AreaLiving).Sum(),
                        AreaLivingNotLivingMkd = x.Select(y => y.AreaLivingNotLivingMkd).Sum(),
                        AreaLivingOwned = x.Select(y => y.AreaLivingOwned).Sum(),
                        NumberApartments = x.Select(y => y.NumberApartments).Sum(),
                        NumberLiving = x.Select(y => y.NumberLiving).Sum()
                    });

            // итоговые значения по группе мун. образований ст. 20-25
            var objCrFeaturesByGrMu = objectsCrList
                .GroupBy(x => x.MunicipalityGroup)
                .ToDictionary(
                x => x.Key,
                x =>
                new RealtyObjectFeatureValues
                {
                    AreaMkd = x.Select(y => y.AreaMkd).Sum(),
                    AreaLiving = x.Select(y => y.AreaLiving).Sum(),
                    AreaLivingNotLivingMkd = x.Select(y => y.AreaLivingNotLivingMkd).Sum(),
                    AreaLivingOwned = x.Select(y => y.AreaLivingOwned).Sum(),
                    NumberApartments = x.Select(y => y.NumberApartments).Sum(),
                    NumberLiving = x.Select(y => y.NumberLiving).Sum()
                });

            // итоговые значения ст. 20-25
            var oblCrFeaturesTotal = new RealtyObjectFeatureValues
                    {
                        AreaMkd = objectsCrList.Select(x => x.AreaMkd).Sum(),
                        AreaLiving = objectsCrList.Select(x => x.AreaLiving).Sum(),
                        AreaLivingNotLivingMkd = objectsCrList.Select(x => x.AreaLivingNotLivingMkd).Sum(),
                        AreaLivingOwned = objectsCrList.Select(x => x.AreaLivingOwned).Sum(),
                        NumberApartments = objectsCrList.Select(x => x.NumberApartments).Sum(),
                        NumberLiving = objectsCrList.Select(x => x.NumberLiving).Sum()
                    };

            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("Группа");
            var sectionGroupName = sectionGroup.ДобавитьСекцию("ИмяГруппы");
            var sectionGrMuTotals = sectionGroup.ДобавитьСекцию("СекцияИтогиПоГруппе");
            var sectionGrMuGrFinSources = sectionGrMuTotals.ДобавитьСекцию("ГруппыИсточниковГруппы");
            var sectionGrMuFinSources = sectionGrMuGrFinSources.ДобавитьСекцию("ИсточникиГруппы");
            var sectionRegion = sectionGroup.ДобавитьСекцию("Районы");
            var sectionObjects = sectionRegion.ДобавитьСекцию("Объекты");
            var sectionSourcesGroup = sectionObjects.ДобавитьСекцию("ГруппыИсточников");
            var sectionSources = sectionSourcesGroup.ДобавитьСекцию("Источники");
            var sectionMuTotals = sectionRegion.ДобавитьСекцию("ИтогиПоРайону");
            var sectionMuGrFinSources = sectionMuTotals.ДобавитьСекцию("ГруппыИсточниковРайона");
            var sectionMuFinSources = sectionMuGrFinSources.ДобавитьСекцию("ИсточникиРайона");
            var sectionTotals = reportParams.ComplexReportParams.ДобавитьСекцию("Итоги");
            var sectionTotalsGrFinSources = sectionTotals.ДобавитьСекцию("ГруппыИсточниковОтчета");
            var sectionTotalsFinSources = sectionTotalsGrFinSources.ДобавитьСекцию("ИсточникиОтчета");

            var finsourcesIdsResource = objCrFinSourceResourceDict.SelectMany(x => x.Value.SelectMany(y => y.Value.Select(z => z.Key))).Distinct().ToList();
            var finsourcesIdsWorks = objCrWorksData.SelectMany(x => x.Value.SelectMany(y => y.Value.Select(z => z.Key))).Distinct().ToList();

            this.finSourcesDict = this.Container.Resolve<IDomainService<FinanceSource>>().GetAll()
                .Where(x => finsourcesIdsResource.Contains(x.Id) || finsourcesIdsWorks.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            int i = 0;

            foreach (var groupMu in municipalityList)
            {
                sectionGroup.ДобавитьСтроку();
                var groupName = this.municipalityInfoDict[groupMu.First()].Group;
                if (groupName != string.Empty)
                {
                    sectionGroupName.ДобавитьСтроку();
                    sectionGroupName["НаименованиеГруппы"] = groupName;
                }

                foreach (var municipalityId in groupMu)
                {
                    if (!objCrFinSourceResourceDictMuTotal.ContainsKey(municipalityId) && !objCrWorksDataMuTotal.ContainsKey(municipalityId))
                    {
                        continue;
                    }
                    
                    var municipalityName = this.municipalityInfoDict[municipalityId].Name;
                    sectionRegion.ДобавитьСтроку();
                    sectionRegion["Район"] = municipalityName;

                    var programLimit = programsLimit.ContainsKey(this.programCrId) ?
                    (programsLimit[this.programCrId].ContainsKey(municipalityId) ? programsLimit[this.programCrId][municipalityId] : 0M)
                    : 0M;

                    var subProgramLimit = programsLimit.ContainsKey(this.dopProgramCrId) ?
                        (programsLimit[this.dopProgramCrId].ContainsKey(municipalityId) ? programsLimit[this.dopProgramCrId][municipalityId] : 0M)
                        : 0M;

                    if (subProgramLimit - programLimit != 0)
                    {
                        sectionRegion["ЛимитПревышен"] =
                            $"ЛИМИТ ФИНАНСИРОВАНИЯ ПО РАЙОНУ ПРЕВЫШЕН! (ЛИМИТ ПРОГРАММЫ КАПРЕМОНТА {programLimit}, ЛИМИТ ТЕКУШЕЙ ПРОГРАММЫ {subProgramLimit}, РАЗНИЦА {subProgramLimit - programLimit})";
                    }

                    // объекты капитального ремонта текущего мун.образования
                    var objectsCr = objectsCrDict.ContainsKey(municipalityId) ? objectsCrDict[municipalityId] : null;
                    
                    if (objectsCr == null)
                    {
                        continue;
                    }

                    int j = 0;

                    foreach (var objectCr in objectsCr.OrderBy(x => x.Address))
                    {
                        i++;
                        j++;
                        var objCrManOrg = manOrgRealityObj.ContainsKey(objectCr.RealityObjectId) ? manOrgRealityObj[objectCr.RealityObjectId] : null;

                        var manOrgManager = new ManagerInfoProxy();

                        if (objCrManOrg != null && managerManOrg.ContainsKey(objCrManOrg.ContragentId.ToLong()))
                        {
                            manOrgManager = managerManOrg[objCrManOrg.ContragentId.ToLong()]; 
                        }

                        var objCrFinSourceResource = objCrFinSourceResourceDict.ContainsKey(objectCr.Id) ? objCrFinSourceResourceDict[objectCr.Id] : null;
                        var objCrWorks = objCrWorksData.ContainsKey(objectCr.Id) ? objCrWorksData[objectCr.Id] : null;

                        var realtyObjInfo = new RealtyObjInfoProxy { objectCr = objectCr, manOrgInfo = objCrManOrg, managerInfo = manOrgManager, number = i, numberByMu = j };
                        this.FillRows(objCrFinSourceResource, objCrWorks, realtyObjInfo, null, null, sectionObjects, sectionSourcesGroup, sectionSources);
                    }

                    var currentMuObjCrFeatures = objCrFeaturesByMu.ContainsKey(municipalityId) ? objCrFeaturesByMu[municipalityId] : null;

                    // заполнение итогов по мун.образованию
                    var currentMuFinSourceResourceDict = objCrFinSourceResourceDictMuTotal.ContainsKey(municipalityId) ? objCrFinSourceResourceDictMuTotal[municipalityId] : null;
                    var currentWorksDataMuTotal = objCrWorksDataMuTotal.ContainsKey(municipalityId) ? objCrWorksDataMuTotal[municipalityId] : null;
                    this.FillRows(currentMuFinSourceResourceDict, currentWorksDataMuTotal, null, currentMuObjCrFeatures, null, sectionMuTotals, sectionMuGrFinSources, sectionMuFinSources);
                }

                var currentGrMuObjCrFeatures = objCrFeaturesByGrMu.ContainsKey(groupName) ? objCrFeaturesByGrMu[groupName] : null;
                
                // заполнение итогов по группе мун.образований
                if (groupName != string.Empty)
                {
                    var currentGrMuFinSourceResourceDict = objCrFinSourceResourceDictMuGrTotal.ContainsKey(groupName) ? objCrFinSourceResourceDictMuGrTotal[groupName] : null;
                    var currentWorksDataGrMuTotal = objCrWorksDataMuGrTotal.ContainsKey(groupName) ? objCrWorksDataMuGrTotal[groupName] : null;
                    this.FillRows(currentGrMuFinSourceResourceDict, currentWorksDataGrMuTotal, null, currentGrMuObjCrFeatures, groupName, sectionGrMuTotals, sectionGrMuGrFinSources, sectionGrMuFinSources);
                }
            }

            // заполнение конечных итогов
            this.FillRows(objCrFinSourceResourceDictTotal, objCrWorksDataTotal, null, oblCrFeaturesTotal, null, sectionTotals, sectionTotalsGrFinSources, sectionTotalsFinSources);
        }

        private List<ObjectCrProxy> GetRealtyObjInfo()
        {
             return this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                         .Where(x => x.ProgramCr.Id == this.programCrId)
                         .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Select(x => new ObjectCrProxy
                         {
                             Id = x.Id,
                             RealityObjectId = x.RealityObject.Id,
                             Address = x.RealityObject.Address,
                             MunicipalityId = x.RealityObject.Municipality.Id,
                             Municipality = x.RealityObject.Municipality.Name,
                             MunicipalityGroup = x.RealityObject.Municipality.Group ?? string.Empty,
                             RealityObjectFloors = x.RealityObject.MaximumFloors,
                             SeriesHome = x.RealityObject.TypeProject.Name,
                             CapitalGroup = x.RealityObject.CapitalGroup.Name,
                             AreaMkd = x.RealityObject.AreaMkd.HasValue ? x.RealityObject.AreaMkd.Value : 0M,
                             AreaLivingNotLivingMkd = x.RealityObject.AreaLivingNotLivingMkd.HasValue ? x.RealityObject.AreaLivingNotLivingMkd.Value : 0M,
                             AreaLiving = x.RealityObject.AreaLiving.HasValue ? x.RealityObject.AreaLiving.Value : 0M,
                             AreaLivingOwned = x.RealityObject.AreaLivingOwned.HasValue ? x.RealityObject.AreaLivingOwned.Value : 0M,
                             NumberApartments = x.RealityObject.NumberApartments.HasValue ? x.RealityObject.NumberApartments.Value : 0,
                             NumberLiving = x.RealityObject.NumberLiving.HasValue ? x.RealityObject.NumberLiving.Value : 0,
                             WallMaterial = x.RealityObject.WallMaterial.Name,
                             RoofingMaterial = x.RealityObject.RoofingMaterial.Name,
                             DateCommissioning = x.RealityObject.DateCommissioning,
                             PhysicalWear = x.RealityObject.PhysicalWear.HasValue ? x.RealityObject.PhysicalWear.Value : 0M,
                             DateLastOverhaul = x.RealityObject.DateLastOverhaul
                         })
                         .ToList();
        }

        private List<List<long>> GetMunicipalitys()
        {
            var municipalityList = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                .OrderBy(x => x.Name)
                .ToList();

            var alphabeticalGroups = new List<List<long>>();

            var lastGroup = "extraordinaryString";

            foreach (var municipality in municipalityList)
            {
                if (municipality.Group != lastGroup)
                {
                    lastGroup = municipality.Group;
                    alphabeticalGroups.Add(new List<long>());
                }

                if (alphabeticalGroups.Any())
                {
                    alphabeticalGroups.Last().Add(municipality.Id);
                }
            }

            return alphabeticalGroups;
        }

        private Dictionary<long, ManagerInfoProxy> GetManagerInfo()
        {
            return this.Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                         .Where(x => x.Position.Code == "1" || x.Position.Code == "4")
                         .WhereIf(this.reportDate != DateTime.MinValue, x => x.DateEndWork >= this.reportDate || x.DateEndWork == null || x.DateEndWork <= DateTime.MinValue)
                         .WhereIf(this.reportDate != DateTime.MinValue, x => x.DateStartWork <= this.reportDate || x.DateEndWork == null || x.DateEndWork <= DateTime.MinValue)
                         .Select(
                             x =>
                             new
                             {
                                 contragentId = x.Contragent.Id,
                                 Fio = $"{x.Surname} {x.Name} {x.Patronymic}",
                                 x.Phone,
                                 x.Email
                             })
                         .AsEnumerable()
                         .GroupBy(x => x.contragentId)
                         .ToDictionary(x => x.Key, x => x.Select(y => new ManagerInfoProxy { Fio = y.Fio, Phone = y.Phone, Email = y.Email }).FirstOrDefault());
        }

        private Dictionary<long, ManOrgInfoProxy> GetManOrgRealityObj()
        {
            var realtyObjIdsQuery = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => x.RealityObject.Id);

            var managingOrgRealityObjectQuery = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => realtyObjIdsQuery.Contains(x.RealityObject.Id))
                    .Where(x => x.ManOrgContract.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet
                    || x.ManOrgContract.ManagingOrganization.ActivityDateEnd > this.reportDate)
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= this.reportDate)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= this.reportDate)
                    .Where(x => x.ManOrgContract.ManagingOrganization != null);

            // дома УО (исключая ТСЖ, передавшие управление)
            return managingOrgRealityObjectQuery
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.ManOrgContract.StartDate,
                    ContractId = x.ManOrgContract.Id,
                    ContragentId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                    TypeManagement = (TypeManagementManOrg?)x.ManOrgContract.ManagingOrganization.TypeManagement,
                    x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    x.ManOrgContract.ManagingOrganization.Contragent.JuridicalAddress,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasJuridicalAddress.PostCode,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasJuridicalAddress.StreetName,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasJuridicalAddress.House,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasJuridicalAddress.Flat,
                    x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                    x.ManOrgContract.ManagingOrganization.Contragent.Kpp,
                    x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                    x.ManOrgContract.ManagingOrganization.Contragent.Email,
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                x => x.Key,
                x =>
                {
                        var tmpDataTsjUk = x.OrderBy(y => y.StartDate).ToList();

                        var tmpDataTsjUkNames = tmpDataTsjUk.Select(y => y.Name).ToList();
                    
                        var tmpData = x.OrderByDescending(y => y.StartDate).First();
                        
                        var contracts = x.Where(y => y.StartDate == tmpData.StartDate).ToList();

                        if (contracts.Count > 1)
                        {
                            var res = contracts.FirstOrDefault(y => y.TypeManagement == TypeManagementManOrg.UK);

                            if (res != null)
                            {
                                tmpData = res;
                            }
                        }
                        
                        return new ManOrgInfoProxy
                               {
                                   ContragentId = tmpData.ContragentId,
                                   TypeManagement = tmpData.TypeManagement.HasValue ? tmpData.TypeManagement.Value.GetEnumMeta().Display : "Непосредственное управление",
                                   Name = tmpDataTsjUkNames.Aggregate(string.Empty, (first, second) => first + (!string.IsNullOrEmpty(first) ? "(обслуживание передано " + second + ")" : second)),
                                   JuridicalAddress = tmpData.JuridicalAddress,
                                   PostCode = tmpData.PostCode,
                                   StreetName = tmpData.StreetName,
                                   House = tmpData.House,
                                   Flat = tmpData.Flat,
                                   Inn = tmpData.Inn,
                                   Kpp = tmpData.Kpp,
                                   Phone = tmpData.Phone,
                                   Email = tmpData.Email
                               };
                });
        }

        private Dictionary<long, string> GetDateLastOverhaulByRo()
        {
            return
                this.Container.Resolve<IDomainService<ObjectCr>>()
                    .GetAll()
                    .Join(
                        this.Container.Resolve<IDomainService<TehPassport>>().GetAll(),
                        x => x.RealityObject.Id,
                        y => y.RealityObject.Id,
                        (a, b) => new { ObjectCr = a, TehPassport = b })
                    .Join(
                        this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll(),
                        x => x.TehPassport.Id,
                        y => y.TehPassport.Id,
                        (c, d) => new { c.ObjectCr, c.TehPassport, TehPassportValue = d })
                    .WhereIf(
                        this.municipalityIds.Length > 0,
                        x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                    .Where(x => x.TehPassportValue.FormCode == "Form_1" && x.TehPassportValue.CellCode == "4:1")
                    .Select(x => new { realtyObjId = x.ObjectCr.RealityObject.Id, x.TehPassportValue.Value })
                    .AsEnumerable()
                    .GroupBy(x => x.realtyObjId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Value).FirstOrDefault());
        }

        // Данные по средствам источников финансирования
        private List<FinSourceResourceFullProxy> GetFinSourceResourceData()
        {
            var data = this.Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    crObjId = x.ObjectCr.Id,
                    municipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                    finSourceId = x.FinanceSource.Id,
                    x.FinanceSource.TypeFinanceGroup,
                    x.FundResource,
                    x.BudgetSubject,
                    x.BudgetMu,
                    x.OwnerResource
                })
                .AsEnumerable()
                .Distinct()
                .Select(x => new FinSourceResourceFullProxy
                {
                    crObjId = x.crObjId,
                    municipalityId = x.municipalityId,
                    finSourceId = x.finSourceId,
                    TypeFinanceGroup = x.TypeFinanceGroup,
                    FundResource = decimal.Round(x.FundResource ?? 0, 2),
                    BudgetSubject = decimal.Round(x.BudgetSubject ?? 0, 2),
                    BudgetMu = decimal.Round(x.BudgetMu ?? 0, 2),
                    OwnerResource = decimal.Round(x.OwnerResource ?? 0, 2)
                })
                .ToList();

            return data;
        }

        /// <summary>
        /// Возвращает средства источников финансирования (ст. 33-36) по объектам кр
        /// </summary>
        private Dictionary<long, Dictionary<TypeFinanceGroup, Dictionary<long, FinSourceResourceProxy>>> GetCrObjFinSourceResourceDictByCrObj(List<FinSourceResourceFullProxy> data)
        {
            return data
                .GroupBy(x => x.crObjId)
                .ToDictionary(
                x => x.Key, 
                x => x.GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.finSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => new FinSourceResourceProxy
                            {
                                FundResource = z.Sum(p => p.FundResource).ToDecimal(),
                                BudgetMu = z.Sum(p => p.BudgetMu).ToDecimal(),
                                OwnerResource = z.Sum(p => p.OwnerResource).ToDecimal(),
                                BudgetSubject = z.Sum(p => p.BudgetSubject).ToDecimal()
                            })));
        }

        /// <summary>
        /// Возвращает итоговые значения средств источников финансирования (ст. 33-36) по муниципальным образованиям
        /// </summary>
        private Dictionary<long, Dictionary<TypeFinanceGroup, Dictionary<long, FinSourceResourceProxy>>> GetCrObjFinSourceResourceDictByMu(List<FinSourceResourceFullProxy> data)
        {
            return data
                .GroupBy(x => x.municipalityId)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.finSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => new FinSourceResourceProxy
                            {
                                FundResource = z.Sum(p => p.FundResource).ToDecimal(),
                                BudgetMu = z.Sum(p => p.BudgetMu).ToDecimal(),
                                OwnerResource = z.Sum(p => p.OwnerResource).ToDecimal(),
                                BudgetSubject = z.Sum(p => p.BudgetSubject).ToDecimal()
                            })));
        }

        /// <summary>
        /// Возвращает итоговые значения средств источников финансирования (ст. 33-36) по группам муниципальных образований
        /// </summary>
        private Dictionary<string, Dictionary<TypeFinanceGroup, Dictionary<long, FinSourceResourceProxy>>> GetCrObjFinSourceResourceDictByMuGr(List<FinSourceResourceFullProxy> data)
        {
            return data
                .Select(x => new
                {
                    group = this.municipalityInfoDict.ContainsKey(x.municipalityId) ? this.municipalityInfoDict[x.municipalityId].Group : string.Empty,
                    x.crObjId,
                    x.finSourceId,
                    x.TypeFinanceGroup,
                    x.FundResource,
                    x.BudgetSubject,
                    x.BudgetMu,
                    x.OwnerResource
                })
                .AsEnumerable()
                .Distinct()
                .GroupBy(x => x.group)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.finSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => new FinSourceResourceProxy
                            {
                                FundResource = z.Sum(p => p.FundResource).ToDecimal(),
                                BudgetMu = z.Sum(p => p.BudgetMu).ToDecimal(),
                                OwnerResource = z.Sum(p => p.OwnerResource).ToDecimal(),
                                BudgetSubject = z.Sum(p => p.BudgetSubject).ToDecimal()
                            })));
        }

        /// <summary>
        /// Возвращает итоговые значения средств источников финансирования (ст. 33-36)
        /// </summary>
        private Dictionary<TypeFinanceGroup, Dictionary<long, FinSourceResourceProxy>> GetCrObjFinSourceResourceDictTotal(List<FinSourceResourceFullProxy> data)
        {
            return data
                .GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.finSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => new FinSourceResourceProxy
                            {
                                FundResource = z.Sum(p => p.FundResource).ToDecimal(),
                                BudgetMu = z.Sum(p => p.BudgetMu).ToDecimal(),
                                OwnerResource = z.Sum(p => p.OwnerResource).ToDecimal(),
                                BudgetSubject = z.Sum(p => p.BudgetSubject).ToDecimal()
                            }));
        }

        // Данные по работам
        private List<TypeWorkCrFullProxy> GetTypeWorkData()
        {
            var data = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    municipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                    crObjId = x.ObjectCr.Id,
                    FinanceSourceId = x.FinanceSource.Id,
                    x.FinanceSource.TypeFinanceGroup,
                    x.Volume,
                    x.Sum,
                    x.Work.Code
                })
                .AsEnumerable()
                .Select(x => new TypeWorkCrFullProxy
                {
                    municipalityId = x.municipalityId,
                    crObjId = x.crObjId,
                    FinanceSourceId = x.FinanceSourceId,
                    Code = x.Code,
                    Sum = decimal.Round(x.Sum ?? 0, 2),
                    Volume = x.Volume ?? 0,
                    TypeFinanceGroup = x.TypeFinanceGroup
                })
                .ToList();

            return data;
        }

        /// <summary>
        /// Возвращает значения суммы и объема по кодам работ (ст. 39-91) по домам
        /// </summary>
        private Dictionary<long, Dictionary<TypeFinanceGroup, Dictionary<long, Dictionary<string, TypeWorkCrProxy>>>> GetWorksInfoByCrObj(List<TypeWorkCrFullProxy> data)
        {
            return data
                .GroupBy(x => x.crObjId)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.FinanceSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => z.GroupBy(p => p.Code)
                                  .ToDictionary(
                                  p => p.Key,
                                  p => new TypeWorkCrProxy
                                  {
                                      Volume = p.Sum(r => r.Volume).ToDecimal(),
                                      Sum = p.Sum(r => r.Sum).ToDecimal(),
                                  }))));
        }

        /// <summary>
        /// Возвращает итоговые значения суммы и объема по кодам работ (ст. 39-91) по муниципальным образованиям
        /// </summary>
        private Dictionary<long, Dictionary<TypeFinanceGroup, Dictionary<long, Dictionary<string, TypeWorkCrProxy>>>> GetWorksInfoByMu(List<TypeWorkCrFullProxy> data)
        {
            return data
                .GroupBy(x => x.municipalityId)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.FinanceSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => z.GroupBy(p => p.Code)
                                  .ToDictionary(
                                  p => p.Key,
                                  p => new TypeWorkCrProxy
                                  {
                                      Volume = p.Sum(r => r.Volume).ToDecimal(),
                                      Sum = p.Sum(r => r.Sum).ToDecimal(),
                                  }))));
        }

        /// <summary>
        /// Возвращает итоговые значения суммы и объема по кодам работ (ст. 39-91) по группам муниципальных образований
        /// </summary>
        private Dictionary<string, Dictionary<TypeFinanceGroup, Dictionary<long, Dictionary<string, TypeWorkCrProxy>>>> GetWorksInfoByMuGr(List<TypeWorkCrFullProxy> data)
        {
            return data
                .Select(x => new
                {
                    group = this.municipalityInfoDict.ContainsKey(x.municipalityId) ? this.municipalityInfoDict[x.municipalityId].Group : string.Empty,
                    x.crObjId,
                    x.FinanceSourceId,
                    x.TypeFinanceGroup,
                    x.Volume,
                    x.Sum,
                    x.Code
                })
                .AsEnumerable()
                .GroupBy(x => x.group)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.FinanceSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => z.GroupBy(p => p.Code)
                                  .ToDictionary(
                                  p => p.Key,
                                  p => new TypeWorkCrProxy
                                  {
                                      Volume = p.Sum(r => r.Volume).ToDecimal(),
                                      Sum = p.Sum(r => r.Sum).ToDecimal(),
                                  }))));
        }

        /// <summary>
        /// Возвращает итоговые значения суммы и объема по кодам работ (ст. 39-91)
        /// </summary>
        private Dictionary<TypeFinanceGroup, Dictionary<long, Dictionary<string, TypeWorkCrProxy>>> GetWorksInfoTotal(List<TypeWorkCrFullProxy> data)
        {
            return data
                .GroupBy(y => y.TypeFinanceGroup)
                      .ToDictionary(
                      y => y.Key,
                      y => y.GroupBy(z => z.FinanceSourceId)
                            .ToDictionary(
                            z => z.Key,
                            z => z.GroupBy(p => p.Code)
                                  .ToDictionary(
                                  p => p.Key,
                                  p => new TypeWorkCrProxy
                                  {
                                      Volume = p.Sum(r => r.Volume).ToDecimal(),
                                      Sum = p.Sum(r => r.Sum).ToDecimal(),
                                  })));
        }
        
        private Dictionary<long, Dictionary<long, decimal>> GetProgramLimitsDict()
        {
            return this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                        .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                        .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId || x.ObjectCr.ProgramCr.Id == this.dopProgramCrId)
                        .Select(x =>
                            new
                            {
                                ProgramCrId = x.ObjectCr.ProgramCr.Id,
                                MunicipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                                Sum = x.Sum != null ? x.Sum.Value : 0M
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.ProgramCrId)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.MunicipalityId).ToDictionary(y => y.Key, y => y.Sum(z => decimal.Round(z.Sum, 2))));
        }

        private IEnumerable<string> GetWorkCodes()
        {
            if (this.workCodes == null)
            {
                this.workCodes = new List<string>();

                for (var i = 1; i <= 31; ++i)
                {
                    this.workCodes.Add(i.ToString());
                }

                // Код работы "Изготовление технических паспортов"
                this.workCodes.Add("1021");

                // Оснащение придомовой территории системой видеонаблюдения
                this.workCodes.Add("100");

                // Восстановление частей имущества, не входящего в состав общего имущества в МКД, 
                // демонтированного при капремонте в составе общего имущества в МКД, 
                // а также восстановление благоустройства после окончания ремонтных работ
                this.workCodes.Add("101");

                // Благоустройство придомовой территории (с установкой детских игровых площадок и спортивных комплексов) 
                // при комплексном капитальном ремонте многоквартирных домов
                this.workCodes.Add("102");

                // Размещение на фасадах многоквартирных домов социальной рекламы
                this.workCodes.Add("103");

                // Установка ИТП
                this.workCodes.Add("88");

                //Ремонт вентиляционных каналов и дымоходов
                this.workCodes.Add("145");

                //Ремонт лифтов
                this.workCodes.Add("141");

                //Замена лифтов
                this.workCodes.Add("142");

                //Модернизация лифтов
                this.workCodes.Add("143");

                //Замена отопительных приборов (радиаторов)
                this.workCodes.Add("2101");

                //Замена полотенцесушителей
                this.workCodes.Add("2102");

                //Ремонт системы противопожарной защиты многоквартирного дома
                this.workCodes.Add("2103");

                //Проведение работ по технической инвентаризации многоквартирных домов и изготовление технических паспортов
                this.workCodes.Add("2104");

                //Установка индивидуальных тепловых пунктов (ИТП)
                this.workCodes.Add("2105");

                //Оборудование входных групп подъездов пандусами
                this.workCodes.Add("2106");
                
                //Ремонт входных групп
                this.workCodes.Add("2107");
                
                //Ремонт спусков в подвал
                this.workCodes.Add("2108");
                
                //Усиление конструкций балконов
                this.workCodes.Add("2109");
            }

            return this.workCodes;
        }

        private void FillObjCrSection(Section section, RealtyObjInfoProxy realtyObjInfo)
        {
            var objectCr = realtyObjInfo.objectCr;
            var managingOrganization = realtyObjInfo.manOrgInfo;
            var managerInfo = realtyObjInfo.managerInfo;

            section["НомерОбъекта"] = realtyObjInfo.number;
            section["НомПоМО"] = realtyObjInfo.numberByMu;
            section["ТипУправления"] = managingOrganization != null ? managingOrganization.TypeManagement : string.Empty;
            section["ФИО"] = managerInfo.Fio;
            section["Адрес"] = objectCr.Address;
            section["Этажность"] = objectCr.RealityObjectFloors;
            section["Серия"] = objectCr.SeriesHome;
            section["ГруппаКапитальности"] = objectCr.CapitalGroup;
            section["ОбщаяПл"] = objectCr.AreaMkd;
            section["ПлЖилИНеЖил"] = objectCr.AreaLivingNotLivingMkd;
            section["ЖилаяПл"] = objectCr.AreaLiving;
            section["ЖилаяПлГр"] = objectCr.AreaLivingOwned;
            section["КолКвартир"] = objectCr.NumberApartments;
            section["КолПроживающих"] = objectCr.NumberLiving;
            section["МатериалСтен"] = objectCr.WallMaterial;
            section["МатериалКровли"] = objectCr.RoofingMaterial;
            section["ГодСдачиВЭкспл"] = objectCr.DateCommissioning.HasValue ? objectCr.DateCommissioning.Value.Year.ToStr() : string.Empty;
            section["ФизичИзнос"] = objectCr.PhysicalWear;
            section["ГодПоследнегоКапРем"] = this.dateLastOverhaulByRoDict.ContainsKey(objectCr.RealityObjectId) ? this.dateLastOverhaulByRoDict[objectCr.RealityObjectId] : string.Empty;

            if (managingOrganization == null) return;

            section["УпрОрг"] = managingOrganization.Name;
            section["НасПунктУпрОрг"] = managingOrganization.JuridicalAddress;

            if (managingOrganization.JuridicalAddress != null)
            {
                section["Индекс"] = managingOrganization.PostCode;
                section["Улица"] = managingOrganization.StreetName;
                section["Дом"] = managingOrganization.House;
                section["Квартира"] = managingOrganization.Flat;
            }

            section["ИНН"] = managingOrganization.Inn;
            section["КПП"] = managingOrganization.Kpp;
            section["Телефон"] = !string.IsNullOrEmpty(managerInfo.Phone) ? managerInfo.Phone : managingOrganization.Phone ?? string.Empty;
            section["ЭлПочта"] = !string.IsNullOrEmpty(managerInfo.Email) ? managerInfo.Email : managingOrganization.Email ?? string.Empty;
        }

        private void FillFinanceSection(Section section, FinSourceResourceProxy finSourceResource, Dictionary<string, TypeWorkCrProxy> worksDict)
        {
            // в данную ячейку идет сумма по работам, а не сумма 33-36 (реализовано как в Б3)
            section["СуммаПоВидам"] = worksDict.Values.Select(x => x.Sum).Sum();

            section["СредстваФонда"] = finSourceResource.FundResource;
            section["БюджетРТ"] = finSourceResource.BudgetSubject;
            section["БюджетМО"] = finSourceResource.BudgetMu;
            section["СредстваСобственников"] = finSourceResource.OwnerResource;

            section["СуммаНаРазИЭкспПСД"] = worksDict.Where(x => x.Key == "1018" || x.Key == "1019").Sum(x => x.Value.Sum);
            section["СуммаНаТехНадзор"] = worksDict.ContainsKey("1020") ? worksDict["1020"].Sum : 0;

            foreach (var workCode in this.GetWorkCodes())
            {
                var sumCell = $"Сумма{workCode}";
                var volumeCell = $"Объем{workCode}";

                if (worksDict.ContainsKey(workCode))
                {
                    var work = worksDict[workCode];

                    section[sumCell] = work.Sum;
                    section[volumeCell] = work.Volume;
                }
                else
                {
                    section[sumCell] = 0;
                    section[volumeCell] = 0;
                }
            }
        }

        private void FillTotalAreaSection(Section section, RealtyObjectFeatureValues realtyObjectFeatureValues)
        {
            section["ОбщаяПл"] = realtyObjectFeatureValues.AreaMkd;
            section["ПлЖилИНеЖил"] = realtyObjectFeatureValues.AreaLivingNotLivingMkd;
            section["ЖилаяПл"] = realtyObjectFeatureValues.AreaLiving;
            section["ЖилаяПлГр"] = realtyObjectFeatureValues.AreaLivingOwned;
            section["КолКвартир"] = realtyObjectFeatureValues.NumberApartments;
            section["КолПроживающих"] = realtyObjectFeatureValues.NumberLiving;
        }

        private void AttachDictionaries(Dictionary<string, TypeWorkCrProxy> totals, Dictionary<string, TypeWorkCrProxy> addable)
        {
            foreach (var key in addable.Keys)
            {
                if (totals.ContainsKey(key))
                {
                    totals[key] += addable[key];
                }
                else
                {
                    totals[key] = addable[key];
                }
            }
        }

        private void AttachDictionaries(Dictionary<TypeFinanceGroup, List<long>> totals, Dictionary<TypeFinanceGroup, List<long>> addable)
        {
            foreach (var key in addable.Keys)
            {
                if (totals.ContainsKey(key))
                {
                    var tmpList = totals[key].Union(addable[key]).Distinct().ToList();
                    totals[key] = tmpList;
                }
                else
                {
                    totals[key] = addable[key];
                }
            }
        }

        /// <summary>
        /// Метод создает и заполняет динамически строки - по Источникам финансирования, Группам финансирования и Итого по всем группам
        /// </summary>
        private void FillRows(
            Dictionary<TypeFinanceGroup, Dictionary<long, FinSourceResourceProxy>> finSourceResourceDict,
            Dictionary<TypeFinanceGroup, Dictionary<long, Dictionary<string, TypeWorkCrProxy>>> worksDict,
            RealtyObjInfoProxy realtyObjInfo,
            RealtyObjectFeatureValues realtyObjectFeatureValues,
            string groupName, 
            Section totalSection, 
            Section sourceGroupSection, 
            Section sourceSection)
        {
            // словарь Разрезов финансирования по группам финансирования
            var finSourcesIdsDict = new Dictionary<TypeFinanceGroup, List<long>>();
            var finSourcesIdsDict2 = new Dictionary<TypeFinanceGroup, List<long>>();
            if (finSourceResourceDict != null)
            {
                finSourcesIdsDict = finSourceResourceDict.ToDictionary(x => x.Key, x => x.Value.Keys.ToList());
            }

            if (worksDict != null)
            {
                finSourcesIdsDict2 = worksDict.ToDictionary(x => x.Key, x => x.Value.Keys.ToList());
            }

            this.AttachDictionaries(finSourcesIdsDict, finSourcesIdsDict2);

            if (finSourcesIdsDict.Count == 0)
            {
                return;
            }

            totalSection.ДобавитьСтроку();
            if (groupName != null)
            {
                totalSection["НаименованиеГруппы"] = groupName;
            }

            var totalsRoWorks = new Dictionary<string, TypeWorkCrProxy>();
            var totalsRoResourceBySource = new FinSourceResourceProxy();

            foreach (var finSourceGroup in finSourcesIdsDict)
            {
                sourceGroupSection.ДобавитьСтроку();
                sourceGroupSection["ГруппаИсточников"] = finSourceGroup.Key.GetEnumMeta().Display;

                if (groupName != null)
                {
                    sourceGroupSection["НаименованиеГруппы"] = groupName;
                }

                // итоги по группе источников финансирования
                var totalsGroupWorks = new Dictionary<string, TypeWorkCrProxy>();
                var totalsGroupResourceBySource = new FinSourceResourceProxy();

                foreach (var finSourceId in finSourceGroup.Value)
                {
                    sourceSection.ДобавитьСтроку();
                    sourceSection["ИсточникФинансирования"] = this.finSourcesDict[finSourceId];

                    if (groupName != null)
                    {
                        sourceSection["НаименованиеГруппы"] = groupName;
                    }

                    var resourceBySource = finSourceResourceDict != null && finSourceResourceDict.ContainsKey(finSourceGroup.Key) && finSourceResourceDict[finSourceGroup.Key].ContainsKey(finSourceId) 
                        ? finSourceResourceDict[finSourceGroup.Key][finSourceId] 
                        : new FinSourceResourceProxy();

                    var works = worksDict != null && worksDict.ContainsKey(finSourceGroup.Key) && worksDict[finSourceGroup.Key].ContainsKey(finSourceId)
                        ? worksDict[finSourceGroup.Key][finSourceId]
                        : new Dictionary<string, TypeWorkCrProxy>();

                    if (realtyObjInfo != null)
                    {
                        this.FillObjCrSection(sourceSection, realtyObjInfo);
                    }

                    this.FillFinanceSection(sourceSection, resourceBySource, works);

                    // заполнение итогов по группе источников финансирования
                    this.AttachDictionaries(totalsGroupWorks, works);
                    totalsGroupResourceBySource += resourceBySource;
                }

                if (realtyObjInfo != null)
                {
                    this.FillObjCrSection(sourceGroupSection, realtyObjInfo);
                }

                this.FillFinanceSection(sourceGroupSection, totalsGroupResourceBySource, totalsGroupWorks);

                // заполнение итогов
                this.AttachDictionaries(totalsRoWorks, totalsGroupWorks);
                totalsRoResourceBySource += totalsGroupResourceBySource;
            }

            if (realtyObjInfo != null)
            {
                this.FillObjCrSection(totalSection, realtyObjInfo);
            }

            if (realtyObjectFeatureValues != null)
            {
                this.FillTotalAreaSection(totalSection, realtyObjectFeatureValues);
            }

            this.FillFinanceSection(totalSection, totalsRoResourceBySource, totalsRoWorks);
        }
    }

    internal class ManOrgInfoProxy
    {
        public string TypeManagement;

        public long? ContragentId;

        public string Name;

        public string JuridicalAddress;

        public string PostCode;

        public string StreetName;

        public string House;

        public string Flat;

        public string Inn;

        public string Kpp;

        public string Phone;

        public string Email;
    }

    internal class ManagerInfoProxy
    {
        public string Fio;

        public string Phone;

        public string Email;
    }

    internal class FinSourceResourceFullProxy
    {
        public long crObjId;

        public long finSourceId;

        public long municipalityId;

        public decimal FundResource;

        public decimal BudgetSubject;

        public decimal BudgetMu;

        public decimal OwnerResource;

        public TypeFinanceGroup TypeFinanceGroup;
    }

    internal class FinSourceResourceProxy
    {
        public decimal FundResource;

        public decimal BudgetSubject;

        public decimal BudgetMu;

        public decimal OwnerResource;

        public static FinSourceResourceProxy operator +(FinSourceResourceProxy obj1, FinSourceResourceProxy obj2)
        {
            return new FinSourceResourceProxy
                       {
                           FundResource = obj1.FundResource + obj2.FundResource,
                           BudgetSubject = obj1.BudgetSubject + obj2.BudgetSubject,
                           BudgetMu = obj1.BudgetMu + obj2.BudgetMu,
                           OwnerResource = obj1.OwnerResource + obj2.OwnerResource
                       };
        }
    }

    internal sealed class ObjectCrProxy
    {
        public long Id { get; set; }

        public long RealityObjectId { get; set; }

        public string Address { get; set; }

        public long MunicipalityId { get; set; }

        public string Municipality { get; set; }

        public string MunicipalityGroup { get; set; }

        public int? RealityObjectFloors { get; set; }

        public string SeriesHome { get; set; }

        public string CapitalGroup { get; set; }

        public decimal AreaMkd { get; set; }

        public decimal AreaLivingNotLivingMkd { get; set; }

        public decimal AreaLiving { get; set; }

        public decimal AreaLivingOwned { get; set; }

        public int NumberApartments { get; set; }

        public int NumberLiving { get; set; }

        public string WallMaterial { get; set; }

        public string RoofingMaterial { get; set; }

        public DateTime? DateCommissioning { get; set; }

        public decimal PhysicalWear { get; set; }

        public DateTime? DateLastOverhaul { get; set; }
    }

    internal sealed class RealtyObjInfoProxy
    {
        public ObjectCrProxy objectCr;

        public ManOrgInfoProxy manOrgInfo;

        public ManagerInfoProxy managerInfo;

        public int numberByMu;

        public int number;
    }

    internal sealed class TypeWorkCrFullProxy
    {
        public long municipalityId { get; set; }

        public long crObjId { get; set; }

        public long FinanceSourceId { get; set; }

        public TypeFinanceGroup TypeFinanceGroup { get; set; }

        public string Code { get; set; }

        public decimal Volume { get; set; }

        public decimal Sum { get; set; }
    }

    internal sealed class TypeWorkCrProxy
    {
        public decimal Volume { get; set; }

        public decimal Sum { get; set; }

        public static TypeWorkCrProxy operator +(TypeWorkCrProxy obj1, TypeWorkCrProxy obj2)
        {
            return new TypeWorkCrProxy { Sum = obj1.Sum + obj2.Sum, Volume = obj1.Volume + obj2.Volume };
        }
    }

    internal sealed class MunicipalityInfoProxy
    {
        public string Name;

        public string Group;
    }

    internal class RealtyObjectFeatureValues
    {
        public decimal? AreaMkd;

        public decimal? AreaLivingNotLivingMkd;

        public decimal? AreaLiving;

        public decimal? AreaLivingOwned;

        public int? NumberApartments;

        public int? NumberLiving;
    }
}