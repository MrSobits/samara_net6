namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;

    public class CreditOrgService : ICreditOrgService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListExceptChildren(BaseParams baseParams)
        {
            var currOrgId = baseParams.Params.GetAs<long>("currOrgId");

            var loadParams = baseParams.GetLoadParam();

            var service = Container.Resolve<IDomainService<CreditOrg>>();

            var data = service.GetAll()
                .Where(x => x.Parent == null || x.Parent.Id != currOrgId)
                .Where(x => x.Id != currOrgId)
                .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Inn,
                        x.Kpp,
                        x.Address,
                        x.Bik,
                        x.Okpo,
                        x.AddressOutSubject,
                        x.IsAddressOut,
                        x.FiasAddress
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}