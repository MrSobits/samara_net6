namespace Bars.GkhGji.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Entities;
    using Gkh.Report;
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class NotificationAttendanceAtProtocolReport : GkhBaseStimulReport
    {
        private long requestId;

        public IDomainService<ActCheck> RequestDomain { get; set; }

        public IDbConfigProvider dbConfigProvider { get; set; }

        public NotificationAttendanceAtProtocolReport() : base(new ReportTemplateBinary(Properties.Resources.NotificationAttendanceAtProtocol))
        {
        }

        public override string Permission
        {
            get { return "Reports.GJI.NotificationAttendanceAtProtocolReport"; }
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var request = this.RequestDomain.Get(this.requestId);

            this.ReportParams["ИдентификаторДокументаГЖИ"] = request.Id.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = this.dbConfigProvider.ConnectionString;
        }

        public override string Id
        {
            get { return "NotificationAttendanceAtProtocolReport"; }
        }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override string Name
        {
            get { return "Уведомление о явке на протокол"; }
        }

        public override string Description
        {
            get { return "Уведомление о явке представителя на протокол"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NotificationAttendanceAtProtocolReport",
                    Description = "Уведомление о явке представителя на протокол",
                    Name = "Уведомление о явке на протокол",
                    Template = Properties.Resources.NotificationAttendanceAtProtocol
                }
            };
        }
    }
}