namespace Bars.GkhGji.ViewModel
{
    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Gkh.Authentification;
    using System.Collections.Generic;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SpecialAccountOwnerViewModel : BaseViewModel<SpecialAccountOwner>
    {
        public override IDataResult List(IDomainService<SpecialAccountOwner> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    x.ActivityDateEnd,
                    x.ActivityGroundsTermination,
                    Contragent = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.OrgStateRole                 
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
