namespace Bars.Gkh.Decisions.Nso.CollectionFilters
{
    using System.Linq;
    using B4;

    using Gkh.Domain;
    using Gkh.Entities;

    public class ContragentFilter : ICustomQueryFilter<Contragent>
    {
        public ContragentFilter(
            IDomainService<ManagingOrganization> manOrgDomain)
        {
            _manOrgDomain = manOrgDomain;
        }

        public IQueryable<Contragent> Filter(IQueryable<Contragent> source, BaseParams baseParams)
        {
            //var decisionType = baseParams.Params.GetAs<AccountOwnerDecisionType>("decisionType");

            return source;
        }

        private readonly IDomainService<ManagingOrganization> _manOrgDomain;
    }
}