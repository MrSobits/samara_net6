namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Properties;
    using Bars.GkhGji.Report;
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class WarningDocGjiReport : GjiBaseStimulReport
    {
        public WarningDocGjiReport()
            : base(new ReportTemplateBinary(Resources.WarningDoc))
        {
            this.ExportFormat = StiExportFormat.Word2007;
        }

        private long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public override string Id => "WarningDoc";

        public override string CodeForm => "WarningDoc";

        public override string Name => "Предостережение";

        public override string Description => "Предостережение";

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
            var valTypeBase = userParamsValues.GetValue<object>("TypeBase").ToInt();
            this.CodeTemplate = this.GetCodeTemplate((TypeBase)valTypeBase);
        }

        public override List<TemplateInfo> GetTemplateInfo() =>
            new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "CautionKnmWithoutInteraction",
                    Code = "CautionKnmWithoutInteraction",
                    Description = "Предостережение о недопустимости нарушения обязательных требований по КНМ без взаимодействия",
                    Template = Resources.CautionKnmWithoutInteraction
                },
                new TemplateInfo
                {
                    Name = "VerificationCaution",
                    Code = "VerificationCaution",
                    Description = "Предостережение о недопустимости нарушения обязательных требований по проверкам",
                    Template = Resources.VerificationCaution
                },
                new TemplateInfo
                {
                    Name = "CitizenStatementVerificationCaution",
                    Code = "CitizenStatementVerificationCaution",
                    Description = "Предостережение о недопустимости нарушения обязательных требований по обращению гражданина",
                    Template = Resources.EmptyReport
                }
            };

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }

        private string GetCodeTemplate(TypeBase value)
        {
            switch (value)
            {
                case TypeBase.ActionIsolated:
                    return "CautionKnmWithoutInteraction";
                case TypeBase.PlanJuridicalPerson:
                case TypeBase.ProsecutorsClaim:
                case TypeBase.DisposalHead:
                case TypeBase.CitizenStatement:
                case TypeBase.InspectionActionIsolated:
                    return "VerificationCaution";
                case TypeBase.GjiWarning:
                    return "CitizenStatementVerificationCaution";
                default: return "";
            }
        }
    }
}