namespace Bars.GkhGji.Report.Form123
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.MicroKernel.ModelBuilder.Descriptors;
    using Castle.Windsor;

    /// <summary>
    /// Отчет "Форма 123"
    /// </summary>
    public class Form123Report : BasePrintForm
    {
        private readonly string[] listCodeJur = new[] { "0", "9", "11", "8", "15", "18", "4" };
        private readonly string[] listCodeOffic = new[] { "1", "10", "12", "13", "16", "19", "5" };
        private readonly string[] listCodeIndivid = new[] { "6", "7", "14" };

        private readonly ReportData reportData = new ReportData();
        private readonly ReportData dataZji = new ReportData();

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        // учитывать постановления, возвращенные на новое рассмотрение
        private bool returned;

        #region Ссылки на данные столбцов

        private List<long> column4;
        private List<long> column9;
        private List<long> column15;
        private List<long> column17;

        #endregion

        private Dictionary<long, ActCheckRobjectData> actCheckRoDict;
        private Dictionary<long, long> municipalityByActDict;

        public Form123Report()
            : base(new ReportTemplateBinary(Properties.Resources.Form123))
        {
        }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.Form123"; }
        }

        public override string Desciption
        {
            get { return "Отчет \"Форма 123\""; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Form123"; }
        }

        public override string Name
        {
            get { return "Отчет \"Форма 123\""; }
        }

        public IWindsorContainer Container { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            returned = baseParams.Params["returned"].ToBool();

            var strMunIds = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(strMunIds) ? strMunIds.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["началоПериода"] = dateStart.ToString("d MMMM");
            reportParams.SimpleReportParams["конецПериода"] = dateEnd.ToString("d MMMM");
            reportParams.SimpleReportParams["год"] = dateEnd.Year;

            // секция для вывода наименования зжи и итогов по этой зжи
            var sectionZji = reportParams.ComplexReportParams.ДобавитьСекцию("SectionZji");

            /*Словарь ЗЖИ
              Ключ: ЗЖИ
              Значение: Список Муниц. образований
            */
            var dictZji = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .Select(x => new
                    {
                        zjiId = x.ZonalInspection.Id,
                        zjiName = x.ZonalInspection.Name,
                        muId = x.Municipality.Id,
                        muName = x.Municipality.Name
                    })
                .AsEnumerable()
                .GroupBy(x => new { x.zjiId, x.zjiName })
                .Select(x => new
                    {
                        zonalInspection = new ZonalInspection { Id = x.Key.zjiId, Name = x.Key.zjiName },
                        municipalities = x.Select(y => new Municipality{ Id = y.muId, Name = y.muName }).ToArray()
                    })
                .ToDictionary(x => x.zonalInspection, x => x.municipalities);

            // получение данных
            FillData();

            foreach (var zji in dictZji)
            {
                sectionZji.ДобавитьСтроку();
                sectionZji["ЗЖИ"] = zji.Key.ZoneName;

                // информация по зжи (по муниципальному образованию)
                var sectionData = sectionZji.ДобавитьСекцию("SectionData");

                foreach (var municipality in zji.Value)
                {
                    sectionData.ДобавитьСтроку();
                    sectionData["Район"] = municipality.Name;

                    this.FillRow(sectionData, municipality.Id);
                }

                this.SumZji(zji);
                FillRow(sectionZji, dataZji, zji.Key.Id);
            }

            FillTotalRow(reportParams, this.SumZjiTotal(dictZji));
        }

        private void FillData()
        {
            this.GetActData(); 
            this.GetViolationData(); 
            this.GetDataPrescriptionIssued(); 
            this.GetDataProtocols();
            this.GetDataActRemoval();
            this.GetDataResolution();
        }

        private void GetActData()
        {
            var serviceActCheckRealityObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceManagingOrganization = Container.Resolve<IDomainService<ManagingOrganization>>();
            var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceActCheck = Container.Resolve<IDomainService<ActCheck>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            var queryActCheckRo = serviceActCheckRealityObject.GetAll()
                         .WhereIf(dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= dateEnd)
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Where(x => x.RealityObject != null)
                         .Where(x => x.ActCheck.TypeDocumentGji == TypeDocumentGji.ActCheck);

            municipalityByActDict = queryActCheckRo
                         .Where(x => x.ActCheck != null && x.RealityObject != null && x.RealityObject.Municipality != null)
                         .Select(x => new { actid = x.ActCheck.Id, municipalityId = x.RealityObject.Municipality.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.actid)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.municipalityId).First());

            var actCheckIds = queryActCheckRo.Select(x => x.ActCheck.Id).Distinct();

            var queryActCheckIdDisposalId = serviceDocumentGjiChildren.GetAll()
                                          .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                          .Where(x => actCheckIds.Contains(x.Children.Id));

            var listActCheckIdDisposalId = queryActCheckIdDisposalId
                                          .Select(x => new ParentChildProxy(x.Parent.Id, x.Children.Id))
                                          .AsEnumerable()
                                          .Distinct()
                                          .ToList();

            var queryActId = queryActCheckIdDisposalId.Select(x => x.Children.Id).Distinct();
            var queryDisposalId = queryActCheckIdDisposalId.Select(x => x.Parent.Id).Distinct();

            var queryListActCheck = serviceActCheck.GetAll().Where(x => queryActId.Contains(x.Id));
            var listActCheck = queryListActCheck.Select(x => new
            {
                x.Id,
                x.Area,
                InspectionContragentId = (long?)x.Inspection.Contragent.Id,
                InspectionTypeBase = x.Inspection.TypeBase
            })
                                                .AsEnumerable()
                                                .Select(x =>
                                                {
                                                    var res = new ActCheckProxy
                                                    {
                                                        Id = x.Id,
                                                        Area = x.Area,
                                                        InspectionTypeBase = x.InspectionTypeBase
                                                    };

                                                    if (x.InspectionContragentId != null && x.InspectionContragentId > 0)
                                                    {
                                                        res.InspectionContragentId = x.InspectionContragentId;
                                                    }

                                                    return res;
                                                })
                                                .ToList();

            var listDisposal = serviceDisposal.GetAll()
                                             .Where(x => queryDisposalId.Contains(x.Id))
                                             .Select(x => new
                                             {
                                                 x.Id,
                                                 KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0,
                                                 InspectionTypeBase = x.Inspection.TypeBase,
                                                 InspectionPersonInspection = x.Inspection.PersonInspection,
                                                 x.TypeDisposal
                                             })
                                             .AsEnumerable()
                                             .Select(x => new DisposalProxy
                                             {
                                                 Id = x.Id,
                                                 KindCheckCode = x.KindCheckCode,
                                                 InspectionTypeBase = x.InspectionTypeBase,
                                                 InspectionPersonInspection = x.InspectionPersonInspection,
                                                 TypeDisposal = x.TypeDisposal
                                             })
                                            .ToList();

            var actCheckDict = listActCheck.Distinct().ToDictionary(x => x.Id, x => x);
            var disposalDict = listDisposal.Distinct().ToDictionary(x => x.Id, x => x);

            var queryContragentsIds = queryListActCheck.Where(x => x.Inspection.Contragent != null).Select(x => x.Inspection.Contragent.Id);

            var actTypeManagementList = serviceManagingOrganization.GetAll()
                                           .Where(x => queryContragentsIds.Contains(x.Contragent.Id))
                                           .Select(x => new { x.TypeManagement, contragentId = x.Contragent.Id })
                                           .ToList();

            var contragentTypeManagementDict = actTypeManagementList.GroupBy(x => x.contragentId)
                                                .ToDictionary(x => x.Key, x => x.Select(y => y.TypeManagement).First());

            var actTypeManagementDict = listActCheck.Where(x => x.InspectionContragentId != null && contragentTypeManagementDict.ContainsKey((long)x.InspectionContragentId))
                                                .ToDictionary(x => x.Id, x => contragentTypeManagementDict[(long)x.InspectionContragentId]);

            var listActCheckDisposal = listActCheckIdDisposalId
                .Where(x => municipalityByActDict.ContainsKey(x.child))
                .Select(x => new DisposalActCheckReport
                {
                    muId = municipalityByActDict[x.child],
                    TypeManagement = actTypeManagementDict.ContainsKey(x.child) ? actTypeManagementDict[x.child] : 0,
                    disposal = disposalDict.ContainsKey(x.parent) ? disposalDict[x.parent] : null,
                    actCheck = actCheckDict.ContainsKey(x.child) ? actCheckDict[x.child] : null
                })
                .Where(x => x.disposal != null && x.actCheck != null)
                .ToList();

            var actProxyList = listActCheckDisposal
                                .Select(x => new ActCheckProxy { Id = x.actCheck.Id, Area = x.actCheck.Area })
                                .ToList();

            actCheckRoDict = this.InitActCheckRoDict(actProxyList);

            GetActRobjectData(listActCheckDisposal);
            GetActCheckData(listActCheckDisposal);
        }

        private void GetActRobjectData(List<DisposalActCheckReport> actCheckDisposalList)
        {
            Func<IGrouping<long, DisposalActCheckReport>, CountAreaHousesActCheckReport> calcRoData = x => GetCountActCheckRo(x.Select(y => y.actCheck).ToList());

            // Плановое обледование
            var actsList = actCheckDisposalList
                .Where(x => (x.disposal.KindCheckCode == TypeCheck.PlannedExit || x.disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                            || x.actCheck.InspectionTypeBase == TypeBase.Inspection)
                .ToList();

            column4 = actsList.Select(x => x.actCheck.Id).ToList();

            var realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn4 = realityObjData.ToDictionary(x => x.Key, x => (long)x.Value.Count);

            reportData.DataColumn5 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);

            reportData.DataColumn6 = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)calcRoData(x).Count);
            reportData.DataColumn7 = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)calcRoData(x).Count);

            // Внеплановое обледование
            actsList = actCheckDisposalList
                .Where(x => (x.disposal.KindCheckCode == TypeCheck.NotPlannedExit
                                || x.disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation)
                         && (x.disposal.InspectionTypeBase != TypeBase.CitizenStatement))
                .ToList();

            column9 = actsList.Select(x => x.actCheck.Id).ToList();

            realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn9 = realityObjData.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn10 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);

            reportData.DataColumn11 = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)calcRoData(x).Count);
            reportData.DataColumn12 = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)calcRoData(x).Count);

            reportData.DataColumn13 = actsList.Where(x => x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                                              .Where(x => x.disposal.InspectionPersonInspection == PersonInspection.PhysPerson)
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)calcRoData(x).Count);

            // Инспекционное обследование
            actsList = actCheckDisposalList.Where(x => (x.disposal.KindCheckCode == TypeCheck.InspectionSurvey) && (x.disposal.InspectionTypeBase != TypeBase.Inspection))
                                           .ToList();

            column15 = actsList.Select(x => x.actCheck.Id).ToList();

            realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn15 = realityObjData.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn16 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);

            // по жалобам
            actsList = actCheckDisposalList.Where(x => x.disposal.InspectionTypeBase == TypeBase.CitizenStatement).ToList();
            column17 = actsList.Select(x => x.actCheck.Id).ToList();

            realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn17 = realityObjData.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn18 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);
        }

        private void GetActCheckData(List<DisposalActCheckReport> actCheckDisposalList)
        {
            var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceDisposalTypeSurvey = Container.Resolve<IDomainService<DisposalTypeSurvey>>();

            reportData.DataColumn54 = actCheckDisposalList.GroupBy(x => x.muId).ToDictionary(x => x.Key, x => (long)x.Select(y => y.actCheck.Id).Distinct().Count());

            reportData.DataColumn55 = actCheckDisposalList
                .Where(x => x.disposal.KindCheckCode == TypeCheck.PlannedExit
                            || x.disposal.KindCheckCode == TypeCheck.PlannedDocumentation
                            || x.actCheck.InspectionTypeBase == TypeBase.Inspection)
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actCheck.Id).Distinct().Count());

            var unplannedInspActs = actCheckDisposalList
                .Where(x => (x.disposal.KindCheckCode == TypeCheck.NotPlannedExit
                                || x.disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation)
                            && x.disposal.InspectionTypeBase != TypeBase.CitizenStatement)
                .ToList();

            reportData.DataColumn56 = unplannedInspActs.GroupBy(x => x.muId).ToDictionary(x => x.Key, x => (long)x.Select(y => y.actCheck.Id).Distinct().Count());

            // Количество актов проверки внеплановых проверок по исполн. предписания
            var listDisposalsId = unplannedInspActs.Where(x => x.disposal.TypeDisposal == TypeDisposalGji.DocumentGji).Select(x => x.disposal.Id).Distinct().ToList();
            var disposalsOfUnplannedInspByPrescription = new List<long>();

            var start = 0;

            while (start < listDisposalsId.Count)
            {
                var tmpId = listDisposalsId.Skip(start).Take(1000).ToArray();
                disposalsOfUnplannedInspByPrescription.AddRange(serviceDocumentGjiChildren.GetAll()
                         .Where(x => tmpId.Contains(x.Children.Id))
                         .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                         .Select(x => x.Children.Id));
                start += 1000;
            }

            reportData.DataColumn57 = unplannedInspActs
                .Where(x => disposalsOfUnplannedInspByPrescription.Contains(x.disposal.Id))
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actCheck.Id).Distinct().Count());

            reportData.DataColumn58 = actCheckDisposalList
                .Where(x => x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actCheck.Id).Distinct().Count());

            // по инспекционным обследованиям
            var inspectionalActs = actCheckDisposalList
                    .Where(x => x.disposal.KindCheckCode == TypeCheck.InspectionSurvey && x.disposal.InspectionTypeBase != TypeBase.Inspection)
                    .ToList();

            reportData.DataColumn59 = inspectionalActs.GroupBy(x => x.muId).ToDictionary(x => x.Key, x => (long)x.Select(y => y.actCheck.Id).Distinct().Count());

            var listDispId = inspectionalActs.Select(x => x.disposal.Id).Distinct().ToList();

            var str = 1000;
            var listDispIdFirst = listDispId.Count > str ? listDispId.Take(1000).ToList() : listDispId;

            var listDisposalTypeSurvey = serviceDisposalTypeSurvey.GetAll()
                                     .Where(x => listDispIdFirst.Contains(x.Disposal.Id))
                                     .Select(x => new { x.Disposal.Id, x.TypeSurvey.Code })
                                     .ToList();

            while (str < listDispId.Count)
            {
                var tmpList = listDispId.Skip(str).Take(1000).ToArray();
                listDisposalTypeSurvey.AddRange(
                    serviceDisposalTypeSurvey.GetAll()
                                         .Where(x => tmpList.Contains(x.Disposal.Id))
                                         .Select(x => new { x.Disposal.Id, x.TypeSurvey.Code })
                                         .ToList());
                str += 1000;
            }

            var disposalTypeSurveyDict = listDisposalTypeSurvey
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Code).ToList());

            var actDisposalMap = inspectionalActs
                .Where(x => disposalTypeSurveyDict.ContainsKey(x.disposal.Id))
                .GroupBy(x => x.actCheck.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.disposal.Id).First());

            Func<long, string, long> actsCountByTypeSurvey = (actId, typeSurveyCode) => actDisposalMap.ContainsKey(actId) ? (disposalTypeSurveyDict[actDisposalMap[actId]].Contains(typeSurveyCode) ? 1 : 0) : 0;
            Func<IGrouping<long, DisposalActCheckReport>, string, long> countByTypeSurvey = (acts, typeSurveyCode) => acts.Select(x => actsCountByTypeSurvey(x.actCheck.Id, typeSurveyCode)).Sum();
            Func<List<DisposalActCheckReport>, string, Dictionary<long, long>> makeDictByTypeSurvey = (list, typeSurveyCode) => list.GroupBy(x => x.muId).ToDictionary(x => x.Key, x => countByTypeSurvey(x, typeSurveyCode));

            reportData.DataColumn60 = makeDictByTypeSurvey(inspectionalActs, "8");
            reportData.DataColumn61 = makeDictByTypeSurvey(inspectionalActs, "5");
            reportData.DataColumn62 = makeDictByTypeSurvey(inspectionalActs, "6");
            reportData.DataColumn63 = makeDictByTypeSurvey(inspectionalActs, "4");
            reportData.DataColumn64 = makeDictByTypeSurvey(inspectionalActs, "7");
            reportData.DataColumn65 = makeDictByTypeSurvey(inspectionalActs, "9");
            reportData.DataColumn66 = makeDictByTypeSurvey(inspectionalActs, "10");
            reportData.DataColumn67 = makeDictByTypeSurvey(inspectionalActs, "11");
            reportData.DataColumn68 = makeDictByTypeSurvey(inspectionalActs, "12");
            reportData.DataColumn69 = makeDictByTypeSurvey(inspectionalActs, "13");
            reportData.DataColumn70 = makeDictByTypeSurvey(inspectionalActs, "14");
            reportData.DataColumn71 = makeDictByTypeSurvey(inspectionalActs, "15");
            reportData.DataColumn72 = makeDictByTypeSurvey(inspectionalActs, "19");
        }

        private void GetViolationData()
        {
            var serviceActCheckViolation = Container.Resolve<IDomainService<ActCheckViolation>>();
            var serviceViolationFeatureGji = Container.Resolve<IDomainService<ViolationFeatureGji>>();

            var listAllActCheckIds = new List<long>();

            listAllActCheckIds.AddRange(column4);
            listAllActCheckIds.AddRange(column9);
            listAllActCheckIds.AddRange(column15);
            listAllActCheckIds.AddRange(column17);

            // Список ActCheck с Violation
            var start = 0;
            var listActCheckViolation = new List<ActCheckViolationProxy>();
            while (start < listAllActCheckIds.Count)
            {
                var tmpId = listAllActCheckIds.Skip(start).Take(1000).ToArray();
                listActCheckViolation.AddRange(serviceActCheckViolation.GetAll()
                         .Where(x => tmpId.Contains(x.ActObject.ActCheck.Id))
                         .Select(x => new ActCheckViolationProxy
                         {
                             actCheckId = x.ActObject.ActCheck.Id,
                             violationId = x.InspectionViolation.Violation.Id
                         })
                         .ToList());
                start += 1000;
            }

            var actCheckWithViolations = listActCheckViolation
                .GroupBy(x => x.actCheckId)
                .ToDictionary(
                        x => x.Key,
                        x => new
                        {
                            actChieckId = x.Key,
                            muId = municipalityByActDict[x.Key],
                            violations = x.Select(y => y.violationId).ToList()
                        });

            reportData.DataColumn20 = column4
                .Where(actCheckWithViolations.ContainsKey)
                .Select(x => actCheckWithViolations[x])
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn21 = column9
               .Where(actCheckWithViolations.ContainsKey)
               .Select(x => actCheckWithViolations[x])
               .GroupBy(x => x.muId)
               .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn22 = column15
               .Where(actCheckWithViolations.ContainsKey)
               .Select(x => actCheckWithViolations[x])
               .GroupBy(x => x.muId)
               .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn23 = column17
               .Where(actCheckWithViolations.ContainsKey)
               .Select(x => actCheckWithViolations[x])
               .GroupBy(x => x.muId)
               .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            start = 0;
            var listViolationId = actCheckWithViolations.SelectMany(x => x.Value.violations).Distinct().ToArray();

            var listViolationFeatureGji = new List<ViolationFeatureProxy>();

            while (start < listViolationId.Length)
            {
                var tmplist = listViolationId.Skip(start).Take(1000).ToArray();

                listViolationFeatureGji.AddRange(serviceViolationFeatureGji
                             .GetAll()
                             .Where(x => tmplist.Contains(x.ViolationGji.Id))
                             .Select(x => new ViolationFeatureProxy
                             {
                                 violationId = x.ViolationGji.Id,
                                 featureCode = x.FeatureViolGji.Code
                             })
                             .ToList());
                start += 1000;
            }

            var featureCodes1 = new[]
                                    {
                                        "1", "2", "3", "4", "5", "13", "14", "18", "19", "20", "21", "22", "23", "24",
                                        "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36"
                                    };

            var featureCodes2 = new[] { "15", "16", "17" };

            var featureCodes3 = new[] { "6", "7", "8", "9", "10", "11", "12" };

            var violationFeatureCodesDict = listViolationFeatureGji
                                                .GroupBy(x => x.violationId)
                                                .ToDictionary(
                                                    x => x.Key,
                                                    x =>
                                                    {
                                                        var featureCodes = x.Select(y => y.featureCode).ToList();

                                                        return new
                                                        {
                                                            inColumn24 = featureCodes.Any(featureCodes1.Contains),
                                                            inColumn25 = featureCodes.Any(featureCodes2.Contains),
                                                            inColumn26 = featureCodes.Any(featureCodes3.Contains)
                                                        };
                                                    });

            var compositeDataColumn = actCheckWithViolations.Values
                .GroupBy(x => x.muId)
                .Select(x =>
                {
                    var actGroups = x.Select(y =>
                    {
                        var violGroups = y.violations.Where(violationFeatureCodesDict.ContainsKey)
                         .Select(z => violationFeatureCodesDict[z])
                         .ToList();

                        return new
                        {
                            col24 = violGroups.Count(z => z.inColumn24),
                            col25 = violGroups.Count(z => z.inColumn25),
                            col26 = violGroups.Count(z => z.inColumn26)
                        };
                    }).ToList();

                    return new
                    {
                        muId = x.Key,
                        col24 = (long)actGroups.Sum(z => z.col24),
                        col25 = (long)actGroups.Sum(z => z.col25),
                        col26 = (long)actGroups.Sum(z => z.col26)
                    };
                })
                .ToList();

            reportData.DataColumn24 = compositeDataColumn.ToDictionary(x => x.muId, x => x.col24);
            reportData.DataColumn25 = compositeDataColumn.ToDictionary(x => x.muId, x => x.col25);
            reportData.DataColumn26 = compositeDataColumn.ToDictionary(x => x.muId, x => x.col26);
        }

        private void GetDataPrescriptionIssued()
        {
            var servicePrescriptionViol = Container.Resolve<IDomainService<PrescriptionViol>>();
            var servicePrescription = Container.Resolve<IDomainService<Prescription>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            var queryPrescriptionMu = servicePrescriptionViol.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
                         .Where(x => x.InspectionViolation.RealityObject != null && x.InspectionViolation.RealityObject.Municipality != null);

            var listPrescriptionMu = queryPrescriptionMu.Select(x => new
            {
                docId = x.Document.Id,
                muId = x.InspectionViolation.RealityObject.Municipality.Id
            })
                .AsEnumerable()
                .Distinct()
                .ToList();

            var dictPrescriptionMu = listPrescriptionMu.GroupBy(x => x.docId).ToDictionary(x => x.Key, x => x.Select(y => y.muId).First());

            var queryPrescriptionsId = queryPrescriptionMu.Select(x => x.Document.Id).Distinct();

            reportData.DataColumn27 = listPrescriptionMu.GroupBy(x => x.muId).ToDictionary(x => x.Key, v => (long)v.Select(y => y.docId).Distinct().Count());

            var queryPrescriptions = servicePrescription.GetAll().Where(x => queryPrescriptionsId.Contains(x.Id));

            var prescriptions = queryPrescriptions.Select(x => new PrescriptionProxy
                                                                    {
                                                                        Id = x.Id,
                                                                        ExecutantCode = x.Executant != null ? x.Executant.Code : string.Empty,
                                                                        parentStage = x.Stage.Parent.Id.ToLong()
                                                                    })
                                                 .ToList();


            var dictPrescripExecutantCode = prescriptions.GroupBy(x => x.Id)
                                                         .ToDictionary(x => x.Key, x => x.Select(y => y.ExecutantCode).ToList());

            var queryDisposalStageIds = queryPrescriptions.Select(x => x.Stage.Parent.Id).Distinct();

            var listPresDisposalProxy = serviceDisposal.GetAll()
                                            .Where(x => queryDisposalStageIds.Contains(x.Stage.Id))
                                            .Select(x => new DisposalProxy
                                            {
                                                Id = x.Id,
                                                InspectionTypeBase = x.Inspection.TypeBase,
                                                stageId = x.Stage.Id.ToLong(),
                                                KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0
                                            })
                                            .ToList();

            var disposalsByStageDict = listPresDisposalProxy.Distinct().ToDictionary(x => x.stageId, x => x);

            var listPresDisposal = prescriptions.Where(x => disposalsByStageDict.ContainsKey(x.parentStage))
                                                .Select(x => new { disposal = disposalsByStageDict[x.parentStage], prescription = x })
                                                .ToList();

            // на юрид.лиц
            reportData.DataColumn28 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)dictPrescripExecutantCode[x.Key].Where(listCodeJur.Contains).Count()
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn29 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)dictPrescripExecutantCode[x.Key].Where(listCodeOffic.Contains).Count()
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на физ.лиц
            reportData.DataColumn30 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)dictPrescripExecutantCode[x.Key].Where(listCodeIndivid.Contains).Count()
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по жалобам
            reportData.DataColumn31 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listPresDisposal.Count(y => y.prescription.Id == x.Key && y.disposal != null &&
                                         y.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            var listPlanCheckUpEntities = listPresDisposal
                .Where(x => x.disposal != null && x.prescription != null
                    && (x.disposal.KindCheckCode == TypeCheck.PlannedExit || x.disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                                                                    || (x.disposal.InspectionTypeBase == TypeBase.Inspection));

            reportData.DataColumn32 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listPlanCheckUpEntities.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeJur.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по внеплан. проверкам
            var listNotPlanInspection = listPresDisposal.Where(x => x.disposal != null && ((x.disposal.KindCheckCode == TypeCheck.NotPlannedExit) || (x.disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation))
                                        && (x.disposal.InspectionTypeBase != TypeBase.CitizenStatement)).ToList();

            // на юрид.лиц
            reportData.DataColumn33 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listNotPlanInspection.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeJur.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn34 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listNotPlanInspection.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeOffic.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по инспекц. обсл.            
            var listInspectSurvey = listPresDisposal.Where(x => x.disposal != null && (x.disposal.KindCheckCode == TypeCheck.InspectionSurvey)
                    && (x.disposal.InspectionTypeBase != TypeBase.Inspection)).ToList();

            // на юрид.лиц
            reportData.DataColumn35 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listInspectSurvey.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeJur.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn36 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listInspectSurvey.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeOffic.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по жалобам
            var listDispComplaints = listPresDisposal.Where(x => x.prescription != null && (x.prescription.ExecutantCode != string.Empty)
                                                && x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                                .ToList();

            // на юрид.лиц
            reportData.DataColumn37 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listDispComplaints.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeJur.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn38 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listDispComplaints.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeOffic.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на физ.лиц
            reportData.DataColumn39 = dictPrescriptionMu.Select(x => new
            {
                muId = x.Value,
                count = (long)listDispComplaints.Where(y => y.prescription.Id == x.Key)
                        .Count(y => y.prescription != null && listCodeIndivid.Contains(y.prescription.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));
        }

        private void GetDataProtocols()
        {
            var serviceProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var serviceProtocol = Container.Resolve<IDomainService<Protocol>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            var serviceProtocolArticleLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>();

            var queryProtocolsMuId = serviceProtocolViolation.GetAll()
                                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                                         .WhereIf(dateEnd != DateTime.MaxValue, x => x.Document.DocumentDate <= dateEnd)
                                         .Where(x => x.InspectionViolation.RealityObject != null);

            var listProtocolsMuId = queryProtocolsMuId
                                         .Select(x => new
                                         {
                                             docId = x.Document.Id,
                                             muId = x.InspectionViolation.RealityObject.Municipality.Id
                                         })
                                         .AsEnumerable()
                                         .Distinct()
                                         .ToList();

            var dictProtocolsMuId = listProtocolsMuId.GroupBy(x => x.docId).ToDictionary(x => x.Key, v => v.Select(z => z.muId).First());

            var queryProtocolsId = queryProtocolsMuId.Select(x => x.Document.Id).Distinct();

            var queryProtocols = serviceProtocol.GetAll()
                                    .Where(x => queryProtocolsId.Contains(x.Id));

            var listProtocols = queryProtocols.Select(x => new ProtocolProxy
            {
                Id = x.Id,
                ExecutantCode = x.Executant.Code ?? string.Empty,
                parentStage = x.Stage.Parent.Id.ToLong(),
                InspectionTypeBase = x.Inspection.TypeBase
            })
                                                .ToList();

            var protocols = listProtocols.Distinct().ToList();

            reportData.DataColumn40 = listProtocolsMuId.GroupBy(x => x.muId).ToDictionary(x => x.Key, v => (long)v.Select(y => y.docId).Distinct().Count());

            var dictProtocolsExecutantCode = protocols.GroupBy(x => x.Id)
                                                         .ToDictionary(x => x.Key, x => x.Select(y => y.ExecutantCode).ToList());

            var queryDisposalStageIds = queryProtocols.Select(x => x.Stage.Parent.Id).Distinct();
            var listProtDisposalProxy = serviceDisposal.GetAll()
                                            .Where(x => queryDisposalStageIds.Contains(x.Stage.Id))
                                            .Select(x => new DisposalProxy
                                            {
                                                Id = x.Id,
                                                InspectionTypeBase = x.Inspection.TypeBase,
                                                stageId = x.Stage.Id.ToLong(),
                                                KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0
                                            })
                                            .ToList();

            var disposalsByStageDict = listProtDisposalProxy.Distinct().ToDictionary(x => x.stageId, x => x);

            var listProtocolsDisposal = protocols.Where(x => disposalsByStageDict.ContainsKey(x.parentStage))
                                                 .Select(x => new { disposal = disposalsByStageDict[x.parentStage], protocol = x })
                                                 .ToList();

            // на юрид.лиц
            reportData.DataColumn41 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count =  dictProtocolsExecutantCode.ContainsKey(x.Key) ? (long)dictProtocolsExecutantCode.Get(x.Key).Where(listCodeJur.Contains).Count() : 0
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn42 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = dictProtocolsExecutantCode.ContainsKey(x.Key) ? (long)dictProtocolsExecutantCode[x.Key].Where(listCodeOffic.Contains).Count() : 0
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на физ.лиц
            reportData.DataColumn43 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count =  dictProtocolsExecutantCode.ContainsKey(x.Key) ? (long)dictProtocolsExecutantCode[x.Key].Where(listCodeIndivid.Contains).Count(): 0
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по жалобам
            reportData.DataColumn44 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listProtocolsDisposal.Count(y => y.protocol.Id == x.Key && y.disposal != null &&
                                            y.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            var listPlanCheckUpEntities = listProtocolsDisposal.Where(x => x.disposal != null && x.protocol != null &&
                                                                    (x.disposal.KindCheckCode == TypeCheck.PlannedExit || x.disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                                                                    || (x.disposal.InspectionTypeBase == TypeBase.Inspection));

            reportData.DataColumn45 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listPlanCheckUpEntities.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeJur.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по внеплан. проверкам
            var listNotPlanInspection = listProtocolsDisposal.Where(x => x.disposal != null && ((x.disposal.KindCheckCode == TypeCheck.NotPlannedExit) || (x.disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation))
                                        && (x.disposal.InspectionTypeBase != TypeBase.CitizenStatement)).ToList();

            // на юрид.лиц
            reportData.DataColumn46 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listNotPlanInspection.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeJur.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn47 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listNotPlanInspection.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeOffic.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по инспекц. обсл.            
            var listInspectSurvey = listProtocolsDisposal.Where(x => x.disposal != null && (x.disposal.KindCheckCode == TypeCheck.InspectionSurvey)
                    && (x.disposal.InspectionTypeBase != TypeBase.Inspection)).ToList();

            // на юрид.лиц
            reportData.DataColumn48 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listInspectSurvey.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeJur.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn49 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listInspectSurvey.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeOffic.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // по жалобам
            var listDispComplaints = listProtocolsDisposal.Where(x => x.protocol != null && (x.protocol.ExecutantCode != string.Empty)
                                                && x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                                .ToList();

            // на юрид.лиц
            reportData.DataColumn50 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listDispComplaints.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeJur.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на долж.лиц
            reportData.DataColumn51 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listDispComplaints.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeOffic.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            // на физ.лиц
            reportData.DataColumn52 = dictProtocolsMuId.Select(x => new
            {
                muId = x.Value,
                count = (long)listDispComplaints.Where(y => y.protocol.Id == x.Key)
                        .Count(y => y.protocol != null && listCodeIndivid.Contains(y.protocol.ExecutantCode))
            })
                                                        .GroupBy(x => x.muId)
                                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            var listProtocolsIdArticle19_5 = serviceProtocolArticleLaw.GetAll()
                                         .Where(x => x.ArticleLaw.Code == "1" || x.ArticleLaw.Code == "7")
                                         .Where(x => queryProtocolsId.Contains(x.Protocol.Id))
                                         .Select(x => x.Protocol.Id)
                                         .ToList();

            reportData.DataColumn53 = listProtocolsIdArticle19_5.Distinct()
                                                  .Where(dictProtocolsMuId.ContainsKey)
                                                  .Select(x => new { mu = dictProtocolsMuId[x], docId = x})
                                                  .GroupBy(x => x.mu)
                                                  .ToDictionary(x => x.Key, v => (long)v.Select(x => x.docId).Distinct().Count());
        }

        private void GetDataActRemoval()
        {
            var start = 0;

            var serviceActRemovalViolation = Container.Resolve<IDomainService<ActRemovalViolation>>();
            var serviceActRemoval = Container.Resolve<IDomainService<ActRemoval>>();
            var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servicePrescription = Container.Resolve<IDomainService<Prescription>>();
            var serviceProtocol = Container.Resolve<IDomainService<Protocol>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            var serviceProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();

            // чтобы получить все Акты устранения для МО необходимо Через нарушения поулчить Дом и по его МО брать фильтр
            var queryActRemovalIdMu = serviceActRemovalViolation.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MaxValue, x => x.Document.DocumentDate <= dateEnd)
                         .Where(x => x.InspectionViolation.RealityObject != null && x.InspectionViolation.RealityObject.Municipality != null);

            var listActRemovalIdMuId = queryActRemovalIdMu.Select(x => new
            {
                x.InspectionViolation.DateFactRemoval,
                x.Document.Id,
                MuId = x.InspectionViolation.RealityObject.Municipality.Id
            })
                .AsEnumerable()
                .Distinct()
                .ToList();

            var queryActRemovalId = queryActRemovalIdMu.Select(x => x.Document.Id).Distinct();
            var dictActRemovalMu = listActRemovalIdMuId.GroupBy(x => x.Id)
                                                       .ToDictionary(x => x.Key, v => v.Select(z => z.MuId).First());
            
            var listActRemoval = serviceActRemoval.GetAll()
                                .Where(x => queryActRemovalId.Contains(x.Id))
                                .Select(x => new { x.Id, x.Area, InspectionTypeBase = x.Inspection.TypeBase })
                                .ToList();

            var listActRemovalProxy = listActRemoval.Select(x => new
                                                                     {
                                                                         muId = dictActRemovalMu[x.Id],
                                                                         x.Id,
                                                                         x.Area,
                                                                         x.InspectionTypeBase
                                                                     }).ToList();

            // Количество актов проверки предписаний
            reportData.DataColumn74 = listActRemovalProxy.GroupBy(x => x.muId)
                                                         .ToDictionary(x => x.Key, x => (long)x.Select(y => y.Id).Distinct().Count());

            // Сумма поля "Площадь" из актов проверки предписаний
            reportData.DataColumn75 = listActRemovalProxy.GroupBy(x => x.muId)
                                                         .ToDictionary(
                                                             x => x.Key,
                                                             x => x.Select(y => y.Area).Sum(y => y.HasValue ? y.Value : 0));
            
            var queryActRemovalPres = serviceDocumentGjiChildren.GetAll()
                                         .Where(x => queryActRemovalId.Contains(x.Children.Id)
                                             && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription);
            var listActRemovalPres = queryActRemovalPres.Select(x => new { parentId = x.Parent.Id, childrenId = x.Children.Id })
                                         .ToList();

            var listActRemovalPrescription = listActRemovalPres.Distinct().ToList();

            var dictPrecriptionActRemov = listActRemovalPrescription.GroupBy(x => x.childrenId)
                                                                    .ToDictionary(
                                                                        x => x.Key,
                                                                        v => v.Select(y => y.parentId).Last());

            var queryPrescriptionId = queryActRemovalPres.Select(x => x.Parent.Id).Distinct();

            var listPrescription = servicePrescription.GetAll()
                             .Where(x => queryPrescriptionId.Contains(x.Id))
                             .Select(x => new PrescriptionProxy
                                             {
                                                 Id = x.Id,
                                                 ExecutantCode = x.Executant.Code,
                                                 InspectionTypeBase = x.Inspection.TypeBase,
                                                 parentStage = x.Stage.Parent.Id.ToLong()
                                             })
                             .ToList();

            var listActRemovalDateFact = listActRemovalIdMuId.Where(x => x.DateFactRemoval.HasValue).ToList();

            // Выпол-нено предп (закрыто актами) всего
            reportData.DataColumn76 = listActRemovalDateFact.Select(x => new
                                                                        {
                                                                            muId = dictActRemovalMu[x.Id],
                                                                            count = (long)listActRemoval.Where(i => i.Id == x.Id).Select(y => y.Id).Distinct().Count()
                                                                        })
                                                            .GroupBy(x => x.muId)
                                                            .ToDictionary(x => x.Key, v => v.Sum(y => y.count));

            var listActRemPrescription = listActRemovalDateFact.Select(x => new 
                                                        {
                                                            muId = dictActRemovalMu[x.Id],
                                                            actRemoval = listActRemoval.First(i => i.Id == x.Id),
                                                            actRemovalViolation = listActRemovalDateFact.Count(y => y.Id == x.Id),
                                                            prescription = listPrescription.Last(y => y.Id == dictPrecriptionActRemov[x.Id])
                                                        });

            var listActRemovalViolation = listActRemPrescription.Where(x => x.actRemovalViolation > 0)
            .Select(x =>
                    {
                        var isJurPerson = false;
                        var isOffPerson = false;
                        var isPhysPerson = false;
                        var isCitizenStatement = x.prescription.InspectionTypeBase == TypeBase.CitizenStatement;

                        var executantCode = x.prescription.ExecutantCode;

                        if (listCodeJur.Contains(executantCode))
                        {
                            isJurPerson = true;
                        }

                        if (listCodeOffic.Contains(executantCode))
                        {
                            isOffPerson = true;
                        }

                        if (listCodeIndivid.Contains(executantCode))
                        {
                            isPhysPerson = true;
                        }

                        return
                            new
                                {
                                    muId = dictActRemovalMu[x.actRemoval.Id],
                                    x.actRemoval,
                                    actRemovalId = x.actRemoval.Id,
                                    x.prescription,
                                    isJurPerson,
                                    isOffPerson,
                                    isPhysPerson,
                                    isCitizenStatement
                                };
                    }).ToList();

            // по юрид лицам
            reportData.DataColumn77 =
                listActRemovalViolation.Where(x => x.isJurPerson)
                                       .GroupBy(x => x.muId)
                                       .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actRemovalId).Distinct().Count());

            // по долж лицам
            reportData.DataColumn78 =
                listActRemovalViolation.Where(x => x.isOffPerson)
                                       .GroupBy(x => x.muId)
                                       .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actRemovalId).Distinct().Count());

            // по физ лицам
            reportData.DataColumn79 =
                listActRemovalViolation.Where(x => x.isPhysPerson)
                                       .GroupBy(x => x.muId)
                                       .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actRemovalId).Distinct().Count());

            // по жал
            reportData.DataColumn80 =
                listActRemovalViolation.Where(x => x.isCitizenStatement)
                                       .GroupBy(x => x.muId)
                                       .ToDictionary(x => x.Key, x => (long)x.Select(y => y.actRemovalId).Distinct().Count());

            // Невыполн.предпис, составлено протоколов

            var queryProtocolIdActId = serviceDocumentGjiChildren.GetAll()
                                         .Where(x => queryActRemovalId.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                         .Select(x => new { parentId = x.Parent.Id, childId = x.Children.Id })
                                         .AsEnumerable();
            
            var queryProtocolIdPrescriptionId = serviceDocumentGjiChildren.GetAll()
                                                        .Where(x => queryPrescriptionId.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                                        .Select(x => new { parentId = x.Parent.Id, childId = x.Children.Id })
                                                        .AsEnumerable();


            var queryProtocolIdParentId = queryProtocolIdActId.Concat(queryProtocolIdPrescriptionId);

            var dictProtocolIdParentId = queryProtocolIdParentId.ToArray().Select(x => {
                long muId = 0;

                                                                                            if(dictActRemovalMu.ContainsKey(x.parentId))
                                                                                            {
                                                                                                muId = dictActRemovalMu[x.parentId];
                                                                                            }
                                                                                            else if (dictPrecriptionActRemov.ContainsKey(x.parentId))
                                                                                            {
                                                                                                if(dictActRemovalMu.ContainsKey(dictPrecriptionActRemov[x.parentId]))
                                                                                                { 
                                                                                                    muId = dictActRemovalMu[dictPrecriptionActRemov[x.parentId]];
                                                                                                }
                                                                                            }

                                                                                            return new {x.childId, muId};
                                                                                        })
                                                                        .GroupBy(x => x.childId)
                                                                        .ToDictionary(x => x.Key, v => v.Last().muId);

            var listProtocolsId = queryProtocolIdParentId.Select(x => x.childId).Distinct().ToList();

            
            var protocols = new List<ProtocolProxy>();
            start = 0;
            while (start < listProtocolsId.Count)
            {
                var tmpId = listProtocolsId.Skip(start).Take(1000).ToArray();
                protocols.AddRange(
                    serviceProtocol.GetAll()
                                  .Where(x => tmpId.Contains(x.Id))
                                  .Select(
                                      x =>
                                      new ProtocolProxy
                                          {
                                              Id = x.Id,
                                              ExecutantCode = x.Executant.Code,
                                              InspectionTypeBase = x.Inspection.TypeBase,
                                              parentStage = x.Stage.Parent.Id.ToLong()
                                          })
                                  .ToList());
                start += 1000;
            }

            var listStageParentId = protocols.Select(x => x.parentStage).Distinct().ToList();
            start = 0;
            var listDisposal = new List<DisposalProxy>();
            while (start < listStageParentId.Count)
            {
                var tmpStageId = listStageParentId.Skip(start).Take(1000).ToArray();
                listDisposal.AddRange(serviceDisposal.GetAll()
                            .Where(x => tmpStageId.Contains(x.Stage.Id))
                            .Select(x => new DisposalProxy
                                                 {
                                                     Id = x.Id,
                                                     InspectionPersonInspection = x.Inspection.PersonInspection,
                                                     InspectionTypeBase = x.Inspection.TypeBase,
                                                     KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0,
                                                     TypeDisposal = x.TypeDisposal,
                                                     stageId = x.Stage.Id
                                                 })
                            .ToList());
                start += 1000;
            }

            // Кол-во нарушений по невыполн.предпис, где оформлен протокол
            reportData.DataColumn81 =protocols
                .Select(x => new { muId = dictProtocolIdParentId[x.Id], id = x.Id })
                         .GroupBy(x => x.muId)
                         .ToDictionary(x => x.Key, v => (long)v.Select(y => y.id).Count());

            var listDispidPlanCheck =
                listDisposal.Where(
                    x =>
                    (x.KindCheckCode == TypeCheck.PlannedDocumentation || x.KindCheckCode == TypeCheck.PlannedExit)
                    || x.InspectionTypeBase == TypeBase.Inspection);

            var listDispidUnscheduledInspection =
                listDisposal.Where(
                    x =>
                    (x.KindCheckCode == TypeCheck.NotPlannedDocumentation || x.KindCheckCode == TypeCheck.NotPlannedExit)
                    && x.InspectionTypeBase != TypeBase.CitizenStatement);

            var listDispidInspectionSurvey =
                listDisposal.Where(
                    x => x.KindCheckCode == TypeCheck.InspectionSurvey && x.InspectionTypeBase != TypeBase.Inspection);

            var listDispidComplaints = listDisposal.Where(x => x.InspectionTypeBase == TypeBase.CitizenStatement);

            reportData.DataColumn82 =
                protocols.Where(x => listDispidPlanCheck.Select(y => y.stageId).Contains(x.parentStage))
                         .Select(x => new { muId = dictProtocolIdParentId[x.Id], id = x.Id })
                         .GroupBy(x => x.muId)
                         .ToDictionary(x => x.Key, v => (long)v.Select(y => y.id).Count());

            reportData.DataColumn83 =
                protocols.Where(x => listDispidUnscheduledInspection.Select(y => y.stageId).Contains(x.parentStage))
                         .Select(x => new { muId = dictProtocolIdParentId[x.Id], id = x.Id })
                         .GroupBy(x => x.muId)
                         .ToDictionary(x => x.Key, v => (long)v.Select(y => y.id).Count());

            reportData.DataColumn84 =
                protocols.Where(x => listDispidInspectionSurvey.Select(y => y.stageId).Contains(x.parentStage))
                         .Select(x => new { muId = dictProtocolIdParentId[x.Id], id = x.Id })
                         .GroupBy(x => x.muId)
                         .ToDictionary(x => x.Key, v => (long)v.Select(y => y.id).Count());

            reportData.DataColumn85 =
                protocols.Where(x => listDispidComplaints.Select(y => y.stageId).Contains(x.parentStage))
                         .Select(x => new { muId = dictProtocolIdParentId[x.Id], id = x.Id })
                         .GroupBy(x => x.muId)
                         .ToDictionary(x => x.Key, v => (long)v.Select(y => y.id).Count());

            // Кол-во нарушений по невыполн.предпис, где оформлен протокол
            start = 0;
            var countProtocolViolation = new List<long>();            
            while (start < protocols.Count)
            {
                var tmpList = protocols.Skip(start).Take(1000).Select(x => x.Id).ToArray();
                countProtocolViolation.AddRange(serviceProtocolViolation.GetAll()
                                            .Where(x => tmpList.Contains(x.Document.Id))
                                            .Select(x => x.Document.Id));
                start += 1000;
            }

            reportData.DataColumn86 = protocols.Select(x => new
                                                                {
                                                                    muId = dictProtocolIdParentId[x.Id],
                                                                    count = (long)countProtocolViolation.Count(y => y == x.Id)
                                                                })
                                                .GroupBy(x => x.muId)
                                                .ToDictionary(x => x.Key, v => v.Sum(z => z.count));
        }

        private void GetDataResolution()
        {
            var serviceInspectionGjiViolStage =Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceResolPros =Container.Resolve<IDomainService<ResolPros>>();
            var serviceResolution = Container.Resolve<IDomainService<Resolution>>();
            var serviceResolutionPayFine = Container.Resolve<IDomainService<ResolutionPayFine>>();

            var start = 0;

            // Типы исполнителей
            var owners = new[] { "17", "7", "14" };
            var companyHousingStock = new[] { "0", "1", "2", "3", "9", "10", "11", "12" };
            var tenantry = new[] { "4", "5", "6" };
            var other = new[] { "8", "13", "15", "16" };

            // Для того чтобы получить постановления по Мо необходимо получить родительские нарушения и через дом получить МО
            var queryParentDocument = serviceInspectionGjiViolStage.GetAll()
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                     .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Prescription || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol);

            var listParentDocumentIdMuId = queryParentDocument
                .Where(x => x.InspectionViolation.RealityObject.Municipality != null)
                .Select(x => new { muId = x.InspectionViolation.RealityObject.Municipality.Id, docId = x.Document.Id })
                .ToList();

            var dictParentDocMuId = listParentDocumentIdMuId.GroupBy(x => x.docId).ToDictionary(x => x.Key, x => x.Select(y => y.muId).First());
            var queryParentDocumentId = queryParentDocument.Select(x => x.Document.Id).Distinct();

            var queryResolProsMu = serviceResolPros.GetAll()
                                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                                         .Where(x => x.Municipality != null);

            var listResolProsMu = queryResolProsMu
                                         .Select(x => new { x.Id, muId = x.Municipality.Id })
                                         .ToList();

            var queryResolProsId = queryResolProsMu.Select(x => x.Id).Distinct();
            var dictResolProsMu = listResolProsMu.GroupBy(x => x.Id).ToDictionary(x => x.Key, v => v.Select(x => x.muId).Last());

            var queryPresProtResolutions = serviceDocumentGjiChildren.GetAll()
                                                    .Where(x => queryParentDocumentId.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                                                    .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                                                    .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                                                    .Select(x => new {parentId = x.Parent.Id, childId = x.Children.Id})
                                                    .AsEnumerable();

            var queryResolProsIdResolutions = serviceDocumentGjiChildren.GetAll()
                                .Where(x => queryResolProsId.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                                .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                                .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                                .Select(x => new {parentId = x.Parent.Id, childId = x.Children.Id})
                                .AsEnumerable();

            var queryParentIdResolutions = queryPresProtResolutions.Concat(queryResolProsIdResolutions);

            var dictChildMuId = queryParentIdResolutions.ToArray().Select(x => new
                                                    {
                                                     docId = x.childId,
                                                     muId = dictParentDocMuId.ContainsKey(x.parentId) ?
                                                        dictParentDocMuId[x.parentId]
                                                        : dictResolProsMu.ContainsKey(x.parentId) ? dictResolProsMu[x.parentId] : 0
                                                    })
                                 .GroupBy(x => x.docId)
                                 .ToDictionary(x => x.Key, v => v.Select(z => z.muId).Last());

            var listResolutionIds = queryParentIdResolutions.Select(x => x.childId).Distinct().ToList();

            start = 0;
            var listResolution = new List<ResolutionProxy>();
            while (start < listResolutionIds.Count)
            {
                var tmpId = listResolutionIds.Skip(start).Take(1000).ToArray();
                listResolution.AddRange(serviceResolution.GetAll()
                    .Where(x => tmpId.Contains(x.Id))
                    .Where(x => x.Executant != null && x.Sanction != null)
                    .Select(x => new ResolutionProxy
                                    {
                                        Id = x.Id,
                                        ExecutantCode = x.Executant.Code,
                                        PenaltyAmount = x.PenaltyAmount,
                                        SanctionCode = x.Sanction.Code,
                                        TypeInitiativeOrg = x.TypeInitiativeOrg,
                                        Paided = x.Paided
                                    })
                    .ToArray());
                start += 1000;
            }

            var listResolutionDispute = new List<long>();
                if (returned)
                {
                    listResolutionDispute = Container.Resolve<IDomainService<ResolutionDispute>>().GetAll()
                                 .Where(x => x.CourtVerdict.Code == "3")
                                 .Select(x => x.Resolution.Id)
                                 .ToList();
                }

            var resolutions = listResolution.Where(x => !listResolutionDispute.Contains(x.Id)).Where(x => dictChildMuId.ContainsKey(x.Id)).ToArray();

            var dictResolution = resolutions.GroupBy(x => x.Id)
                                            .ToDictionary(x => x.Key, v => v.Select(y => y.ExecutantCode).ToList());

            var listResolutionId = resolutions.Select(x => x.Id).Distinct().ToList();
            start = 0;
            var listResolutionpayFineProxy = new List<ResolutionpayFineProxy>();
            while (start < listResolutionId.Count)
            {
                var tmpId = listResolutionId.Skip(start).Take(1000).ToArray();
                listResolutionpayFineProxy.AddRange(
                    serviceResolutionPayFine.GetAll()
                             .Where(x => tmpId.Contains(x.Resolution.Id))
                             .Select(x => new ResolutionpayFineProxy { Id = x.Resolution.Id, Amount = x.Amount })
                             .ToArray());

                start += 1000;
            }

            var dictResolutionpayFineProxy = listResolutionpayFineProxy.GroupBy(x => x.Id)
                                                                       .ToDictionary(
                                                                           x => x.Key,
                                                                           v => v.Sum(y => y.Amount.HasValue ? (decimal)y.Amount : 0M));

            // Вынесено постановлений
            reportData.DataColumn87 = resolutions.Select(x => new { muId = dictChildMuId[x.Id], docId = x.Id })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, x => (long)x.Select(y => y.docId).Distinct().Count());

            // ГЖИ
            reportData.DataColumn88 = resolutions.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection)
                                                 .Select(x => new { muId = dictChildMuId[x.Id], docId = x.Id })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, x => (long)x.Count());
            
            // Суд
            reportData.DataColumn89 = resolutions.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court)
                                                 .Select(x => new { muId = dictChildMuId[x.Id], docId = x.Id })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, x => (long)x.Count());

            // на юрид. лиц
            reportData.DataColumn90 = resolutions.Select(x => new
                                                    {
                                                        muId = dictChildMuId[x.Id],
                                                        count = (long)dictResolution[x.Id].Count(listCodeJur.Contains)
                                                    })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => v.Sum(z => z.count));
            
            // на должн. лиц
            reportData.DataColumn91 = resolutions.Select(x => new
                                                    {
                                                        muId = dictChildMuId[x.Id],
                                                        count = (long)dictResolution[x.Id].Count(listCodeOffic.Contains)
                                                    })
                                                  .GroupBy(x => x.muId)
                                                  .ToDictionary(x => x.Key, v => v.Sum(z => z.count));
            
            // на физ. лиц
            reportData.DataColumn92 = resolutions.Select(x => new
                                                                {
                                                                    muId = dictChildMuId[x.Id],
                                                                    count = (long)dictResolution[x.Id].Count(listCodeIndivid.Contains)
                                                                })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => v.Sum(z => z.count));

            // Постановление о прекращении произв.
            reportData.DataColumn93 = resolutions.Where(x => x.SanctionCode == "2")
                                                 .Select(x => new
                                                                {
                                                                    muId = dictChildMuId[x.Id]
                                                                })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => (long)v.Count());
            
            // Административный штраф
            reportData.DataColumn94 = resolutions.Where(x => x.SanctionCode == "1")
                                                 .Select(x => new
                                                            {
                                                                muId = dictChildMuId[x.Id]
                                                            })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => (long)v.Count()); 
            
            // Устное замечание
            reportData.DataColumn95 = resolutions.Where(x => x.SanctionCode == "3")
                                                 .Select(x => new
                                                                {
                                                                    muId = dictChildMuId[x.Id]
                                                                })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => (long)v.Count()); 
            
            // Предупреждение
            reportData.DataColumn96 = resolutions.Where(x => x.SanctionCode == "4")
                                                 .Select(x => new
                                                                {
                                                                    muId = dictChildMuId[x.Id]
                                                                })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => (long)v.Count()); 

            // Административный арест
            reportData.DataColumn97 = resolutions.Where(x => x.SanctionCode == "5")
                                                 .Select(x => new
                                                                {
                                                                    muId = dictChildMuId[x.Id]
                                                                })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, v => (long)v.Count()); 

            // Предъявлено штрафов
            var resSanction1 = resolutions.Where(x => x.SanctionCode == "1").ToList();
            reportData.DataColumn98 = resSanction1.Select(x => new
                                                    {
                                                        muId = dictChildMuId[x.Id],
                                                        penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                    })
                                                  .GroupBy(x => x.muId)
                                                  .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // ГЖИ
            reportData.DataColumn99 = resSanction1.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection)
                                                  .Select(x => new 
                                                    {
                                                        muId = dictChildMuId[x.Id],
                                                        penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                    })
                                                  .GroupBy(x => x.muId)
                                                  .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));
            
            // Суд
            reportData.DataColumn100 = resSanction1.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court)
                                                   .Select(x => new
                                                                {
                                                                    muId = dictChildMuId[x.Id],
                                                                    penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                })
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // на юрид. лиц
            reportData.DataColumn101 = resSanction1.Select(x => new 
                                                                {
                                                                    muId = dictChildMuId[x.Id],
                                                                    count = dictResolution[x.Id].Count(listCodeJur.Contains),
                                                                    penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));
            
            // на должн. лиц
            reportData.DataColumn102 = resSanction1.Select(x => new 
                                                                {
                                                                    muId = dictChildMuId[x.Id],
                                                                    count = dictResolution[x.Id].Count(listCodeOffic.Contains),
                                                                    penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // на физ. лиц
            reportData.DataColumn103 = resSanction1.Select(x => new 
                                                                    {
                                                                        muId = dictChildMuId[x.Id],
                                                                        count = dictResolution[x.Id].Count(listCodeIndivid.Contains),
                                                                        penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                    })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // Собственники жилых помещений
            reportData.DataColumn104 = resSanction1.Select(x => new 
                                                                  {
                                                                      muId = dictChildMuId[x.Id],
                                                                      count = dictResolution[x.Id].Count(owners.Contains),
                                                                      penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                  })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // Предприятия, обслуживающие жил.фонд
            reportData.DataColumn105 = resSanction1.Select(x => new 
                                                                  {
                                                                      muId = dictChildMuId[x.Id],
                                                                      count = dictResolution[x.Id].Count(companyHousingStock.Contains),
                                                                      penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                  })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // Наниматели, арендаторы
            reportData.DataColumn106 = resSanction1.Select(x => new 
                                                                    {
                                                                        muId = dictChildMuId[x.Id],
                                                                        count = dictResolution[x.Id].Count(tenantry.Contains),
                                                                        penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                    })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // иным
            reportData.DataColumn107 = resSanction1.Select(x => new 
                                                                    {
                                                                        muId = dictChildMuId[x.Id],
                                                                        count = dictResolution[x.Id].Count(other.Contains),
                                                                        penaltyAmount = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M
                                                                    })
                                                   .Where(x => x.count > 0)
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // Получено штрафов ГЖИ
            reportData.DataColumn109 = resolutions.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection)
                                                   .Select(x => new
                                                               {
                                                                   muId = dictChildMuId[x.Id],
                                                                   penaltyAmount = dictResolutionpayFineProxy.ContainsKey(x.Id) ? dictResolutionpayFineProxy[x.Id] : 0M
                                                               })
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));

            // Суд
            reportData.DataColumn110 = resolutions.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court)
                                                   .Select(x => new
                                                               {
                                                                   muId = dictChildMuId[x.Id],
                                                                   penaltyAmount = dictResolutionpayFineProxy.ContainsKey(x.Id) ? dictResolutionpayFineProxy[x.Id] : 0M
                                                               })
                                                   .GroupBy(x => x.muId)
                                                   .ToDictionary(x => x.Key, v => v.Sum(z => z.penaltyAmount));
        }

        /// <summary>
        /// Итоги по ЗЖИ
        /// </summary>
        /// <param name="zji"></param>
        private void SumZji(KeyValuePair<ZonalInspection, Municipality[]> zji)
        {
            foreach (var municipality in zji.Value)
            {
                SumLong(ref dataZji.DataColumn4, reportData.DataColumn4, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn5, reportData.DataColumn5, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn6, reportData.DataColumn6, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn7, reportData.DataColumn7, municipality.Id, zji.Key.Id);

                SumLong(ref dataZji.DataColumn9, reportData.DataColumn9, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn10, reportData.DataColumn10, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn11, reportData.DataColumn11, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn12, reportData.DataColumn12, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn13, reportData.DataColumn13, municipality.Id, zji.Key.Id);

                SumLong(ref dataZji.DataColumn15, reportData.DataColumn15, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn16, reportData.DataColumn16, municipality.Id, zji.Key.Id);

                SumLong(ref dataZji.DataColumn17, reportData.DataColumn17, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn18, reportData.DataColumn18, municipality.Id, zji.Key.Id);

                SumLong(ref dataZji.DataColumn20, reportData.DataColumn20, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn21, reportData.DataColumn21, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn22, reportData.DataColumn22, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn23, reportData.DataColumn23, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn24, reportData.DataColumn24, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn25, reportData.DataColumn25, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn26, reportData.DataColumn26, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn27, reportData.DataColumn27, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn28, reportData.DataColumn28, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn29, reportData.DataColumn29, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn30, reportData.DataColumn30, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn31, reportData.DataColumn31, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn32, reportData.DataColumn32, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn33, reportData.DataColumn33, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn34, reportData.DataColumn34, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn35, reportData.DataColumn35, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn36, reportData.DataColumn36, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn37, reportData.DataColumn37, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn38, reportData.DataColumn38, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn39, reportData.DataColumn39, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn40, reportData.DataColumn40, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn41, reportData.DataColumn41, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn42, reportData.DataColumn42, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn43, reportData.DataColumn43, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn44, reportData.DataColumn44, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn45, reportData.DataColumn45, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn46, reportData.DataColumn46, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn47, reportData.DataColumn47, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn48, reportData.DataColumn48, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn49, reportData.DataColumn49, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn50, reportData.DataColumn50, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn51, reportData.DataColumn51, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn52, reportData.DataColumn52, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn53, reportData.DataColumn53, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn54, reportData.DataColumn54, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn55, reportData.DataColumn55, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn56, reportData.DataColumn56, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn57, reportData.DataColumn57, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn58, reportData.DataColumn58, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn59, reportData.DataColumn59, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn60, reportData.DataColumn60, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn61, reportData.DataColumn61, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn62, reportData.DataColumn62, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn63, reportData.DataColumn63, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn64, reportData.DataColumn64, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn65, reportData.DataColumn65, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn66, reportData.DataColumn66, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn67, reportData.DataColumn67, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn68, reportData.DataColumn68, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn69, reportData.DataColumn69, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn70, reportData.DataColumn70, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn71, reportData.DataColumn71, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn72, reportData.DataColumn72, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn73, reportData.DataColumn73, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn74, reportData.DataColumn74, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn75, reportData.DataColumn75, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn76, reportData.DataColumn76, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn77, reportData.DataColumn77, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn78, reportData.DataColumn78, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn79, reportData.DataColumn79, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn80, reportData.DataColumn80, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn81, reportData.DataColumn81, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn82, reportData.DataColumn82, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn83, reportData.DataColumn83, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn84, reportData.DataColumn84, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn85, reportData.DataColumn85, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn86, reportData.DataColumn86, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn87, reportData.DataColumn87, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn88, reportData.DataColumn88, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn89, reportData.DataColumn89, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn90, reportData.DataColumn90, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn91, reportData.DataColumn91, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn92, reportData.DataColumn92, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn93, reportData.DataColumn93, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn94, reportData.DataColumn94, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn95, reportData.DataColumn95, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn96, reportData.DataColumn96, municipality.Id, zji.Key.Id);
                SumLong(ref dataZji.DataColumn97, reportData.DataColumn97, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn98, reportData.DataColumn98, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn99, reportData.DataColumn99, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn100, reportData.DataColumn100, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn101, reportData.DataColumn101, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn102, reportData.DataColumn102, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn103, reportData.DataColumn103, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn104, reportData.DataColumn104, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn105, reportData.DataColumn105, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn106, reportData.DataColumn106, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn107, reportData.DataColumn107, municipality.Id, zji.Key.Id);

                Sumdecimal(ref dataZji.DataColumn109, reportData.DataColumn109, municipality.Id, zji.Key.Id);
                Sumdecimal(ref dataZji.DataColumn110, reportData.DataColumn110, municipality.Id, zji.Key.Id);
            }
        }

        /// <summary>
        /// Итоги общие
        /// </summary>
        /// <param name="dictZji"></param>
        /// <returns></returns>
        private ReportData SumZjiTotal(Dictionary<ZonalInspection, Municipality[]> dictZji)
        {
            var total = new ReportData();

            foreach (var zji in dictZji.Keys)
            {
                SumLong(ref total.DataColumn4, dataZji.DataColumn4, zji.Id);
                Sumdecimal(ref total.DataColumn5, dataZji.DataColumn5, zji.Id);
                SumLong(ref total.DataColumn6, dataZji.DataColumn6, zji.Id);
                SumLong(ref total.DataColumn7, dataZji.DataColumn7, zji.Id);

                SumLong(ref total.DataColumn9, dataZji.DataColumn9, zji.Id);
                Sumdecimal(ref total.DataColumn10, dataZji.DataColumn10, zji.Id);
                SumLong(ref total.DataColumn11, dataZji.DataColumn11, zji.Id);
                SumLong(ref total.DataColumn12, dataZji.DataColumn12, zji.Id);
                SumLong(ref total.DataColumn13, dataZji.DataColumn13, zji.Id);

                SumLong(ref total.DataColumn15, dataZji.DataColumn15, zji.Id);
                Sumdecimal(ref total.DataColumn16, dataZji.DataColumn16, zji.Id);

                SumLong(ref total.DataColumn17, dataZji.DataColumn17, zji.Id);
                Sumdecimal(ref total.DataColumn18, dataZji.DataColumn18, zji.Id);

                SumLong(ref total.DataColumn20, dataZji.DataColumn20, zji.Id);
                SumLong(ref total.DataColumn21, dataZji.DataColumn21, zji.Id);
                SumLong(ref total.DataColumn22, dataZji.DataColumn22, zji.Id);
                SumLong(ref total.DataColumn23, dataZji.DataColumn23, zji.Id);
                SumLong(ref total.DataColumn24, dataZji.DataColumn24, zji.Id);
                SumLong(ref total.DataColumn25, dataZji.DataColumn25, zji.Id);
                SumLong(ref total.DataColumn26, dataZji.DataColumn26, zji.Id);
                SumLong(ref total.DataColumn27, dataZji.DataColumn27, zji.Id);
                SumLong(ref total.DataColumn28, dataZji.DataColumn28, zji.Id);
                SumLong(ref total.DataColumn29, dataZji.DataColumn29, zji.Id);
                SumLong(ref total.DataColumn30, dataZji.DataColumn30, zji.Id);
                SumLong(ref total.DataColumn31, dataZji.DataColumn31, zji.Id);
                SumLong(ref total.DataColumn32, dataZji.DataColumn32, zji.Id);
                SumLong(ref total.DataColumn33, dataZji.DataColumn33, zji.Id);
                SumLong(ref total.DataColumn34, dataZji.DataColumn34, zji.Id);
                SumLong(ref total.DataColumn35, dataZji.DataColumn35, zji.Id);
                SumLong(ref total.DataColumn36, dataZji.DataColumn36, zji.Id);
                SumLong(ref total.DataColumn37, dataZji.DataColumn37, zji.Id);
                SumLong(ref total.DataColumn38, dataZji.DataColumn38, zji.Id);
                SumLong(ref total.DataColumn39, dataZji.DataColumn39, zji.Id);
                SumLong(ref total.DataColumn40, dataZji.DataColumn40, zji.Id);
                SumLong(ref total.DataColumn41, dataZji.DataColumn41, zji.Id);
                SumLong(ref total.DataColumn42, dataZji.DataColumn42, zji.Id);
                SumLong(ref total.DataColumn43, dataZji.DataColumn43, zji.Id);
                SumLong(ref total.DataColumn44, dataZji.DataColumn44, zji.Id);
                SumLong(ref total.DataColumn45, dataZji.DataColumn45, zji.Id);
                SumLong(ref total.DataColumn46, dataZji.DataColumn46, zji.Id);
                SumLong(ref total.DataColumn47, dataZji.DataColumn47, zji.Id);
                SumLong(ref total.DataColumn48, dataZji.DataColumn48, zji.Id);
                SumLong(ref total.DataColumn49, dataZji.DataColumn49, zji.Id);
                SumLong(ref total.DataColumn50, dataZji.DataColumn50, zji.Id);
                SumLong(ref total.DataColumn51, dataZji.DataColumn51, zji.Id);
                SumLong(ref total.DataColumn52, dataZji.DataColumn52, zji.Id);
                SumLong(ref total.DataColumn53, dataZji.DataColumn53, zji.Id);
                SumLong(ref total.DataColumn54, dataZji.DataColumn54, zji.Id);
                SumLong(ref total.DataColumn55, dataZji.DataColumn55, zji.Id);
                SumLong(ref total.DataColumn56, dataZji.DataColumn56, zji.Id);
                SumLong(ref total.DataColumn57, dataZji.DataColumn57, zji.Id);
                SumLong(ref total.DataColumn58, dataZji.DataColumn58, zji.Id);
                SumLong(ref total.DataColumn59, dataZji.DataColumn59, zji.Id);
                SumLong(ref total.DataColumn60, dataZji.DataColumn60, zji.Id);
                SumLong(ref total.DataColumn61, dataZji.DataColumn61, zji.Id);
                SumLong(ref total.DataColumn62, dataZji.DataColumn62, zji.Id);
                SumLong(ref total.DataColumn63, dataZji.DataColumn63, zji.Id);
                SumLong(ref total.DataColumn64, dataZji.DataColumn64, zji.Id);
                SumLong(ref total.DataColumn65, dataZji.DataColumn65, zji.Id);
                SumLong(ref total.DataColumn66, dataZji.DataColumn66, zji.Id);
                SumLong(ref total.DataColumn67, dataZji.DataColumn67, zji.Id);
                SumLong(ref total.DataColumn68, dataZji.DataColumn68, zji.Id);
                SumLong(ref total.DataColumn69, dataZji.DataColumn69, zji.Id);
                SumLong(ref total.DataColumn70, dataZji.DataColumn70, zji.Id);
                SumLong(ref total.DataColumn71, dataZji.DataColumn71, zji.Id);
                SumLong(ref total.DataColumn72, dataZji.DataColumn72, zji.Id);
                SumLong(ref total.DataColumn73, dataZji.DataColumn73, zji.Id);
                SumLong(ref total.DataColumn74, dataZji.DataColumn74, zji.Id);
                Sumdecimal(ref total.DataColumn75, dataZji.DataColumn75, zji.Id);
                SumLong(ref total.DataColumn76, dataZji.DataColumn76, zji.Id);
                SumLong(ref total.DataColumn77, dataZji.DataColumn77, zji.Id);
                SumLong(ref total.DataColumn78, dataZji.DataColumn78, zji.Id);
                SumLong(ref total.DataColumn79, dataZji.DataColumn79, zji.Id);
                SumLong(ref total.DataColumn80, dataZji.DataColumn80, zji.Id);
                SumLong(ref total.DataColumn81, dataZji.DataColumn81, zji.Id);
                SumLong(ref total.DataColumn82, dataZji.DataColumn82, zji.Id);
                SumLong(ref total.DataColumn83, dataZji.DataColumn83, zji.Id);
                SumLong(ref total.DataColumn84, dataZji.DataColumn84, zji.Id);
                SumLong(ref total.DataColumn85, dataZji.DataColumn85, zji.Id);
                SumLong(ref total.DataColumn86, dataZji.DataColumn86, zji.Id);
                SumLong(ref total.DataColumn87, dataZji.DataColumn87, zji.Id);
                SumLong(ref total.DataColumn88, dataZji.DataColumn88, zji.Id);
                SumLong(ref total.DataColumn89, dataZji.DataColumn89, zji.Id);
                SumLong(ref total.DataColumn90, dataZji.DataColumn90, zji.Id);
                SumLong(ref total.DataColumn91, dataZji.DataColumn91, zji.Id);
                SumLong(ref total.DataColumn92, dataZji.DataColumn92, zji.Id);
                SumLong(ref total.DataColumn93, dataZji.DataColumn93, zji.Id);
                SumLong(ref total.DataColumn94, dataZji.DataColumn94, zji.Id);
                SumLong(ref total.DataColumn95, dataZji.DataColumn95, zji.Id);
                SumLong(ref total.DataColumn96, dataZji.DataColumn96, zji.Id);
                SumLong(ref total.DataColumn97, dataZji.DataColumn97, zji.Id);
                Sumdecimal(ref total.DataColumn98, dataZji.DataColumn98, zji.Id);
                Sumdecimal(ref total.DataColumn99, dataZji.DataColumn99, zji.Id);
                Sumdecimal(ref total.DataColumn100, dataZji.DataColumn100, zji.Id);
                Sumdecimal(ref total.DataColumn101, dataZji.DataColumn101, zji.Id);
                Sumdecimal(ref total.DataColumn102, dataZji.DataColumn102, zji.Id);
                Sumdecimal(ref total.DataColumn103, dataZji.DataColumn103, zji.Id);
                Sumdecimal(ref total.DataColumn104, dataZji.DataColumn104, zji.Id);
                Sumdecimal(ref total.DataColumn105, dataZji.DataColumn105, zji.Id);
                Sumdecimal(ref total.DataColumn106, dataZji.DataColumn106, zji.Id);
                Sumdecimal(ref total.DataColumn107, dataZji.DataColumn107, zji.Id);

                Sumdecimal(ref total.DataColumn109, dataZji.DataColumn109, zji.Id);
                Sumdecimal(ref total.DataColumn110, dataZji.DataColumn110, zji.Id);
            }

            return total;
        }

        private void SumLong(ref Dictionary<long, long> result, Dictionary<long, long> data, long id = 0, long zjiId = 0)
        {
            if (result == null)
            {
                result = new Dictionary<long, long>();
            }

            if (data == null)
            {
                return;
            }

            if (result.ContainsKey(zjiId))
            {
                result[zjiId] += data.ContainsKey(id) ? data[id] : 0;
            }
            else
            {
                result.Add(zjiId, data.ContainsKey(id) ? data[id] : 0);
            }
        }

        private void Sumdecimal(ref Dictionary<long, decimal> result, Dictionary<long, decimal> data, long id = 0, long zjiId = 0)
        {
            if (result == null)
            {
                result = new Dictionary<long, decimal>();
            }

            if (data == null)
            {
                return;
            }

            if (result.ContainsKey(zjiId))
            {
                result[zjiId] += data.ContainsKey(id) ? data[id] : 0;
            }
            else
            {
                result.Add(zjiId, data.ContainsKey(id) ? data[id] : 0);
            }
        }

        private CountAreaHousesActCheckReport GetCountActCheckRo(IEnumerable<ActCheckProxy> listAct)
        {
            var arrId = listAct.Select(x => x.Id).Distinct().ToArray();

            var actProxies = actCheckRoDict.Where(x => arrId.Contains(x.Key)).Select(x => x.Value).ToList();
            var result = new CountAreaHousesActCheckReport
            {
                Area = actProxies.Select(x => x.Area).Sum(),
                Count = actProxies.SelectMany(x => x.roIds).Distinct().Count()
            };

            return result;
        }

        private Dictionary<long, ActCheckRobjectData> InitActCheckRoDict(List<ActCheckProxy> listActProxy)
        {
            var arrId = listActProxy.Select(x => x.Id).Distinct().ToArray();
            var actAreas = listActProxy.Distinct().GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.Sum(z => z.Area));

            var firstId = arrId.Take(1000).ToArray();
            var listActRo =
                Container.Resolve<IDomainService<ActCheckRealityObject>>()
                         .GetAll()
                         .Where(x => firstId.Contains(x.ActCheck.Id))
                         .Select(x => new { actId = x.ActCheck.Id, roId = x.RealityObject.Id })
                         .ToList();

            var start = 1000;
            while (start < arrId.Length)
            {
                var tmpId = arrId.Skip(start).Take(1000).ToArray();
                listActRo.AddRange(
                    Container.Resolve<IDomainService<ActCheckRealityObject>>()
                             .GetAll()
                             .Where(x => tmpId.Contains(x.ActCheck.Id))
                             .Select(x => new { actId = x.ActCheck.Id, roId = x.RealityObject.Id })
                             .ToList());
                start += 1000;
            }

            var actRobjects = listActRo
                .GroupBy(x => x.actId)
                .ToDictionary(
                        x => x.Key,
                        x => new ActCheckRobjectData
                        {
                            Area = actAreas.ContainsKey(x.Key) ? (actAreas[x.Key].HasValue ? actAreas[x.Key].Value : 0M) : 0M,
                            roIds = x.Select(y => y.roId).Distinct().ToList()
                        });

            return actRobjects;
        }

        /// <summary>
        /// Заполнение секции отчета
        /// </summary>
        /// <param name="section"></param>
        /// <param name="municipalityId"></param>
        private void FillRow(Section section, long municipalityId)
        {
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimalArea = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] / 1000 : 0;
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimal = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;
            Func<Dictionary<long, long>, long, long> getStr = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;

            var col4 = getStr(reportData.DataColumn4, municipalityId);
            var col6 = getStr(reportData.DataColumn6, municipalityId);
            var col7 = getStr(reportData.DataColumn7, municipalityId);

            section["КолвоДомовПлановое"] = col4;
            section["ПлощадьПлановое"] = getStrDecimalArea(reportData.DataColumn5, municipalityId);
            section["ПлощадьДомовПлановоеМун"] = col6;
            section["ПлощадьДомовПлановоеТсж"] = col7;
            section["ПлощадьДомовПланИные"] = col4 - (col6 + col7);

            var col9 = getStr(reportData.DataColumn9, municipalityId);
            var col11 = getStr(reportData.DataColumn11, municipalityId);
            var col12 = getStr(reportData.DataColumn12, municipalityId);
            var col13 = getStr(reportData.DataColumn13, municipalityId);
            section["КолвоДомовВнеплановое"] = col9;
            section["ПлощадьВнеплановое"] = getStrDecimalArea(reportData.DataColumn10, municipalityId);
            section["ПлощадьДомовВнеплановоеМун"] = col11;
            section["ПлощадьДомовВнеплановоеТсж"] = col12;
            section["ПлощадьВнеплановоеФизЛица"] = col13;
            section["ПлощадьДомовВнеПланИные"] = col9 - (col11 + col12 + col13);

            section["КолвоДомовИнспекционное"] = getStr(reportData.DataColumn15, municipalityId);
            section["ПлощадьИнспекционное"] = getStrDecimalArea(reportData.DataColumn16, municipalityId);

            section["КолвоДомовПоЖалобе"] = getStr(reportData.DataColumn17, municipalityId);
            section["ПлощадьПоЖалобе"] = getStrDecimalArea(reportData.DataColumn18, municipalityId);

            var col20 = getStr(reportData.DataColumn20, municipalityId);
            var col21 = getStr(reportData.DataColumn21, municipalityId);
            var col22 = getStr(reportData.DataColumn22, municipalityId);
            var col23 = getStr(reportData.DataColumn23, municipalityId);
            section["ОбщееКолвоНарушений"] = col20 + col21 + col22 + col23;
            section["КолвоНарушенийПлановое"] = col20;
            section["КолвоНарушенийВнеплановое"] = col21;
            section["КолвоНарушенийИнспекционное"] = col22;
            section["КолвоНарушенийПоЖалобам"] = col23;
            section["КолвоНарушПравилТехЭ"] = getStr(reportData.DataColumn24, municipalityId);
            section["КолвоНарушПрПользЖил"] = getStr(reportData.DataColumn25, municipalityId);
            section["КолвоНарушНормУровня"] = getStr(reportData.DataColumn26, municipalityId);

            section["ОбщееКолвоПредписаний"] = getStr(reportData.DataColumn27, municipalityId);
            section["КолвоПредписанийЮрлиц"] = getStr(reportData.DataColumn28, municipalityId);
            section["КолвоПредписанийДолжлиц"] = getStr(reportData.DataColumn29, municipalityId);
            section["КолвоПредписанийФизлиц"] = getStr(reportData.DataColumn30, municipalityId);
            section["КолвоПредписанийПоЖалобам"] = getStr(reportData.DataColumn31, municipalityId);
            section["КолвоПредписанийПланЮр"] = getStr(reportData.DataColumn32, municipalityId);
            section["КолвоПредписанийВнепланЮр"] = getStr(reportData.DataColumn33, municipalityId);
            section["КолвоПредписанийВнепланДл"] = getStr(reportData.DataColumn34, municipalityId);
            section["КолвоПредписанийИнспЮр"] = getStr(reportData.DataColumn35, municipalityId);
            section["КолвоПредписанийИнспДл"] = getStr(reportData.DataColumn36, municipalityId);
            section["КолвоПредписанийЖалобыЮр"] = getStr(reportData.DataColumn37, municipalityId);
            section["КолвоПредписанийЖалобыДл"] = getStr(reportData.DataColumn38, municipalityId);
            section["КолвоПредписанийЖалобыФиз"] = getStr(reportData.DataColumn39, municipalityId);

            section["ОбщееКолвоПротоколов"] = getStr(reportData.DataColumn40, municipalityId);
            section["КолвоПротоколовЮр"] = getStr(reportData.DataColumn41, municipalityId);
            section["КолвоПротоколовДл"] = getStr(reportData.DataColumn42, municipalityId);
            section["КолвоПротоколовФиз"] = getStr(reportData.DataColumn43, municipalityId);
            section["КолвоПротоколовЖалоб"] = getStr(reportData.DataColumn44, municipalityId);
            section["КолвоПротоколовЮрлицПлановое"] = getStr(reportData.DataColumn45, municipalityId);
            section["КолвоПротоколовЮрлицВнеплановое"] = getStr(reportData.DataColumn46, municipalityId);
            section["КолвоПротоколовДолжлицВнеплановое"] = getStr(reportData.DataColumn47, municipalityId);
            section["КолвоПротоколовЮрлицИнспекционное"] = getStr(reportData.DataColumn48, municipalityId);
            section["КолвоПротоколовДолжлицИнспекционное"] = getStr(reportData.DataColumn49, municipalityId);
            section["КолвоПротоколовЖалобЮрлиц"] = getStr(reportData.DataColumn50, municipalityId);
            section["КолвоПротоколовЖалобДолжлиц"] = getStr(reportData.DataColumn51, municipalityId);
            section["КолвоПротоколовЖалобФизлиц"] = getStr(reportData.DataColumn52, municipalityId);
            section["КолвоПротоколовПоСт19_5"] = getStr(reportData.DataColumn53, municipalityId);

            section["ОбщееКолвоАктов"] = getStr(reportData.DataColumn54, municipalityId);
            section["КолвоАктовПлановое"] = getStr(reportData.DataColumn55, municipalityId);
            section["КолвоАктовВнеплановое"] = getStr(reportData.DataColumn56, municipalityId);
            section["КолвоАктовВнеплановоеИспПредписания"] = getStr(reportData.DataColumn57, municipalityId);
            section["КолвоАктовПоЖалобе"] = getStr(reportData.DataColumn58, municipalityId);
            var col59 = getStr(reportData.DataColumn59, municipalityId);
            var col60 = getStr(reportData.DataColumn60, municipalityId);
            var col61 = getStr(reportData.DataColumn61, municipalityId);
            var col62 = getStr(reportData.DataColumn62, municipalityId);
            var col63 = getStr(reportData.DataColumn63, municipalityId);
            var col64 = getStr(reportData.DataColumn64, municipalityId);
            var col65 = getStr(reportData.DataColumn65, municipalityId);
            var col66 = getStr(reportData.DataColumn66, municipalityId);
            var col67 = getStr(reportData.DataColumn67, municipalityId);
            var col68 = getStr(reportData.DataColumn68, municipalityId);
            var col69 = getStr(reportData.DataColumn69, municipalityId);
            var col70 = getStr(reportData.DataColumn70, municipalityId);
            var col71 = getStr(reportData.DataColumn71, municipalityId);
            var col72 = getStr(reportData.DataColumn72, municipalityId);
            section["КолвоАктовИнспекционное"] = col59;
            section["КолвоАктовИнспекционноеЗима"] = col60;
            section["КолвоАктовИнспекционноеСосульки"] = col61;
            section["КолвоАктовИнспекционноеПаводки"] = col62;
            section["КолвоАктовИнспекционноеКапремонт"] = col63;
            section["КолвоАктовИнспекционноеАдрАтрибутика"] = col64;
            section["КолвоАктовПоКомУчету"] = col65;
            section["КолвоАктовПридомТерр"] = col66;
            section["КолвоАктовАнтитеррор"] = col67;
            section["КолвоАктовВнутриГаз"] = col68;
            section["КолвоАктовПоПереустр"] = col69;
            section["КолвоАктовНежилПом"] = col70;
            section["КолвоАктовПоЛилфтам"] = col71;
            section["КолвоАктовПоСанитСост"] = col72;
            section["КолвоАктовДругие"] = col59 - (col60 + col61 + col62 + col63 + col64 + col65 + col66 + col67 + col68 + col69 + col70 + col71 + col72);

            section["КолвоАктовПроверкиВнеплановоеИспПредписания"] = getStr(reportData.DataColumn74, municipalityId);
            section["ПлощадьАктовВнеплановоеИспПредписания"] = getStrDecimalArea(reportData.DataColumn75, municipalityId);
            section["ОбщееКолвоВыполненныхПредписаний"] = getStr(reportData.DataColumn76, municipalityId);
            section["КолвоВыполненныхПредписанийЮрлиц"] = getStr(reportData.DataColumn77, municipalityId);
            section["КолвоВыполненныхПредписанийДолжлиц"] = getStr(reportData.DataColumn78, municipalityId);
            section["КолвоВыполненныхПредписанийФизлиц"] = getStr(reportData.DataColumn79, municipalityId);
            section["КолвоВыполненныхПредписанийВнеплановое"] = getStr(reportData.DataColumn80, municipalityId);
            section["ОбщееКолвоНевыполненныхПредписаний"] = getStr(reportData.DataColumn81, municipalityId);
            section["КолвоНевыполненныхПредписанийПлановое"] = getStr(reportData.DataColumn82, municipalityId);
            section["КолвоНевыполненныхПредписанийВнеплановое"] = getStr(reportData.DataColumn83, municipalityId);
            section["КолвоНевыполненныхПредписанийИнспекционное"] = getStr(reportData.DataColumn84, municipalityId);
            section["КолвоНевыполненныхПредписанийПоЖалобам"] = getStr(reportData.DataColumn85, municipalityId);
            section["ОбщееКолвоНарушенийНевыполненныхПредписаний"] = getStr(reportData.DataColumn86, municipalityId);

            section["ОбщееКолвоПостановлений"] = getStr(reportData.DataColumn87, municipalityId);
            section["КолвоПостановленийГжи"] = getStr(reportData.DataColumn88, municipalityId);
            section["КолвоПостановленийСуд"] = getStr(reportData.DataColumn89, municipalityId);
            section["КолвоПостановленийЮрлиц"] = getStr(reportData.DataColumn90, municipalityId);
            section["КолвоПостановленийДолжлиц"] = getStr(reportData.DataColumn91, municipalityId);
            section["КолвоПостановленийФизжлиц"] = getStr(reportData.DataColumn92, municipalityId);
            section["КолвоПрекращенных"] = getStr(reportData.DataColumn93, municipalityId);
            section["КолвоШтрафов"] = getStr(reportData.DataColumn94, municipalityId);
            section["КолвоЗамечаний"] = getStr(reportData.DataColumn95, municipalityId);
            section["КолвоПредупреждений"] = getStr(reportData.DataColumn96, municipalityId);
            section["АдминистративныйАрест"] = getStr(reportData.DataColumn97, municipalityId);

            section["ОбщаяСуммаПредъявленоШтрафов"] = getStrDecimal(reportData.DataColumn98, municipalityId);
            section["СуммаПредъявленоШтрафовГжи"] = getStrDecimal(reportData.DataColumn99, municipalityId);
            section["СуммаПредъявленоШтрафовСуд"] = getStrDecimal(reportData.DataColumn100, municipalityId);
            section["СуммаПредъявленоШтрафовЮрлиц"] = getStrDecimal(reportData.DataColumn101, municipalityId);
            section["СуммаПредъявленоШтрафовДолжлиц"] = getStrDecimal(reportData.DataColumn102, municipalityId);
            section["СуммаПредъявленоШтрафовФизлиц"] = getStrDecimal(reportData.DataColumn103, municipalityId);
            section["ШтрафСобственники"] = getStrDecimal(reportData.DataColumn104, municipalityId);
            section["ШтрафПредприятия"] = getStrDecimal(reportData.DataColumn105, municipalityId);
            section["ШтрафНаниматели"] = getStrDecimal(reportData.DataColumn106, municipalityId);
            section["ШтрафИные"] = getStrDecimal(reportData.DataColumn107, municipalityId);

            var col109 = getStrDecimal(reportData.DataColumn109, municipalityId);
            var col110 = getStrDecimal(reportData.DataColumn110, municipalityId);
            section["ОбщаяСуммаПолученоШтрафов"] = col109 + col110;
            section["СуммаПолученоШтрафовГжи"] = col109;
            section["СуммаПолученоШтрафовСуд"] = col110;
        }

        private void FillRow(Section section, ReportData rp, long id)
        {
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimalArea = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] / 1000 : 0;
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimal = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;
            Func<Dictionary<long, long>, long, long> getStr = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;

            var col4 = getStr(rp.DataColumn4, id);
            var col6 = getStr(rp.DataColumn6, id);
            var col7 = getStr(rp.DataColumn7, id);

            section["КолвоДомовПлановое"] = col4;
            section["ПлощадьПлановое"] = getStrDecimalArea(rp.DataColumn5, id);
            section["ПлощадьДомовПлановоеМун"] = col6;
            section["ПлощадьДомовПлановоеТсж"] = col7;
            section["ПлощадьДомовПланИные"] = col4 - (col6 + col7);

            var col9 = getStr(rp.DataColumn9, id);
            var col11 = getStr(rp.DataColumn11, id);
            var col12 = getStr(rp.DataColumn12, id);
            var col13 = getStr(rp.DataColumn13, id);
            section["КолвоДомовВнеплановое"] = col9;
            section["ПлощадьВнеплановое"] = getStrDecimalArea(rp.DataColumn10, id);
            section["ПлощадьДомовВнеплановоеМун"] = col11;
            section["ПлощадьДомовВнеплановоеТсж"] = col12;
            section["ПлощадьВнеплановоеФизЛица"] = col13;
            section["ПлощадьДомовВнеПланИные"] = col9 - (col11 + col12 + col13);

            section["КолвоДомовИнспекционное"] = getStr(rp.DataColumn15, id);
            section["ПлощадьИнспекционное"] = getStrDecimalArea(rp.DataColumn16, id);

            section["КолвоДомовПоЖалобе"] = getStr(rp.DataColumn17, id);
            section["ПлощадьПоЖалобе"] = getStrDecimalArea(rp.DataColumn18, id);

            var col20 = getStr(rp.DataColumn20, id);
            var col21 = getStr(rp.DataColumn21, id);
            var col22 = getStr(rp.DataColumn22, id);
            var col23 = getStr(rp.DataColumn23, id);
            section["ОбщееКолвоНарушений"] = col20 + col21 + col22 + col23;
            section["КолвоНарушенийПлановое"] = col20;
            section["КолвоНарушенийВнеплановое"] = col21;
            section["КолвоНарушенийИнспекционное"] = col22;
            section["КолвоНарушенийПоЖалобам"] = col23;
            section["КолвоНарушПравилТехЭ"] = getStr(rp.DataColumn24, id);
            section["КолвоНарушПрПользЖил"] = getStr(rp.DataColumn25, id);
            section["КолвоНарушНормУровня"] = getStr(rp.DataColumn26, id);

            section["ОбщееКолвоПредписаний"] = getStr(rp.DataColumn27, id);
            section["КолвоПредписанийЮрлиц"] = getStr(rp.DataColumn28, id);
            section["КолвоПредписанийДолжлиц"] = getStr(rp.DataColumn29, id);
            section["КолвоПредписанийФизлиц"] = getStr(rp.DataColumn30, id);
            section["КолвоПредписанийПоЖалобам"] = getStr(rp.DataColumn31, id);
            section["КолвоПредписанийПланЮр"] = getStr(rp.DataColumn32, id);
            section["КолвоПредписанийВнепланЮр"] = getStr(rp.DataColumn33, id);
            section["КолвоПредписанийВнепланДл"] = getStr(rp.DataColumn34, id);
            section["КолвоПредписанийИнспЮр"] = getStr(rp.DataColumn35, id);
            section["КолвоПредписанийИнспДл"] = getStr(rp.DataColumn36, id);
            section["КолвоПредписанийЖалобыЮр"] = getStr(rp.DataColumn37, id);
            section["КолвоПредписанийЖалобыДл"] = getStr(rp.DataColumn38, id);
            section["КолвоПредписанийЖалобыФиз"] = getStr(rp.DataColumn39, id);

            section["ОбщееКолвоПротоколов"] = getStr(rp.DataColumn40, id);
            section["КолвоПротоколовЮр"] = getStr(rp.DataColumn41, id);
            section["КолвоПротоколовДл"] = getStr(rp.DataColumn42, id);
            section["КолвоПротоколовФиз"] = getStr(rp.DataColumn43, id);
            section["КолвоПротоколовЖалоб"] = getStr(rp.DataColumn44, id);
            section["КолвоПротоколовЮрлицПлановое"] = getStr(rp.DataColumn45, id);
            section["КолвоПротоколовЮрлицВнеплановое"] = getStr(rp.DataColumn46, id);
            section["КолвоПротоколовДолжлицВнеплановое"] = getStr(rp.DataColumn47, id);
            section["КолвоПротоколовЮрлицИнспекционное"] = getStr(rp.DataColumn48, id);
            section["КолвоПротоколовДолжлицИнспекционное"] = getStr(rp.DataColumn49, id);
            section["КолвоПротоколовЖалобЮрлиц"] = getStr(rp.DataColumn50, id);
            section["КолвоПротоколовЖалобДолжлиц"] = getStr(rp.DataColumn51, id);
            section["КолвоПротоколовЖалобФизлиц"] = getStr(rp.DataColumn52, id);
            section["КолвоПротоколовПоСт19_5"] = getStr(rp.DataColumn53, id);

            section["ОбщееКолвоАктов"] = getStr(rp.DataColumn54, id);
            section["КолвоАктовПлановое"] = getStr(rp.DataColumn55, id);
            section["КолвоАктовВнеплановое"] = getStr(rp.DataColumn56, id);
            section["КолвоАктовВнеплановоеИспПредписания"] = getStr(rp.DataColumn57, id);
            section["КолвоАктовПоЖалобе"] = getStr(rp.DataColumn58, id);
            var col59 = getStr(rp.DataColumn59, id);
            var col60 = getStr(rp.DataColumn60, id);
            var col61 = getStr(rp.DataColumn61, id);
            var col62 = getStr(rp.DataColumn62, id);
            var col63 = getStr(rp.DataColumn63, id);
            var col64 = getStr(rp.DataColumn64, id);
            var col65 = getStr(rp.DataColumn65, id);
            var col66 = getStr(rp.DataColumn66, id);
            var col67 = getStr(rp.DataColumn67, id);
            var col68 = getStr(rp.DataColumn68, id);
            var col69 = getStr(rp.DataColumn69, id);
            var col70 = getStr(rp.DataColumn70, id);
            var col71 = getStr(rp.DataColumn71, id);
            var col72 = getStr(rp.DataColumn72, id);
            section["КолвоАктовИнспекционное"] = col59;
            section["КолвоАктовИнспекционноеЗима"] = col60;
            section["КолвоАктовИнспекционноеСосульки"] = col61;
            section["КолвоАктовИнспекционноеПаводки"] = col62;
            section["КолвоАктовИнспекционноеКапремонт"] = col63;
            section["КолвоАктовИнспекционноеАдрАтрибутика"] = col64;
            section["КолвоАктовПоКомУчету"] = col65;
            section["КолвоАктовПридомТерр"] = col66;
            section["КолвоАктовАнтитеррор"] = col67;
            section["КолвоАктовВнутриГаз"] = col68;
            section["КолвоАктовПоПереустр"] = col69;
            section["КолвоАктовНежилПом"] = col70;
            section["КолвоАктовПоЛилфтам"] = col71;
            section["КолвоАктовПоСанитСост"] = col72;
            section["КолвоАктовДругие"] = col59 - (col60 + col61 + col62 + col63 + col64 + col65 + col66 + col67 + col68 + col69 + col70 + col71 + col72);

            section["КолвоАктовПроверкиВнеплановоеИспПредписания"] = getStr(rp.DataColumn74, id);
            section["ПлощадьАктовВнеплановоеИспПредписания"] = getStrDecimalArea(rp.DataColumn75, id);
            section["ОбщееКолвоВыполненныхПредписаний"] = getStr(rp.DataColumn76, id);
            section["КолвоВыполненныхПредписанийЮрлиц"] = getStr(rp.DataColumn77, id);
            section["КолвоВыполненныхПредписанийДолжлиц"] = getStr(rp.DataColumn78, id);
            section["КолвоВыполненныхПредписанийФизлиц"] = getStr(rp.DataColumn79, id);
            section["КолвоВыполненныхПредписанийВнеплановое"] = getStr(rp.DataColumn80, id);
            section["ОбщееКолвоНевыполненныхПредписаний"] = getStr(rp.DataColumn81, id);
            section["КолвоНевыполненныхПредписанийПлановое"] = getStr(rp.DataColumn82, id);
            section["КолвоНевыполненныхПредписанийВнеплановое"] = getStr(rp.DataColumn83, id);
            section["КолвоНевыполненныхПредписанийИнспекционное"] = getStr(rp.DataColumn84, id);
            section["КолвоНевыполненныхПредписанийПоЖалобам"] = getStr(rp.DataColumn85, id);
            section["ОбщееКолвоНарушенийНевыполненныхПредписаний"] = getStr(rp.DataColumn86, id);

            section["ОбщееКолвоПостановлений"] = getStr(rp.DataColumn87, id);
            section["КолвоПостановленийГжи"] = getStr(rp.DataColumn88, id);
            section["КолвоПостановленийСуд"] = getStr(rp.DataColumn89, id);
            section["КолвоПостановленийЮрлиц"] = getStr(rp.DataColumn90, id);
            section["КолвоПостановленийДолжлиц"] = getStr(rp.DataColumn91, id);
            section["КолвоПостановленийФизжлиц"] = getStr(rp.DataColumn92, id);
            section["КолвоПрекращенных"] = getStr(rp.DataColumn93, id);
            section["КолвоШтрафов"] = getStr(rp.DataColumn94, id);
            section["КолвоЗамечаний"] = getStr(rp.DataColumn95, id);
            section["КолвоПредупреждений"] = getStr(rp.DataColumn96, id);
            section["АдминистративныйАрест"] = getStr(rp.DataColumn97, id);

            section["ОбщаяСуммаПредъявленоШтрафов"] = getStrDecimal(rp.DataColumn98, id);
            section["СуммаПредъявленоШтрафовГжи"] = getStrDecimal(rp.DataColumn99, id);
            section["СуммаПредъявленоШтрафовСуд"] = getStrDecimal(rp.DataColumn100, id);
            section["СуммаПредъявленоШтрафовЮрлиц"] = getStrDecimal(rp.DataColumn101, id);
            section["СуммаПредъявленоШтрафовДолжлиц"] = getStrDecimal(rp.DataColumn102, id);
            section["СуммаПредъявленоШтрафовФизлиц"] = getStrDecimal(rp.DataColumn103, id);
            section["ШтрафСобственники"] = getStrDecimal(rp.DataColumn104, id);
            section["ШтрафПредприятия"] = getStrDecimal(rp.DataColumn105, id);
            section["ШтрафНаниматели"] = getStrDecimal(rp.DataColumn106, id);
            section["ШтрафИные"] = getStrDecimal(rp.DataColumn107, id);

            var col109 = getStrDecimal(rp.DataColumn109, id);
            var col110 = getStrDecimal(rp.DataColumn110, id);
            section["ОбщаяСуммаПолученоШтрафов"] = col109 + col110;
            section["СуммаПолученоШтрафовГжи"] = col109;
            section["СуммаПолученоШтрафовСуд"] = col110;
        }

        private void FillTotalRow(ReportParams section, ReportData sumZji)
        {
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimalArea = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] / 1000 : 0;
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimal = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;
            Func<Dictionary<long, long>, long, long> getStr = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;

            var id = 0;

            var col4 = getStr(sumZji.DataColumn4, id);
            var col6 = getStr(sumZji.DataColumn6, id);
            var col7 = getStr(sumZji.DataColumn7, id);

            section.SimpleReportParams["КолвоДомовПлановое"] = col4;
            section.SimpleReportParams["ПлощадьПлановое"] = getStrDecimalArea(sumZji.DataColumn5, id);
            section.SimpleReportParams["ПлощадьДомовПлановоеМун"] = col6;
            section.SimpleReportParams["ПлощадьДомовПлановоеТсж"] = col7;
            section.SimpleReportParams["ПлощадьДомовПланИные"] = col4 - (col6 + col7);

            var col9 = getStr(sumZji.DataColumn9, id);
            var col11 = getStr(sumZji.DataColumn11, id);
            var col12 = getStr(sumZji.DataColumn12, id);
            var col13 = getStr(sumZji.DataColumn13, id);
            section.SimpleReportParams["КолвоДомовВнеплановое"] = col9;
            section.SimpleReportParams["ПлощадьВнеплановое"] = getStrDecimalArea(sumZji.DataColumn10, id);
            section.SimpleReportParams["ПлощадьДомовВнеплановоеМун"] = col11;
            section.SimpleReportParams["ПлощадьДомовВнеплановоеТсж"] = col12;
            section.SimpleReportParams["ПлощадьВнеплановоеФизЛица"] = col13;
            section.SimpleReportParams["ПлощадьДомовВнеПланИные"] = col9 - (col11 + col12 + col13);

            section.SimpleReportParams["КолвоДомовИнспекционное"] = getStr(sumZji.DataColumn15, id);
            section.SimpleReportParams["ПлощадьИнспекционное"] = getStrDecimalArea(sumZji.DataColumn16, id);

            section.SimpleReportParams["КолвоДомовПоЖалобе"] = getStr(sumZji.DataColumn17, id);
            section.SimpleReportParams["ПлощадьПоЖалобе"] = getStrDecimalArea(sumZji.DataColumn18, id);

            var col20 = getStr(sumZji.DataColumn20, id);
            var col21 = getStr(sumZji.DataColumn21, id);
            var col22 = getStr(sumZji.DataColumn22, id);
            var col23 = getStr(sumZji.DataColumn23, id);
            section.SimpleReportParams["ОбщееКолвоНарушений"] = col20 + col21 + col22 + col23;
            section.SimpleReportParams["КолвоНарушенийПлановое"] = col20;
            section.SimpleReportParams["КолвоНарушенийВнеплановое"] = col21;
            section.SimpleReportParams["КолвоНарушенийИнспекционное"] = col22;
            section.SimpleReportParams["КолвоНарушенийПоЖалобам"] = col23;
            section.SimpleReportParams["КолвоНарушПравилТехЭ"] = getStr(sumZji.DataColumn24, id);
            section.SimpleReportParams["КолвоНарушПрПользЖил"] = getStr(sumZji.DataColumn25, id);
            section.SimpleReportParams["КолвоНарушНормУровня"] = getStr(sumZji.DataColumn26, id);

            section.SimpleReportParams["ОбщееКолвоПредписаний"] = getStr(sumZji.DataColumn27, id);
            section.SimpleReportParams["КолвоПредписанийЮрлиц"] = getStr(sumZji.DataColumn28, id);
            section.SimpleReportParams["КолвоПредписанийДолжлиц"] = getStr(sumZji.DataColumn29, id);
            section.SimpleReportParams["КолвоПредписанийФизлиц"] = getStr(sumZji.DataColumn30, id);
            section.SimpleReportParams["КолвоПредписанийПоЖалобам"] = getStr(sumZji.DataColumn31, id);
            section.SimpleReportParams["КолвоПредписанийПланЮр"] = getStr(sumZji.DataColumn32, id);
            section.SimpleReportParams["КолвоПредписанийВнепланЮр"] = getStr(sumZji.DataColumn33, id);
            section.SimpleReportParams["КолвоПредписанийВнепланДл"] = getStr(sumZji.DataColumn34, id);
            section.SimpleReportParams["КолвоПредписанийИнспЮр"] = getStr(sumZji.DataColumn35, id);
            section.SimpleReportParams["КолвоПредписанийИнспДл"] = getStr(sumZji.DataColumn36, id);
            section.SimpleReportParams["КолвоПредписанийЖалобыЮр"] = getStr(sumZji.DataColumn37, id);
            section.SimpleReportParams["КолвоПредписанийЖалобыДл"] = getStr(sumZji.DataColumn38, id);
            section.SimpleReportParams["КолвоПредписанийЖалобыФиз"] = getStr(sumZji.DataColumn39, id);

            section.SimpleReportParams["ОбщееКолвоПротоколов"] = getStr(sumZji.DataColumn40, id);
            section.SimpleReportParams["КолвоПротоколовЮр"] = getStr(sumZji.DataColumn41, id);
            section.SimpleReportParams["КолвоПротоколовДл"] = getStr(sumZji.DataColumn42, id);
            section.SimpleReportParams["КолвоПротоколовФиз"] = getStr(sumZji.DataColumn43, id);
            section.SimpleReportParams["КолвоПротоколовЖалоб"] = getStr(sumZji.DataColumn44, id);
            section.SimpleReportParams["КолвоПротоколовЮрлицПлановое"] = getStr(sumZji.DataColumn45, id);
            section.SimpleReportParams["КолвоПротоколовЮрлицВнеплановое"] = getStr(sumZji.DataColumn46, id);
            section.SimpleReportParams["КолвоПротоколовДолжлицВнеплановое"] = getStr(sumZji.DataColumn47, id);
            section.SimpleReportParams["КолвоПротоколовЮрлицИнспекционное"] = getStr(sumZji.DataColumn48, id);
            section.SimpleReportParams["КолвоПротоколовДолжлицИнспекционное"] = getStr(sumZji.DataColumn49, id);
            section.SimpleReportParams["КолвоПротоколовЖалобЮрлиц"] = getStr(sumZji.DataColumn50, id);
            section.SimpleReportParams["КолвоПротоколовЖалобДолжлиц"] = getStr(sumZji.DataColumn51, id);
            section.SimpleReportParams["КолвоПротоколовЖалобФизлиц"] = getStr(sumZji.DataColumn52, id);
            section.SimpleReportParams["КолвоПротоколовПоСт19_5"] = getStr(sumZji.DataColumn53, id);

            section.SimpleReportParams["ОбщееКолвоАктов"] = getStr(sumZji.DataColumn54, id);
            section.SimpleReportParams["КолвоАктовПлановое"] = getStr(sumZji.DataColumn55, id);
            section.SimpleReportParams["КолвоАктовВнеплановое"] = getStr(sumZji.DataColumn56, id);
            section.SimpleReportParams["КолвоАктовВнеплановоеИспПредписания"] = getStr(sumZji.DataColumn57, id);
            section.SimpleReportParams["КолвоАктовПоЖалобе"] = getStr(sumZji.DataColumn58, id);
            var col59 = getStr(sumZji.DataColumn59, id);
            var col60 = getStr(sumZji.DataColumn60, id);
            var col61 = getStr(sumZji.DataColumn61, id);
            var col62 = getStr(sumZji.DataColumn62, id);
            var col63 = getStr(sumZji.DataColumn63, id);
            var col64 = getStr(sumZji.DataColumn64, id);
            var col65 = getStr(sumZji.DataColumn65, id);
            var col66 = getStr(sumZji.DataColumn66, id);
            var col67 = getStr(sumZji.DataColumn67, id);
            var col68 = getStr(sumZji.DataColumn68, id);
            var col69 = getStr(sumZji.DataColumn69, id);
            var col70 = getStr(sumZji.DataColumn70, id);
            var col71 = getStr(sumZji.DataColumn71, id);
            var col72 = getStr(sumZji.DataColumn72, id);
            section.SimpleReportParams["КолвоАктовИнспекционное"] = col59;
            section.SimpleReportParams["КолвоАктовИнспекционноеЗима"] = col60;
            section.SimpleReportParams["КолвоАктовИнспекционноеСосульки"] = col61;
            section.SimpleReportParams["КолвоАктовИнспекционноеПаводки"] = col62;
            section.SimpleReportParams["КолвоАктовИнспекционноеКапремонт"] = col63;
            section.SimpleReportParams["КолвоАктовИнспекционноеАдрАтрибутика"] = col64;
            section.SimpleReportParams["КолвоАктовПоКомУчету"] = col65;
            section.SimpleReportParams["КолвоАктовПридомТерр"] = col66;
            section.SimpleReportParams["КолвоАктовАнтитеррор"] = col67;
            section.SimpleReportParams["КолвоАктовВнутриГаз"] = col68;
            section.SimpleReportParams["КолвоАктовПоПереустр"] = col69;
            section.SimpleReportParams["КолвоАктовНежилПом"] = col70;
            section.SimpleReportParams["КолвоАктовПоЛилфтам"] = col71;
            section.SimpleReportParams["КолвоАктовПоСанитСост"] = col72;
            section.SimpleReportParams["КолвоАктовДругие"] = col59 - (col60 + col61 + col62 + col63 + col64 + col65 + col66 + col67 + col68 + col69 + col70 + col71 + col72);

            section.SimpleReportParams["КолвоАктовПроверкиВнеплановоеИспПредписания"] = getStr(sumZji.DataColumn74, id);
            section.SimpleReportParams["ПлощадьАктовВнеплановоеИспПредписания"] = getStrDecimalArea(sumZji.DataColumn75, id);
            section.SimpleReportParams["ОбщееКолвоВыполненныхПредписаний"] = getStr(sumZji.DataColumn76, id);
            section.SimpleReportParams["КолвоВыполненныхПредписанийЮрлиц"] = getStr(sumZji.DataColumn77, id);
            section.SimpleReportParams["КолвоВыполненныхПредписанийДолжлиц"] = getStr(sumZji.DataColumn78, id);
            section.SimpleReportParams["КолвоВыполненныхПредписанийФизлиц"] = getStr(sumZji.DataColumn79, id);
            section.SimpleReportParams["КолвоВыполненныхПредписанийВнеплановое"] = getStr(sumZji.DataColumn80, id);
            section.SimpleReportParams["ОбщееКолвоНевыполненныхПредписаний"] = getStr(sumZji.DataColumn81, id);
            section.SimpleReportParams["КолвоНевыполненныхПредписанийПлановое"] = getStr(sumZji.DataColumn82, id);
            section.SimpleReportParams["КолвоНевыполненныхПредписанийВнеплановое"] = getStr(sumZji.DataColumn83, id);
            section.SimpleReportParams["КолвоНевыполненныхПредписанийИнспекционное"] = getStr(sumZji.DataColumn84, id);
            section.SimpleReportParams["КолвоНевыполненныхПредписанийПоЖалобам"] = getStr(sumZji.DataColumn85, id);
            section.SimpleReportParams["ОбщееКолвоНарушенийНевыполненныхПредписаний"] = getStr(sumZji.DataColumn86, id);

            section.SimpleReportParams["ОбщееКолвоПостановлений"] = getStr(sumZji.DataColumn87, id);
            section.SimpleReportParams["КолвоПостановленийГжи"] = getStr(sumZji.DataColumn88, id);
            section.SimpleReportParams["КолвоПостановленийСуд"] = getStr(sumZji.DataColumn89, id);
            section.SimpleReportParams["КолвоПостановленийЮрлиц"] = getStr(sumZji.DataColumn90, id);
            section.SimpleReportParams["КолвоПостановленийДолжлиц"] = getStr(sumZji.DataColumn91, id);
            section.SimpleReportParams["КолвоПостановленийФизжлиц"] = getStr(sumZji.DataColumn92, id);
            section.SimpleReportParams["КолвоПрекращенных"] = getStr(sumZji.DataColumn93, id);
            section.SimpleReportParams["КолвоШтрафов"] = getStr(sumZji.DataColumn94, id);
            section.SimpleReportParams["КолвоЗамечаний"] = getStr(sumZji.DataColumn95, id);
            section.SimpleReportParams["КолвоПредупреждений"] = getStr(sumZji.DataColumn96, id);
            section.SimpleReportParams["АдминистративныйАрест"] = getStr(sumZji.DataColumn97, id);

            section.SimpleReportParams["ОбщаяСуммаПредъявленоШтрафов"] = getStrDecimal(sumZji.DataColumn98, id);
            section.SimpleReportParams["СуммаПредъявленоШтрафовГжи"] = getStrDecimal(sumZji.DataColumn99, id);
            section.SimpleReportParams["СуммаПредъявленоШтрафовСуд"] = getStrDecimal(sumZji.DataColumn100, id);
            section.SimpleReportParams["СуммаПредъявленоШтрафовЮрлиц"] = getStrDecimal(sumZji.DataColumn101, id);
            section.SimpleReportParams["СуммаПредъявленоШтрафовДолжлиц"] = getStrDecimal(sumZji.DataColumn102, id);
            section.SimpleReportParams["СуммаПредъявленоШтрафовФизлиц"] = getStrDecimal(sumZji.DataColumn103, id);
            section.SimpleReportParams["ШтрафСобственники"] = getStrDecimal(sumZji.DataColumn104, id);
            section.SimpleReportParams["ШтрафПредприятия"] = getStrDecimal(sumZji.DataColumn105, id);
            section.SimpleReportParams["ШтрафНаниматели"] = getStrDecimal(sumZji.DataColumn106, id);
            section.SimpleReportParams["ШтрафИные"] = getStrDecimal(sumZji.DataColumn107, id);

            var col109 = getStrDecimal(sumZji.DataColumn109, id);
            var col110 = getStrDecimal(sumZji.DataColumn110, id);
            section.SimpleReportParams["ОбщаяСуммаПолученоШтрафов"] = col109 + col110;
            section.SimpleReportParams["СуммаПолученоШтрафовГжи"] = col109;
            section.SimpleReportParams["СуммаПолученоШтрафовСуд"] = col110;
        }
    }
}