using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Report;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Tatarstan.Properties;
using System.Collections.Generic;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class ActCheckReport : GkhBaseStimulReport
    {
        public ActCheckReport()
            : base(new ReportTemplateBinary(Resources.InspectionVisitAct))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        public override string Id => "ActCheckTat";

        public override string CodeForm => "ActCheckTat";

        public override string Name => "Акт проверки";

        public override string Description => "Акт проверки по 248 ФЗ";

        protected override string CodeTemplate { get; set; }

        protected long DocumentId { get; set; }

        public override List<TemplateInfo> GetTemplateInfo() =>
        new List<TemplateInfo>
        {
            new TemplateInfo
            {
                Name = "InspectionVisitAct",
                Code = "InspectionVisitAct",
                Description = "Акт инспекционного визита",
                Template = Resources.InspectionVisitAct
            },
            new TemplateInfo
            {
                Name = "FieldInspectionAct",
                Code = "FieldInspectionAct",
                Description = "Акт выездной проверки",
                Template = Resources.FieldInspectionAct
            },
            new TemplateInfo
            {
                Name = "DocumentaryVerificationAct",
                Code = "DocumentaryVerificationAct",
                Description = "Акт документарной проверки",
                Template = Resources.DocumentaryVerificationAct
            }
        };

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
            this.CodeTemplate = this.GetCodeTemplate();
        }

        private string GetCodeTemplate()
        {
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var documentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            using (this.Container.Using(disposalDomain, documentChildrenDomain))
            {
                var disposalId = documentChildrenDomain.GetAll()
                    .FirstOrDefault(x => x.Children.Id == this.DocumentId
                        && new[] { TypeDocumentGji.Disposal, TypeDocumentGji.Decision }.Contains(x.Parent.TypeDocumentGji))?
                    .Parent?.Id;

                if (disposalId == null)
                {
                    return "";
                }

                var typeBase = disposalDomain.Get(disposalId.Value).KindCheck.Code;

                switch (typeBase)
                {
                    case TypeCheck.PlannedExit:
                    case TypeCheck.NotPlannedExit:
                        return "FieldInspectionAct";
                    case TypeCheck.PlannedDocumentation:
                    case TypeCheck.NotPlannedDocumentation:
                        return "DocumentaryVerificationAct";
                    case TypeCheck.PlannedInspectionVisit:
                    case TypeCheck.NotPlannedInspectionVisit:
                        return "InspectionVisitAct";
                    default:
                        return "";
                }
            }
        }
    }
}
