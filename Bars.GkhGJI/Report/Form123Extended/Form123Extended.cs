namespace Bars.GkhGji.Report.Form123Extended
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Форма 123 (расширенная)"
    /// </summary>
    public class Form123ExtendedReport : BasePrintForm
    {
        List<string> listCodeJurTsjJSk = new List<string> { "9", "11" };
        List<string> listCodeJurOther = new List<string> { "8", "15", "18", "4" };
        List<string> listCodeOfficTsjJSk = new List<string> { "10", "12" };
        List<string> listCodeOfficOther = new List<string> { "13", "16", "19", "5" };
        List<string> listCodeIndivid = new List<string> { "6", "7", "14" };

        public IWindsorContainer Container { get; set; }

        private readonly ReportExtendedData reportData = new ReportExtendedData();

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        // учитывать постановления, возвращенные на новое рассмотрение
        private bool returned;

        #region Ссылки на данные столбцов
        private List<DisposalActCheckReport> column4;
        private List<DisposalActCheckReport> column12;
        private List<DisposalActCheckReport> column22;
        #endregion

        private Dictionary<long, ActCheckRobjectData> actCheckRoDict;
        private Dictionary<long, long> municipalityByActDict;

        public Form123ExtendedReport()
            : base(new ReportTemplateBinary(Properties.Resources.Forma123Extended))
        {
        }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.Form123Extended"; }
        }

        public override string Desciption
        {
            get { return "Форма 123 (расширенная)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Form123Extended"; }
        }

        public override string Name
        {
            get { return "Форма 123 (расширенная)"; }
        }

        public IDomainService<DocumentGjiChildren> serviceDocumentGjiChildren { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            returned = baseParams.Params["returned"].ToBool();

            var muIdsStr = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = string.IsNullOrEmpty(muIdsStr)
                                  ? new long[0]
                                  : muIdsStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["MonthDateStart"] = dateStart.ToString("MMMM");
            reportParams.SimpleReportParams["MonthDateEnd"] = dateEnd.ToString("MMMM");
            
            var sectionData = reportParams.ComplexReportParams.ДобавитьСекцию("SectionData");
            
            var dictMu = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var total = new ReportExtendedData();

            // получение данных
            FillData();

            foreach (var municipality in dictMu)
            {
                sectionData.ДобавитьСтроку();
                sectionData["MuName"] = municipality.Value;

                this.FillRow(sectionData, municipality.Key);
            }

            this.Total(total);            

            FillTotalRow(reportParams, total);
        }

        private void FillData()
        {
            this.GetActData();
            this.GetViolationData();
            this.GetDataPrescriptionIssued();
            this.GetDataProtocols();
            this.GetDataActRemoval();
            this.GetDataProtocols19_5();
            this.GetDataResolution();
        }

        private void GetActData()
        {
            var serviceActCheckRealityObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceManagingOrganization = Container.Resolve<IDomainService<ManagingOrganization>>();
            var serviceActCheck = Container.Resolve<IDomainService<ActCheck>>();
            var start = 0;

            var queryActCheckRo = serviceActCheckRealityObject.GetAll()
                         .WhereIf(dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= dateEnd)
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Where(x => x.ActCheck.TypeDocumentGji == TypeDocumentGji.ActCheck);


            municipalityByActDict = queryActCheckRo
                         .Select(x => new { actid = x.ActCheck.Id, municipalityId = x.RealityObject.Municipality.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.actid)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.municipalityId).Distinct().First());
           
            var actCheckIds = queryActCheckRo.Select(x => x.ActCheck.Id).Distinct();
            
            var listActCheckIdDisposalId = serviceDocumentGjiChildren.GetAll()
                                          .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                          .Where(x => actCheckIds.Contains(x.Children.Id))
                                          .Select(x => new ParentChildProxy(x.Parent.Id, x.Children.Id))
                                          .AsEnumerable()
                                          .Distinct()
                                          .ToList();

            var listActId = listActCheckIdDisposalId.Select(x => x.child).Distinct().ToList();
            var listDisposalId = listActCheckIdDisposalId.Select(x => x.parent).Distinct().ToList();

            var listActCheck = new List<ActCheckProxy>();
            var listDisposal = new List<DisposalProxy>();


            while (start < listActId.Count)
            {
                var tmpId = listActId.Skip(start).Take(1000).ToArray();
                listActCheck.AddRange(
                    serviceActCheck.GetAll().Where(x => tmpId.Contains(x.Id))
                    .Select(x => new
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
                                    if (x.InspectionContragentId.HasValue)
                                    {
                                        res.InspectionContragentId = x.InspectionContragentId.Value;
                                    }
                                    
                                    return res;
                                })
                    .ToArray());
                start += 1000;
            }

            var t = listActCheck.Where(x => x.InspectionContragentId == null || x.InspectionContragentId == 0).ToList();

            start = 0;
            while (start < listDisposalId.Count)
            {
                var tmpId = listDisposalId.Skip(start).Take(1000).ToArray();
                listDisposal.AddRange(Container.Resolve<IDomainService<Disposal>>().GetAll()
                    .Where(x => tmpId.Contains(x.Id))
                    .Select(x => new DisposalProxy
                    {
                        Id = x.Id,
                        KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0,
                        InspectionTypeBase = x.Inspection.TypeBase,
                        InspectionPersonInspection = x.Inspection.PersonInspection,
                        TypeDisposal = x.TypeDisposal
                    })
                    .AsEnumerable()
                    .ToArray());
                start += 1000;
            }

            var actCheckDict = listActCheck.Distinct().ToDictionary(x => x.Id, x => x);
            var disposalDict = listDisposal.Distinct().ToDictionary(x => x.Id, x => x);

            var listContragentsId = listActCheck.Where(x => x.InspectionContragentId > 0).Select(x => x.InspectionContragentId).Distinct().ToArray();

            start = 1000;

            var tmpFirstIds = listContragentsId.Take(start).ToArray();

            var actTypeManagementList = serviceManagingOrganization.GetAll()
                                           .Where(x => tmpFirstIds.Contains(x.Contragent.Id))
                                           .Select(x => new { x.TypeManagement, contragentId = x.Contragent.Id })
                                           .ToList();

            while (start < listContragentsId.Length)
            {
                var tmpIds = listContragentsId.Skip(start).Take(1000).ToArray();
                actTypeManagementList.AddRange(serviceManagingOrganization.GetAll()
                                           .Where(x => tmpIds.Contains(x.Contragent.Id))
                                           .Select(x => new { x.TypeManagement, contragentId = x.Contragent.Id })
                                           .ToList());

                start += 1000;
            }

            var contragentTypeManagementDict = actTypeManagementList
                .GroupBy(x => x.contragentId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.TypeManagement).First());

            var actTypeManagementDict = listActCheck
                .Where(x => x.InspectionContragentId != null && contragentTypeManagementDict.ContainsKey(x.InspectionContragentId))
                .ToDictionary(x => x.Id, x => contragentTypeManagementDict[x.InspectionContragentId]);

            var listActCheckDisposal = listActCheckIdDisposalId
                .Select(x => new DisposalActCheckReport
                    {
                        muId = municipalityByActDict[x.child],
                        TypeManagement = actTypeManagementDict.ContainsKey(x.child) ? actTypeManagementDict[x.child] : 0,
                        disposal = disposalDict.ContainsKey(x.parent) ? disposalDict[x.parent] : null,
                        actCheck = actCheckDict.ContainsKey(x.child) ? actCheckDict[x.child] : null
                    })
                .Where(x => x.disposal != null && x.actCheck != null).ToList();

            var actProxyList = listActCheckDisposal
                .Select(x => new ActCheckProxy { Id = x.actCheck.Id, Area = x.actCheck.Area })
                .ToList();

            actCheckRoDict = this.InitActCheckRoDict(actProxyList);

            //4-31
            GetActRobjectData(listActCheckDisposal);
        }

        //4-31
        private void GetActRobjectData(List<DisposalActCheckReport> actCheckDisposalList)
        {
            Func<IGrouping<long, DisposalActCheckReport>, CountAreaHousesActCheckReport> calcRoData = x => GetCountActCheckRo(x.Select(y => y.actCheck).ToList());

        // Плановое обледование
            var actsList = actCheckDisposalList
                .Where(x => (x.disposal.KindCheckCode == TypeCheck.PlannedExit || x.disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                            || x.actCheck.InspectionTypeBase == TypeBase.Inspection)
                .ToList();

            column4 = actsList.Select(x => x).ToList();

            var realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);
            reportData.DataColumn4 = realityObjData.ToDictionary(x => x.Key, x => (long)(long)x.Value.Count);
            reportData.DataColumn5 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);

            var actsListUk = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                                     .GroupBy(x => x.muId)
                                     .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn6 = actsListUk.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn7 = actsListUk.ToDictionary(x => x.Key, x => x.Value.Area);

            var actsListTsjJsk = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                         .GroupBy(x => x.muId)
                                         .ToDictionary(x => x.Key, calcRoData);
            reportData.DataColumn8 = actsListTsjJsk.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn9 = actsListTsjJsk.ToDictionary(x => x.Key, x => x.Value.Area);
            
        // Внеплановое обледование
            // по обращениям граждан (жалобы)
            actsList.Clear();
            actsList = actCheckDisposalList
                .Where(x => (x.disposal.KindCheckCode == TypeCheck.NotPlannedExit
                                || x.disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation)
                         && (x.disposal.InspectionTypeBase == TypeBase.CitizenStatement))
                .ToList();

            column12 = actsList.Select(x => x).ToList();

            realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn12 = realityObjData.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn13 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);

            actsListUk = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                .Where(x => x.disposal.InspectionPersonInspection != PersonInspection.PhysPerson)
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn14 = actsListUk.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn15 = actsListUk.ToDictionary(x => x.Key, x => x.Value.Area);

            actsListTsjJsk = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                     .Where(x => x.disposal.InspectionPersonInspection != PersonInspection.PhysPerson)
                                     .GroupBy(x => x.muId)
                                     .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn16 = actsListTsjJsk.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn17 = actsListTsjJsk.ToDictionary(x => x.Key, x => x.Value.Area);

            var actsListOther = actsList.Where(x => x.TypeManagement != TypeManagementManOrg.JSK && x.TypeManagement != TypeManagementManOrg.TSJ && x.TypeManagement != TypeManagementManOrg.UK)
                                        .Where(x => x.disposal.InspectionPersonInspection != PersonInspection.PhysPerson)
                                        .GroupBy(x => x.muId)
                                        .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn18 = actsListOther.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn19 = actsListOther.ToDictionary(x => x.Key, x => x.Value.Area);

            var actsListPhysPerson = actsList.Where(x => x.TypeManagement != TypeManagementManOrg.UK)
                                             .Where(x => x.TypeManagement != TypeManagementManOrg.TSJ || x.TypeManagement != TypeManagementManOrg.JSK)
                                             .Where(x => x.disposal.InspectionPersonInspection == PersonInspection.PhysPerson)
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn20 = actsListPhysPerson.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn21 = actsListPhysPerson.ToDictionary(x => x.Key, x => x.Value.Area);

            // Инспекционное обследование
            actsList.Clear();
            actsList = actCheckDisposalList.Where(x => (x.disposal.KindCheckCode == TypeCheck.InspectionSurvey) 
                                                        && (x.disposal.InspectionTypeBase != TypeBase.Inspection))
                                           .ToList();

            column22 = actsList.Select(x => x).ToList();

            realityObjData = actsList.GroupBy(x => x.muId).ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn22 = realityObjData.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn23 = realityObjData.ToDictionary(x => x.Key, x => x.Value.Area);

            actsListUk = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                                 .Where(x => x.disposal.InspectionPersonInspection != PersonInspection.PhysPerson)
                                 .GroupBy(x => x.muId)
                                 .ToDictionary(x => x.Key, calcRoData);


            reportData.DataColumn24 = actsListUk.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn25 = actsListUk.ToDictionary(x => x.Key, x => x.Value.Area);

            actsListTsjJsk = actsList.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                .Where(x => x.disposal.InspectionPersonInspection != PersonInspection.PhysPerson)
                                     .GroupBy(x => x.muId)
                                     .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn26 = actsListTsjJsk.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn27 = actsListTsjJsk.ToDictionary(x => x.Key, x => x.Value.Area);

            actsListPhysPerson = actsList.Where(x => x.TypeManagement != TypeManagementManOrg.UK)
                                         .Where(x => x.TypeManagement != TypeManagementManOrg.TSJ || x.TypeManagement != TypeManagementManOrg.JSK)
                                         .Where(x => x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                                         .Where(x => x.disposal.InspectionPersonInspection == PersonInspection.PhysPerson)
                                         .GroupBy(x => x.muId)
                                         .ToDictionary(x => x.Key, calcRoData);

            reportData.DataColumn30 = actsListPhysPerson.ToDictionary(x => x.Key, x => (long)x.Value.Count);
            reportData.DataColumn31 = actsListPhysPerson.ToDictionary(x => x.Key, x => x.Value.Area);
        }

        //33-42
        private void GetViolationData()
        {
            var serviceActCheckViolation = Container.Resolve<IDomainService<ActCheckViolation>>();

            var listAllActCheck = new List<long>();

            listAllActCheck.AddRange(column4.Select(x => x.actCheck.Id));
            listAllActCheck.AddRange(column12.Select(x => x.actCheck.Id));
            listAllActCheck.AddRange(column22.Select(x => x.actCheck.Id));

            // Список ActCheck с Violation
            var start = 0;
            var listActCheckViolation = new List<ActCheckViolationProxy>();
            var listAllActCheckIdsDistinct = listAllActCheck.Distinct().ToList();
            while (start < listAllActCheckIdsDistinct.Count)
            {
                var tmpId = listAllActCheckIdsDistinct.Skip(start).Take(1000).ToArray();
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

            reportData.DataColumn33 = column4.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                                             .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                             .Select(x => actCheckWithViolations[x.actCheck.Id])
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn34 = column4.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                             .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                             .Select(x => actCheckWithViolations[x.actCheck.Id])
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn35 = column4.Where(x => x.TypeManagement != TypeManagementManOrg.TSJ && x.TypeManagement != TypeManagementManOrg.JSK
                                                        && x.TypeManagement != TypeManagementManOrg.UK)
                                             .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                             .Select(x => actCheckWithViolations[x.actCheck.Id])
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn36 = column12.Where(x => x.TypeManagement == TypeManagementManOrg.UK)                                              
                                              .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                              .Select(x => actCheckWithViolations[x.actCheck.Id])
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn37 = column12.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                              .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                              .Select(x => actCheckWithViolations[x.actCheck.Id])
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn39 = column12.Where(x => x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                                              .Where(x => x.disposal.InspectionPersonInspection == PersonInspection.PhysPerson)
                                              .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                              .Select(x => actCheckWithViolations[x.actCheck.Id])
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn38 = column12.Where(x => x.TypeManagement != TypeManagementManOrg.UK 
                                                            && (x.TypeManagement != TypeManagementManOrg.TSJ || x.TypeManagement != TypeManagementManOrg.JSK))
                                              .Where(x => x.disposal.InspectionTypeBase != TypeBase.CitizenStatement)
                                              .Where(x => x.disposal.InspectionPersonInspection != PersonInspection.PhysPerson)
                                              .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                              .Select(x => actCheckWithViolations[x.actCheck.Id])
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn40 = column22.Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                                              .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                              .Select(x => actCheckWithViolations[x.actCheck.Id])
                                              .GroupBy(x => x.muId)
                                              .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn41 = column22.Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                                             .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                             .Select(x => actCheckWithViolations[x.actCheck.Id])
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());

            reportData.DataColumn42 = column22.Where(x => x.TypeManagement != TypeManagementManOrg.TSJ && x.TypeManagement != TypeManagementManOrg.JSK
                                                        && x.TypeManagement != TypeManagementManOrg.UK)
                                             .Where(x => actCheckWithViolations.ContainsKey(x.actCheck.Id))
                                             .Select(x => actCheckWithViolations[x.actCheck.Id])
                                             .GroupBy(x => x.muId)
                                             .ToDictionary(x => x.Key, x => (long)x.SelectMany(y => y.violations).Count());
        }

        //43-63
        private void GetDataPrescriptionIssued()
        {
            var servicePrescriptionViol = Container.Resolve<IDomainService<PrescriptionViol>>();
            var servicePrescription = Container.Resolve<IDomainService<Prescription>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            var start = 0;

            var listPrescriptionMu = servicePrescriptionViol.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
                         .Select(x => new { docId = x.Document.Id, muId = x.InspectionViolation.RealityObject.Municipality.Id })                         
                         .AsEnumerable()
                         .Distinct()
                         .ToList();

            var dictPrescriptionMu = listPrescriptionMu.GroupBy(x => x.docId).ToDictionary(x => x.Key, x => x.Select(y => y.muId).Distinct().First());

            var listPrescriptionsId = dictPrescriptionMu.Keys;

            reportData.DataColumn43 = listPrescriptionMu.GroupBy(x => x.muId).ToDictionary(x => x.Key, v => (long)v.Select(y => y.docId).Distinct().Count());

            var prescriptions = new List<PrescriptionProxy>();
            while (start < listPrescriptionsId.Count)
            {
                var tmpId = listPrescriptionsId.Skip(start).Take(1000).ToArray();
                prescriptions.AddRange(servicePrescription.GetAll()
                    .Where(x => tmpId.Contains(x.Id))
                    .Select(x => new PrescriptionProxy
                                        {
                                            Id = x.Id,
                                            ExecutantCode = x.Executant != null ? x.Executant.Code : string.Empty,
                                            parentStage = x.Stage.Parent.Id.ToLong()
                                        })
                    .ToList());
                start += 1000;
            }

            start = 0;
            var listPresDisposalProxy = new List<DisposalProxy>();
            var disposalStageIds = prescriptions.Select(x => x.parentStage).Distinct().ToList();

            while (start < disposalStageIds.Count)
            {
                var tmpId = disposalStageIds.Skip(start).Take(1000).ToArray();
                listPresDisposalProxy.AddRange(serviceDisposal.GetAll()
                                            .Where(x => tmpId.Contains(x.Stage.Id))
                                            .Select(x => new DisposalProxy
                                            {
                                                Id = x.Id,
                                                InspectionTypeBase = x.Inspection.TypeBase,
                                                stageId = x.Stage.Id,
                                                KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0
                                            })
                                            .ToList());
                start += 1000;
            }

            var disposalsByStageDict = listPresDisposalProxy.Distinct().ToDictionary(x => x.stageId, x => x);

            var listPresDisposal = prescriptions.Where(x => disposalsByStageDict.ContainsKey(x.parentStage))
                                                .Select(x => new DocGjiReportDisposal { Disposal = disposalsByStageDict[x.parentStage], DocGjiForm123Extended = x })
                                                .ToList();
    //при плановых проверках
        //юридическим лицам
            var listEntitiesColumn44_49 = listPresDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                                          .Where(x => (x.Disposal.KindCheckCode == TypeCheck.PlannedExit || x.Disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                                                                      || (x.Disposal.InspectionTypeBase == TypeBase.Inspection));
            //УК
            reportData.DataColumn44 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn44_49, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn45 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn44_49, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn46 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn44_49, listCodeJurOther);
        //должностным лицам
            //УК
            reportData.DataColumn47 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn44_49, new List<string> { "1" });
            //ТСЖ / ЖСК
            reportData.DataColumn48 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn44_49, listCodeOfficTsjJSk);
            //иные
            reportData.DataColumn49 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn44_49, listCodeOfficOther);

    //при плановых проверках
        //юридическим лицам
            var listEntitiesColumn50_56 = listPresDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                              .Where(x => x.Disposal.KindCheckCode == TypeCheck.NotPlannedExit || x.Disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation
                                                            && x.Disposal.InspectionTypeBase == TypeBase.CitizenStatement);
            //УК
            reportData.DataColumn50 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn51 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, listCodeJurTsjJSk);                
            // иные
            reportData.DataColumn52 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, listCodeJurOther);
        //по обращениям граждан (жалобам)
            //УК
            reportData.DataColumn53 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, new List<string> { "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn54 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, listCodeOfficTsjJSk);
            // иные
            reportData.DataColumn55 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, listCodeOfficOther);
            //физ. лицам
            reportData.DataColumn56 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn50_56, listCodeIndivid);

    //при инспекционных обследованиях (по направлениям деятельности )
        //юридическим лицам
            var listEntitiesColumn57_63 = listPresDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                              .Where(x => x.Disposal.KindCheckCode == TypeCheck.InspectionSurvey && x.Disposal.InspectionTypeBase != TypeBase.Inspection);
            //УК
            reportData.DataColumn57 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn58 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn59 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, listCodeJurOther);
            //по обращениям граждан (жалобам)
            //УК
            reportData.DataColumn60 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, new List<string> { "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn61 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, listCodeOfficTsjJSk);
            // иные
            reportData.DataColumn62 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, listCodeOfficOther);
            //физ. лицам
            reportData.DataColumn63 = GetDictDataDocumentGji(dictPrescriptionMu, listEntitiesColumn57_63, listCodeIndivid);
        }

        //64-84
        private void GetDataProtocols()
        {
            var serviceProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var serviceProtocol = Container.Resolve<IDomainService<Protocol>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            var start = 0;

            var listProtocolsMuId = serviceProtocolViolation.GetAll()
                                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                                         .WhereIf(dateEnd != DateTime.MaxValue, x => x.Document.DocumentDate <= dateEnd)
                                         .Select(x => new { docId = x.Document.Id, muId = x.InspectionViolation.RealityObject.Municipality.Id })
                                         .ToList();

            var dictProtocolsMuId = listProtocolsMuId.Distinct(x => x.docId).GroupBy(x => x.docId).ToDictionary(x => x.Key, v => v.Select(z => z.muId).First());

            var listProtocolsId = dictProtocolsMuId.Keys;

            var listProtocols = new List<ProtocolProxy>();
            while (start < listProtocolsId.Count)
            {
                var tmpId = listProtocolsId.Skip(start).Take(1000).ToArray();
                listProtocols.AddRange(serviceProtocol.GetAll()
                    .Where(x => tmpId.Contains(x.Id))
                    .Select(x => new ProtocolProxy
                            {
                                Id = x.Id,
                                ExecutantCode = x.Executant.Code ?? string.Empty,
                                parentStage = x.Stage.Parent.Id.ToLong(),
                                InspectionTypeBase = x.Inspection.TypeBase
                            })
                    .ToList());
                start += 1000;
            }

            var protocols = listProtocols.Distinct().ToList();

            reportData.DataColumn64 = listProtocolsMuId.GroupBy(x => x.muId).ToDictionary(x => x.Key, v => (long)v.Select(y => y.docId).Distinct().Count());

            start = 0;
            var listProtDisposalProxy = new List<DisposalProxy>();
            var disposalStageIds = protocols.Select(x => x.parentStage).Distinct().ToList();

            while (start < disposalStageIds.Count)
            {
                var tmpId = disposalStageIds.Skip(start).Take(1000).ToArray();
                listProtDisposalProxy.AddRange(serviceDisposal.GetAll()
                                            .Where(x => tmpId.Contains(x.Stage.Id))
                                            .Select(x => new DisposalProxy
                                            {
                                                Id = x.Id,
                                                InspectionTypeBase = x.Inspection.TypeBase,
                                                stageId = x.Stage.Id.ToLong(),
                                                KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0
                                            })
                                            .ToList());
                start += 1000;
            }

            var disposalsByStageDict = listProtDisposalProxy.Distinct().ToDictionary(x => x.stageId, x => x);

            var listProtocolsDisposal = protocols.Where(x => disposalsByStageDict.ContainsKey(x.parentStage))
                                                 .Select(x => new DocGjiReportDisposal { Disposal = disposalsByStageDict[x.parentStage], DocGjiForm123Extended = x })
                                                 .ToList();
    //при плановых проверках
        //юридическим лицам
            var listEntitiesColumn65_70 = listProtocolsDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                                               .Where(x => (x.Disposal.KindCheckCode == TypeCheck.PlannedExit || x.Disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                                                                    || (x.Disposal.InspectionTypeBase == TypeBase.Inspection));
            //УК
            reportData.DataColumn65 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn65_70, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn66 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn65_70, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn67 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn65_70, listCodeJurOther);
        //должностным лицам
            //УК
            reportData.DataColumn68 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn65_70, new List<string> { "1" });
            //ТСЖ / ЖСК
            reportData.DataColumn69 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn65_70, listCodeOfficTsjJSk);
            //иные
            reportData.DataColumn70 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn65_70, listCodeOfficOther);

