using Bars.B4;
using Bars.GkhGji.Entities.Dict;
using System.Linq;

namespace Bars.GkhGji.ViewModel
{
    class DirectoryERKNMViewModel : BaseViewModel<DirectoryERKNM>
    {
        public override IDataResult List(IDomainService<DirectoryERKNM> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.CodeERKNM,
                    x.EntityName
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
