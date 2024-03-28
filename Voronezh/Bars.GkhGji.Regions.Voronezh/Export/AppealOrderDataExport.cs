namespace Bars.GkhGji.Regions.Voronezh.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using GkhGji.Entities;
    using System.Collections.Generic;
    using GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealOrderDataExport : BaseDataExportService
    {
        public IDomainService<AppealOrder> domainService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());           

            var data = domainService.GetAll()
                 .Where(x => x.OrderDate >= dateStart2 && x.OrderDate <= dateEnd2)
                 .Select(x => new
                 {
                     x.Id,
                     x.Confirmed,
                     x.AppealCits.State,
                     Executant = x.Executant.Name,
                     ContragentINN = x.Executant.Inn,
                     x.Person,
                     x.AppealCits.DocumentNumber,
                     x.AppealCits.DateFrom,
                     x.OrderDate,
                     x.PerformanceDate,
                     x.YesNoNotSet,
                     x.AppealCits.Correspondent,
                     x.AppealCits.CorrespondentAddress,
                     x.AppealCits.File,
                     x.Description

                 })
                 .Filter(loadParam, Container);

            return data.ToList();
        }
    }
}