//при внеплановых проверках
    //по обращениям граждан (жалобам)
            var listEntitiesColumn71_76 = listProtocolsDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                              .Where(x => (x.Disposal.KindCheckCode == TypeCheck.NotPlannedExit || x.Disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation)
                                                            && x.Disposal.InspectionTypeBase == TypeBase.CitizenStatement);
        //юридическим лицам
            //УК
            reportData.DataColumn71 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn72 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn73 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, listCodeJurOther);
        //должностным лицам
            //УК
            reportData.DataColumn74 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, new List<string> { "1" });
            //ТСЖ / ЖСК
            reportData.DataColumn75 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, listCodeOfficTsjJSk);
            //иные
            reportData.DataColumn76 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, listCodeOfficOther);
            //физ. лицам
            reportData.DataColumn77 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn71_76, listCodeIndivid);

    //при инспекционных обследованиях (по направлениям деятельности)
        //юридическим лицам
            var listEntitiesColumn78_84 = listProtocolsDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                              .Where(x => x.Disposal.KindCheckCode == TypeCheck.InspectionSurvey && x.Disposal.InspectionTypeBase != TypeBase.Inspection);
            //УК
            reportData.DataColumn78 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn79 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn80 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, listCodeJurOther);
        //по обращениям граждан (жалобам)
            //УК
            reportData.DataColumn81 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, new List<string> { "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn82 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, listCodeOfficTsjJSk);
            // иные
            reportData.DataColumn83 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, listCodeOfficOther);
            //физ. лицам
            reportData.DataColumn84 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn78_84, listCodeIndivid);
        }

        //85-118
        private void GetDataActRemoval()
        {
            var serviceActRemovalViolation = Container.Resolve<IDomainService<ActRemovalViolation>>();
            var servesActRemoval = Container.Resolve<IDomainService<ActRemoval>>();
            //var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servicePrescription = Container.Resolve<IDomainService<Prescription>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            var start = 0;

            // чтобы получить все Акты устранения для МО необходимо Через нарушения получить Дом и по его МО брать фильтр
            var queryActRemovalIdMu = serviceActRemovalViolation.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MaxValue, x => x.Document.DocumentDate <= dateEnd);

            var queryActRemovalId = queryActRemovalIdMu.Select(x => x.Document.Id).Distinct();

            var listActRemovalIdMuId = queryActRemovalIdMu
                                            .Select(x => new ActRemovalViolationProxy
                                                    {
                                                        DateFactRemoval = x.InspectionViolation.DateFactRemoval,
                                                        Id = x.Document.Id,
                                                        MuId = x.InspectionViolation.RealityObject.Municipality.Id,
                                                        InspectionViolationId = x.InspectionViolation.Id
                                                    });
            
            var dictActRemovalMu = queryActRemovalIdMu.Select(x => new
                                                                       {
                                                                           docId = x.Document.Id, 
                                                                           MuId = x.InspectionViolation.RealityObject.Municipality.Id
                                                                       })
                                                      .AsEnumerable()
                                                      .GroupBy(x => x.docId)
                                                      .ToDictionary(x => x.Key, v => v.Select(z => z.MuId).First());
            

          var listActRemoval = servesActRemoval.GetAll()
                                .Where(x => queryActRemovalId.Contains(x.Id))
                                .Select(x => new ActRemovalProxy
                                                 {
                                                     Id = x.Id,
                                                     Area = x.Area,
                                                     InspectionTypeBase = x.Inspection.TypeBase,
                                                     parentStage = x.Stage.Parent.Id.ToLong()
                                                 })
                                .ToList();

            var listActRemovalProxy = listActRemoval.Select(x => new { muId = dictActRemovalMu[x.Id], x.Id, x.Area, x.InspectionTypeBase });

            var listActRemovalDateFact = listActRemovalIdMuId.Where(x => x.DateFactRemoval.HasValue).ToList();

            // Количество актов проверки предписаний
            reportData.DataColumn85 = listActRemovalProxy.GroupBy(x => x.muId).ToDictionary(x => x.Key, x => (long)x.Select(y => y.Id).Distinct().Count());

            // Сумма поля "Площадь" из актов проверки предписаний
            reportData.DataColumn86 = listActRemovalProxy.GroupBy(x => x.muId)
                                                         .ToDictionary(
                                                             x => x.Key,
                                                             x => x.Select(y => y.Area).Sum(y => y.HasValue ? y.Value : 0));

            // Исполнено предписаний всего (ед.)
            reportData.DataColumn87 = listActRemovalDateFact.GroupBy(x => x.MuId)
                                                            .ToDictionary(x => x.Key, v => (long)v.Select(y => y.Id).Distinct().Count());

            var listActRemovalPres = serviceDocumentGjiChildren.GetAll()
                                         .Where(x => queryActRemovalId.Contains(x.Children.Id))
                                         .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                                         .Select(x => new { parentId = x.Parent.Id, childrenId = x.Children.Id })
                                         .ToList();

            var dictPrecriptionActRemov = listActRemovalPres.GroupBy(x => x.childrenId)
                                                                    .ToDictionary(
                                                                        x => x.Key,
                                                                        v => v.Select(y => y.parentId).Last());

            var listPrescriptionId = listActRemovalPres.Select(x => x.parentId).Distinct().ToList();

            start = 0;
            var listPrescription = new List<PrescriptionProxy>();
            while (start < listPrescriptionId.Count)
            {
                var tmpId = listPrescriptionId.Skip(start).Take(1000).ToArray();
                listPrescription.AddRange(servicePrescription.GetAll()
                             .Where(x => tmpId.Contains(x.Id))
                             .Select(
                                 x =>
                                 new PrescriptionProxy
                                     {
                                         Id = x.Id,
                                         ExecutantCode = x.Executant.Code,
                                         InspectionTypeBase = x.Inspection.TypeBase,
                                         parentStage = x.Stage.Parent.Id.ToLong()
                                     })
                             .ToList());
                start += 1000;
            }


            var listPresDisposal = new List<DisposalProxy>();
            var listPresStageParentId = listPrescription.Select(x => x.parentStage).Distinct().ToList();

            start = 0;
            while (start < listPresStageParentId.Count)
            {
                var tmpIds = listPresStageParentId.Skip(start).Take(1000).ToList();
                listPresDisposal.AddRange(serviceDisposal.GetAll()
                                   .Where(x => tmpIds.Contains(x.Stage.Id))
                                   .Select(x => new DisposalProxy
                                                       {
                                                           Id = x.Id,
                                                           InspectionTypeBase = x.Inspection.TypeBase,
                                                           stageId = x.Stage.Id,
                                                           KindCheckCode = x.KindCheck != null ? x.KindCheck.Code : 0
                                                       })
                                   .AsEnumerable());
                start += 1000;
            }

            var disposalsByStageDict = listPresDisposal.Distinct().ToDictionary(x => x.stageId, x => x);

            var listActRemPresDisposal = listActRemoval.Where(x => disposalsByStageDict.ContainsKey(x.parentStage)).Select(x => new 
                                                      {
                                                          actRemoval = x,
                                                          prescription = dictPrecriptionActRemov[x.Id],
                                                          disposal = disposalsByStageDict[x.parentStage]
                                                      })
                                      .ToList();
    //при плановых проверках
        //юридическим лицам
            var listEntitiesColumn88_93 = listActRemPresDisposal.Where(x => x.disposal != null && x.prescription != null && x.actRemoval != null)
                                                .Where(x => (x.disposal.KindCheckCode == TypeCheck.PlannedExit || x.disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                                                        || (x.disposal.InspectionTypeBase == TypeBase.Inspection))
                                                .Select(x => new ActRemovalPrescription
                                                                {
                                                                    actRemoval = x.actRemoval,
                                                                    DocGjiForm123Extended = listPrescription.Last(y => y.Id == x.prescription)
                                                                })
                                                .ToList();
            //УК
            reportData.DataColumn88 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn88_93, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn89 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn88_93, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn90 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn88_93, listCodeJurOther);
        //должностным лицам
            //УК
            reportData.DataColumn91 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn88_93, new List<string> { "1" });
            //ТСЖ / ЖСК
            reportData.DataColumn92 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn88_93, listCodeOfficTsjJSk);
            //иные
            reportData.DataColumn93 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn88_93, listCodeOfficOther);

