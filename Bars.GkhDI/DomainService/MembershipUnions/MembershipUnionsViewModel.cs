namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Entities;

    using Entities;

    public class MembershipUnionsViewModel : BaseViewModel<ManagingOrgMembership>
    {
        public override IDataResult List(IDomainService<ManagingOrgMembership> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().GetAll()
                    .Where(x => x.Id == disclosureInfoId)
                    .Select(x => new { x.ManagingOrganization, x.PeriodDi })
                    .FirstOrDefault();

            if (disclosureInfo == null)
            {
                return new ListDataResult();
            }

            var serviceMembership = this.Container.Resolve<IDomainService<ManagingOrgMembership>>();

            // Алгоритм проверки пересечения периодов: [нач1, кон1] и [нач2, кон2]
            // нач2>=нач1 и кон1>=нач2
            // или
            // нач1>=нач2 и кон2>=нач1
            var data = serviceMembership
                .GetAll()
                .Where(x => x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)
                .Where(x => (x.DateStart.Value >= disclosureInfo.PeriodDi.DateStart.Value && disclosureInfo.PeriodDi.DateEnd.Value >= x.DateStart.Value)
                            || (disclosureInfo.PeriodDi.DateStart.Value >= x.DateStart.Value
                                && ((x.DateEnd.HasValue && x.DateEnd.Value >= disclosureInfo.PeriodDi.DateStart.Value)
                                    || !x.DateEnd.HasValue)))
                .Select(x => new { x.Id, x.Name, x.Address, x.OfficialSite });

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}