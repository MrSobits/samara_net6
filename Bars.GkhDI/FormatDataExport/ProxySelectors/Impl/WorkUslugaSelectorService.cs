namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Сервис получения <see cref="WorkUslugaProxy"/>
    /// </summary>
    public class WorkUslugaSelectorService : BaseProxySelectorService<WorkUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, WorkUslugaProxy> GetCache()
        {
            var templateServiceRepository = this.Container.ResolveRepository<TemplateService>();

            try
            {
                return templateServiceRepository.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Name,
                            UnitMeasureCode = x.UnitMeasure.OkeiCode,
                            UnitMeasureName = x.UnitMeasure.Name,
                            x.KindServiceDi
                        })
                    .AsEnumerable()
                    .Select(
                        x => new WorkUslugaProxy
                        {
                            Id = x.Id,
                            Name = x.Name,
                            OkeiCode = x.UnitMeasureCode,
                            AnotherUnit = x.UnitMeasureName,
                            Type = this.GetType(x.KindServiceDi)
                        })
                    .ToDictionary(x => x.Id);
            }
            finally 
            {
                this.Container.Release(templateServiceRepository);
            }
        }

        private int? GetType(KindServiceDi kindServiceDi)
        {
            switch (kindServiceDi)
            {
                case KindServiceDi.Communal:
                case KindServiceDi.Housing:
                case KindServiceDi.Managing:
                    return 1;
                case KindServiceDi.Repair:
                    return 2;
                case KindServiceDi.Additional:
                case KindServiceDi.CapitalRepair:
                    return 7;

            }

            return null;
        }
    }
}