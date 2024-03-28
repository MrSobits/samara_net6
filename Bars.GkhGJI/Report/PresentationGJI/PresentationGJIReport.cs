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
    using Bars.GkhGji.Enums;

    public class PresentationGjiReport : GjiBaseReport
    {
        private long DocumentId { get; set; }
        protected override string CodeTemplate { get; set; }

        public PresentationGjiReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Presentation))
        {
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Name = "Presentation",
                            Code = "BlockGJI_Presentation",
                            Description = "Представление",
                            Template = Properties.Resources.BlockGJI_Presentation
                        }
                };
        }

        public override string Id
        {
            get { return "Presentation"; }
        }

        public override string CodeForm
        {
            get { return "Presentation"; }
        }

        public override string Name
        {
            get { return "Представление"; }
        }

        public override string Description
        {
            get { return "Представление"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // самый крутой печатный бланк
            var presentation = Container.Resolve<IDomainService<Presentation>>().Load(DocumentId);

            if (presentation == null)
            {
                throw new ReportProviderException("Не удалось получить представление");
            }

            FillCommonFields(reportParams, presentation);

            CodeTemplate = "BlockGJI_Presentation";

            reportParams.SimpleReportParams["Name"] = "Presentation";

            var dataDoc = presentation.DocumentDate.HasValue
                              ? presentation.DocumentDate.Value.ToShortDateString()
                              : string.Empty;

            reportParams.SimpleReportParams["НомерДокумента"] = presentation.DocumentNumber;
            reportParams.SimpleReportParams["ДатаДокумента"] = dataDoc;
            reportParams.SimpleReportParams["НомерПредставления"] = presentation.DocumentNumber;
            reportParams.SimpleReportParams["ДатаПредставления"] = dataDoc;

            if (presentation.Official != null)
            {
                reportParams.SimpleReportParams["ДолжностьДЛПредставление"] = presentation.Official.Position;
                reportParams.SimpleReportParams["ФИОДЛПредставление"] = presentation.Official.Fio;
                reportParams.SimpleReportParams["ФИОДЛПредставлениеСокр"] = presentation.Official.ShortFio;
            }

            reportParams.SimpleReportParams["ЮрАдресКонтрагента"] = reportParams.SimpleReportParams["АдресКонтрагента"];

            var dispDoc = GetParentDocument(presentation, TypeDocumentGji.Disposal);

            if (dispDoc != null)
            {
                var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                                     .Where(x => x.DocumentGji.Id == dispDoc.Id)
                                                     .Select(x => x.Inspector.Id);

                var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                    .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                    .Select(x => x.ZonalInspection.Locality)
                                    .Distinct()
                                    .ToList();

                reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
            }

            if (presentation.Executant != null)
            {
                var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "2" };
                var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5", "3" };
                var listTypePhysicalPerson = new List<string> { "6", "7", "14" };

                var contr = string.Empty;
                var contragentName = presentation.Contragent != null ? presentation.Contragent.Name : string.Empty;
                var physicalPerson = presentation.PhysicalPerson;
                if (listTypeContragent.Contains(presentation.Executant.Code))
                {
                    contr = contragentName;
                }
                if (listTypeContrPhysicalPerson.Contains(presentation.Executant.Code))
                {
                    contr = contragentName + ", " + physicalPerson;
                }
                if (listTypePhysicalPerson.Contains(presentation.Executant.Code))
                {
                    contr = physicalPerson;
                }

                reportParams.SimpleReportParams["Контрагент3"] = contr;
            }
        }
    }
}
