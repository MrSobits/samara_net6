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

    public class GISERPDataExport : BaseDataExportService
    {
        public IDomainService<GISERP> domainService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var dateStart = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd = baseParams.Params.GetAs("dateEnd", new DateTime());

            var data = domainService.GetAll()
                   .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                   .Select(x => new
                   {
                       x.Id,
                       x.RequestDate,
                       x.Answer,
                       Inspector = x.Inspector.Fio,
                       x.RequestState,
                       x.MessageId,
                       ERPID = x.ERPID.Trim(),
                       x.Disposal.DocumentDate,
                       x.Disposal.DocumentNumber,
                       x.ERPInspectionType,
                       x.GisErpRequestType,
                       x.KindKND,
                       x.Goals

                   })
               .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RequestDate,
                    x.Answer,
                    x.Inspector,
                    x.RequestState,
                    x.MessageId,
                    x.ERPID,
                    Disposal = x.DocumentDate.HasValue ? x.DocumentNumber + " от " + x.DocumentDate.Value.ToShortDateString() : x.DocumentNumber,
                    x.ERPInspectionType,
                    x.GisErpRequestType,
                    x.KindKND,
                    x.Goals
                }).AsQueryable()
                .Filter(loadParams, Container);

            return data.ToList();
        }
    }
}