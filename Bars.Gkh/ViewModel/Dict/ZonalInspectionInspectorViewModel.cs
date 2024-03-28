namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    public class ZonalInspectionInspectorViewModel : BaseViewModel<ZonalInspectionInspector>
    {
        public override IDataResult List(IDomainService<ZonalInspectionInspector> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var zonalInspectionId = baseParams.Params.GetAs<long>("zonalInspectionId");

            var data = domain.GetAll()
                .Where(x => x.ZonalInspection.Id == zonalInspectionId)
                .Select(x => new
                {
                    x.Id,
                    x.Inspector.Fio,
                    x.Inspector.Code
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Fio);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}