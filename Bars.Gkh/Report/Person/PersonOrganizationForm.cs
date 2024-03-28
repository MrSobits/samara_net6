using Bars.Gkh.Utils;
 using Bars.B4.Modules.Analytics.Reports.Enums;

namespace Bars.Gkh.Report
{
    using Bars.B4;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using B4.Modules.Reports;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Slepov.Russian.Morpher;
    using Bars.B4.Utils;

    class PersonOrganizationForm : GkhBaseStimulReport
    {

        #region Fields
        private long requestId;

        protected Person Person { get; set; }
        
        protected PersonRequestToExam PersonRequestToExam { get; set; }
        #endregion

        public PersonOrganizationForm()
            : base(new ReportTemplateBinary(Properties.Resources.PersonOrganizationForm))
        {
        }
        public IDomainService<QualifyTestSettings> QualifyTestSettingsDomain { get; set; }
        public IDomainService<QExamQuestion> ExamQuestionDomain { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("PersonId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "PersonProtocolResult_1",
                        Name = "PersonProtocolResult",
                        Description =
                            "Аттестационный лист",
                        Template = Properties.Resources.PersonRequestToExamReport
                    }
            };
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Аттестационный лист"; }
        }

        public override string Description
        {
            get { return "Аттестационный лист"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "PersonProtocolResult"; }
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
            set { }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var endExamTime = ExamQuestionDomain.GetAll()
                            .Where(x => x.PersonRequestToExam.Id == this.requestId)
                            .OrderByDescending(x => x.ObjectEditDate).FirstOrDefault();

            double questionCount = ExamQuestionDomain.GetAll()
                                .Where(x => x.PersonRequestToExam.Id == requestId)
                                .Count();

            var qExamSetings = QualifyTestSettingsDomain.GetAll()
                    .Where(x => x.DateFrom <= DateTime.Now)
                    .Where(x => !x.DateTo.HasValue || x.DateTo.Value > DateTime.Now)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

            TimeSpan examTime = endExamTime.ObjectEditDate - endExamTime.ObjectCreateDate;

            this.ReportParams["ОценкаВбаллах"] = PersonRequestToExam.CorrectAnswersPercent;
            this.ReportParams["ОценкаВпроцентах"] = Math.Round(Convert.ToDouble(PersonRequestToExam.CorrectAnswersPercent) / (questionCount * 2) * 100, 2);
            this.ReportParams["Фамилия"] = Person.Surname;
            this.ReportParams["Имя"] = Person.Name;
            this.ReportParams["Отчество"] = Person.Patronymic;
            this.ReportParams["ДатаРождения"] = Person.Birthdate;
            this.ReportParams["ДатаТестирования"] = PersonRequestToExam.ExamDate;
            this.ReportParams["ВремяТестирования"] = PersonRequestToExam.ExamTime;
            this.ReportParams["Статус"] = qExamSetings.AcceptebleRate >= PersonRequestToExam.CorrectAnswersPercent ? "Сдан" : "Не сдан";
        }
    }
}
