namespace Bars.GkhGji.Regions.Tomsk.Report.DisposalGji
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;
    using GkhCalendar.Enums;
    using GkhGji.Entities.Dict;
    using GkhGji.Report;

    public class DisposalGjiStimulReport : GjiBaseStimulReport
    {
        public DisposalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Disposal))
        {
        }

        public virtual ITomskDisposalReportData TomskDisposalReportData { get; set; }

        private long DocumentId { get; set; }

        private Disposal Disposal { get; set; }

        private DisposalReportData DisposalReportData { get; set; }

        public override string Id
        {
            get { return "Disposal"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var dispText = Container.Resolve<IDisposalText>();
            try
            {
                DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

                Disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

                if (Disposal == null)
                {
                    throw new ReportProviderException(string.Format("Не удалось получить {0}", dispText.SubjectiveCase.ToLower()));
                }

                DisposalReportData = TomskDisposalReportData.GetData(Disposal);
            }
            finally 
            {
                Container.Release(disposalDomain);
                Container.Release(dispText);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "TomskDisposal",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 1 (Плановая выездная)",
                            Template = Properties.Resources.Disposal
                        }
                };
        }

        public override string Name
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        public override string Description
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        protected override string CodeTemplate { get; set; }


        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = Disposal.Id.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = Container.Resolve<IDbConfigProvider>().ConnectionString;
        }
    }
}