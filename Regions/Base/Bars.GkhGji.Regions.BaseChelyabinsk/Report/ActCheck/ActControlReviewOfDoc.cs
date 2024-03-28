namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActCheck
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;

    public class ActControlReviewOfDoc : GjiBaseReport
    {
        private long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ActControlReviewOfDoc()
            : base(new ReportTemplateBinary(Resources.ActMonitoringSurveyOfHousing))
        {
        }

        public override string ReportGeneratorName
        {
            get { return "DocIoGenerator"; }
        }

        public override string Id
        {
            get { return "ActControlReviewOfDoc"; }
        }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override string Name
        {
            get { return "Акт (рассмотр. док.)"; }
        }

        public override string Description
        {
            get { return "Акт (контроль по рассмотрению документов)"; }
        }

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
                                   Code = "ActMonitoringSurveyOfHousing",
                                   Name = "ActCheck",
                                   Description = "Акт (контроль по обследованию ЖФ)",
                                   Template = Resources.ActMonitoringSurveyOfHousing
                               }
                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.CodeTemplate = "ActMonitoringSurveyOfHousing";
        }
    }
}