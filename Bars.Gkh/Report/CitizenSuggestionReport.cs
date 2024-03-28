namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.Enums;

    public class CitizenSuggestionReport : GkhBaseReport
    {
        private int CitizenSuggestionId { get; set; }

        protected override string CodeTemplate { get; set; }

        public CitizenSuggestionReport()
            : base(new ReportTemplateBinary(Properties.Resources.CitizenSuggestion))
        {
        }

        public override string Id
        {
            get { return "CitizenSuggestion"; }
        }

        public override string CodeForm
        {
            get { return "CitizenSuggestion"; }
        }

        public override string Name
        {
            get { return "Обращения граждан"; }
        }

        public override string Description
        {
            get { return "Обращения граждан"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.CitizenSuggestionId = userParamsValues.GetValue<object>("CitizenSuggestionId").ToInt();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "CitizenSuggestion",
                            Name = "CitizenSuggestion",
                            Description = "Обращения граждан",
                            Template = Properties.Resources.CitizenSuggestion
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var suggestion = Container.ResolveDomain<CitizenSuggestion>().GetAll()
                .Where(x => x.Id == this.CitizenSuggestionId)
                .Select(x => new
                    {
                        x.ApplicantFio,
                        x.ApplicantAddress,
                        x.ApplicantPhone,
                        x.ApplicantEmail,
                        x.Number,
                        x.CreationDate,
                        ProblemPlace = x.ProblemPlace.Name,
                        x.Description,
                        x.AnswerText,
                        Executor = x.Rubric.FirstExecutorType == ExecutorType.Mo ? x.ExecutorManagingOrganization.Contragent.Name :
                                x.Rubric.FirstExecutorType == ExecutorType.Mu ? x.ExecutorMunicipality.Name :
                                x.Rubric.FirstExecutorType == ExecutorType.Gji ? x.ExecutorZonalInspection.ZoneName :
                                x.Rubric.FirstExecutorType == ExecutorType.CrFund 
                                    ? string.Format("{0}, {1}", x.ExecutorCrFund.FullName, x.ExecutorCrFund.Position.Name) 
                                    : string.Empty
                    })
                .FirstOrDefault();

            if (suggestion != null)
            {
                reportParams.SimpleReportParams["Заявитель"] = suggestion.ApplicantFio;
                reportParams.SimpleReportParams["ПочтовыйАдресЗаявителя"] = suggestion.ApplicantAddress;
                reportParams.SimpleReportParams["Контактный телефон"] = suggestion.ApplicantPhone;
                reportParams.SimpleReportParams["E-mail"] = suggestion.ApplicantEmail;
                reportParams.SimpleReportParams["НомерОбращения"] = suggestion.Number;
                reportParams.SimpleReportParams["ДатаОбращения"] = suggestion.CreationDate.ToString("dd.MM.yyyy");
                reportParams.SimpleReportParams["МестоПроблемы"] = suggestion.ProblemPlace;
                reportParams.SimpleReportParams["ОписаниеПроблемы"] = suggestion.Description;
                reportParams.SimpleReportParams["Исполнитель"] = suggestion.Executor;
                reportParams.SimpleReportParams["Ответ"] = suggestion.AnswerText;
            }
        }
    }
}