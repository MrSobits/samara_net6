namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.Enum;

    using Castle.Windsor;

    public class OwnerAccountDecisionService : IOwnerAccountDecisionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }
        
        public IDataResult ListContragents(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var typeOwner = baseParams.Params.GetAs("typeOwner", OwnerAccountDecisionType.OtherCooperative);

            var dataQuery = ContragentDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Inn,
                    x.Kpp
                });
 
            switch (typeOwner)
            {
                case OwnerAccountDecisionType.UK:
                    {
                        dataQuery = dataQuery.Where(x => ManagingOrganizationDomain.GetAll().Any(y => y.Contragent.Id == x.Id && y.TypeManagement == TypeManagementManOrg.UK));
                    }
                    break;
                case OwnerAccountDecisionType.TSJ:
                    {
                        dataQuery = dataQuery.Where(x => ManagingOrganizationDomain.GetAll().Any(y => y.Contragent.Id == x.Id && y.TypeManagement == TypeManagementManOrg.TSJ));
                    }
                    break;
                case OwnerAccountDecisionType.JSK:
                    {
                        dataQuery = dataQuery.Where(x => ManagingOrganizationDomain.GetAll().Any(y => y.Contragent.Id == x.Id && y.TypeManagement == TypeManagementManOrg.JSK));
                    }
                    break;
            }

            var data = dataQuery
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).AsEnumerable(), data.Count());
        }
    }
}