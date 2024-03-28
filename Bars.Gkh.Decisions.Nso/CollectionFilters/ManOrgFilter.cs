namespace Bars.Gkh.Decisions.Nso.CollectionFilters
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;

    using Gkh.Entities;

    public class ManOrgFilter : ICustomQueryFilter<ManagingOrganization>
    {
        public IQueryable<ManagingOrganization> Filter(IQueryable<ManagingOrganization> source, BaseParams baseParams)
        {
            var decisionType = baseParams.Params.GetAs<MkdManagementDecisionType>("decisionType");

            return source.WhereIf(decisionType == MkdManagementDecisionType.ManOrg,
                x => x.TypeManagement == TypeManagementManOrg.Other)
                .WhereIf(decisionType == MkdManagementDecisionType.Cooperative,
                    x => x.TypeManagement == TypeManagementManOrg.JSK)
                .WhereIf(decisionType == MkdManagementDecisionType.Direct,
                    x => x.TypeManagement == TypeManagementManOrg.Other)
                .WhereIf(decisionType == MkdManagementDecisionType.TsjMoreMkd,
                    x => x.TypeManagement == TypeManagementManOrg.TSJ)
                .WhereIf(decisionType == MkdManagementDecisionType.TsjOneMkd,
                    x => x.TypeManagement == TypeManagementManOrg.TSJ);
        }
    }
}