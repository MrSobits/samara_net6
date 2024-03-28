namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;

    public class SMEVEGRNFileViewModel : BaseViewModel<SMEVEGRNFile>
    {
        public override IDataResult List(IDomainService<SMEVEGRNFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("smevEGRN", 0L);
            var showFiles = loadParams.Filter.GetAs("showSysFiles", false);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.SMEVEGRN.Id == id)
             .WhereIf(!showFiles, x => x.SMEVFileType == SMEVFileType.ResponseAttachment || x.SMEVFileType == SMEVFileType.ResponseAttachmentFTP)
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