//при внеплановых проверках
    //по обращениям граждан (жалобам)
            var listEntitiesColumn94_100 = listActRemPresDisposal.Where(x => x.disposal != null && x.prescription != null && x.actRemoval != null)
                                                .Where(x => (x.disposal.KindCheckCode == TypeCheck.NotPlannedExit || x.disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation)
                                                            && x.disposal.InspectionTypeBase == TypeBase.CitizenStatement)
                                                .Select(x => new ActRemovalPrescription
                                                                {
                                                                    actRemoval = x.actRemoval,
                                                                    DocGjiForm123Extended = listPrescription.Last(y => y.Id == x.prescription)
                                                                })
                                                .ToList();
        //юридическим лицам
            //УК
            reportData.DataColumn94 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn95 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn96 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, listCodeJurOther);
        //должностным лицам
            //УК
            reportData.DataColumn97 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, new List<string> { "1" });
            //ТСЖ / ЖСК
            reportData.DataColumn98 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, listCodeOfficTsjJSk);
            //иные
            reportData.DataColumn99 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, listCodeOfficOther);
            //физ. лицам
            reportData.DataColumn100 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn94_100, listCodeIndivid);

    //при инспекционных обследованиях (по направлениям деятельности)
        //юридическим лицам
            var listEntitiesColumn101_107 = listActRemPresDisposal.Where(x => x.disposal != null && x.prescription != null && x.actRemoval != null)
                                                 .Where(x => x.disposal.KindCheckCode == TypeCheck.InspectionSurvey && x.disposal.InspectionTypeBase != TypeBase.Inspection)
                                                 .Select(x => new ActRemovalPrescription
                                                                    {
                                                                        actRemoval = x.actRemoval,
                                                                        DocGjiForm123Extended = listPrescription.Last(y => y.Id == x.prescription)
                                                                    })
                                                 .ToList();
            //УК
            reportData.DataColumn101 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn102 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn103 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, listCodeJurOther);
        //на должностных лиц
            //УК
            reportData.DataColumn104 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, new List<string> { "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn105 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, listCodeOfficTsjJSk);
            // иные
            reportData.DataColumn106 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, listCodeOfficOther);
            //физ. лицам
            reportData.DataColumn107 = GetDictDataDocumentGji(dictActRemovalMu, listEntitiesColumn101_107, listCodeIndivid);

            //К-во устранных нарушений всего (ед.)
            reportData.DataColumn108 = listActRemovalDateFact.GroupBy(x => x.MuId)
                                                             .ToDictionary(x => x.Key, v => (long)v.Count());
    //при плановых проверках
            //УК
            reportData.DataColumn109 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn88_93, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn110 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn88_93, listCodeJurTsjJSk);
            //иные
            reportData.DataColumn111 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn88_93, listCodeJurOther);

