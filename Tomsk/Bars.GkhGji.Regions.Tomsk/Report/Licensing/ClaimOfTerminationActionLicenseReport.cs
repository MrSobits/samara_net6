namespace Bars.GkhGji.Regions.Tomsk.Report.Licensing
{
	using System.Collections.Generic;
	using B4.DataAccess;
	using B4.Modules.Reports;

	using Bars.B4.Modules.Analytics.Reports.Enums;

	using Gkh.Report;
	using Properties;

	/// <summary>
	/// Заявление о прекращении действия лицензии
	/// </summary>
	public class ClaimOfTerminationActionLicenseReport : GkhBaseStimulReport
    {
        public ClaimOfTerminationActionLicenseReport()
            : base(new ReportTemplateBinary(Resources.ClaimOfTerminationActionLicenseReport))
        {
        }

        private long _licenseId;

        protected override string CodeTemplate { get; set; }

        public override string Name
        {
            get { return "Заявление о прекращении действия лицензии"; }
        }

        public override string Description
        {
            get { return "Заявление о прекращении действия лицензии"; }
        }

        public override string Id
        {
            get { return "ClaimOfTerminationActionLicenseReport"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicense"; }
        }

        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

		public override void SetUserParams(UserParamsValues userParamsValues)
		{
			_licenseId = userParamsValues.GetValue<long>("LicenseId");
		}

		public override List<TemplateInfo> GetTemplateInfo()
		{
			return new List<TemplateInfo>
			{
				new TemplateInfo
				{
					Name = "Заявление о прекращении действия лицензии",
					Description = "Заявление о прекращении действия лицензии",
					Code = "ClaimOfTerminationActionLicenseReport",
					Template = Resources.ClaimOfTerminationActionLicenseReport
				}
			};
		}

		public override void PrepareReport(ReportParams reportParams)
        {
			//отчет делается силами внедрения
			this.ReportParams["ИдентификаторДокументаГЖИ"] = _licenseId.ToString();
			this.ReportParams["СтрокаПодключениякБД"] = Container.Resolve<IDbConfigProvider>().ConnectionString;
		}
    }
}