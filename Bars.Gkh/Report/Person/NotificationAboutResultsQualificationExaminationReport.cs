namespace Bars.Gkh.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class NotificationAboutResultsQualificationExaminationReport : GkhBaseStimulReport
    {
        private long requestId;
        
        public IDomainService<PersonRequestToExam> RequestDomain { get; set; }

        public IDbConfigProvider dbConfigProvider { get; set; }

        public NotificationAboutResultsQualificationExaminationReport() : base(new ReportTemplateBinary(Resources.NotificationAboutResultsQualificationExamination))
        {
        }

        public override string Permission
        {
            get { return "Reports.GKH.NotificationAboutResultsQualificationExaminationReport"; }
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
            get { return "NotificationAboutResultsQualificationExaminationReport"; }
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Уведомление о результатах квалификационного экзамена"; }
        }

        public override string Description
        {
            get { return "Уведомление о результатах квалификационного экзамена"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("PersonId").ToLong();
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
                    Code = "NotificationAboutResultsQualificationExaminationReport",
                    Description = "Уведомление о результатах квалификационного экзамена",
                    Name = "Уведомление о результатах квалификационного экзамена",
                    Template = Resources.NotificationAboutResultsQualificationExamination
                }
            };
        }
    }
}