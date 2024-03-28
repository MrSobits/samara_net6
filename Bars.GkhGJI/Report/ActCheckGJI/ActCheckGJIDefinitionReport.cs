using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;

    public class ActCheckGjiDefinitionReport : GjiBaseReport
    {
        private long DefinitionId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ActCheckGjiDefinitionReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Definition_2))
        {
        }

        public override string Id
        {
            get { return "ActCheckDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ActCheckDefinition"; }
        }

        public override string Name
        {
            get { return "Определение акта проверки"; }
        }

        public override string Description
        {
            get { return "Определение акта проверки"; }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Name = "ActCheck_Definition_2",
                            Code = "BlockGJI_Definition_2",
                            Description = "Тип определения - О доставлении лица для составления протокола",
                            Template = Properties.Resources.BlockGJI_Definition_2
                        },
                    new TemplateInfo
                        {
                            Name = "ActCheck_Definition_7",
                            Code = "BlockGJI_Definition_7",
                            Description = "Тип определения - Об отказе в возбуждении дела",
                            Template = Properties.Resources.BlockGJI_Definition_7
                        },
                };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var definition = Container.Resolve<IDomainService<ActCheckDefinition>>().Load(DefinitionId);

            if (definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            switch (definition.TypeDefinition)
            {
                case TypeDefinitionAct.RefusingProsecute:
                    CodeTemplate = "BlockGJI_Definition_7";
                    break;
                case TypeDefinitionAct.TransportationToProtocol:
                    CodeTemplate = "BlockGJI_Definition_2";
                    break;
            }

            // Заполняем общие поля
            FillCommonFields(reportParams, definition.ActCheck);

            reportParams.SimpleReportParams["НомерОпределения"] = definition.DocumentNum;
            reportParams.SimpleReportParams["ДатаОпределения"] = definition.DocumentDate.HasValue
                                                                     ? definition.DocumentDate.Value.ToShortDateString()
                                                                     : string.Empty;

            if (definition.IssuedDefinition != null)
            {
                reportParams.SimpleReportParams["Руководитель"] = definition.IssuedDefinition.Fio;
                reportParams.SimpleReportParams["РуководительФИО"] = definition.IssuedDefinition.ShortFio;
                reportParams.SimpleReportParams["ДолжностьРуководителя"] = definition.IssuedDefinition.Position;
                reportParams.SimpleReportParams["КодРуководителяФИО(сИнициалами)"] =
                    definition.IssuedDefinition.Position + " - " + definition.IssuedDefinition.ShortFio;
            }

            var act = definition.ActCheck;

            reportParams.SimpleReportParams["ДатаАкта"] = act.DocumentDate.HasValue
                                                                   ? act.DocumentDate.Value.ToString("dd MMMM yyyy")
                                                                   : string.Empty;

            reportParams.SimpleReportParams["НомерАкта"] = act.DocumentNumber;

            var inspector = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                          .Where(x => x.DocumentGji.Id == act.Id)
                                          .Select(x => new { x.Inspector.Fio, x.Inspector.Phone })
                                          .FirstOrDefault();

            reportParams.SimpleReportParams["Инспектор"] = inspector != null ? inspector.Fio : string.Empty;
            reportParams.SimpleReportParams["ТелефонИнспектора"] = inspector != null ? inspector.Phone : string.Empty;

            var realtyObjects = Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .Select(x => new
                {
                    x.RealityObject.FiasAddress.House,
                    x.RealityObject.FiasAddress.PlaceName,
                    x.RealityObject.FiasAddress.StreetName
                })
                .ToArray();

            if (realtyObjects.Length == 1)
            {
                var realObj = realtyObjects.First();
                reportParams.SimpleReportParams["НомерДома"] = realObj.House;
                reportParams.SimpleReportParams["НаселенныйПункт"] = realObj.PlaceName;
                reportParams.SimpleReportParams["Адрес"] = string.Format("{0}, {1}", realObj.PlaceName, realObj.StreetName);
            }

            var disposal = Container.Resolve<IDomainService<Disposal>>().Get(GetParentDocument(act, TypeDocumentGji.Disposal).Id);

            if (disposal != null)
            {
                reportParams.SimpleReportParams["КодРуководителя"] = disposal.IssuedDisposal != null
                                                   ? disposal.IssuedDisposal.Code
                                                   : string.Empty;

                reportParams.SimpleReportParams["КодРуководителяФИО(сИнициалами)"] = disposal.IssuedDisposal != null
                    ? string.Format(
                        "{0} {1}",
                        disposal.IssuedDisposal.Position,
                        string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio)
                            ? disposal.IssuedDisposal.Fio
                            : disposal.IssuedDisposal.ShortFio)
                    : string.Empty;

                var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                                     .Where(x => x.DocumentGji.Id == disposal.Id)
                                                     .Select(x => x.Inspector.Id);

                var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                    .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                    .Select(x => x.ZonalInspection.Locality)
                                    .Distinct()
                                    .ToList();

                reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
            }

            var prescriptionDoc = GetChildDocument(act, TypeDocumentGji.Prescription);

            if (prescriptionDoc != null)
            {
                var prescription = Container.Resolve<IDomainService<Prescription>>()
                             .GetAll().FirstOrDefault(x => x.Id == prescriptionDoc.Id);

                if (prescription != null)
                {
                    reportParams.SimpleReportParams["Правонарушитель"] = prescription.Contragent != null
                                                                             ? prescription.Contragent.Name
                                                                             : prescription.PhysicalPerson;
                }
            }

            if (act.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var appealsBaseStatementQuery = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == act.Inspection.Id);

                var appealsBaseStatementIdsQuery = appealsBaseStatementQuery.Select(x => x.AppealCits.Id);

                var subjects = string.Join(
                    ", ",
                    Container.Resolve<IDomainService<AppealCitsStatSubject>>().GetAll()
                        .Where(x => appealsBaseStatementIdsQuery.Contains(x.AppealCits.Id))
                        .Select(x => x.Subject.Name)
                        .AsEnumerable()
                        .Distinct()
                        .Where(x => !string.IsNullOrWhiteSpace(x)));

                var appealsBaseStatement = appealsBaseStatementQuery
                    .Select(x => new
                    {
                        x.AppealCits.NumberGji,
                        x.AppealCits.DateFrom
                    })
                    .ToList();

                var appealsBaseStatementFio = string.Join(
                    ", ",
                    appealsBaseStatementQuery
                        .Select(x => x.AppealCits.Correspondent)
                        .AsEnumerable()
                        .Where(x => !string.IsNullOrWhiteSpace(x)));

                var appealsBaseStatementNum = string.Join(
                    ", ",
                    appealsBaseStatement
                        .Select(y => string.Format("{0} от {1}", y.NumberGji, y.DateFrom.ToDateTime().ToShortDateString()))
                        .Where(x => !string.IsNullOrWhiteSpace(x)));

                reportParams.SimpleReportParams["ФИООбр"] = appealsBaseStatementFio;
                reportParams.SimpleReportParams["Обращения"] = appealsBaseStatementNum;
                reportParams.SimpleReportParams["ТематикаОбращения"] = subjects;
            }

            reportParams.SimpleReportParams["Контрагент"] = act.Inspection.Return(x => x.Contragent).Return(x => x.Name);

            FillParams(reportParams, definition);
        }

        protected virtual void FillParams(ReportParams reportParams, ActCheckDefinition definition)
        {
        }
    }
}