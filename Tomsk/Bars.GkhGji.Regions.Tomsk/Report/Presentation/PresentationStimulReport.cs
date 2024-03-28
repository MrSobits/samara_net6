namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4;
    using Bars.B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.GkhGji.Report;
    using Entities;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using Slepov.Russian.Morpher;
    using Utils;

    public class PresentationStimulReport : GjiBaseStimulReport
    {
        public PresentationStimulReport() : base(new ReportTemplateBinary(Properties.Resources.Presentation))
        {
        }

        protected long PresentationId;

        public IDomainService<Presentation> PresentationDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRobjectDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArticleDomain { get; set; }

        public IDomainService<ProtocolDescription> ProtocolDescriptionDomain { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            PresentationId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var presentation = PresentationDomain.Get(PresentationId);

            if (presentation == null)
            {
                return;
            }

            FillCommonFields(presentation);

            this.ReportParams["АдресПравонарушения"] = InspectionRobjectDomain.GetAll()
                .Where(x => x.Inspection.Id == presentation.Inspection.Id)
                .Select(x => x.RealityObject.FiasAddress.AddressName)
                .Distinct()
                .ToList()
                .AggregateWithSeparator("; ");

            this.ReportParams["ДатаПредставления"] = presentation.DocumentDate.ToDateString("«d» MMMM y");
            this.ReportParams["НомерПредставления"] = presentation.DocumentNumber;

            var parentAdminCase = GetParentDocument(presentation, TypeDocumentGji.AdministrativeCase);

            if (parentAdminCase != null)
            {
                this.ReportParams["НомерОснования"] = parentAdminCase.DocumentNumber;
            }

            var parentProtocol = GetParentDocument(presentation, TypeDocumentGji.Protocol);

            if (parentProtocol != null)
            {
                var protocolArticles = ProtocolArticleDomain.GetAll()
                    .Where(x => x.Protocol.Id == parentProtocol.Id)
                    .Select(x => new
                    {
                        x.ArticleLaw.Article,
                        x.ArticleLaw.Part,
                        x.ArticleLaw.Description
                    })
                    .ToArray();

                this.ReportParams["СтатьяЗакона"] = protocolArticles
                    .Where(x => !x.Article.IsEmpty())
                    .Select(x =>
                        x.Part.IsEmpty()
                            ? "ст.{0}".FormatUsing(x.Article)
                            : "ч.{0} ст.{1}".FormatUsing(x.Part, x.Article))
                    .AggregateWithSeparator(", ");

                var decliner = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

                this.ReportParams["ОписаниеСтатьи"] = protocolArticles
                    .Where(x => !x.Description.IsEmpty())
                    .Select(x =>
                    {
                        try
                        {
                            var splits = x.Description.Split(' ');

                            var all = decliner.Проанализировать(splits[0]);

                            var result = new StringBuilder(all.Родительный.ToLower());

                            for (int i = 1; i < splits.Length; i++)
                            {
                                result.Append(" ");
                                result.Append(splits[i].ToLower());
                            }

                            return result.ToString();
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    })
                    .AggregateWithSeparator(", ");

                var protocolDescription = ProtocolDescriptionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == parentProtocol.Id);

                if (protocolDescription != null)
                {
                    this.ReportParams["Установил"] = protocolDescription.DescriptionSet.Encode();
                    this.ReportParams["УстановилПодробнее"] = protocolDescription.Description.Encode();
                }
            }

            this.ReportParams["ФИОФизЛицаСокр"] = CutFio(presentation.PhysicalPerson);

            if (presentation.Official != null)
            {
                this.ReportParams["Должность"] = presentation.Official.Position;
                this.ReportParams["Инспектор"] = presentation.Official.Fio;
                this.ReportParams["ИнспекторСокр"] = presentation.Official.ShortFio ?? CutFio(presentation.Official.Fio);
            }

            this.ReportParams["ДолжностьФЛ"] = presentation.ExecutantPost;
            this.ReportParams["Требование"] = presentation.DescriptionSet;
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "Presentation_Stimul",
                    Name = "Presentation",
                    Description = "Представление",
                    Template = Properties.Resources.Presentation
                }
            };
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Name
        {
            get { return "Представление"; }
        }

        public override string Description
        {
            get { return "Представление"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "PresentationStimulReport"; }
        }

        public override string CodeForm
        {
            get { return "Presentation"; }
        }
    }
}