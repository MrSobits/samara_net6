namespace Bars.GkhGji.Regions.Zabaykalye.Report.ActCheckGji
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Properties;
    using Bars.GkhGji.Report;

    public class ActCheckGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        public ActCheckGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_Definition_2))
        {
        }

        public IDomainService<ActCheckDefinition> ActCheckDefinitionDomainService { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRealityObjectDomainService { get; set; }

        public IDomainService<Disposal> DisposalDomainService { get; set; }

        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomainService { get; set; }

        public IDomainService<Prescription> PrescriptionDomainService { get; set; }

        private long DefinitionId { get; set; }

        protected override string CodeTemplate { get; set; }

        private ActCheckDefinition _definition;

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        public override string Id
        {
            get
            {
                return "ActCheckDefinition";
            }
        }

        public override string CodeForm
        {
            get
            {
                return "ActCheckDefinition";
            }
        }

        public override string Name
        {
            get
            {
                return "Определение акта проверки";
            }
        }

        public override string Description
        {
            get
            {
                return "Определение акта проверки";
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Name = "ZabaykalyeActCheck_Definition_2",
                                   Code = "ZabaykalyeBlockGJI_Definition_2",
                                   Description = "Тип определения - О доставлении лица для составления протокола",
                                   Template = Resources.BlockGJI_Definition_2
                               },
                           new TemplateInfo
                               {
                                   Name = "ZabaykalyeActCheck_Definition_7",
                                   Code = "ZabaykalyeBlockGJI_Definition_7",
                                   Description = "Тип определения - Об отказе в возбуждении дела",
                                   Template = Resources.BlockGJI_Definition_7
                               }
                       };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
            _definition = ActCheckDefinitionDomainService.Load(DefinitionId);

            if (_definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            switch (_definition.TypeDefinition)
            {
                case TypeDefinitionAct.RefusingProsecute:
                    CodeTemplate = "ZabaykalyeBlockGJI_Definition_7";
                    break;
                case TypeDefinitionAct.TransportationToProtocol:
                    CodeTemplate = "ZabaykalyeBlockGJI_Definition_2";
                    break;
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // Заполняем общие поля
            FillCommonFields(_definition.ActCheck);

            this.ReportParams["НомерОпределения"] = _definition.DocumentNum;
            this.ReportParams["ДатаОпределения"] = _definition.DocumentDate.HasValue ? _definition.DocumentDate.Value.ToShortDateString() : string.Empty;

            if (_definition.IssuedDefinition != null)
            {
                this.ReportParams["Руководитель"] = _definition.IssuedDefinition.Fio;
                this.ReportParams["РуководительФИО"] = _definition.IssuedDefinition.ShortFio;
                this.ReportParams["ДолжностьРуководителя"] = _definition.IssuedDefinition.Position;
                this.ReportParams["КодРуководителяФИО(сИнициалами)"] = _definition.IssuedDefinition.Position + " - " + _definition.IssuedDefinition.ShortFio;
            }

            var act = _definition.ActCheck;

            this.ReportParams["ДатаАкта"] = act.DocumentDate.HasValue ? act.DocumentDate.Value.ToString("dd MMMM yyyy") : string.Empty;

            this.ReportParams["НомерАкта"] = act.DocumentNumber;

            var inspector =
                DocumentGjiInspectorDomainService.GetAll()
                    .Where(x => x.DocumentGji.Id == act.Id)
                    .Select(x => new { x.Inspector.Fio, x.Inspector.Phone, x.Inspector.Position, x.Inspector.ShortFio })
                    .FirstOrDefault();

            if (inspector != null)
            {
                this.ReportParams["Инспектор"] = inspector.Fio;
                this.ReportParams["ДолжностьИнспектора"] = inspector.Position;
                this.ReportParams["ТелефонИнспектора"] = inspector.Phone;
                this.ReportParams["ИнспекторСокр"] = inspector.ShortFio;
            }

            var realtyObjects =
                ActCheckRealityObjectDomainService.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new { x.RealityObject.FiasAddress.House, x.RealityObject.FiasAddress.PlaceName, x.RealityObject.FiasAddress.StreetName })
                    .ToArray();

            if (realtyObjects.Length == 1)
            {
                var realObj = realtyObjects.First();
                this.ReportParams["НомерДома"] = realObj.House;
                this.ReportParams["НаселенныйПункт"] = realObj.PlaceName;
                this.ReportParams["Адрес"] = string.Format("{0}, {1}", realObj.PlaceName, realObj.StreetName);
            }

            var disposal = DisposalDomainService.Get(GetParentDocument(act, TypeDocumentGji.Disposal).Id);

            if (disposal != null)
            {
                this.ReportParams["КодРуководителя"] = disposal.IssuedDisposal != null ? disposal.IssuedDisposal.Code : string.Empty;

                this.ReportParams["КодРуководителяФИО_сИнициалами"] = disposal.IssuedDisposal != null
                                                               ? string.Format(
                                                                   "{0} {1}",
                                                                   disposal.IssuedDisposal.Position,
                                                                   string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio) ? disposal.IssuedDisposal.Fio : disposal.IssuedDisposal.ShortFio)
                                                               : string.Empty;

                var queryInspectorId = DocumentGjiInspectorDomainService.GetAll().Where(x => x.DocumentGji.Id == disposal.Id).Select(x => x.Inspector.Id);

                var listLocality = ZonalInspectionInspectorDomainService.GetAll().Where(x => queryInspectorId.Contains(x.Inspector.Id)).Select(x => x.ZonalInspection.Locality).Distinct().ToList();

                this.ReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
            }

            var prescriptionDoc = GetChildDocument(act, TypeDocumentGji.Prescription);

            if (prescriptionDoc != null)
            {
                var prescription = PrescriptionDomainService.GetAll().FirstOrDefault(x => x.Id == prescriptionDoc.Id);

                if (prescription != null)
                {
                    this.ReportParams["Правонарушитель"] = prescription.Contragent != null ? prescription.Contragent.Name : prescription.PhysicalPerson;
                }
            }

            if (act.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var inspectionAppealCitsDomainService = Container.ResolveDomain<InspectionAppealCits>();
                var appealCitsStatSubjectDomainService = Container.ResolveDomain<AppealCitsStatSubject>();
                using (Container.Using(inspectionAppealCitsDomainService, appealCitsStatSubjectDomainService))
                {
                    var appealsBaseStatementQuery = inspectionAppealCitsDomainService.GetAll().Where(x => x.Inspection.Id == act.Inspection.Id);

                    var appealsBaseStatementIdsQuery = appealsBaseStatementQuery.Select(x => x.AppealCits.Id);

                    var subjects = string.Join(
                        ", ",
                        appealCitsStatSubjectDomainService.GetAll()
                            .Where(x => appealsBaseStatementIdsQuery.Contains(x.AppealCits.Id))
                            .Select(x => x.Subject.Name)
                            .AsEnumerable()
                            .Distinct()
                            .Where(x => !string.IsNullOrWhiteSpace(x)));

                    var appealsBaseStatement = appealsBaseStatementQuery.Select(x => new { x.AppealCits.NumberGji, x.AppealCits.DateFrom }).ToList();

                    var appealsBaseStatementFio = string.Join(", ", appealsBaseStatementQuery.Select(x => x.AppealCits.Correspondent).AsEnumerable().Where(x => !string.IsNullOrWhiteSpace(x)));

                    var appealsBaseStatementNum = string.Join(
                        ", ",
                        appealsBaseStatement.Select(y => string.Format("{0} от {1}", y.NumberGji, y.DateFrom.ToDateTime().ToShortDateString())).Where(x => !string.IsNullOrWhiteSpace(x)));

                    this.ReportParams["ФИООбр"] = appealsBaseStatementFio;
                    this.ReportParams["Обращения"] = appealsBaseStatementNum;
                    this.ReportParams["ТематикаОбращения"] = subjects;
                    this.ReportParams["НомерОбращения"] = string.Join(", ", appealsBaseStatement.Select(x => x.NumberGji));
                    this.ReportParams["ДатаОбращения"] = string.Join(", ", appealsBaseStatement.Select(x => x.DateFrom.ToDateTime().ToShortDateString()));
                }
            }

            if (act.Inspection.TypeBase == TypeBase.DisposalHead)
            {
                var domain = Container.ResolveDomain<BaseDispHead>();
                using (Container.Using(domain))
                {
                    var doc = domain.GetAll().FirstOrDefault(x => x.Id == act.Inspection.Id);
                    if (doc != null)
                    {
                        this.ReportParams["НомерДокумента"] = doc.DocumentNumber;
                        this.ReportParams["ДатаДокумента"] = doc.DocumentDate.HasValue ? doc.DocumentDate.Value.ToShortDateString() : string.Empty;
                    }
                }
            }

            if (act.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
            {
                var domain = Container.ResolveDomain<BaseProsClaim>();
                using (Container.Using(domain))
                {
                    var doc = domain.GetAll().FirstOrDefault(x => x.Id == act.Inspection.Id);
                    if (doc != null)
                    {
                        this.ReportParams["НомерДокумента"] = doc.DocumentNumber;
                        this.ReportParams["ДатаДокумента"] = doc.DocumentDate.HasValue ? doc.DocumentDate.Value.ToShortDateString() : string.Empty;
                    }
                }
            }

            this.ReportParams["ТипОснования"] = ((int)act.Inspection.TypeBase).ToString();
            this.ReportParams["Контрагент"] = act.Inspection.Return(x => x.Contragent).Return(x => x.Name);
        }
    }
}