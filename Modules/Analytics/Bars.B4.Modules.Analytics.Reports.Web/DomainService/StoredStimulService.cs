namespace Bars.B4.Modules.Analytics.Reports.Web.DomainService
{
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Web.Controllers;
    using Bars.B4.Utils.Annotations;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для генерации хранимых стимул отчетов
    /// </summary>
    public class StoredStimulService : IStimulService
    {
        /// <summary>
        /// Код сервиса
        /// </summary>
        public static string Code => "StoredReport";

        private readonly IWindsorContainer container;

        public StoredStimulService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public void SaveTemplate(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);

            var reportDomain = this.container.ResolveDomain<StoredReport>();
            try
            {
                var report = reportDomain.GetAll().FirstOrDefault(x => x.Id == id);
                ArgumentChecker.NotNull(report, "Передан неверный идентификатор отчета", "id");

                reportDomain.Update(report);
            }
            finally
            {
                this.container.Release(reportDomain);
            }
        }

    }
}