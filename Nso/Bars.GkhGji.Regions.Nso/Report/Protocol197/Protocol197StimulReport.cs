namespace Bars.GkhGji.Regions.Nso.Report
{
    using System.Collections.Generic;
    using System.IO;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Report;
    using GkhGji.Report;
    using Properties;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.GkhGji.Regions.Nso.Entities;

	public class Protocol197StimulReport : GjiBaseStimulReport
    {
		public Protocol197StimulReport()
            : base(new ReportTemplateBinary(Resources.NsoProtocol))
        {
        }

        #region Properties

        public override string Id
        {
            get { return "Protocol197"; }
        }

        public override string CodeForm
        {
			get { return "Protocol197"; }
        }

        public override string Name
        {
			get { return "Протокол без привязки к другим документам"; }
        }

        public override string Description
        {
			get { return "Протокол без привязки к другим документам"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected long DocumentId;

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
                    Code = "Protocol197",
                    Name = "Protocol197",
                    Description = "Протокол без привязки к другим документам",
                    Template = Resources.NsoProtocol
                }
            };
        }

        public override Stream GetTemplate()
        {
            GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
			CodeTemplate = "Protocol197";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocol = Container.ResolveDomain<Protocol197>().Get(DocumentId);
            if (protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }
            FillCommonFields(protocol);
        }
    }
}