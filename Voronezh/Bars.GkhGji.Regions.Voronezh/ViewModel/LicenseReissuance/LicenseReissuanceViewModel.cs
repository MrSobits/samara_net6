namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Gkh.Authentification;
    using System.Collections.Generic;
    using B4.Utils;
    using System;
    using Enums;

    public class LicenseReissuanceViewModel : BaseViewModel<LicenseReissuance>
    {
        public override IDataResult List(IDomainService<LicenseReissuance> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
               .Select(x => new
               {
                   x.Id,
                   Contragent = x.Contragent.Name,
                   x.ConfirmationOfDuty,
                   ManOrgLicense = x.ManOrgLicense.LicNum,
                   x.RegisterNum,
                   x.ObjectCreateDate,
                   x.State,
                   x.ReissuanceDate
               })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

    }
}