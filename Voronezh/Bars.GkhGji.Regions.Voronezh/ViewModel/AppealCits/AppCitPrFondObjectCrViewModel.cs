using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{

    using B4;
    using B4.Utils;
    using Entities;
    using System;

    public class AppCitPrFondObjectCrViewModel : BaseViewModel<AppCitPrFondObjectCr>
    {
        public override IDataResult List(IDomainService<AppCitPrFondObjectCr> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("AppealCitsPrescriptionFond", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.AppealCitsPrescriptionFond.Id == id)
            .Select(x => new
            {
                x.Id,
                Address = x.ObjectCr.RealityObject.Address,
                Program = x.ObjectCr.ProgramCr.Name
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<AppCitPrFondObjectCr> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       Address = x.ObjectCr.RealityObject.Address,
                       Program = x.ObjectCr.ProgramCr.Name
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}