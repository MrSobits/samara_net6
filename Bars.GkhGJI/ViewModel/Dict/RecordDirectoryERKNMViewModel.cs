using Bars.B4;
using Bars.GkhGji.Entities.Dict;
using System.Linq;

namespace Bars.GkhGji.ViewModel
{
    class RecordDirectoryERKNMViewModel : BaseViewModel<RecordDirectoryERKNM>
    {
        public override IDataResult List(IDomainService<RecordDirectoryERKNM> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var id = loadParams.Filter.GetAs("Id", 0L);
            var data = domainService.GetAll().Where(z=> z.DirectoryERKNM.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.CodeERKNM,
                    x.IdentERKNM,
                    x.IdentSMEV,
                    x.EntityId,
                    DirectoryERKNM = x.DirectoryERKNM
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
