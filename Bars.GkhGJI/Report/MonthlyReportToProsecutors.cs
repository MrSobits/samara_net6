namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Ежемесячный отчет по отделам Инспекции, предоставляемый на сайт Правительства ЯНАО"
    /// </summary>
    public class MonthlyReportToProsecutors : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> zonInspIdsList = new List<long>();

        public MonthlyReportToProsecutors()
            : base(new ReportTemplateBinary(Properties.Resources.MonthlyReportToProsecutors))
        {

        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.MonthlyReportToProsecutors";
            }
        }

        public override string Desciption
        {
            get { return "Отчет \"Ежемесячный отчет по отделам Инспекции, предоставляемый на сайт Правительства ЯНАО\""; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.MonthlyReportToProsecutors"; }
        }

        public override string Name
        {
            get { return "Отчет \"Ежемесячный отчет по отделам Инспекции, предоставляемый на сайт Правительства ЯНАО\""; }
        }
        
        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            var zonInspIds = baseParams.Params.GetAs("zonInspIds", string.Empty);

            this.zonInspIdsList = !string.IsNullOrEmpty(zonInspIds) ? zonInspIds.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityListId = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                .WhereIf(zonInspIdsList.Count > 0, x => zonInspIdsList.Contains(x.ZonalInspection.Id)).Select(x => x.Municipality.Id).Distinct().ToList();

            var municipalityIds = userManager.GetMunicipalityIds();

            if (municipalityIds.Count > 0)
            {
                municipalityListId = municipalityIds.Intersect(municipalityListId).ToList();
            }

            var zjimo = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                 .WhereIf(zonInspIdsList.Count > 0, x => zonInspIdsList.Contains(x.ZonalInspection.Id))
                 .Select(x => new
                 {
                     ZonalInspectionName = x.ZonalInspection.Name + x.ZonalInspection.ZoneName,
                     MunicipalityId = x.Municipality.Id
                 })
                 .ToList();

            var moByZji = zjimo.GroupBy(x => x.ZonalInspectionName).ToDictionary(x => x.Key, x => x.Select(y => y.MunicipalityId).ToList());

            var zjiActCheckTotal = moByZji.Keys.ToDictionary(x => x, x => 0);
            var zjiPrescriptionActs = moByZji.Keys.ToDictionary(x => x, x => 0);
            var zjiStatementTotal = moByZji.Keys.ToDictionary(x => x, x => 0);
            var zjiActs = moByZji.Keys.ToDictionary(x => x, x => 0);
            var zjiAreas = moByZji.Keys.ToDictionary(x => x, x => 0M);
            var zjiPrescriptions = moByZji.Keys.ToDictionary(x => x, x => 0M);
            var zjiResolutions = moByZji.Keys.ToDictionary(x => x, x => 0);
            var zjiResolutionsPenaltyAmount = moByZji.Keys.ToDictionary(x => x, x => 0M);
            var zjiProtocols = moByZji.Keys.ToDictionary(x => x, x => 0);
            var zjiAppeals = moByZji.Keys.ToDictionary(x => x, x => 0);

            foreach (var zji in moByZji)
            {
                // запрос идентификаторов плановых проверок юр.лиц текущего зжи
                var jurPersonInspectionsQuery = this.Container.Resolve<IDomainService<InspectionGji>>().GetAll()
                   .Where(x => zji.Value.Contains(x.Contragent.Municipality.Id))
                   .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                   .Select(x => x.Id);

                // запрос идентификаторов "не главных" распоряжений в проверках юр.лиц по текущему зжи
                var disposalsChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>()
                             .GetAll()
                             .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                             .Where(x => jurPersonInspectionsQuery.Contains(x.Children.Inspection.Id)).Select(x => x.Children.Id);

                // количество актов проверки, сформированных из главного распоряжения, у которого основание - Плановая проверка юр.лица
                var actsCount = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                    .Where(x => jurPersonInspectionsQuery.Contains(x.Parent.Inspection.Id))
                                    .Where(x => !disposalsChildren.Contains(x.Parent.Id))
                                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= this.dateStart)
                                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= this.dateEnd)
                                    .Select(x => x.Children.Id)
                                    .Distinct()
                                    .Count();

                zjiActCheckTotal[zji.Key] = actsCount;

                // запрос идентификаторов проверок текущего зжи
                var inspectionIdsQuery = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => zji.Value.Contains(x.RealityObject.Municipality.Id))
                .Select(x => x.Inspection.Id);

                // запрос идентификаторов распоряжений на проверку предписаний
                var dispsalsByPrescriptionIds =
                    this.Container.Resolve<IDomainService<Disposal>>().GetAll()
                        .Where(x => x.TypeDisposal == TypeDisposalGji.DocumentGji)
                        .Where(x => inspectionIdsQuery.Contains(x.Inspection.Id))
                        .Select(x => x.Id);

                // количество актов проверки, сформированных из распоряжения на проверку предписаний
                var actsOnPrescriptionCheckCount =
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                                 && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= this.dateEnd)
                        .Where(x => inspectionIdsQuery.Contains(x.Parent.Inspection.Id))
                        .Where(x => dispsalsByPrescriptionIds.Contains(x.Parent.Id))
                        .Distinct()
                        .Count();

                zjiPrescriptionActs[zji.Key] = actsOnPrescriptionCheckCount;

                // запрос идентификаторов проверок по обращениям граждан текущей зональной инспекции
                var citizenStatementInspectionsQuery =
                    this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                        .Where(x => zji.Value.Contains(x.RealityObject.Municipality.Id))
                        .Where(x => x.ActCheck.Inspection.TypeBase == TypeBase.CitizenStatement)
                        .Select(x => x.ActCheck.Inspection.Id);

                // запрос идентификаторов "не главных" распоряжений проверок по обращениям граждантекущей зональной инспекции
                var disposalsByCitizenStatementQuery =
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>()
                        .GetAll()
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => citizenStatementInspectionsQuery.Contains(x.Children.Inspection.Id))
                        .Select(x => x.Children.Id);

                // количество актов проверки, сформированных из главного распоряжения, у которого основание - Обращение граждан
                var actDisposalsByCitizenStatementCount =
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                        .Where(x => citizenStatementInspectionsQuery.Contains(x.Parent.Inspection.Id))
                        .Where(x => !disposalsByCitizenStatementQuery.Contains(x.Parent.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= this.dateEnd)
                        .Distinct()
                        .Count();

                zjiStatementTotal[zji.Key] = actDisposalsByCitizenStatementCount;

                // запрос идентификаторов "не главных" распоряжений проверок по текущей зональной инспекции
                var disposalIdsQuery =
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>()
                        .GetAll()
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => inspectionIdsQuery.Contains(x.Children.Inspection.Id))
                        .Select(x => x.Children.Id);

                // запрос идентификаторов "главных" распоряжений проверок по текущей зональной инспекции, у которых есть тип обследования с кодом 890006 и/или 890007
                var disposalsTypeSurveyQuery =
                    this.Container.Resolve<IDomainService<DisposalTypeSurvey>>()
                        .GetAll()
                        .Where(x => !disposalIdsQuery.Contains(x.Disposal.Id))
                        .Where(x => x.TypeSurvey.Code == "890006" || x.TypeSurvey.Code == "890007")
                        .Where(x => inspectionIdsQuery.Contains(x.Disposal.Inspection.Id))
                        .Select(x => x.Disposal.Id);

                // количество актов проверки, сформированных из главного распоряжения, у которых есть тип обследования с кодом 890006 и/или 890007
                var actDisposalsTechConditionCount =
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                        .Where(x => inspectionIdsQuery.Contains(x.Parent.Inspection.Id))
                        .Where(x => disposalsTypeSurveyQuery.Contains(x.Parent.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= this.dateEnd)
                        .Distinct()
                        .Count();

                zjiActs[zji.Key] = actDisposalsTechConditionCount;

                // запрос идентификаторов предписаний по текущей зональной инспекции
                var prescriptionIdsQuery = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                     .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Prescription)
                     .Where(x => zji.Value.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                     .Select(x => x.Document.Id);

                // запрос идентификаторов актов проверки предписаний
                var actremovalsQuery = 
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => prescriptionIdsQuery.Contains(x.Parent.Id))
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= this.dateEnd)
                    .Select(x => x.Children.Id);

                var actsQuery1 = this.Container.Resolve<IDomainService<DocumentGjiChildren>>()
                                .GetAll()
                                .Where(x => actremovalsQuery.Contains(x.Children.Id))
                                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                .Select(x => x.Parent.Id);

                var actsQuery2 = this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                                .WhereIf(this.dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= this.dateStart)
                                .WhereIf(this.dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= this.dateEnd)
                                .Where(x => zji.Value.Contains(x.RealityObject.Municipality.Id))
                                .Select(x => x.ActCheck.Id);

                // Cумма значений поля "Площадь" из всех актов проверок. Если это акт проверки предписаний, учитывать площадь главного акта
                var actCheckAreaSum =
                    this.Container.Resolve<IDomainService<ActCheck>>().GetAll()
                        .Where(x => actsQuery1.Contains(x.Id) || actsQuery2.Contains(x.Id))
                        .Sum(x => x.Area);

                zjiAreas[zji.Key] = actCheckAreaSum ?? 0M;

                // идентификаторы предписаний по текущей ЗЖИ (по нарушениям)
                var prescriptionIdsByViolation = 
                    this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                        .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Where(x => zji.Value.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= this.dateEnd)
                        .Select(x => x.Document.Id)
                        .ToList();

                // идентификаторы предписаний по текущей ЗЖИ (по проверкам)
                var prescriptionIdsByInspections =
                    this.Container.Resolve<IDomainService<Prescription>>().GetAll()
                        .Where(x => inspectionIdsQuery.Contains(x.Inspection.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentDate <= this.dateEnd)
                        .Select(x => x.Id)
                        .ToList();

                // количество предписаний
                var prescriptionCount = prescriptionIdsByViolation.Union(prescriptionIdsByInspections).Distinct().Count();

                zjiPrescriptions[zji.Key] = prescriptionCount;

                // идентификаторы постановлений и суммы значений поля "Сумма штрафа" по постановлениям по текущей ЗЖИ
                var resolutionsPenaltyAmountList =
                    this.Container.Resolve<IDomainService<Resolution>>()
                        .GetAll()
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentDate <= this.dateEnd)
                        .Where(x => inspectionIdsQuery.Contains(x.Inspection.Id))
                        .Select(x => new { x.Id, x.PenaltyAmount })
                        .AsEnumerable()
                        .Distinct()
                        .ToList();

                zjiResolutions[zji.Key] = resolutionsPenaltyAmountList.Count;
                zjiResolutionsPenaltyAmount[zji.Key] = resolutionsPenaltyAmountList.Sum(x => x.PenaltyAmount) ?? 0M;


                // идентификаторы протоколов по текущей ЗЖИ (по нарушениям)
                var protocolIdsByViolation =
                    this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                        .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                        .Where(x => zji.Value.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= this.dateEnd)
                        .Select(x => x.Document.Id)
                        .ToList();

                // идентификаторы протоколов по текущей ЗЖИ (по проверкам)
                var protocolIdsByInspections =
                    this.Container.Resolve<IDomainService<Protocol>>().GetAll()
                        .Where(x => inspectionIdsQuery.Contains(x.Inspection.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentDate <= this.dateEnd)
                        .Select(x => x.Id)
                        .ToList();

                // количество протоколов
                var protocolCount = protocolIdsByViolation.Union(protocolIdsByInspections).Distinct().Count();

                zjiProtocols[zji.Key] = protocolCount;

                var appealsCount =
                    this.Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll()
                        .Where(x => zji.Value.Contains(x.RealityObject.Municipality.Id))
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.AppealCits.DateFrom >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.AppealCits.DateFrom <= this.dateEnd)
                        .Select(x => x.AppealCits.Id)
                        .Distinct()
                        .Count();

                zjiAppeals[zji.Key] = appealsCount;
            }


            // Количество уникальных домов из актов проверок
            var actsRoCount =
                this.Container.Resolve<IDomainService<ActCheckRealityObject>>()
                    .GetAll()
                    .Where(x => municipalityListId.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= this.dateEnd)
                    .GroupBy(x => x.RealityObject.Municipality.Id)
                    .Select(x => new { x.Key, RoCount = x.Select(y => y.RealityObject.Id).Distinct().Count() })
                    .ToDictionary(x => x.Key, x => x.RoCount);

            var zjiRoActs = moByZji.Keys.ToDictionary(x => x, x => 0);
            
            // заполнение словаря по зональным инспекциям
            moByZji.Keys.ForEach(x => zjiRoActs[x] = moByZji[x].Select(y => actsRoCount.ContainsKey(y) ? actsRoCount[y] : 0).Sum());

            // Количество нарушений из актов проверок. Не учитываются нарушения из актов проверки предписаний
            var actViolations = 
                this.Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
                    .Where(x => municipalityListId.Contains(x.ActObject.RealityObject.Municipality.Id))
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.ActObject.ActCheck.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.ActObject.ActCheck.DocumentDate <= this.dateEnd)
                    .GroupBy(x => x.InspectionViolation.RealityObject.Municipality.Id)
                    .Select(x => new { x.Key, ViolationCount = x.Select(y => y.InspectionViolation.Id).Distinct().Count() })
                    .ToDictionary(x => x.Key, x => x.ViolationCount);

            var zjiActViolations = moByZji.Keys.ToDictionary(x => x, x => 0);

            // заполнение словаря по зональным инспекциям
            moByZji.Keys.ForEach(x => zjiActViolations[x] = moByZji[x].Select(y => actViolations.ContainsKey(y) ? actViolations[y] : 0).Sum());
            

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var key in moByZji.Keys)
            {
                section.ДобавитьСтроку();
                section["ZonalInspection"] = key;
                section["RoutineChecksCount"] = zjiActCheckTotal[key];
                section["InspectionPrescriptionsCount"] = zjiPrescriptionActs[key];
                section["InspectionAppealsCount"] = zjiStatementTotal[key];
                section["InspectionsTotalCount"] = zjiPrescriptionActs[key] + zjiStatementTotal[key];
                section["InspectionHouseConditionCount"] = zjiActs[key];
                section["ROArea"] = (zjiAreas[key] / 1000).ToString();
                section["ROCount"] = zjiRoActs[key];
                section["ViolationsCount"] = zjiActViolations[key];
                section["PrescriptionsCount"] = zjiPrescriptions[key];
                section["ResolutionsCount"] = zjiResolutions[key];
                section["ResolutionsAmount"] = (zjiResolutionsPenaltyAmount[key] / 1000).ToString();
                section["ProtocolsCount"] = zjiProtocols[key];
                section["AppealsCount"] = zjiAppeals[key];
            }

            reportParams.SimpleReportParams["RoutineChecksCountTotal"] = zjiActCheckTotal.Values.Sum();
            reportParams.SimpleReportParams["InspectionPrescriptionsCountTotal"] = zjiPrescriptionActs.Values.Sum();
            reportParams.SimpleReportParams["InspectionAppealsCountTotal"] = zjiStatementTotal.Values.Sum();
            reportParams.SimpleReportParams["InspectionsTotalCountTotal"] = zjiPrescriptionActs.Values.Sum() + zjiStatementTotal.Values.Sum();
            reportParams.SimpleReportParams["InspectionHouseConditionCountTotal"] = zjiActs.Values.Sum();
            reportParams.SimpleReportParams["ROAreaTotal"] = (zjiAreas.Values.Sum() / 1000).ToString();
            reportParams.SimpleReportParams["ROCountTotal"] = zjiRoActs.Values.Sum();
            reportParams.SimpleReportParams["ViolationsCountTotal"] = zjiActViolations.Values.Sum();
            reportParams.SimpleReportParams["PrescriptionsCountTotal"] = zjiPrescriptions.Values.Sum();
            reportParams.SimpleReportParams["ResolutionsCountTotal"] = zjiResolutions.Values.Sum();
            reportParams.SimpleReportParams["ResolutionsAmountTotal"] = (zjiResolutionsPenaltyAmount.Values.Sum() / 1000).ToString();
            reportParams.SimpleReportParams["ProtocolsCountTotal"] = zjiProtocols.Values.Sum();
            reportParams.SimpleReportParams["AppealsCountTotal"] = zjiAppeals.Values.Sum();
        }
    }
}