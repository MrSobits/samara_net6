namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVCertInfoFileViewModel : BaseViewModel<SMEVCertInfoFile>
    {
        public override IDataResult List(IDomainService<SMEVCertInfoFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("smevCertInfo", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.SMEVCertInfo.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.FileInfo.Name,
                x.SMEVFileType
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
    }
}