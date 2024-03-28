namespace Bars.Gkh.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using System.Collections.Generic;

    public class PersonAnswerRequest : GkhBaseStimulReport
    {
        private long requestId;

        public IDomainService<PersonRequestToExam> RequestDomain { get; set; }

        public IDbConfigProvider dbConfigProvider { get; set; }

        public PersonAnswerRequest() : base(new ReportTemplateBinary(Resources.PersonAnswerRequest))
        {
        }

        public override string Permission
        {
            get { return "Reports.GKH.PersonAnswerRequest"; }
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
            this.ReportParams["ДатаЗапроса"] = request.RequestDate;
        }

        public override string Id
        {
            get { return "PersonAnswerRequest"; }
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Ответ на запрос ЛК"; }
        }

        public override string Description
        {
            get { return "Ответ на запрос ЛК"; }
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
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "PersonAnswerRequest_1",
                    Description = "Ответ на запрос ЛК",
                    Name = "Ответ на запрос ЛК",
                    Template = Resources.PersonAnswerRequest
                }
            };
        }
    }
}