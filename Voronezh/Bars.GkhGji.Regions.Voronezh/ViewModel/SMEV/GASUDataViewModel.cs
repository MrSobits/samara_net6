namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class GASUDataViewModel : BaseViewModel<GASUData>
    {
        public override IDataResult List(IDomainService<GASUData> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("GASU", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.GASU.Id == id)
            .Select(x => new
            {
                x.Id,
                UnitMeasure = x.UnitMeasure != null? x.UnitMeasure.Name:"",
                x.Value,
                x.Indexname,
                x.IndexUid
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}