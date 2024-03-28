using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{

    using B4;
    using B4.Utils;
    using Entities;
    using System;

    public class AppCitPrFondVoilationViewModel : BaseViewModel<AppCitPrFondVoilation>
    {
        public override IDataResult List(IDomainService<AppCitPrFondVoilation> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("AppealCitsPrescriptionFond", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.AppealCitsPrescriptionFond.Id == id)
            .Select(x => new
            {
                x.Id,
                x.PlanedDate,
                x.FactDate,
                ViolationGji = x.ViolationGji.Name
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());


            //if (id != 0)
            //{

            //}
            //else 
            //{
            //    var data2 = domain.GetAll()
            //   .Select(x => new
            //   {
            //       x.Id,
            //       x.PlanedDate,
            //       x.FactDate,
            //       ViolationGji = x.ViolationGji.Name
            //   })
            //   .AsQueryable()
            //   .Filter(loadParams, Container);

            //    return new ListDataResult(data2.Order(loadParams).Paging(loadParams).ToList(), data2.Count());
            //}

        }

        public override IDataResult Get(IDomainService<AppCitPrFondVoilation> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       x.PlanedDate,
                       x.FactDate,
                       ViolationGji = x.ViolationGji.Name
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}