//при внеплановых проверках
    //по обращениям граждан (жалобам)
            //УК
            reportData.DataColumn112 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn94_100, new List<string> { "0", "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn113 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn94_100, listCodeJurTsjJSk.Concat(listCodeOfficTsjJSk).ToList());
            //иные
            reportData.DataColumn114 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn94_100, listCodeJurOther.Concat(listCodeOfficOther).ToList());
            //физ. лиц
            reportData.DataColumn115 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn94_100, listCodeIndivid);
    //по инспекционным обследованиям (по направлениям деятельности)
            //УК
            reportData.DataColumn116 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn101_107, listCodeJurOther);
            //ТСЖ / ЖСК
            reportData.DataColumn117 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn101_107, listCodeJurOther);
            //иные
            reportData.DataColumn118 = GetDictDataDocumentGji(listActRemovalDateFact, listEntitiesColumn101_107, listCodeJurOther);
        }

        //119-139
        private void GetDataProtocols19_5()
        {
            var serviceProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var serviceProtocolArticleLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            var queryProtocolsMuId = serviceProtocolViolation.GetAll()
                                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                                         .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                                         .WhereIf(dateEnd != DateTime.MaxValue, x => x.Document.DocumentDate <= dateEnd);

            var queryProtocolsId = queryProtocolsMuId.Select(x => x.Document.Id);

            var listProtocolsMuId = queryProtocolsMuId.Select(x => new
                                                                       {
                                                                           docId = x.Document.Id, 
                                                                           muId = x.InspectionViolation.RealityObject.Municipality.Id
                                                                       })
                                                      .ToList();

            var dictProtocolsMuId = listProtocolsMuId.GroupBy(x => x.docId).ToDictionary(x => x.Key, v => v.Select(y => y.muId).Last());

            var queryProtocolsIdArticle19_5 = serviceProtocolArticleLaw.GetAll()
                                         .Where(x => x.ArticleLaw.Code == "1" || x.ArticleLaw.Code == "7")
                                         .Where(x => queryProtocolsId.Contains(x.Protocol.Id));

            var queryProtocolParentStageUd = queryProtocolsIdArticle19_5.Select(x => x.Protocol.Stage.Parent.Id);

            var listProtocolsIdArticle19_5 = queryProtocolsIdArticle19_5
                                                .Select(x => new ProtocolProxy
                                                        {
                                                            Id = x.Protocol.Id,
                                                            ExecutantCode = x.Protocol.Executant.Code,
                                                            parentStage = x.Protocol.Stage.Parent.Id.ToLong(),
                                                            InspectionTypeBase = x.Protocol.Inspection.TypeBase
                                                        })
                                                .ToList();

            reportData.DataColumn119 = listProtocolsIdArticle19_5.Select(x => new { docId = x.Id, muId = dictProtocolsMuId[x.Id] })
                                                                 .GroupBy(x => x.muId)
                                                                 .ToDictionary(x => x.Key, v => (long)v.Select(y => y.docId).Distinct().Count());

            var dictDisposal = serviceDisposal.GetAll()
                               .Where(x => queryProtocolParentStageUd.Contains(x.Stage.Id))
                               .Select(x => new DisposalProxy
                                            {
                                                Id = x.Id,
                                                InspectionTypeBase = x.Inspection.TypeBase,
                                                stageId = x.Stage.Id,
                                                KindCheckCode = x.KindCheck.Code
                                            })
                                .ToDictionary(x => x.stageId, x => x);

            var listProtocolDisposal = listProtocolsIdArticle19_5.Select(x => new DocGjiReportDisposal { Disposal = dictDisposal[x.parentStage], DocGjiForm123Extended = x }).ToList();

//при плановых проверках
            
            var listEntitiesColumn120_125 = listProtocolDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                                                .Where(x => (x.Disposal.KindCheckCode == TypeCheck.PlannedExit || x.Disposal.KindCheckCode == TypeCheck.PlannedDocumentation)
                                                                            || (x.Disposal.InspectionTypeBase == TypeBase.Inspection));
    //юридическим лицам
            //УК
            reportData.DataColumn120 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn120_125, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn121 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn120_125, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn122 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn120_125, listCodeJurOther);
    //должностным лицам
            //УК
            reportData.DataColumn123 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn120_125, new List<string> { "1" });
            //ТСЖ / ЖСК
            reportData.DataColumn124 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn120_125, listCodeOfficTsjJSk);
            //иные
            reportData.DataColumn125 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn120_125, listCodeOfficOther);

