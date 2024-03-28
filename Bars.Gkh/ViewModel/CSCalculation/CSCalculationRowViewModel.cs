namespace Bars.Gkh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class CSCalculationRowViewModel : BaseViewModel<CSCalculationRow>
    {
        public override IDataResult List(IDomainService<CSCalculationRow> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("cscalculationId", 0L);

            var data = domain.GetAll()
             .Where(x => x.CSCalculation.Id == id)
            .Select(x => new
            {
                x.Id,
                x.DisplayValue,
                x.Value,
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}