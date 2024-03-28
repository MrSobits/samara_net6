namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;

    public class MotivationConclusionDataExport : BaseDataExportService
    {
        /// <summary>
        /// Получить данные для экспорта
        /// </summary>
        /// <param name="baseParams">
        /// dateStart - период с
        /// dateEnd - период по
        /// realityObjectId - жилой дом
        /// </param>
        public override IList GetExportData(BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var viewDomain = this.Container.Resolve<IDomainService<ViewMotivationConclusion>>();

            using (this.Container.Using(userManager, viewDomain))
            {
                var municipalityList = userManager.GetMunicipalityIds();
                return viewDomain.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue
                        && municipalityList.Contains(x.MunicipalityId.Value))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                    .ToList();
            }
        }
    }
}