namespace Bars.GisIntegration.Smev.Extractors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    using Castle.Windsor;

    public class DecisionDataExtractor : IDataExtractor<Decision>
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <inheritdoc />
        public RisContragent Contragent { get; set; }

        /// <inheritdoc />
        public List<Decision> Extract(DynamicDictionary parameters = null)
        {
            var decisionDomain = this.Container.ResolveDomain<Decision>();

            using (this.Container.Using(decisionDomain))
            {
                var id = parameters?.GetAsId();

                return decisionDomain.GetAll()
                    .WhereIf(id.HasValue, x => x.Id == id)
                    .ToList();
            }
        }

        /// <inheritdoc />
        public List<ILogRecord> Log { get; set; }
    }
}