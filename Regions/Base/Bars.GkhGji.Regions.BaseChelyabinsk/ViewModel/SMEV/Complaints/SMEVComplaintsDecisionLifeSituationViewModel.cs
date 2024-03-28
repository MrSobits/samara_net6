namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVComplaintsDecisionLifeSituationViewModel : BaseViewModel<SMEVComplaintsDecisionLifeSituation>
    {
        public override IDataResult List(IDomainService<SMEVComplaintsDecisionLifeSituation> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var decId = loadParams.Filter.GetAs<long>("decId", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.SMEVComplaintsDecision.Id == decId)
            .Select(x => new
            {
                x.Id,
                x.Code,
                x.Name,
                x.FullName
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}