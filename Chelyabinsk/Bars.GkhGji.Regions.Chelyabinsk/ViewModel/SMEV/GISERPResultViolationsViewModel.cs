namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class GISERPResultViolationsViewModel : BaseViewModel<GISERPResultViolations>
    {
        public override IDataResult List(IDomainService<GISERPResultViolations> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("GISERP", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.GISERP.Id == id)
            .Select(x => new
            {
                x.Id,
                x.CODE,
                x.DATE_APPOINTMENT,
                x.EXECUTION_DEADLINE,
                x.EXECUTION_NOTE,
                x.NUM_GUID,
                x.TEXT,
                x.VIOLATION_ACT,
                x.VIOLATION_NOTE,
                x.VLAWSUIT_TYPE_ID
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}