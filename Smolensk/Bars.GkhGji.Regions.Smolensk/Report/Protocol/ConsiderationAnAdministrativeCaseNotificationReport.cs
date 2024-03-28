namespace Bars.GkhGji.Regions.Smolensk.Report.ProtocolGji
{
    using System.Collections.Generic;

    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Entities.Protocol;
    using Gkh.Report;

    using System.IO;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary> Уведомление о рассмотрении протокола </summary>
    public class ConsiderationAnAdministrativeCaseNotificationReport : GjiBaseStimulReport
    {
        #region .ctor

        public ConsiderationAnAdministrativeCaseNotificationReport()
            : base(new ReportTemplateBinary(Properties.Resources.ConsiderationAnAdministrativeCaseNotificationReport))
        {

        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ConsiderationAnAdministrativeCaseNotification"; }
        }

        public override string Name
        {
            get { return "Уведомление на рассмотрение дела"; }
        }

        public override string Description
        {
            get { return "Уведомление на рассмотрение дела"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DocumentId;

        protected ProtocolSmol Protocol;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var protocolDomain = Container.ResolveDomain<ProtocolSmol>();

            using (Container.Using(protocolDomain))
            {
                Protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ConsiderationAnAdministrativeCaseNotification",
                    Name = "ConsiderationAnAdministrativeCaseNotificationReport",
                    Description = "Уведомление на рассмотрение дела об административном правонарушении",
                    Template = Properties.Resources.ConsiderationAnAdministrativeCaseNotificationReport
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "ConsiderationAnAdministrativeCaseNotification";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (Protocol == null)
            {
                return;
            }
            FillCommonFields(Protocol);

            this.ReportParams["УведомлениеДата"] = Protocol.NoticeDocDate.HasValue
                ? Protocol.NoticeDocDate.Value.ToShortDateString() : string.Empty;
            this.ReportParams["УведомлениеНомер"] = Protocol.Return(x => x.NoticeDocNumber, string.Empty);
            this.ReportParams["ЮрЛицо"] = Protocol.Return(x => x.Contragent).Return(x => x.Name, string.Empty);
            this.ReportParams["ЮрЛицоСокр"] = Protocol.Return(x => x.Contragent).Return(x => x.ShortName, string.Empty);
            this.ReportParams["АдресЮл"] = Protocol.Return(x => x.Contragent).Return(x => x.JuridicalAddress, string.Empty);
            this.ReportParams["ТелефонЮл"] = Protocol.Return(x => x.Contragent).Return(x => x.Phone, string.Empty);
            this.ReportParams["ТипИсполнителя"] = Protocol.Return(x => x.Executant).Return(x => x.Code);
        }
    }
}
