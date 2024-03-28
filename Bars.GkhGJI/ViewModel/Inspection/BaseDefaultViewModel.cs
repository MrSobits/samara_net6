namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Entities;

    public class BaseDefaultViewModel: BaseDefaultViewModel<BaseDefault>
    {
    }

    public class BaseDefaultViewModel<T> : BaseViewModel<T>
        where T: BaseDefault
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<ViewBaseDefault>>();
            try
            {
                var loadParams = GetLoadParam(baseParams);

                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                var data = service.GetAll()
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionNumber,
                        x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.ContragentName,
                        x.PhysicalPerson,
                        x.State
                    })
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}