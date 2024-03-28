namespace Bars.GkhGji.Regions.Habarovsk.DataExport
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

    public class VDGOViolatorsDataExport : BaseDataExportService
    {
        public IDomainService<VDGOViolators> domainService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.Name,
                    MinOrgContragent = x.MinOrgContragent.Name,
                    Address = x.Address.Address,
                    x.NotificationDate,
                    x.NotificationNumber,
                    x.FIO,
                    x.Email,
                    x.PhoneNumber,
                    x.DateExecution,
                    x.MarkOfExecution,
                    x.Description,
                    File = x.File.Name,
                    NotificationFile = x.NotificationFile.Name

                })
                .Filter(loadParams, Container);

            return data.ToList();
        }
    }
}