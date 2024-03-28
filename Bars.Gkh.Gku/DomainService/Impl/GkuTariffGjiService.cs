namespace Bars.Gkh.Gku.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Enums;

    using Castle.Windsor;

    public class GkuTariffGjiService : IGkuTariffGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetContragents(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var type = baseParams.Params.GetAs<int>("service_kind");

            IQueryable<Contragent> result;

            if (type == (int)ServiceKindGku.Communal)
            {
                var service = Container.Resolve<IDomainService<ServiceOrganization>>();
                result = service.GetAll().Select(x => x.Contragent);
                Container.Release(service);
            } 
            else if (type == (int)ServiceKindGku.Communal)
            {
                var service = Container.Resolve<IDomainService<ServiceOrganization>>();
                result = service.GetAll().Select(x => x.Contragent);
                Container.Release(service);
            }
            else
            {
                var service = Container.Resolve<IDomainService<Contragent>>();
                result = service.GetAll();
                Container.Release(service);
            }
            
            int totalCount = result.Count();

            result = result.Filter(loadParam, Container);

            return new ListDataResult(result.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}