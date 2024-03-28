namespace Bars.Gkh.DomainService
{
    using System;
    using System.IO;
    using System.Linq;

    using B4;

    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.Gkh.Entities.Suggestion;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для печати отчетов обращений граждан
    /// </summary>
    public class CitizenSuggestionReportService : ICitizenSuggestionReportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Напечатать Обращение граждан с (портала)
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public Stream PrintSuggestionPortalReport(BaseParams baseParams)
        {
            var citizenSuggestionId = baseParams.Params.GetAs<long>("citizenSuggestionId");
            var citizenSuggestionDomain = this.Container.Resolve<IDomainService<CitizenSuggestion>>();
            var citizenSuggestionFilesDomain = this.Container.Resolve<IDomainService<CitizenSuggestionFiles>>();

            try
            {
                var citizenSuggestion = citizenSuggestionDomain.Get(citizenSuggestionId);
                if (citizenSuggestion == null)
                {
                    throw new Exception("Обращение не найдено");
                }

                var fileIds = citizenSuggestionFilesDomain.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == citizenSuggestionId)
                    .Select(x => x.DocumentFile.Id)
                    .ToArray();

                var reportParams = new BaseParams();
                reportParams.Params.Add("ДатаОбращения", citizenSuggestion.CreationDate.ToShortDateString());
                reportParams.Params.Add("НомерОбращения", citizenSuggestion.Number);
                reportParams.Params.Add("ФИО", citizenSuggestion.ApplicantFio);
                reportParams.Params.Add("Рубрика", citizenSuggestion.Rubric?.Name);
                reportParams.Params.Add("Адрес", citizenSuggestion.RealityObject?.Address);
                reportParams.Params.Add("ПочтовыйАдрес", citizenSuggestion.ApplicantAddress);
                reportParams.Params.Add("Телефон", citizenSuggestion.ApplicantPhone);
                reportParams.Params.Add("Email", citizenSuggestion.ApplicantEmail);
                reportParams.Params.Add("ОписаниеПроблемы", citizenSuggestion.Description);
                reportParams.Params.Add("ВложенныеФайлы", string.Join(",", fileIds));

                var report = this.Container.Resolve<ICodedReport>("CitizenSuggestionPortalReport");
                if (report != null)
                {
                    var generator = this.Container.Resolve<ICodedReportManager>();
                    using (this.Container.Using(report, generator))
                    {
                        var reportStream = generator.GenerateReport(report, reportParams, ReportPrintFormat.docx);
                        reportStream.Seek(0, SeekOrigin.Begin);

                        return reportStream;
                    }
                }
            }
            finally
            {
                this.Container.Release(citizenSuggestionDomain);
                this.Container.Release(citizenSuggestionFilesDomain);
            }

            throw new Exception("Отчет не найден");
        }
    }
}