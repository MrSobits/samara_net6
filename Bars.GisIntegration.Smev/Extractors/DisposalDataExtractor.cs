namespace Bars.GisIntegration.Smev.Extractors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class DisposalDataExtractor : IDataExtractor<TatarstanDisposal>
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public RisContragent Contragent { get; set; }

        /// <inheritdoc />
        public List<TatarstanDisposal> Extract(DynamicDictionary parameters = null)
        {
            var disposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            using (this.Container.Using(disposalDomain, inspectionRiskDomain))
            {
                var id = parameters.GetAsId();

                var query = disposalDomain.GetAll().Where(x => x.Id == id);
                var risks = inspectionRiskDomain.GetAll()
                    .Where(x => query.Any(y => y.Inspection == x.Inspection))
                    .Where(x => !x.EndDate.HasValue)
                    .GroupBy(x => x.Inspection.Id)
                    .ToDictionary(x => x.Key, x => x.First().RiskCategory);

                var data = query.ToList();
                data.ForEach(x => x.Inspection.RiskCategory = risks.Get(x.Inspection.Id));
                return data;
            }
        }

        public List<ILogRecord> Log { get; set; } = new List<ILogRecord>();
    }
}