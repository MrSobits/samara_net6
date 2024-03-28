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

    public class ResolutionGjiDefinitionReport : GjiBaseReport
    {
        private long DefinitionId { get; set; }

        private ResolutionDefinition definition;
        public ResolutionDefinition Definition
        {
            get
            {
                return definition;
            }
            set
            {
                definition = value;
            }
        }

        protected override string CodeTemplate { get; set; }

        public ResolutionGjiDefinitionReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Definition_5))
        {
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Name = "Resolution_Definition_5",
                            Code = "BlockGJI_Definition_5",
                            Description = "о предоставлении (отказе в предоставлении) отсрочки (рассрочки) исполнения",
                            Template = Properties.Resources.BlockGJI_Definition_5
                        }
                };
        }

        public override string Id
        {
            get { return "ResolutionDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDefinition"; }
        }

        public override string Name
        {
            get { return "Определение постановления"; }
        }

        public override string Description
        {
            get { return "Определение постановления"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Definition = Container.Resolve<IDomainService<ResolutionDefinition>>().Get(DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            switch (definition.TypeDefinition)
            {
                case TypeDefinitionResolution.Deferment:
                    CodeTemplate = "BlockGJI_Definition_5";
                    break;
            }

            var resolution = definition.Resolution;

            // Заполняем общие поля
            FillCommonFields(reportParams, resolution);

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

            reportParams.SimpleReportParams["ДатаПостановления"] = resolution.DocumentDate.HasValue
                                                                   ? resolution.DocumentDate.Value.ToShortDateString()
                                                                   : string.Empty;

            reportParams.SimpleReportParams["НомерПостановления"] = resolution.DocumentNumber;

            reportParams.SimpleReportParams["Правонарушитель"] = resolution.Contragent != null
                                                                     ? resolution.Contragent.Name
                                                                     : resolution.PhysicalPerson;

            var dispId = base.GetParentDocument(resolution, TypeDocumentGji.Disposal);

            if (dispId != null)
            {
                var disposal = Container.Resolve<IDomainService<Disposal>>().Load(dispId.Id);
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
            if (resolution.Executant != null)
            {
                var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
                var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5", "3" };
                var listTypePhysicalPerson = new List<string> { "6", "7", "14" };

                var contr1 = string.Empty;
                var withRespect = string.Empty;
                var issue = string.Empty;
                var contragentName = resolution.Contragent != null ? resolution.Contragent.Name : string.Empty;
                var physicalPerson = resolution.PhysicalPerson;
                if (listTypeContragent.Contains(resolution.Executant.Code))
                {
                    contr1 = contragentName;
                    withRespect = contragentName;
                }
                if (listTypeContrPhysicalPerson.Contains(resolution.Executant.Code))
                {
                    contr1 = contragentName + ", " + physicalPerson;
                    withRespect = contragentName + " - " + physicalPerson;
                    issue = physicalPerson;
                }
                if (listTypePhysicalPerson.Contains(resolution.Executant.Code))
                {
                    contr1 = physicalPerson;
                    withRespect = physicalPerson;
                    issue = physicalPerson;
                }

                reportParams.SimpleReportParams["Контрагент2"] = contr1;
                reportParams.SimpleReportParams["Вотношении"] = withRespect;
                reportParams.SimpleReportParams["Вручено"] = issue;
            }

            reportParams.SimpleReportParams["Контрагент"] = resolution.Contragent != null
                                                                ? resolution.Contragent.Name
                                                                : string.Empty;

            var actDoc = GetParentDocument(resolution, TypeDocumentGji.ActCheck);

            if (actDoc != null)
            {
                reportParams.SimpleReportParams["ДатаАкта"] = actDoc.DocumentDate != null
                                                                  ? actDoc.DocumentDate.ToDateTime().ToShortDateString()
                                                                  : string.Empty;

                reportParams.SimpleReportParams["НомерАкта"] = actDoc.DocumentNumber;
            }
        }
    }
}