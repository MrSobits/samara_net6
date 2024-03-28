namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    public class HeatSeasonViewModel : BaseViewModel<HeatSeason>
    {
        public override IDataResult List(IDomainService<HeatSeason> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var periodId = baseParams.Params.ContainsKey("periodId")
                ? baseParams.Params["periodId"].ToLong()
                : 0;

            // если параметра нет, то ставим true, т.к. запрос списка может быть не только из реестра
            // Если true показываем дома с индивидуальным отоплением
            var showIndividual = baseParams.Params.GetAs("showIndividual", false);

            // Если true показываем дома блокированной застройки
            var showBlocked = baseParams.Params.GetAs("showBlocked", false);

            // Если true показываем индивидуальные дома
            var showIndividualRealObj = baseParams.Params.GetAs("showIndividualRealObj", false);

            var data = domainService
                .GetAll()
                .WhereIf(periodId > 0, x => x.Period.Id == periodId)
                .WhereIf(!showIndividual, x => x.RealityObject.HeatingSystem != HeatingSystem.Individual)
                .WhereIf(!showBlocked, x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding)
                .WhereIf(!showIndividualRealObj, x => x.RealityObject.TypeHouse != TypeHouse.Individual)
                .Select(x => new
                {
                    x.Id,
                    Period = x.Period.Name,
                    RealityObject = x.RealityObject.Address
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
