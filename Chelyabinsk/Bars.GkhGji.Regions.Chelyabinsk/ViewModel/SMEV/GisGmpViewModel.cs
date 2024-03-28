namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Entities;
    using System;
    using System.Linq;
    using BaseChelyabinsk.Enums.SMEV;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;

    public class GisGmpViewModel : BaseViewModel<GisGmp>
    {
        public override IDataResult List(IDomainService<GisGmp> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var paymentsContainer = this.Container.Resolve<IDomainService<PayReg>>();
            var serviceViewDisposal = Container.Resolve<IDomainService<ViewGisGmp>>();
                      

            var data = serviceViewDisposal.GetAll()               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

       
    }
}
