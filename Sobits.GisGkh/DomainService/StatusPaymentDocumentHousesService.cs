namespace Sobits.GisGkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.GkhCr.DomainService;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;
    using Sobits.GisGkh.Entities;

    public class StatusPaymentDocumentHousesService : IStatusPaymentDocumentHousesService
    {
        public IWindsorContainer Container { get; set; }
        public IDataResult GetStatusPaymentDocumentHouses(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var gisGkhPayDocDomain = this.Container.Resolve<IDomainService<ViewStatusPaymentDocumentHouses>>();

            var period = baseParams.Params.GetAs<long>("periodId");

            var result = gisGkhPayDocDomain.GetAll().Where(x => x.Period == period).Filter(loadParams, this.Container);
            var totalCount = result.Count();
            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}