namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ControlDocGjiExecution : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private string _disposalText;

        private string disposalText
        {
            get
            {
                if (string.IsNullOrEmpty(_disposalText))
                {
                    _disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
                }

                return _disposalText;
            }
        }

        private DateTime DateStart = DateTime.MinValue;

        private DateTime DateEnd = DateTime.MaxValue;

        // идентификаторы муниципальных образований
        private List<long> municipalityIds = new List<long>();

        // идентификаторы инспекторов
        private List<long> inspectorIds = new List<long>();

        private List<InspectionRealtyObjectProxy> inspectionRealtyObjectList;

        private IQueryable<long> inspectionIdsQuery;

        public ControlDocGjiExecution()
            : base(new ReportTemplateBinary(Properties.Resources.ControlDocGJIExecution))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ControlDocGjiExecution";
            }
        }

        public override string Desciption
        {
            get { return "Контроль исполнения документов ГЖИ"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ControlDocGjiExecution"; }
        }

        public override string Name
        {
            get { return "Контроль исполнения документов ГЖИ"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.DateStart = baseParams.Params["dateStart"].ToDateTime();
            this.DateEnd = baseParams.Params["dateEnd"].ToDateTime();

            var municipalityIdsStr = baseParams.Params.GetAs("municipalityIds", string.Empty);
            var inspectorIdsStr = baseParams.Params.GetAs("inspectorIds", string.Empty);

            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsStr) ? municipalityIdsStr.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();
            this.inspectorIds = !string.IsNullOrEmpty(inspectorIdsStr) ? inspectorIdsStr.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceZonalInspectionMunicipality = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
            var serviceInspectionGjiRealityObject = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            reportParams.SimpleReportParams["НачалоПериода"] = this.DateStart.ToShortDateString();
            reportParams.SimpleReportParams["ОкончаниеПериода"] = this.DateEnd.ToShortDateString();

            var municipalityByZonalInspectionDict = serviceZonalInspectionMunicipality.GetAll()
                .WhereIf(municipalityIds.Count >0, x => this.municipalityIds.Contains(x.Municipality.Id))
                .Select(x => new
                {
                    ZonalInspectionName = x.ZonalInspection.ZoneName ?? x.ZonalInspection.Name,
                    x.Municipality.Id,
                    x.Municipality.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.ZonalInspectionName)
                .ToDictionary(x => x.Key, x => x.ToList());

            // Дома проверок
            inspectionRealtyObjectList = serviceInspectionGjiRealityObject.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new InspectionRealtyObjectProxy
                {
                    InspectionId = x.Inspection.Id,
                    MunicipalityId = x.RealityObject.Municipality.Id,
                    Address = x.RealityObject.Address
                })
                .ToList();

            // Запрос идентификаторов проверок
            inspectionIdsQuery = serviceInspectionGjiRealityObject.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => x.Inspection.Id);

            var appealsByMunicipalityDict = this.GetCitizenAppeals();
            var prescriptionsByMunicipalityDict = this.GetPrescriptions();
            var disposalsByMunicipalityDict = this.GetDisposals();
            var inspBaseJurPersonByMunicipalityDict = this.GetJurPersonPlannedChecks();
            var inspCheckWithMuByMunicipalityDict = this.GetInspectionsChecks();

            var sectionZonalInspection = reportParams.ComplexReportParams.ДобавитьСекцию("sectionZonalInspection");
            var sectionDocument = sectionZonalInspection.ДобавитьСекцию("sectionDocument");

            var municipalitiesWithDocuments = appealsByMunicipalityDict.Keys
                .Union(prescriptionsByMunicipalityDict.Keys)
                .Union(disposalsByMunicipalityDict.Keys)
                .Union(inspBaseJurPersonByMunicipalityDict.Keys)
                .Union(inspCheckWithMuByMunicipalityDict.Keys)
                .Distinct()
                .ToList();

            foreach (var zonalInspection in municipalityByZonalInspectionDict.OrderBy(x => x.Key))
            {
                var zonalInspectionHasAnyDocument = zonalInspection.Value.Select(x => x.Id).Any(municipalitiesWithDocuments.Contains);

                if (!zonalInspectionHasAnyDocument)
                {
                    continue;
                }

                sectionZonalInspection.ДобавитьСтроку();
                sectionZonalInspection["ЗЖИ"] = zonalInspection.Key;

                Action<Dictionary<long, List<ReportRow>>> fillZonalInspectionDocument = 
                    x => zonalInspection.Value.ForEach(municipality => this.FillReport(sectionDocument, municipality.Id, municipality.Name, x));

                fillZonalInspectionDocument(appealsByMunicipalityDict);
                fillZonalInspectionDocument(prescriptionsByMunicipalityDict);
                fillZonalInspectionDocument(disposalsByMunicipalityDict);
                fillZonalInspectionDocument(inspBaseJurPersonByMunicipalityDict);
                fillZonalInspectionDocument(inspCheckWithMuByMunicipalityDict);
            }
        }

        // Плановая проверка юр.лица
        private Dictionary<long, List<ReportRow>> GetJurPersonPlannedChecks()
        {
            var servDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servInspectionGjiInspector = this.Container.Resolve<IDomainService<InspectionGjiInspector>>();
            
            var documentGjiChilrenQuery = servDocumentGjiChildren.GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal);

            // Запрос на плановые проверки юр.лиц
            var jurPersonInspectionsQuery = this.Container.Resolve<IDomainService<BaseJurPerson>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id))
                .WhereIf(this.DateStart != DateTime.MinValue, x => x.DateStart >= this.DateStart)
                .WhereIf(this.DateEnd != DateTime.MinValue, x => x.DateStart <= this.DateEnd)
                .Where(x => x.TypeFact != TypeFactInspection.NotDone
                        || (x.TypeFact == TypeFactInspection.NotDone && (x.Reason == string.Empty || x.Reason == null)));
                
            var jurPersonInspectionIdsQuery = jurPersonInspectionsQuery.Select(x => x.Id);

            // Инспекторы
            var inspectorsByInspectionDict = servInspectionGjiInspector.GetAll()
                .Where(x => jurPersonInspectionIdsQuery.Contains(x.Inspection.Id))
                .Select(x => new
                {
                    InspectionId = x.Inspection.Id,
                    InspectorId = x.Inspector.Id,
                    x.Inspector.Fio
                })
                .AsEnumerable()
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, x => x.Select(y => new InspectorProxy { Id = y.InspectorId, Fio = y.Fio }).ToList());

            var inspBaseJurPersonResult = jurPersonInspectionsQuery
                .Select(x => new
                {
                    x.Id,
                    MunicipalityId = x.Contragent.Municipality.Id,
                    x.Contragent.JuridicalAddress,
                    ContragentName = x.Contragent.Name,
                    x.InspectionNumber,
                    x.DateStart,
                    x.CountDays,
                    x.TypeForm,
                    x.TypeFact,
                    x.Reason,
                    hasDisposalAndAct = documentGjiChilrenQuery.Any(y => y.Parent.Inspection.Id == x.Id)
                })
                .AsEnumerable()
                .Where(x => (x.TypeFact == TypeFactInspection.NotDone && string.IsNullOrEmpty(x.Reason))
                    || ((x.TypeFact == TypeFactInspection.Done || x.TypeFact == TypeFactInspection.NotSet) && !x.hasDisposalAndAct))
                .Select(x => new { inspectionData = x, inspectors = inspectorsByInspectionDict.ContainsKey(x.Id) ? inspectorsByInspectionDict[x.Id] : null })
                .ToList();

            if (this.inspectorIds.Any())
            {
                inspBaseJurPersonResult = inspBaseJurPersonResult
                    .Where(x => x.inspectors != null)
                    .Select(x => new { x.inspectionData, inspectors = this.FilterInspectors(x.inspectors) })
                    .Where(x => x.inspectors.Any())
                    .ToList();
            }

            var res = inspBaseJurPersonResult
                .GroupBy(x => x.inspectionData.MunicipalityId)
                .ToDictionary(
                    x => x.Key, 
                    x => x.Select(y =>
                            {
                                var data = y.inspectionData;

                                var row = new ReportRow
                                    {
                                        DocumentName = "Плановая проверка юр.лица",
                                        DocumentNumber = data.InspectionNumber,
                                        DateStart = data.DateStart,
                                        ExecutionPeriod = data.CountDays,
                                        Address = data.JuridicalAddress,
                                        Content = "Проверка по распоряжению прокуратуры " + data.ContragentName,
                                        Resolution = data.TypeForm.GetEnumMeta().Display
                                    };

                                if (data.DateStart.HasValue && data.CountDays.HasValue)
                                {
                                    row.DateEnd = data.DateStart.Value.AddDays(data.CountDays.Value);
                                }

                                if (y.inspectors != null && y.inspectors.Any())
                                {
                                    row.Executant = y.inspectors.Select(z => z.Fio).Aggregate((a, b) => a + ", " + b);
                                }

                                return row;
                            })
                          .ToList());

            return res;
        }

        // Инспекционное обследование по плану
        private Dictionary<long, List<ReportRow>> GetInspectionsChecks()
        {
            var servDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servInspectionGjiInspector = this.Container.Resolve<IDomainService<InspectionGjiInspector>>();
            
            // Инспекционное обследование по плану
            var servBaseInsCheck = this.Container.Resolve<IDomainService<BaseInsCheck>>();

            var inspCheckQuery = servBaseInsCheck.GetAll()
                .Where(x => inspectionIdsQuery.Contains(x.Id))
                .WhereIf(this.DateStart != DateTime.MinValue, x => x.InsCheckDate >= this.DateStart)
                .WhereIf(this.DateEnd != DateTime.MinValue, x => x.InsCheckDate <= this.DateEnd)
                .Where(x => x.TypeFact != TypeFactInspection.NotDone || (x.TypeFact == TypeFactInspection.NotDone && (x.Reason == string.Empty || x.Reason == null)));

            var inspCheckIdsQuery = inspCheckQuery.Select(x => x.Id);

            // Инспекторы проверок
            var inspectorsByInspectionDict = servInspectionGjiInspector.GetAll()
                .Where(x => inspCheckIdsQuery.Contains(x.Inspection.Id))
                .Select(x => new
                {
                    InspectionId = x.Inspection.Id,
                    InspectorId = x.Inspector.Id,
                    x.Inspector.Fio
                })
                .AsEnumerable()
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, x => x.Select(y => new InspectorProxy{ Id = y.InspectorId, Fio = y.Fio }).ToList());

            var documentGjiChilrenQuery = servDocumentGjiChildren.GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal);

            var inspectionChecks = inspCheckQuery
                .Select(x => new
                {
                    x.Id,
                    x.InspectionNumber,
                    x.InsCheckDate,
                    x.TypeFact,
                    x.Reason,
                    hasDisposalAndAct = documentGjiChilrenQuery.Any(y => y.Parent.Inspection.Id == x.Id)
                })
                .AsEnumerable()
                .Where(x => (x.TypeFact == TypeFactInspection.NotDone && string.IsNullOrEmpty(x.Reason))
                    || ((x.TypeFact == TypeFactInspection.Done || x.TypeFact == TypeFactInspection.NotSet) && !x.hasDisposalAndAct))
                .Select(x => new { inspectionData = x, inspectors = inspectorsByInspectionDict.ContainsKey(x.Id) ? inspectorsByInspectionDict[x.Id] : null })
                .ToList();

            if (this.inspectorIds.Any())
            {
                inspectionChecks = inspectionChecks
                    .Where(x => x.inspectors != null)
                    .Select(x => new { x.inspectionData, inspectors = this.FilterInspectors(x.inspectors) })
                    .Where(x => x.inspectors.Any())
                    .ToList();
            }

            var inspectionChecksDict = inspectionChecks.ToDictionary(x => x.inspectionData.Id);

            var res = inspectionRealtyObjectList
                .Where(x => inspectionChecksDict.ContainsKey(x.InspectionId))
                .Select(x =>
                    {
                        var data = inspectionChecksDict[x.InspectionId];
                        
                        var reportRow = new ReportRow
                        {
                            DocumentName = "Инспекционное обследование по плану",
                            DocumentNumber = data.inspectionData.InspectionNumber,
                            DateStart = data.inspectionData.InsCheckDate,
                            ExecutionPeriod = 5,
                            Address = x.Address,
                            Content = "Инспекционное обследование по плану",
                            Resolution = "Инспекционное"
                        };

                        if (data.inspectionData.InsCheckDate.HasValue)
                        {
                            reportRow.DateEnd = data.inspectionData.InsCheckDate.Value.AddDays(5);
                        }

                        if (data.inspectors != null && data.inspectors.Any())
                        {
                            reportRow.Executant = data.inspectors.Select(y => y.Fio).Aggregate((a, b) => a + ", " + b);
                        }
                                          
                        return new { x.MunicipalityId, reportRow };
                    })
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.reportRow).ToList());

            return res;
        }

        // Распоряжения
        private Dictionary<long, List<ReportRow>> GetDisposals()
        {
            var serviceDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var serviceDisposalTypeSurvey = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var serviceDocumentGjiInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();

            var documentRelationQuery = serviceDocumentGjiChildren.GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal);
            
            // распоряжения проверки
            var disposalsQuery = serviceDisposal.GetAll()
                .WhereIf(this.DateStart != DateTime.MinValue, x => x.DateStart >= this.DateStart)
                .WhereIf(this.DateEnd != DateTime.MinValue, x => x.DateEnd <= this.DateEnd)
                .Where(x => inspectionIdsQuery.Contains(x.Inspection.Id))
                .Where(x => !(x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement && x.TypeAgreementResult == TypeAgreementResult.NotAgreed)) //  Если у Распоряжения в поле "Согласование с прокуратурой" стоит значение = Требует согласования, в поле - значение = Отказано, то Распоряжение в отчете НЕ попадает
                .Where(x => !documentRelationQuery.Any(y => y.Parent.Id == x.Id)); // Попадает, если из Распоряжения не сформирован акт проверки. 

            var disposalIdsQuery = disposalsQuery.Select(x => x.Id);

            // Типы обследований
            var disposalTypesSurveyDict = serviceDisposalTypeSurvey.GetAll()
                .Where(x => disposalIdsQuery.Contains(x.Disposal.Id))
                .Select(x => new { x.Disposal.Id, x.TypeSurvey.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .Select(x => new
                {
                    x.Key,
                    TypesSurvey = x.Select(y => y.Name).Aggregate((curr, next) => string.Format("{0}, {1}", curr, next))
                })
                .ToDictionary(x => x.Key, x => x.TypesSurvey);

            var disposalInspectorsDict = serviceDocumentGjiInspector.GetAll()
                .Where(x => disposalIdsQuery.Contains(x.DocumentGji.Id))
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Select(x => new { x.DocumentGji.Id, x.Inspector.Fio, InspectorId = x.Inspector.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => new InspectorProxy{ Id = y.InspectorId, Fio = y.Fio }).ToList());

            var disposals = disposalsQuery
                .Select(x => new
                {
                    InspectionId = x.Inspection.Id,
                    x.DocumentNumber,
                    x.DateStart,
                    x.DateEnd,
                    DocumentId = x.Id,
                    TypeCheck = x.KindCheck.Name
                })
                .AsEnumerable()
                .Select(x => new
                {
                    data = x,
                    inspectors = disposalInspectorsDict.ContainsKey(x.DocumentId) ? disposalInspectorsDict[x.DocumentId] : null,
                    typeSurvey = disposalTypesSurveyDict.ContainsKey(x.DocumentId) ? disposalTypesSurveyDict[x.DocumentId] : string.Empty
                })
                .ToList();

            if (this.inspectorIds.Any())
            {
                disposals = disposals
                    .Where(x => x.inspectors != null)
                    .Select(x => new { x.data, inspectors = this.FilterInspectors(x.inspectors), x.typeSurvey })
                    .Where(x => x.inspectors.Any())
                    .ToList();
            }

            var res = disposals.Join(
                inspectionRealtyObjectList, 
                disposal => disposal.data.InspectionId,
                inspectionRo => inspectionRo.InspectionId,
                (disposal, inspectionRo) =>
                    {
                        var reportRow = new ReportRow
                        {
                            DocumentName = disposalText,
                            DocumentNumber = disposal.data.DocumentNumber,
                            DateStart = disposal.data.DateStart,
                            DateEnd = disposal.data.DateEnd,
                            Address = inspectionRo.Address,
                            Content = disposal.typeSurvey,
                            Resolution = disposal.data.TypeCheck
                        };

                        if (disposal.data.DateStart.HasValue && disposal.data.DateEnd.HasValue)
                        {
                            var time = disposal.data.DateEnd.ToDateTime() - disposal.data.DateStart.ToDateTime();
                            reportRow.ExecutionPeriod = time.Days;
                        }

                        if (disposal.inspectors != null && disposal.inspectors.Any())
                        {
                            reportRow.Executant = disposal.inspectors.Select(y => y.Fio).Aggregate((a, b) => a + ", " + b);
                        }

                        return new { inspectionRo.MunicipalityId, reportRow };
                    })
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.reportRow).ToList());

            return res;
        }
        
        // Предписания
        private Dictionary<long, List<ReportRow>> GetPrescriptions()
        {
            var servicePrescriptionViolation = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var serviceProtocolViolation = this.Container.Resolve<IDomainService<ProtocolViolation>>();
            var serviceDocumentGjiInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            
            var prescriptionViolationQuery = servicePrescriptionViolation.GetAll()
                .Where(x => !serviceProtocolViolation.GetAll().Any(y => y.InspectionViolation.Inspection.Id == x.InspectionViolation.Inspection.Id
                    && y.InspectionViolation.Violation.Id == x.InspectionViolation.Violation.Id
                    && y.InspectionViolation.RealityObject.Id == x.InspectionViolation.RealityObject.Id))
                .Where(x => x.InspectionViolation.DateFactRemoval == null)
                .Where(x => x.InspectionViolation.RealityObject != null)
                .WhereIf(municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                .WhereIf(this.DateStart != DateTime.MinValue, x => x.Document.DocumentDate >= this.DateStart)
                .WhereIf(this.DateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= this.DateEnd);

            // инспекторы предписаний
            var prescriptionIdsQuery = prescriptionViolationQuery.Select(x => x.Document.Id);

            var prescriptionInspectorsDict = serviceDocumentGjiInspector.GetAll()
                .WhereIf(this.inspectorIds.Count > 0, x => this.inspectorIds.Contains(x.Inspector.Id))
                .Where(x => prescriptionIdsQuery.Contains(x.DocumentGji.Id))
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Select(x => new { x.DocumentGji.Id, x.Inspector.Fio, InspectorId = x.Inspector.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => new InspectorProxy{ Id = y.InspectorId, Fio = y.Fio }).ToList());

            var prescriptions = prescriptionViolationQuery
                .Select(x => new
                {
                    prescriptionId = x.Document.Id,
                    x.Document.DocumentNumber,
                    x.InspectionViolation.DatePlanRemoval,
                    x.InspectionViolation.Violation.CodePin,
                    TextViolation = x.InspectionViolation.Violation.Name,
                    x.InspectionViolation.RealityObject.Address,
                    MunicipalityId = x.InspectionViolation.RealityObject.Municipality.Id
                })
                .AsEnumerable()
                .Select(x => new
                {
                    prescriptionData = x, 
                    inspectors = prescriptionInspectorsDict.ContainsKey(x.prescriptionId) ? prescriptionInspectorsDict[x.prescriptionId] : null
                })
                .ToList();

            if (this.inspectorIds.Any())
            {
                prescriptions = prescriptions
                    .Where(x => x.inspectors != null)
                    .Select(x => new { x.prescriptionData, inspectors = this.FilterInspectors(x.inspectors)})
                    .Where(x => x.inspectors.Any())
                    .ToList();
            }

            var res = prescriptions
                .GroupBy(x => x.prescriptionData.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y =>
                    {
                        var data = y.prescriptionData;

                        var row = new ReportRow
                        {
                            DocumentName = "Предписание",
                            DocumentNumber = data.DocumentNumber,
                            DateStart = data.DatePlanRemoval,
                            ExecutionPeriod = 5,
                            Address = data.Address,
                            Content = string.Format("{0} {1}", data.CodePin, data.TextViolation),
                            Resolution = "Проверка предписания"
                        };

                        if (data.DatePlanRemoval.HasValue)
                        {
                            row.DateEnd = data.DatePlanRemoval.Value.AddDays(5);
                        }
                        
                        if (y.inspectors != null && y.inspectors.Any())
                        {
                            row.Executant = y.inspectors.Select(z => z.Fio).Aggregate((a, b) => a + ", " + b);
                        }

                        return row;
                    })
                    .ToList());

            return res;
        }

        // Обращения
        private Dictionary<long, List<ReportRow>> GetCitizenAppeals()
        {
            var serviceAppealCitsRealityObject = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var serviceBaseStatementAppealCits = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var serviceActCheck = this.Container.Resolve<IDomainService<ActCheck>>();
            var serviceInspectionGjiViolStage = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var serviceAppealCitsAnswer = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();

            var hasViolationsQuery = serviceInspectionGjiViolStage.GetAll()
                .Where(x => x.InspectionViolation.DateFactRemoval == null)
                .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Document.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Where(x => !serviceInspectionGjiViolStage.GetAll()
                    .Where(y => y.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Any(y => y.InspectionViolation.Inspection.Id == x.InspectionViolation.Inspection.Id
                    && y.InspectionViolation.Violation.Id == x.InspectionViolation.Violation.Id
                    && y.InspectionViolation.RealityObject.Id == x.InspectionViolation.RealityObject.Id));

            var hasAnswerWithNumberQuery = serviceAppealCitsAnswer.GetAll()
                .Where(x => x.DocumentNumber != string.Empty);
            
            var appeals = serviceAppealCitsRealityObject.GetAll().Join(
                serviceBaseStatementAppealCits.GetAll(),
                a => a.AppealCits.Id,
                b => b.AppealCits.Id,
                (a, b) => new
                    {
                        MunicipalityId = a.RealityObject.Municipality.Id,
                        Address = a.RealityObject.Address,
                        numberGJI = b.AppealCits.NumberGji,
                        startDate = b.AppealCits.DateFrom,
                        endDate = b.AppealCits.CheckTime,
                        Text = b.AppealCits.Description,
                        Inspector = b.AppealCits.Tester.Fio ?? b.AppealCits.Executant.Fio,
                        InspectorId = ((long?)b.AppealCits.Tester.Id) ?? ((long?)b.AppealCits.Executant.Id) ?? -1,
                        SuretyResolve = b.AppealCits.SuretyResolve.Code,
                        Resolve = b.AppealCits.SuretyResolve.Name,
                        hasDisposal = serviceDisposal.GetAll().Any(y => y.Inspection.Id == b.Inspection.Id),
                        hasAct = serviceActCheck.GetAll().Any(y => y.Inspection.Id == b.Inspection.Id),
                        hasViolations = hasViolationsQuery.Any(y => y.InspectionViolation.Inspection.Id == b.Inspection.Id),
                        hasAnswerWithNumber = hasAnswerWithNumberQuery.Any(y => y.AppealCits.Id == a.AppealCits.Id)
                    })
                .WhereIf(municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.MunicipalityId))
                .WhereIf(this.DateStart != DateTime.MinValue, x => x.startDate >= this.DateStart)
                .WhereIf(this.DateEnd != DateTime.MinValue, x => x.endDate <= this.DateEnd)
                .AsEnumerable()
                .Where(x => (!(x.hasDisposal && x.hasAct) && (x.SuretyResolve == "1" || x.SuretyResolve == "2")) 
                    || (x.hasDisposal && x.hasAct && x.hasViolations && (x.SuretyResolve == "1" || x.SuretyResolve == "2"))
                    || (x.SuretyResolve == "3" && !x.hasAnswerWithNumber))
                .ToList();

            if (this.inspectorIds.Any())
            {
                appeals = appeals
                    .Where(x => this.inspectorIds.Contains(x.InspectorId))
                    .ToList();
            }

            var res = appeals
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Distinct().Select(y =>
                    {
                        var row = new ReportRow
                        {
                            DocumentName = "Обращение",
                            DocumentNumber = y.numberGJI,
                            DateStart = y.startDate,
                            DateEnd = y.endDate,
                            Address = y.Address,
                            Content = y.Text,
                            Resolution = y.Resolve,
                            Executant = y.Inspector
                        };

                        if (y.startDate.HasValue && y.endDate.HasValue)
                        {
                            var time = y.endDate.ToDateTime() - y.startDate.ToDateTime();
                            row.ExecutionPeriod = time.Days;
                        }

                        return row;
                    })
                    .ToList());

            return res;
        }

        private void FillReport(Section section, long municipalityId, string municipalityName, Dictionary<long, List<ReportRow>> dataByMunicipalityIdDict)
        {
            if (!dataByMunicipalityIdDict.ContainsKey(municipalityId))
            {
                return;
            }

            foreach (var data in dataByMunicipalityIdDict[municipalityId].OrderBy(x => x.DocumentNumber))
            {
                section.ДобавитьСтроку();
                section["Документ"] = data.DocumentName;
                section["Номер"] = data.DocumentNumber;
                section["ДатаНачала"] = data.DateStart;
                section["ДатаОкончания"] = data.DateEnd;
                section["СрокИсполнения"] = data.ExecutionPeriod;
                section["НаименованиеМО"] = municipalityName;
                section["Адрес"] = data.Address;
                section["Содержание"] = data.Content;
                section["Резолюция"] = data.Resolution;
                section["Инспектор"] = data.Executant;
            }
        }

        private List<InspectorProxy> FilterInspectors(IEnumerable<InspectorProxy> inspectors)
        {
            var inspectorsDict = inspectors.Distinct().ToDictionary(y => y.Id, y => y.Fio);
            return this.inspectorIds
                .Where(inspectorsDict.ContainsKey)
                .Select(y => new InspectorProxy
                {
                    Id = y,
                    Fio = inspectorsDict[y]
                })
                .ToList();
        }
    }

    internal sealed class ReportRow
    {
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? ExecutionPeriod { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }
        public string Resolution { get; set; }
        public string Executant { get; set; }
    }

    internal sealed class InspectionRealtyObjectProxy
    {
        public string Address { get; set; }
        public long InspectionId { get; set; }
        public long MunicipalityId { get; set; }
    }

    internal struct InspectorProxy
    {
        public long Id;
        public string Fio;
    }
}