namespace Bars.Gkh1468.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh1468.Enums;

    public class RealityObjectViewModel : Gkh.ViewModel.RealityObjectViewModel
    {
        public override IDataResult List(IDomainService<RealityObject> domain, BaseParams baseParams)
        {
            if (HasAnyBaseViewModelArguments(baseParams))
            {
                return base.List(domain, baseParams);
            }

            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                    {
                        x.Id, 
                        Municipality = x.Municipality.Name, 
                        x.TypeHouse, 
                        x.Address, 
                        x.Municipality.RegionName
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container)
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id, 
                        x.Municipality, 
                        x.Address, 
                        x.RegionName, 
                        TypeRealObj = this.MapRealityObjectType(x.TypeHouse)
                    })
                .AsQueryable();

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        private bool HasAnyBaseViewModelArguments(BaseParams baseParams)
        {
            return baseParams.Params.ContainsKey("dateInspection") || baseParams.Params.ContainsKey("dateStart")
                   || baseParams.Params.ContainsKey("dateEnd") || baseParams.Params.ContainsKey("contragentId")
                   || baseParams.Params.ContainsKey("manOrgId") || baseParams.Params.ContainsKey("typeJurOrg")
                   || baseParams.Params.ContainsKey("municipalities");
        }

        private int MapRealityObjectType(TypeHouse type)
        {
            switch (type)
            {
                case TypeHouse.BlockedBuilding:
                case TypeHouse.Individual:
                    return (int)TypeRealObj.RealityObject;
                case TypeHouse.ManyApartments:
                    return (int)TypeRealObj.Mkd;
                default:
                    return 0;
            }
        }
    }
}