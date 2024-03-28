using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Enums;

    public class LongTermPrObjViewModel : BaseViewModel<LongTermPrObject>
    {
        public override IDataResult List(IDomainService<LongTermPrObject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var showDemolished = baseParams.Params.GetAs<bool>("showDemolished");
            var showEmergency = baseParams.Params.GetAs<bool>("showEmergency");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding
                            || x.RealityObject.TypeHouse == TypeHouse.ManyApartments
                            || x.RealityObject.TypeHouse == TypeHouse.SocialBehavior )
                .WhereIf(!showDemolished, x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                .WhereIf(!showEmergency, x => x.RealityObject.ConditionHouse != ConditionHouse.Emergency)
                .Select(x => new
                {
                    x.Id,
                    RealObjId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.TypeHouse,
                    x.RealityObject.MethodFormFundCr
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<LongTermPrObject> domainService, BaseParams baseParams)
        {
            var value =
                domainService.GetAll()
                    .Where(x => x.Id == baseParams.Params["id"].To<long>())
                    .Select(x => new
                    {
                        x.Id,
                        ReealObjId = x.RealityObject.Id,
                        x.RealityObject.ConditionHouse,
                        x.RealityObject.BuildYear,
                        x.RealityObject.DateCommissioning,
                        x.RealityObject.DateTechInspection,
                        x.RealityObject.PrivatizationDateFirstApartment,
                        x.RealityObject.AreaMkd,
                        x.RealityObject.AreaLivingNotLivingMkd,
                        x.RealityObject.AreaLivingOwned,
                        x.RealityObject.AreaOwned,
                        x.RealityObject.AreaLiving,
                        x.RealityObject.AreaNotLivingFunctional,
                        x.RealityObject.MaximumFloors,
                        x.RealityObject.Floors,
                        x.RealityObject.NumberEntrances,
                        x.RealityObject.NumberLiving,
                        x.RealityObject.PercentDebt,
                        x.RealityObject.NecessaryConductCr,
                        x.RealityObject.DateDemolition 
                    })
                    .FirstOrDefault();

            return new BaseDataResult(value);
        }
    }

}
