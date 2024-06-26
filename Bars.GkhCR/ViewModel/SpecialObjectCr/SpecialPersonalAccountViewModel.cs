﻿namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Entities;

    public class SpecialPersonalAccountViewModel : BaseViewModel<SpecialPersonalAccount>
    {
        public override IDataResult List(IDomainService<SpecialPersonalAccount> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectCrId = baseParams.Params.GetAs("objectCrId", 0l);

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAs("objectCrId", 0l);
            }

            var data = domainService.GetAll()
               .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x => new
                    {
                        x.Id,
                        x.FinanceGroup,
                        x.Closed,
                        x.Account
                    })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
