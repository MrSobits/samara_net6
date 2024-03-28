using System.Linq;

namespace Bars.GkhGji.ViewModel
{

    using B4;
    using B4.Utils;
    using Entities;
    using System;

    public class SPAccOwnerRealityObjectViewModel : BaseViewModel<SPAccOwnerRealityObject>
    {
        public override IDataResult List(IDomainService<SPAccOwnerRealityObject> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("parentId", 0L);
        
            var data = domain.GetAll()
             .Where(x => x.SpecialAccountOwner.Id == id)
            .Select(x => new
            {
                x.Id,
                CreditOrg = x.CreditOrg.Name,
                x.DateStart,
                x.DateEnd,
                RealityObject = x.RealityObject.Address,
                x.SpacAccNumber,
                Municipality = x.RealityObject.Municipality.Name
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }
    }
}