//по внеплановым проверкам
    //по обращениям граждан (жалобам)
            var listEntitiesColumn126_132 = listProtocolDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                                                .Where(x => (x.Disposal.KindCheckCode == TypeCheck.NotPlannedExit || x.Disposal.KindCheckCode == TypeCheck.NotPlannedDocumentation)
                                                                                && x.Disposal.InspectionTypeBase == TypeBase.CitizenStatement);
    
        //юридическим лицам
            //УК
            reportData.DataColumn126 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn127 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn128 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, listCodeJurOther);
        //на должностных лиц
            //УК
            reportData.DataColumn129 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, new List<string> { "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn130 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, listCodeOfficTsjJSk);
            // иные
            reportData.DataColumn131 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, listCodeOfficOther);
        //физ. лицам
            reportData.DataColumn132 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn126_132, listCodeIndivid);

    //при инспекционных обследованиях (по направлениям деятельности )
            var listEntitiesColumn133_139 = listProtocolDisposal.Where(x => x.Disposal != null && x.DocGjiForm123Extended != null)
                                              .Where(x => x.Disposal.KindCheckCode == TypeCheck.InspectionSurvey && x.Disposal.InspectionTypeBase != TypeBase.Inspection);
        //юридическим лицам
            //УК
            reportData.DataColumn133 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, new List<string> { "0" });
            //ТСЖ / ЖСК
            reportData.DataColumn134 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, listCodeJurTsjJSk);
            // иные
            reportData.DataColumn135 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, listCodeJurOther);
        //на должностных лиц
            //УК
            reportData.DataColumn136 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, new List<string> { "1" });
            //ТСЖ/ЖСК
            reportData.DataColumn137 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, listCodeOfficTsjJSk);
            // иные
            reportData.DataColumn138 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, listCodeOfficOther);
        //физ. лицам
            reportData.DataColumn139 = GetDictDataDocumentGji(dictProtocolsMuId, listEntitiesColumn133_139, listCodeIndivid);
        }

        //140-180
        private void GetDataResolution()
        {
            // Типы исполнителей
            var listCodeUk = new[] { "0", "1" };
            var listCodeTsj = new[] { "10", "9" };
            var listCodeJsk = new[] { "11", "12" };
            var listCodeCompHousingStock = new[] { "2", "3", "18", "19" };
            var listCodeOwners = new[] { "17", "7", "14", "4", "5", "6" };
            var listCodeOther = new[] { "8", "13", "15", "16" };
            
            var listCodeJur = new[] { "0", "9", "11", "8", "15", "18", "4" };
            var listCodeOffic = new[] { "1", "10", "12", "13", "16", "19", "5" };

            var serviceInspectionGjiViolStage = Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var serviceResolPros = Container.Resolve<IDomainService<ResolPros>>();
            var serviceResolution = Container.Resolve<IDomainService<Resolution>>();
            var serviceResolutionDispute = Container.Resolve<IDomainService<ResolutionDispute>>();
            var serviceResolutionPayFine = Container.Resolve<IDomainService<ResolutionPayFine>>();
            //var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            //serviceDocumentGjiChildren

            // Для того чтобы получить постановления по Мо необходимо получить родительские нарушения и через дом получить МО
            var queryParentDocument = serviceInspectionGjiViolStage.GetAll()
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                     .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Prescription || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol);

            var listParentDocumentIdMuId = queryParentDocument.Select(x => new { muId = x.InspectionViolation.RealityObject.Municipality.Id, docId = x.Document.Id });

            var queryParentDocumentId = queryParentDocument.Select(x => x.Document.Id).Distinct();

            var dictParentDocMuId = listParentDocumentIdMuId.AsEnumerable().GroupBy(x => x.docId).ToDictionary(x => x.Key, x => x.Select(y => y.muId).First());
            //var listParentDocumentId = queryParentDocumentId.ToList();

            var queryResolProsMu = serviceResolPros.GetAll()
                                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                                         .Where(x => x.Municipality != null);
            var listResolProsId = queryResolProsMu.Select(x => x.Id).Distinct();

            var queryResolProsIdMuId = queryResolProsMu.Select(x => new { x.Id, muId = x.Municipality.Id }).ToList();            
            var dictResolProsMu = queryResolProsIdMuId.AsEnumerable().GroupBy(x => x.Id).ToDictionary(x => x.Key, v => v.Select(x => x.muId).Last());

            var start = 0;
            var parentChildrenIds = serviceDocumentGjiChildren.GetAll()
                             .Where(x => queryParentDocumentId.Contains(x.Parent.Id))
                             .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                             .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                             .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                             .Select(x => new ParentChildProxy(x.Parent.Id, x.Children.Id))
                             .AsEnumerable()
                             .Distinct()
                             .ToList();

            parentChildrenIds.AddRange(serviceDocumentGjiChildren.GetAll()
                             .Where(x => listResolProsId.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                             .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                             .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                             .Select(x => new ParentChildProxy(x.Parent.Id, x.Children.Id))
                             .AsEnumerable()
                             .Distinct());

            var dictChildMuId = parentChildrenIds.Select(x => new 
                                                    {
                                                     docId = x.child,
                                                     muId = dictParentDocMuId.ContainsKey(x.parent) ? 
                                                        dictParentDocMuId[x.parent]
                                                        : dictResolProsMu.ContainsKey(x.parent) ? dictResolProsMu[x.parent] : 0
                                                    })
                                 .GroupBy(x => x.docId)
                                 .ToDictionary(x => x.Key, v => v.Select(z => z.muId).Last());

            var listResolutionIds = parentChildrenIds.Select(x => x.child).Distinct().ToList();

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
                    listResolutionDispute = serviceResolutionDispute.GetAll()
                                 .Where(x => x.CourtVerdict.Code == "3")
                                 .Select(x => x.Resolution.Id)
                                 .ToList();
                }

            var resolutions = listResolution.Where(x => !listResolutionDispute.Contains(x.Id)).Where(x => dictChildMuId.ContainsKey(x.Id)).ToArray();

            var dictResolution = resolutions.GroupBy(x => x.Id)
                                            .ToDictionary(x => x.Key, v => v.Select(y => y.ExecutantCode).ToList());

            var listResolutionId = dictResolution.Keys.ToList();
            start = 0;
            var listResolutionpayFineProxy = new List<ResolutionpayFineProxy>();
            while (start < listResolutionId.Count)
            {
                var tmpId = listResolutionId.Skip(start).Take(1000).ToArray();
                listResolutionpayFineProxy.AddRange(
                             serviceResolutionPayFine.GetAll()
                             .Where(x => tmpId.Contains(x.Resolution.Id))
                             .Select(x => new ResolutionpayFineProxy { Id = x.Resolution.Id, Amount = x.Amount })
                             .AsEnumerable());

                start += 1000;
            }

            // Вынесено постановлений
            reportData.DataColumn140 = resolutions.Select(x => new { muId = dictChildMuId[x.Id], docId = x.Id })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, x => (long)x.Select(y => y.docId).Distinct().Count());

            //Устное замечание (ед.)
            var queryResolutionsSanction3 = resolutions.Where(x => x.SanctionCode == "3");

            reportData.DataColumn141 = GetDataResolution(queryResolutionsSanction3, dictChildMuId);

            //ГЖИ
            reportData.DataColumn142 = GetDataResolution(queryResolutionsSanction3.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
            //Суд
            reportData.DataColumn143 = GetDataResolution(queryResolutionsSanction3.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court), dictChildMuId);

            //на юрид. лиц
            reportData.DataColumn144 = GetDataResolution(queryResolutionsSanction3, dictChildMuId, listCodeJur, dictResolution);
            //на должн. лиц
            reportData.DataColumn145 = GetDataResolution(queryResolutionsSanction3, dictChildMuId, listCodeOffic, dictResolution);
            //на физ. лиц
            reportData.DataColumn146 = GetDataResolution(queryResolutionsSanction3, dictChildMuId, listCodeIndivid, dictResolution);

            //Предупреждение (ед.)
            var queryResolutionsSanction4 = resolutions.Where(x => x.SanctionCode == "4");

            reportData.DataColumn147 = GetDataResolution(queryResolutionsSanction4, dictChildMuId);

            //ГЖИ
            reportData.DataColumn148 = GetDataResolution(queryResolutionsSanction4.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
            //Суд
            reportData.DataColumn149 = GetDataResolution(queryResolutionsSanction4.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court), dictChildMuId);

            //на юрид. лиц
            reportData.DataColumn150 = GetDataResolution(queryResolutionsSanction4, dictChildMuId, listCodeJur, dictResolution);
            //на должн. лиц
            reportData.DataColumn151 = GetDataResolution(queryResolutionsSanction4, dictChildMuId, listCodeOffic, dictResolution);
            //на физ. лиц
            reportData.DataColumn152 = GetDataResolution(queryResolutionsSanction4, dictChildMuId, listCodeIndivid, dictResolution);


            //Административный штраф (ед.)
            var queryResolutionsSanction1 = resolutions.Where(x => x.SanctionCode == "1");

            reportData.DataColumn153 = GetDataResolution(queryResolutionsSanction1, dictChildMuId);

            //ГЖИ
            reportData.DataColumn154 = GetDataResolution(queryResolutionsSanction1.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
            //Суд
            reportData.DataColumn155 = GetDataResolution(queryResolutionsSanction1.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court), dictChildMuId);

            //на юрид. лиц
            reportData.DataColumn156 = GetDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeJur, dictResolution);
            //на должн. лиц
            reportData.DataColumn157 = GetDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeOffic, dictResolution);
            //на физ. лиц
            reportData.DataColumn158 = GetDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeIndivid, dictResolution);


            //Постановление о прекращении произв. (ед.)
            var queryResolutionsSanction2 = resolutions.Where(x => x.SanctionCode == "2");

            reportData.DataColumn159 = queryResolutionsSanction2.Select(x => new { muId = dictChildMuId[x.Id], docId = x.Id })
                                                 .GroupBy(x => x.muId)
                                                 .ToDictionary(x => x.Key, x => (long)x.Select(y => y.docId).Distinct().Count());

            //ГЖИ
            reportData.DataColumn160 = GetDataResolution(queryResolutionsSanction2.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
            //Суд
            reportData.DataColumn161 = GetDataResolution(queryResolutionsSanction2.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court), dictChildMuId);

            //на юрид. лиц
            reportData.DataColumn162 = GetDataResolution(queryResolutionsSanction2, dictChildMuId, listCodeJur, dictResolution);
            //на должн. лиц
            reportData.DataColumn163 = GetDataResolution(queryResolutionsSanction2, dictChildMuId, listCodeOffic, dictResolution);
            //на физ. лиц
            reportData.DataColumn164 = GetDataResolution(queryResolutionsSanction2, dictChildMuId, listCodeIndivid, dictResolution);


            //Предъявлено штрафов (руб.)
            reportData.DataColumn165 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId);

            //ГЖИ
            reportData.DataColumn166 = GetSumDataResolution(queryResolutionsSanction1.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
            //Суд
            reportData.DataColumn167 = GetSumDataResolution(queryResolutionsSanction1.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court), dictChildMuId);

            //на юрид. лиц
            reportData.DataColumn168 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeJur, dictResolution);
            //на должн. лиц
            reportData.DataColumn169 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeOffic, dictResolution);
            //на физ. лиц
            reportData.DataColumn170 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeIndivid, dictResolution);

            //УК
            reportData.DataColumn171 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeUk, dictResolution);
            //ТСЖ
            reportData.DataColumn172 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeTsj, dictResolution);
            //ЖСК
            reportData.DataColumn173 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeJsk, dictResolution);
            //непоср. управл.
            reportData.DataColumn174 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, new[] { "17" }, dictResolution);
            //предпр, обслуж ЖФ
            reportData.DataColumn175 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeCompHousingStock, dictResolution);
            //собств., нанима телям (арендаторам)
            reportData.DataColumn176 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeOwners, dictResolution);
            //иным
            reportData.DataColumn177 = GetSumDataResolution(queryResolutionsSanction1, dictChildMuId, listCodeOther, dictResolution);

        //Получено штрафов (руб.)
            //ГЖИ
            reportData.DataColumn179 = GetSumDataResolution(resolutions.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
            //Суд
            reportData.DataColumn180 = GetSumDataResolution(resolutions.Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection), dictChildMuId);
        }        

        /// <summary>
        /// Итого
        /// </summary>
        private void Total(ReportExtendedData total)
        {
            foreach (var field in reportData.GetType().GetFields())
            {
                var sumField = total.GetType().GetField(field.Name);
                var sumFieldData = sumField.GetValue(total);

                var value = field.GetValue(reportData);

                if (sumField.FieldType == typeof(Dictionary<int, int>))
                {
                    var fieldResult = (Dictionary<int, int>)sumFieldData;
                    this.SumInt(ref fieldResult, (Dictionary<int, int>)value);
                    sumField.SetValue(total, fieldResult);
                }
                else if (sumField.FieldType == typeof(Dictionary<int, decimal>))
                {
                    var fieldResult = (Dictionary<int, decimal>)sumFieldData;
                    this.SumDecimal(ref fieldResult, (Dictionary<int, decimal>)value);
                    sumField.SetValue(total, fieldResult);
                }
            }
        }

        private void SumInt(ref Dictionary<int, int> result, Dictionary<int, int> data)
        {
            int totalId = 0;
            if (result == null)
            {
                result = new Dictionary<int, int>();
            }

            if (data == null)
            {
                return;
            }

            if (result.ContainsKey(totalId))
            {
                result[totalId] += data.Sum(x => x.Value);
            }
            else
            {
                result.Add(totalId, data.Sum(x => x.Value));
            }
        }

        private void SumDecimal(ref Dictionary<int, decimal> result, Dictionary<int, decimal> data)
        {
            var totalId = 0;
            if (result == null)
            {
                result = new Dictionary<int, decimal>();
            }

            if (data == null)
            {
                return;
            }

            if (result.ContainsKey(totalId))
            {
                result[totalId] += data.Sum(x => x.Value);
            }
            else
            {
                result.Add(totalId, data.Sum(x => x.Value));
            }
        }

        private CountAreaHousesActCheckReport GetCountActCheckRo(IEnumerable<ActCheckProxy> listAct)
        {
            var arrId = listAct.Select(x => x.Id).Distinct().ToArray();

            var actProxies = actCheckRoDict.Where(x => arrId.Contains(x.Key)).Select(x => x.Value).ToList();

            var result = new CountAreaHousesActCheckReport
                             {
                                 Area = actProxies.Sum(x => x.Area),
                                 Count = actProxies.SelectMany(x => x.roIds).Distinct().Count()
                             };

            return result;
        }

        private Dictionary<long, ActCheckRobjectData> InitActCheckRoDict(List<ActCheckProxy> listActProxy)
        {
            var serviceActCheckRealityObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();

            var arrId = listActProxy.Select(x => x.Id).Distinct().ToArray();
            var actAreas = listActProxy.Distinct().GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.Sum(z => z.Area));

            var firstId = arrId.Take(1000).ToArray();
            var listActRo = serviceActCheckRealityObject.GetAll()
                                                        .Where(x => firstId.Contains(x.ActCheck.Id))
                                                        .Select(x => new { actId = x.ActCheck.Id, roId = x.RealityObject.Id })
                                                        .ToList();

            var start = 1000;
            while (start < arrId.Length)
            {
                var tmpId = arrId.Skip(start).Take(1000).ToArray();
                listActRo.AddRange(serviceActCheckRealityObject.GetAll()
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

        private Dictionary<long, long> GetDictDataDocumentGji(Dictionary<long, long> dictDocGjiMu, IEnumerable<DocGjiReportDisposal> listEntitiesColumn, List<string> listCodes)
        {
            var result = new Dictionary<long, long>();
            if (listCodes.Count == 1)
            {
                result = dictDocGjiMu.Select(x => new
                                                        {
                                                            muId = x.Value,
                                                            count = (long)listEntitiesColumn.Where(y => y.DocGjiForm123Extended.Id == x.Key)
                                                                                      .Count(y => y.DocGjiForm123Extended.ExecutantCode == listCodes.First())
                                                        })
                                        .GroupBy(x => x.muId)
                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));
            }
            else
            {
                result = dictDocGjiMu.Select(x => new
                                                            {
                                                                muId = x.Value,
                                                                count = (long)listEntitiesColumn.Where(y => y.DocGjiForm123Extended.Id == x.Key)
                                                                                          .Count(y => listCodes.Contains(y.DocGjiForm123Extended.ExecutantCode))
                                                            })
                                        .GroupBy(x => x.muId)
                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));
            }
            return result;
        }

        private Dictionary<long, long> GetDictDataDocumentGji(Dictionary<long, long> dictDocGjiMu, IEnumerable<ActRemovalPrescription> listEntitiesColumn, List<string> listCodes)
        {
            var result = new Dictionary<long, long>();
            if (listCodes.Count == 1)
            {
                result = dictDocGjiMu.Select(x => new
                                                            {
                                                                muId = x.Value,
                                                                count = (long)listEntitiesColumn.Where(y => y.DocGjiForm123Extended.Id == x.Key)
                                                                                          .Count(y => y.DocGjiForm123Extended.ExecutantCode == listCodes.First())
                                                            })
                                        .GroupBy(x => x.muId)
                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));
            }
            else
            {
                result = dictDocGjiMu.Select(x => new
                                                            {
                                                                muId = x.Value,
                                                                count = (long)listEntitiesColumn.Where(y => y.DocGjiForm123Extended.Id == x.Key)
                                                                                          .Count(y => listCodes.Contains(y.DocGjiForm123Extended.ExecutantCode))
                                                            })
                                        .GroupBy(x => x.muId)
                                        .ToDictionary(x => x.Key, x => x.Sum(y => y.count));
            }
            return result;
        }

        private Dictionary<long, long> GetDictDataDocumentGji(List<ActRemovalViolationProxy> listActRemovalDateFact, List<ActRemovalPrescription> listData, List<string> codes)
        {
            IEnumerable<long> tmpList;

            if (codes.Count == 1)
            {
                tmpList = listData.Where(x => x.DocGjiForm123Extended.ExecutantCode == codes.First()).Select(x => x.actRemoval.Id);
            }
            else
            {
                tmpList = listData.Where(x => codes.Contains(x.DocGjiForm123Extended.ExecutantCode)).Select(x => x.actRemoval.Id);
            }

            return listActRemovalDateFact.Where(x => tmpList.Contains(x.Id))
                                      .Select(x => new { x.MuId, x.InspectionViolationId })
                                      .GroupBy(x => x.MuId)
                                      .ToDictionary(x => x.Key, v => (long)v.Count());
        }

        private Dictionary<long, long> GetDataResolution(IEnumerable<ResolutionProxy> query, Dictionary<long, long> dictChildMuId)
        {
            return query.Select(x => new { muId = dictChildMuId[x.Id], docId = x.Id })
                        .GroupBy(x => x.muId)
                        .ToDictionary(x => x.Key, x => (long)x.Select(y => y.docId).Distinct().Count());
        }

        private Dictionary<long, long> GetDataResolution(IEnumerable<ResolutionProxy> query, Dictionary<long, long> dictChildMuId, IEnumerable<string> typePerformer, Dictionary<long, List<string>> dictResolution)
        {
            return query.Select(x => new { muId = dictChildMuId[x.Id], count = dictResolution[x.Id].Count(typePerformer.Contains) })
                        .GroupBy(x => x.muId)
                        .ToDictionary(x => x.Key, x => (long)x.Sum(y => y.count));
        }

        private Dictionary<long, decimal> GetSumDataResolution(IEnumerable<ResolutionProxy> query, Dictionary<long, long> dictChildMuId)
        {
            return query.Select(x => new { muId = dictChildMuId[x.Id], sum = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M })
                        .GroupBy(x => x.muId)
                        .ToDictionary(x => x.Key, x => x.Sum(y => y.sum));
        }

        private Dictionary<long, decimal> GetSumDataResolution(IEnumerable<ResolutionProxy> query, Dictionary<long, long> dictChildMuId, IEnumerable<string> typePerformer, Dictionary<long, List<string>> dictResolution)
        {
            return query.Select(x => new 
                                        {
                                            muId = dictChildMuId[x.Id], 
                                            count = dictResolution[x.Id].Count(typePerformer.Contains),
                                            sum = x.PenaltyAmount.HasValue ? (decimal)x.PenaltyAmount : 0M 
                                        })
                        .Where(x => x.count > 0)
                        .GroupBy(x => x.muId)
                        .ToDictionary(x => x.Key, x => x.Sum(y => y.sum));
        }

        /// <summary>
        /// Запись данных отчета
        /// </summary>
        /// <param name="section"></param>
        /// <param name="municipalityId"></param>
        private void FillRow(Section section, long municipalityId)
        {
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimalArea = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] / 1000 : 0;
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimal = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;
            Func<Dictionary<long, long>, long, long> getStr = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;

            section["Column4"] = getStr(reportData.DataColumn4, municipalityId);
            section["Column5"] = getStrDecimalArea(reportData.DataColumn5, municipalityId);
            section["Column6"] = getStr(reportData.DataColumn6, municipalityId);
            section["Column7"] = getStrDecimalArea(reportData.DataColumn7, municipalityId);
            section["Column8"] = getStr(reportData.DataColumn8, municipalityId);
            section["Column9"] = getStrDecimalArea(reportData.DataColumn9, municipalityId);
            //section["Column10"] = getStr(reportData.DataColumn10, municipalityId);
            //section["Column11"] = getStrDecimalArea(reportData.DataColumn11, municipalityId);
            section["Column12"] = getStr(reportData.DataColumn12, municipalityId);
            section["Column13"] = getStrDecimalArea(reportData.DataColumn13, municipalityId);
            section["Column14"] = getStr(reportData.DataColumn14, municipalityId);
            section["Column15"] = getStrDecimalArea(reportData.DataColumn15, municipalityId);
            section["Column16"] = getStr(reportData.DataColumn16, municipalityId);
            section["Column17"] = getStrDecimalArea(reportData.DataColumn17, municipalityId);
            section["Column18"] = getStr(reportData.DataColumn18, municipalityId);
            section["Column19"] = getStrDecimalArea(reportData.DataColumn19, municipalityId);
            section["Column20"] = getStr(reportData.DataColumn20, municipalityId);
            section["Column21"] = getStrDecimalArea(reportData.DataColumn21, municipalityId);
            section["Column22"] = getStr(reportData.DataColumn22, municipalityId);
            section["Column23"] = getStrDecimalArea(reportData.DataColumn23, municipalityId);
            section["Column24"] = getStr(reportData.DataColumn24, municipalityId);
            section["Column25"] = getStrDecimalArea(reportData.DataColumn25, municipalityId);
            section["Column26"] = getStr(reportData.DataColumn26, municipalityId);
            section["Column27"] = getStrDecimalArea(reportData.DataColumn27, municipalityId);
            //section["Column28"] = getStr(reportData.DataColumn28, municipalityId);
            //section["Column29"] = getStrDecimalArea(reportData.DataColumn29, municipalityId);
            section["Column30"] = getStr(reportData.DataColumn30, municipalityId);
            section["Column31"] = getStrDecimalArea(reportData.DataColumn31, municipalityId);
            section["Column33"] = getStr(reportData.DataColumn33, municipalityId);
            section["Column34"] = getStr(reportData.DataColumn34, municipalityId);
            section["Column35"] = getStr(reportData.DataColumn35, municipalityId);
            section["Column36"] = getStr(reportData.DataColumn36, municipalityId);
            section["Column37"] = getStr(reportData.DataColumn37, municipalityId);
            section["Column38"] = getStr(reportData.DataColumn38, municipalityId);
            section["Column39"] = getStr(reportData.DataColumn39, municipalityId);
            section["Column40"] = getStr(reportData.DataColumn40, municipalityId);
            section["Column41"] = getStr(reportData.DataColumn41, municipalityId);
            section["Column42"] = getStr(reportData.DataColumn42, municipalityId);
            section["Column43"] = getStr(reportData.DataColumn43, municipalityId);
            section["Column44"] = getStr(reportData.DataColumn44, municipalityId);
            section["Column45"] = getStr(reportData.DataColumn45, municipalityId);
            section["Column46"] = getStr(reportData.DataColumn46, municipalityId);
            section["Column47"] = getStr(reportData.DataColumn47, municipalityId);
            section["Column48"] = getStr(reportData.DataColumn48, municipalityId);
            section["Column49"] = getStr(reportData.DataColumn49, municipalityId);
            section["Column50"] = getStr(reportData.DataColumn50, municipalityId);
            section["Column51"] = getStr(reportData.DataColumn51, municipalityId);
            section["Column52"] = getStr(reportData.DataColumn52, municipalityId);
            section["Column53"] = getStr(reportData.DataColumn53, municipalityId);
            section["Column54"] = getStr(reportData.DataColumn54, municipalityId);
            section["Column55"] = getStr(reportData.DataColumn55, municipalityId);
            section["Column56"] = getStr(reportData.DataColumn56, municipalityId);
            section["Column57"] = getStr(reportData.DataColumn57, municipalityId);
            section["Column58"] = getStr(reportData.DataColumn58, municipalityId);
            section["Column59"] = getStr(reportData.DataColumn59, municipalityId);
            section["Column60"] = getStr(reportData.DataColumn60, municipalityId);
            section["Column61"] = getStr(reportData.DataColumn61, municipalityId);
            section["Column62"] = getStr(reportData.DataColumn62, municipalityId);
            section["Column63"] = getStr(reportData.DataColumn63, municipalityId);
            section["Column64"] = getStr(reportData.DataColumn64, municipalityId);
            section["Column65"] = getStr(reportData.DataColumn65, municipalityId);
            section["Column66"] = getStr(reportData.DataColumn66, municipalityId);
            section["Column67"] = getStr(reportData.DataColumn67, municipalityId);
            section["Column68"] = getStr(reportData.DataColumn68, municipalityId);
            section["Column69"] = getStr(reportData.DataColumn69, municipalityId);
            section["Column70"] = getStr(reportData.DataColumn70, municipalityId);
            section["Column71"] = getStr(reportData.DataColumn71, municipalityId);
            section["Column72"] = getStr(reportData.DataColumn72, municipalityId);
            section["Column73"] = getStr(reportData.DataColumn73, municipalityId);
            section["Column74"] = getStr(reportData.DataColumn74, municipalityId);
            section["Column75"] = getStr(reportData.DataColumn75, municipalityId);
            section["Column76"] = getStr(reportData.DataColumn76, municipalityId);
            section["Column77"] = getStr(reportData.DataColumn77, municipalityId);
            section["Column78"] = getStr(reportData.DataColumn78, municipalityId);
            section["Column79"] = getStr(reportData.DataColumn79, municipalityId);
            section["Column80"] = getStr(reportData.DataColumn80, municipalityId);
            section["Column81"] = getStr(reportData.DataColumn81, municipalityId);
            section["Column82"] = getStr(reportData.DataColumn82, municipalityId);
            section["Column83"] = getStr(reportData.DataColumn83, municipalityId);
            section["Column84"] = getStr(reportData.DataColumn84, municipalityId);
            section["Column85"] = getStr(reportData.DataColumn85, municipalityId);
            section["Column86"] = getStrDecimalArea(reportData.DataColumn86, municipalityId);
            section["Column87"] = getStr(reportData.DataColumn87, municipalityId);
            section["Column88"] = getStr(reportData.DataColumn88, municipalityId);
            section["Column89"] = getStr(reportData.DataColumn89, municipalityId);
            section["Column90"] = getStr(reportData.DataColumn90, municipalityId);
            section["Column91"] = getStr(reportData.DataColumn91, municipalityId);
            section["Column92"] = getStr(reportData.DataColumn92, municipalityId);
            section["Column93"] = getStr(reportData.DataColumn93, municipalityId);
            section["Column94"] = getStr(reportData.DataColumn94, municipalityId);
            section["Column95"] = getStr(reportData.DataColumn95, municipalityId);
            section["Column96"] = getStr(reportData.DataColumn96, municipalityId);
            section["Column97"] = getStr(reportData.DataColumn97, municipalityId);
            section["Column98"] = getStr(reportData.DataColumn98, municipalityId);
            section["Column99"] = getStr(reportData.DataColumn99, municipalityId);
            section["Column100"] = getStr(reportData.DataColumn100, municipalityId);
            section["Column101"] = getStr(reportData.DataColumn101, municipalityId);
            section["Column102"] = getStr(reportData.DataColumn102, municipalityId);
            section["Column103"] = getStr(reportData.DataColumn103, municipalityId);
            section["Column104"] = getStr(reportData.DataColumn104, municipalityId);
            section["Column105"] = getStr(reportData.DataColumn105, municipalityId);
            section["Column106"] = getStr(reportData.DataColumn106, municipalityId);
            section["Column107"] = getStr(reportData.DataColumn107, municipalityId);
            section["Column108"] = getStr(reportData.DataColumn108, municipalityId);
            section["Column109"] = getStr(reportData.DataColumn109, municipalityId);
            section["Column110"] = getStr(reportData.DataColumn110, municipalityId);
            section["Column111"] = getStr(reportData.DataColumn111, municipalityId);
            section["Column112"] = getStr(reportData.DataColumn112, municipalityId);
            section["Column113"] = getStr(reportData.DataColumn113, municipalityId);
            section["Column114"] = getStr(reportData.DataColumn114, municipalityId);
            section["Column115"] = getStr(reportData.DataColumn115, municipalityId);
            section["Column116"] = getStr(reportData.DataColumn116, municipalityId);
            section["Column117"] = getStr(reportData.DataColumn117, municipalityId);
            section["Column118"] = getStr(reportData.DataColumn118, municipalityId);
            section["Column119"] = getStr(reportData.DataColumn119, municipalityId);
            section["Column120"] = getStr(reportData.DataColumn120, municipalityId);
            section["Column121"] = getStr(reportData.DataColumn121, municipalityId);
            section["Column122"] = getStr(reportData.DataColumn122, municipalityId);
            section["Column123"] = getStr(reportData.DataColumn123, municipalityId);
            section["Column124"] = getStr(reportData.DataColumn124, municipalityId);
            section["Column125"] = getStr(reportData.DataColumn125, municipalityId);
            section["Column126"] = getStr(reportData.DataColumn126, municipalityId);
            section["Column127"] = getStr(reportData.DataColumn127, municipalityId);
            section["Column128"] = getStr(reportData.DataColumn128, municipalityId);
            section["Column129"] = getStr(reportData.DataColumn129, municipalityId);
            section["Column130"] = getStr(reportData.DataColumn130, municipalityId);
            section["Column131"] = getStr(reportData.DataColumn131, municipalityId);
            section["Column132"] = getStr(reportData.DataColumn132, municipalityId);
            section["Column133"] = getStr(reportData.DataColumn133, municipalityId);
            section["Column134"] = getStr(reportData.DataColumn134, municipalityId);
            section["Column135"] = getStr(reportData.DataColumn135, municipalityId);
            section["Column136"] = getStr(reportData.DataColumn136, municipalityId);
            section["Column137"] = getStr(reportData.DataColumn137, municipalityId);
            section["Column138"] = getStr(reportData.DataColumn138, municipalityId);
            section["Column139"] = getStr(reportData.DataColumn139, municipalityId);
            section["Column140"] = getStr(reportData.DataColumn140, municipalityId);
            section["Column141"] = getStr(reportData.DataColumn141, municipalityId);
            section["Column142"] = getStr(reportData.DataColumn142, municipalityId);
            section["Column143"] = getStr(reportData.DataColumn143, municipalityId);
            section["Column144"] = getStr(reportData.DataColumn144, municipalityId);
            section["Column145"] = getStr(reportData.DataColumn145, municipalityId);
            section["Column146"] = getStr(reportData.DataColumn146, municipalityId);
            section["Column147"] = getStr(reportData.DataColumn147, municipalityId);
            section["Column148"] = getStr(reportData.DataColumn148, municipalityId);
            section["Column149"] = getStr(reportData.DataColumn149, municipalityId);
            section["Column150"] = getStr(reportData.DataColumn150, municipalityId);
            section["Column151"] = getStr(reportData.DataColumn151, municipalityId);
            section["Column152"] = getStr(reportData.DataColumn152, municipalityId);
            section["Column153"] = getStr(reportData.DataColumn153, municipalityId);
            section["Column154"] = getStr(reportData.DataColumn154, municipalityId);
            section["Column155"] = getStr(reportData.DataColumn155, municipalityId);
            section["Column156"] = getStr(reportData.DataColumn156, municipalityId);
            section["Column157"] = getStr(reportData.DataColumn157, municipalityId);
            section["Column158"] = getStr(reportData.DataColumn158, municipalityId);
            section["Column159"] = getStr(reportData.DataColumn159, municipalityId);
            section["Column160"] = getStr(reportData.DataColumn160, municipalityId);
            section["Column161"] = getStr(reportData.DataColumn161, municipalityId);
            section["Column162"] = getStr(reportData.DataColumn162, municipalityId);
            section["Column163"] = getStr(reportData.DataColumn163, municipalityId);
            section["Column164"] = getStr(reportData.DataColumn164, municipalityId);
            section["Column165"] = getStrDecimal(reportData.DataColumn165, municipalityId);
            section["Column166"] = getStrDecimal(reportData.DataColumn166, municipalityId);
            section["Column167"] = getStrDecimal(reportData.DataColumn167, municipalityId);
            section["Column168"] = getStrDecimal(reportData.DataColumn168, municipalityId);
            section["Column169"] = getStrDecimal(reportData.DataColumn169, municipalityId);
            section["Column170"] = getStrDecimal(reportData.DataColumn170, municipalityId);
            section["Column171"] = getStrDecimal(reportData.DataColumn171, municipalityId);
            section["Column172"] = getStrDecimal(reportData.DataColumn172, municipalityId);
            section["Column173"] = getStrDecimal(reportData.DataColumn173, municipalityId);
            section["Column174"] = getStrDecimal(reportData.DataColumn174, municipalityId);
            section["Column175"] = getStrDecimal(reportData.DataColumn175, municipalityId);
            section["Column176"] = getStrDecimal(reportData.DataColumn176, municipalityId);
            section["Column177"] = getStrDecimal(reportData.DataColumn177, municipalityId);
            //section["Column178"] = getStrDecimal(reportData.DataColumn178, municipalityId);
            section["Column179"] = getStrDecimal(reportData.DataColumn179, municipalityId);
            section["Column180"] = getStrDecimal(reportData.DataColumn180, municipalityId);
        }
        
        /// <summary>
        /// Итоги
        /// </summary>
        /// <param name="section"></param>
        /// <param name="total"></param>
        private void FillTotalRow(ReportParams section, ReportExtendedData total)
        {
            var id = 0;

            Func<Dictionary<long, decimal>, long, decimal> getStrDecimalArea = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] / 1000 : 0;
            Func<Dictionary<long, decimal>, long, decimal> getStrDecimal = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;
            Func<Dictionary<long, long>, long, long> getStr = (dict, muId) => dict.ContainsKey(muId) ? dict[muId] : 0;

            section.SimpleReportParams["SumColumn4"] = getStr(total.DataColumn4, id);
            section.SimpleReportParams["SumColumn5"] = getStrDecimalArea(total.DataColumn5, id);
            section.SimpleReportParams["SumColumn6"] = getStr(total.DataColumn6, id);
            section.SimpleReportParams["SumColumn7"] = getStrDecimalArea(total.DataColumn7, id);
            section.SimpleReportParams["SumColumn8"] = getStr(total.DataColumn8, id);
            section.SimpleReportParams["SumColumn9"] = getStrDecimalArea(total.DataColumn9, id);
            //section.SimpleReportParams["SumColumn10"] = getStr(total.DataColumn10, id);
            //section.SimpleReportParams["SumColumn11"] = getStrDecimalArea(total.DataColumn11, id);
            section.SimpleReportParams["SumColumn12"] = getStr(total.DataColumn12, id);
            section.SimpleReportParams["SumColumn13"] = getStrDecimalArea(total.DataColumn13, id);
            section.SimpleReportParams["SumColumn14"] = getStr(total.DataColumn14, id);
            section.SimpleReportParams["SumColumn15"] = getStrDecimalArea(total.DataColumn15, id);
            section.SimpleReportParams["SumColumn16"] = getStr(total.DataColumn16, id);
            section.SimpleReportParams["SumColumn17"] = getStrDecimalArea(total.DataColumn17, id);
            section.SimpleReportParams["SumColumn18"] = getStr(total.DataColumn18, id);
            section.SimpleReportParams["SumColumn19"] = getStrDecimalArea(total.DataColumn19, id);
            section.SimpleReportParams["SumColumn20"] = getStr(total.DataColumn20, id);
            section.SimpleReportParams["SumColumn21"] = getStrDecimalArea(total.DataColumn21, id);
            section.SimpleReportParams["SumColumn22"] = getStr(total.DataColumn22, id);
            section.SimpleReportParams["SumColumn23"] = getStrDecimalArea(total.DataColumn23, id);
            section.SimpleReportParams["SumColumn24"] = getStr(total.DataColumn24, id);
            section.SimpleReportParams["SumColumn25"] = getStrDecimalArea(total.DataColumn25, id);
            section.SimpleReportParams["SumColumn26"] = getStr(total.DataColumn26, id);
            section.SimpleReportParams["SumColumn27"] = getStrDecimalArea(total.DataColumn27, id);
            //section.SimpleReportParams["SumColumn28"] = getStr(total.DataColumn28, id);
            //section.SimpleReportParams["SumColumn29"] = getStrDecimalArea(total.DataColumn29, id);
            section.SimpleReportParams["SumColumn30"] = getStr(total.DataColumn30, id);
            section.SimpleReportParams["SumColumn31"] = getStrDecimalArea(total.DataColumn31, id);
            section.SimpleReportParams["SumColumn33"] = getStr(total.DataColumn33, id);
            section.SimpleReportParams["SumColumn34"] = getStr(total.DataColumn34, id);
            section.SimpleReportParams["SumColumn35"] = getStr(total.DataColumn35, id);
            section.SimpleReportParams["SumColumn36"] = getStr(total.DataColumn36, id);
            section.SimpleReportParams["SumColumn37"] = getStr(total.DataColumn37, id);
            section.SimpleReportParams["SumColumn38"] = getStr(total.DataColumn38, id);
            section.SimpleReportParams["SumColumn39"] = getStr(total.DataColumn39, id);
            section.SimpleReportParams["SumColumn40"] = getStr(total.DataColumn40, id);
            section.SimpleReportParams["SumColumn41"] = getStr(total.DataColumn41, id);
            section.SimpleReportParams["SumColumn42"] = getStr(total.DataColumn42, id);
            section.SimpleReportParams["SumColumn43"] = getStr(total.DataColumn43, id);
            section.SimpleReportParams["SumColumn44"] = getStr(total.DataColumn44, id);
            section.SimpleReportParams["SumColumn45"] = getStr(total.DataColumn45, id);
            section.SimpleReportParams["SumColumn46"] = getStr(total.DataColumn46, id);
            section.SimpleReportParams["SumColumn47"] = getStr(total.DataColumn47, id);
            section.SimpleReportParams["SumColumn48"] = getStr(total.DataColumn48, id);
            section.SimpleReportParams["SumColumn49"] = getStr(total.DataColumn49, id);
            section.SimpleReportParams["SumColumn50"] = getStr(total.DataColumn50, id);
            section.SimpleReportParams["SumColumn51"] = getStr(total.DataColumn51, id);
            section.SimpleReportParams["SumColumn52"] = getStr(total.DataColumn52, id);
            section.SimpleReportParams["SumColumn53"] = getStr(total.DataColumn53, id);
            section.SimpleReportParams["SumColumn54"] = getStr(total.DataColumn54, id);
            section.SimpleReportParams["SumColumn55"] = getStr(total.DataColumn55, id);
            section.SimpleReportParams["SumColumn56"] = getStr(total.DataColumn56, id);
            section.SimpleReportParams["SumColumn57"] = getStr(total.DataColumn57, id);
            section.SimpleReportParams["SumColumn58"] = getStr(total.DataColumn58, id);
            section.SimpleReportParams["SumColumn59"] = getStr(total.DataColumn59, id);
            section.SimpleReportParams["SumColumn60"] = getStr(total.DataColumn60, id);
            section.SimpleReportParams["SumColumn61"] = getStr(total.DataColumn61, id);
            section.SimpleReportParams["SumColumn62"] = getStr(total.DataColumn62, id);
            section.SimpleReportParams["SumColumn63"] = getStr(total.DataColumn63, id);
            section.SimpleReportParams["SumColumn64"] = getStr(total.DataColumn64, id);
            section.SimpleReportParams["SumColumn65"] = getStr(total.DataColumn65, id);
            section.SimpleReportParams["SumColumn66"] = getStr(total.DataColumn66, id);
            section.SimpleReportParams["SumColumn67"] = getStr(total.DataColumn67, id);
            section.SimpleReportParams["SumColumn68"] = getStr(total.DataColumn68, id);
            section.SimpleReportParams["SumColumn69"] = getStr(total.DataColumn69, id);
            section.SimpleReportParams["SumColumn70"] = getStr(total.DataColumn70, id);
            section.SimpleReportParams["SumColumn71"] = getStr(total.DataColumn71, id);
            section.SimpleReportParams["SumColumn72"] = getStr(total.DataColumn72, id);
            section.SimpleReportParams["SumColumn73"] = getStr(total.DataColumn73, id);
            section.SimpleReportParams["SumColumn74"] = getStr(total.DataColumn74, id);
            section.SimpleReportParams["SumColumn75"] = getStr(total.DataColumn75, id);
            section.SimpleReportParams["SumColumn76"] = getStr(total.DataColumn76, id);
            section.SimpleReportParams["SumColumn77"] = getStr(total.DataColumn77, id);
            section.SimpleReportParams["SumColumn78"] = getStr(total.DataColumn78, id);
            section.SimpleReportParams["SumColumn79"] = getStr(total.DataColumn79, id);
            section.SimpleReportParams["SumColumn80"] = getStr(total.DataColumn80, id);
            section.SimpleReportParams["SumColumn81"] = getStr(total.DataColumn81, id);
            section.SimpleReportParams["SumColumn82"] = getStr(total.DataColumn82, id);
            section.SimpleReportParams["SumColumn83"] = getStr(total.DataColumn83, id);
            section.SimpleReportParams["SumColumn84"] = getStr(total.DataColumn84, id);
            section.SimpleReportParams["SumColumn85"] = getStr(total.DataColumn85, id);
            section.SimpleReportParams["SumColumn86"] = getStrDecimalArea(total.DataColumn86, id);
            section.SimpleReportParams["SumColumn87"] = getStr(total.DataColumn87, id);
            section.SimpleReportParams["SumColumn88"] = getStr(total.DataColumn88, id);
            section.SimpleReportParams["SumColumn89"] = getStr(total.DataColumn89, id);
            section.SimpleReportParams["SumColumn90"] = getStr(total.DataColumn90, id);
            section.SimpleReportParams["SumColumn91"] = getStr(total.DataColumn91, id);
            section.SimpleReportParams["SumColumn92"] = getStr(total.DataColumn92, id);
            section.SimpleReportParams["SumColumn93"] = getStr(total.DataColumn93, id);
            section.SimpleReportParams["SumColumn94"] = getStr(total.DataColumn94, id);
            section.SimpleReportParams["SumColumn95"] = getStr(total.DataColumn95, id);
            section.SimpleReportParams["SumColumn96"] = getStr(total.DataColumn96, id);
            section.SimpleReportParams["SumColumn97"] = getStr(total.DataColumn97, id);
            section.SimpleReportParams["SumColumn98"] = getStr(total.DataColumn98, id);
            section.SimpleReportParams["SumColumn99"] = getStr(total.DataColumn99, id);
            section.SimpleReportParams["SumColumn100"] = getStr(total.DataColumn100, id);
            section.SimpleReportParams["SumColumn101"] = getStr(total.DataColumn101, id);
            section.SimpleReportParams["SumColumn102"] = getStr(total.DataColumn102, id);
            section.SimpleReportParams["SumColumn103"] = getStr(total.DataColumn103, id);
            section.SimpleReportParams["SumColumn104"] = getStr(total.DataColumn104, id);
            section.SimpleReportParams["SumColumn105"] = getStr(total.DataColumn105, id);
            section.SimpleReportParams["SumColumn106"] = getStr(total.DataColumn106, id);
            section.SimpleReportParams["SumColumn107"] = getStr(total.DataColumn107, id);
            section.SimpleReportParams["SumColumn108"] = getStr(total.DataColumn108, id);
            section.SimpleReportParams["SumColumn109"] = getStr(total.DataColumn109, id);
            section.SimpleReportParams["SumColumn110"] = getStr(total.DataColumn110, id);
            section.SimpleReportParams["SumColumn111"] = getStr(total.DataColumn111, id);
            section.SimpleReportParams["SumColumn112"] = getStr(total.DataColumn112, id);
            section.SimpleReportParams["SumColumn113"] = getStr(total.DataColumn113, id);
            section.SimpleReportParams["SumColumn114"] = getStr(total.DataColumn114, id);
            section.SimpleReportParams["SumColumn115"] = getStr(total.DataColumn115, id);
            section.SimpleReportParams["SumColumn116"] = getStr(total.DataColumn116, id);
            section.SimpleReportParams["SumColumn117"] = getStr(total.DataColumn117, id);
            section.SimpleReportParams["SumColumn118"] = getStr(total.DataColumn118, id);
            section.SimpleReportParams["SumColumn119"] = getStr(total.DataColumn119, id);
            section.SimpleReportParams["SumColumn120"] = getStr(total.DataColumn120, id);
            section.SimpleReportParams["SumColumn121"] = getStr(total.DataColumn121, id);
            section.SimpleReportParams["SumColumn122"] = getStr(total.DataColumn122, id);
            section.SimpleReportParams["SumColumn123"] = getStr(total.DataColumn123, id);
            section.SimpleReportParams["SumColumn124"] = getStr(total.DataColumn124, id);
            section.SimpleReportParams["SumColumn125"] = getStr(total.DataColumn125, id);
            section.SimpleReportParams["SumColumn126"] = getStr(total.DataColumn126, id);
            section.SimpleReportParams["SumColumn127"] = getStr(total.DataColumn127, id);
            section.SimpleReportParams["SumColumn128"] = getStr(total.DataColumn128, id);
            section.SimpleReportParams["SumColumn129"] = getStr(total.DataColumn129, id);
            section.SimpleReportParams["SumColumn130"] = getStr(total.DataColumn130, id);
            section.SimpleReportParams["SumColumn131"] = getStr(total.DataColumn131, id);
            section.SimpleReportParams["SumColumn132"] = getStr(total.DataColumn132, id);
            section.SimpleReportParams["SumColumn133"] = getStr(total.DataColumn133, id);
            section.SimpleReportParams["SumColumn134"] = getStr(total.DataColumn134, id);
            section.SimpleReportParams["SumColumn135"] = getStr(total.DataColumn135, id);
            section.SimpleReportParams["SumColumn136"] = getStr(total.DataColumn136, id);
            section.SimpleReportParams["SumColumn137"] = getStr(total.DataColumn137, id);
            section.SimpleReportParams["SumColumn138"] = getStr(total.DataColumn138, id);
            section.SimpleReportParams["SumColumn139"] = getStr(total.DataColumn139, id);
            section.SimpleReportParams["SumColumn140"] = getStr(total.DataColumn140, id);
            section.SimpleReportParams["SumColumn141"] = getStr(total.DataColumn141, id);
            section.SimpleReportParams["SumColumn142"] = getStr(total.DataColumn142, id);
            section.SimpleReportParams["SumColumn143"] = getStr(total.DataColumn143, id);
            section.SimpleReportParams["SumColumn144"] = getStr(total.DataColumn144, id);
            section.SimpleReportParams["SumColumn145"] = getStr(total.DataColumn145, id);
            section.SimpleReportParams["SumColumn146"] = getStr(total.DataColumn146, id);
            section.SimpleReportParams["SumColumn147"] = getStr(total.DataColumn147, id);
            section.SimpleReportParams["SumColumn148"] = getStr(total.DataColumn148, id);
            section.SimpleReportParams["SumColumn149"] = getStr(total.DataColumn149, id);
            section.SimpleReportParams["SumColumn150"] = getStr(total.DataColumn150, id);
            section.SimpleReportParams["SumColumn151"] = getStr(total.DataColumn151, id);
            section.SimpleReportParams["SumColumn152"] = getStr(total.DataColumn152, id);
            section.SimpleReportParams["SumColumn153"] = getStr(total.DataColumn153, id);
            section.SimpleReportParams["SumColumn154"] = getStr(total.DataColumn154, id);
            section.SimpleReportParams["SumColumn155"] = getStr(total.DataColumn155, id);
            section.SimpleReportParams["SumColumn156"] = getStr(total.DataColumn156, id);
            section.SimpleReportParams["SumColumn157"] = getStr(total.DataColumn157, id);
            section.SimpleReportParams["SumColumn158"] = getStr(total.DataColumn158, id);
            section.SimpleReportParams["SumColumn159"] = getStr(total.DataColumn159, id);
            section.SimpleReportParams["SumColumn160"] = getStr(total.DataColumn160, id);
            section.SimpleReportParams["SumColumn161"] = getStr(total.DataColumn161, id);
            section.SimpleReportParams["SumColumn162"] = getStr(total.DataColumn162, id);
            section.SimpleReportParams["SumColumn163"] = getStr(total.DataColumn163, id);
            section.SimpleReportParams["SumColumn164"] = getStr(total.DataColumn164, id);
            section.SimpleReportParams["SumColumn165"] = getStrDecimal(total.DataColumn165, id);
            section.SimpleReportParams["SumColumn166"] = getStrDecimal(total.DataColumn166, id);
            section.SimpleReportParams["SumColumn167"] = getStrDecimal(total.DataColumn167, id);
            section.SimpleReportParams["SumColumn168"] = getStrDecimal(total.DataColumn168, id);
            section.SimpleReportParams["SumColumn169"] = getStrDecimal(total.DataColumn169, id);
            section.SimpleReportParams["SumColumn170"] = getStrDecimal(total.DataColumn170, id);
            section.SimpleReportParams["SumColumn171"] = getStrDecimal(total.DataColumn171, id);
            section.SimpleReportParams["SumColumn172"] = getStrDecimal(total.DataColumn172, id);
            section.SimpleReportParams["SumColumn173"] = getStrDecimal(total.DataColumn173, id);
            section.SimpleReportParams["SumColumn174"] = getStrDecimal(total.DataColumn174, id);
            section.SimpleReportParams["SumColumn175"] = getStrDecimal(total.DataColumn175, id);
            section.SimpleReportParams["SumColumn176"] = getStrDecimal(total.DataColumn176, id);
            section.SimpleReportParams["SumColumn177"] = getStrDecimal(total.DataColumn177, id);
            //section.SimpleReportParams["SumColumn178"] = getStrDecimal(total.DataColumn178, id);
            section.SimpleReportParams["SumColumn179"] = getStrDecimal(total.DataColumn179, id);
            section.SimpleReportParams["SumColumn180"] = getStrDecimal(total.DataColumn180, id);
        }
    }
}