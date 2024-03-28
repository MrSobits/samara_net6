namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Entities;

    using Entities;

    public class TerminateContractViewModel : BaseViewModel<ManOrgContractRealityObject>
    {
        public override IDataResult List(IDomainService<ManOrgContractRealityObject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
            var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Load(disclosureInfoId);

            if (disclosureInfo == null)
            {
                return new ListDataResult();
            }

            // Получаем список договоров у которых была дата расторжения в периоде по упр орг из раскрытия инф-ии. Список1
            var listTerminateContracts = domainService.GetAll()
                .Where(x => x.ManOrgContract.EndDate >= disclosureInfo.PeriodDi.DateStart.Value.AddYears(-1)
                    && x.ManOrgContract.EndDate < disclosureInfo.PeriodDi.DateEnd.Value.AddYears(-1)
                    && x.ManOrgContract.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)
                .ToList();

            // Получаем список договоров по упр орг из раскрытия инф-ии. Список2
            var listContracts = domainService.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)
                .ToList();

            // бежим по списку1 и смотрим:
            // 1.Есть ли договор по данному дому в списке2
            // 2.Если есть то дата начала договора из списка2 == дате конца договора из списка1 + 1 (продлили на след день после окончания)
            // Если усл 1 и 2 выполняются то договор не считается расторгнутым, иначе кладем его в список расторгнутых договоров
            var dataList = listTerminateContracts
                .Select(terminateContract => new
                {
                    terminateContract,
                    count = listContracts
                        .Where(x => x.RealityObject.Id == terminateContract.RealityObject.Id)
                        .Count(x =>
                                x.ManOrgContract.StartDate.HasValue 
                                && terminateContract.ManOrgContract.EndDate.HasValue
                                && x.ManOrgContract.StartDate.Value.Date == terminateContract.ManOrgContract.EndDate.Value.Date.AddDays(1))
                })
                .Where(x => x.count == 0)
                .Select(x => new
                {
                    x.terminateContract.Id,
                    x.terminateContract.ManOrgContract.TerminateReason,
                    AddressName =
                        x.terminateContract.RealityObject.FiasAddress != null
                            ? x.terminateContract.RealityObject.FiasAddress.AddressName
                            : string.Empty
                })
                .ToList();

            var data = dataList.AsQueryable().Filter(loadParams, this.Container);
            var totalCount = data.Count();

            data = data.Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}