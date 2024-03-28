namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class CourtPracticeInspectorViewModel : BaseViewModel<CourtPracticeInspector>
    {
        public override IDataResult List(IDomainService<CourtPracticeInspector> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("courtpracticeId", 0L);

            var data = domain.GetAll()
             .Where(x => x.CourtPractice.Id == id)
            .Select(x => new
            {
                x.Id,
                Inspector = x.Inspector.Fio,
                x.Discription,
                x.LawyerInspector
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}