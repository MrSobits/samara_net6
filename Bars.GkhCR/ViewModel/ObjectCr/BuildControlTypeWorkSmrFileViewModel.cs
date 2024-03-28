namespace Bars.GkhCr.ViewModel.ObjectCr
{
    using System;
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class BuildControlTypeWorkSmrFIleViewModel : BaseViewModel<BuildControlTypeWorkSmrFile>
    {
        public override IDataResult List(IDomainService<BuildControlTypeWorkSmrFile> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var typeWorkId = baseParams.Params.GetAsId("SKId");

            var data = domainService.GetAll()
                .Where(x => x.BuildControlTypeWorkSmr.Id == typeWorkId)
             //   .Where(x=> x.FileInfo != null)
                .Select(x => new
                {
                    x.Id,                 
                    x.Description,
                    x.ObjectCreateDate,
                    x.FileInfo
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
