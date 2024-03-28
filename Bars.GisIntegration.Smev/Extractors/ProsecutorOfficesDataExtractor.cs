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
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class ProsecutorOfficesDataExtractor : IDataExtractor<ProsecutorOfficeDict>
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public RisContragent Contragent { get; set; }

        /// <inheritdoc />
        public List<ProsecutorOfficeDict> Extract(DynamicDictionary parameters = null)
        {
            var prosecutorOfficeDict = this.Container.ResolveDomain<ProsecutorOfficeDict>();

            using (this.Container.Using(prosecutorOfficeDict))
            {
                var id = parameters.GetAsId();

                var query = prosecutorOfficeDict.GetAll().Where(x => x.Id == id);
               
                var data = query.ToList();
                return data;
            }
        }

        public List<ILogRecord> Log { get; set; } = new List<ILogRecord>();
    }
}