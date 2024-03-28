namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class TarifNormativeViewModel : BaseViewModel<TarifNormative>
    {
        public override IDataResult List(IDomainService<TarifNormative> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Value,
                    x.UnitMeasure,
                    Municipality = x.Municipality.Name,
                    ParentMO = x.Municipality.ParentMo != null ? x.Municipality.ParentMo.Name : x.Municipality.Name,
                    x.DateFrom,
                    x.DateTo,
                    CategoryCSMKD = x.CategoryCSMKD.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        } 
    }
}