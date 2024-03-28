﻿namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ProtocolMhcRealityObjectViewModel : BaseViewModel<ProtocolMhcRealityObject>
    {
        public override IDataResult List(IDomainService<ProtocolMhcRealityObject> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs("documentId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ProtocolMhc.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObjectId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    Area = x.RealityObject.AreaMkd
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}