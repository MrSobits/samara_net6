namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ActCheckFamiliarizedViewModel : BaseViewModel<ActCheckFamiliarized>
    {
        public override IDataResult List(IDomainService<ActCheckFamiliarized> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActCheck.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Fio
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}