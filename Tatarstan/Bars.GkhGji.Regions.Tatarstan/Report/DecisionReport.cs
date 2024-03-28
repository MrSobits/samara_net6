namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    using Fasterflect;

    public class DecisionReport: GkhBaseStimulReport
    {
        public DecisionReport()
            : base(new ReportTemplateBinary(Resources.DecisionDocumentary))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate { get; set; }

        protected long DocumentId { get; set; }

        public override string Extention => "mrt";

        public override string Id => "Decision";

        public override string CodeForm => "Decision";

        public override string Name => "Решение";

        public override string Description => "Решение о проведении проверки";


        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Решение о проведении документарной проверки",
                    Code = "DecisionDocumentary",
                    Description = "Решение о проведении документарной проверки",
                    Template = Resources.DecisionDocumentary
                },
                new TemplateInfo
                {
                    Name = "Решение о проведении выездной проверки",
                    Code = "DecisionExit",
                    Description = "Решение о проведении выездной проверки",
                    Template = Resources.DecisionExit
                },
                new TemplateInfo
                {
                    Name = "Решение о проведении инспекционного визита",
                    Code = "DecisionInspectionVisit",
                    Description = "Решение о проведении инспекционного визита",
                    Template = Resources.DecisionInspectionVisit
                }
            };
        }

        public override Stream GetTemplate()
        {
            this.ChangeCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["documentId"] = this.DocumentId.ToString();
        }

        private void ChangeCodeTemplate()
        {
            var documentDomain = this.Container.Resolve<IDomainService<DocumentGji>>();
            InspectionGji inspection;

            using (this.Container.Using(documentDomain))
            {
                inspection = documentDomain.Get(this.DocumentId)?.Inspection;
            }

            if (inspection == null)
            {
                throw new ReportProviderException("Не удалось получить Основание проверки");
            }

            var inspectionType = this.GetInspectionType(inspection.TypeBase);
            if (inspectionType.IsNull())
            {
                throw new ReportProviderException("Не удалось получить Основание проверки");
            }

            var domainServiceType = typeof(IDomainService<>).MakeGenericType(inspectionType);
            var domainService = (IDomainService)this.Container.Resolve(domainServiceType);

            using (this.Container.Using(domainService))
            {
                var entity = domainService.Get(inspection.Id);
                if (entity.TryGetPropertyValue("TypeForm") is TypeFormInspection typeForm)
                {
                    this.CodeTemplate = this.GetCodeTemplate(typeForm);
                }
            }
        }

        private string GetCodeTemplate(TypeFormInspection typeForm)
        {
            switch (typeForm)
            {
                case TypeFormInspection.Exit:
                    return "DecisionExit";
                case TypeFormInspection.Documentary:
                    return "DecisionDocumentary";
                case TypeFormInspection.InspectionVisit:
                    return "DecisionInspectionVisit";
                default: return string.Empty;
            }
        }

        private Type GetInspectionType(TypeBase typeBase)
        {
            switch (typeBase)
            {
                case TypeBase.DisposalHead:
                    return typeof(BaseDispHead);
                case TypeBase.PlanJuridicalPerson:
                    return typeof(BaseJurPerson);
                case TypeBase.LicenseApplicants:
                    return typeof(BaseLicenseApplicants);
                case TypeBase.ProsecutorsClaim:
                    return typeof(BaseProsClaim);
                case TypeBase.CitizenStatement:
                    return typeof(BaseStatement);
                case TypeBase.InspectionActionIsolated:
                    return typeof(InspectionActionIsolated);
                default: return null;
            }
        }
    }
}
