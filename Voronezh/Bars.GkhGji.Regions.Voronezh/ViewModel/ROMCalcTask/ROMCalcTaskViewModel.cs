namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class ROMCalcTaskViewModel : BaseViewModel<ROMCalcTask>
    {
        public override IDataResult List(IDomainService<ROMCalcTask> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    TaskDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.YearEnums,
                    x.KindKND,
                    x.CalcDate,
                    x.CalcState,
                    x.FileInfo
                 
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


        public override IDataResult Get(IDomainService<ROMCalcTask> domain, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       Inspector = x.Inspector.Fio,
                       x.YearEnums,
                       x.KindKND,
                       TaskDate = x.ObjectCreateDate.ToShortDateString(),
                       x.CalcDate,
                       x.CalcState,
                       x.FileInfo
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
