namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class CourtPracticeRealityObjectViewModel : BaseViewModel<CourtPracticeRealityObject>
    {
        public override IDataResult List(IDomainService<CourtPracticeRealityObject> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("courtpracticeId", 0L);
         
            var data = domain.GetAll()
             .Where(x => x.CourtPractice.Id == id)
            .Select(x => new
            {
                x.Id,
                RealityObject = x.RealityObject.Address,
                Municipality = x.RealityObject.Municipality.Name
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}