namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;

    public class ViolationNormativeDocItemGjiViewModel : BaseViewModel<ViolationNormativeDocItemGji>
    {
        public override IDataResult List(IDomainService<ViolationNormativeDocItemGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var violationId = baseParams.Params.ContainsKey("violationId")
                ? baseParams.Params["violationId"].ToLong()
                : 0;

            var data = domainService.GetAll()
                .Where(x => x.ViolationGji.Id == violationId)
                .Select(x => new
                {
                    x.Id,
                    Violation = x.ViolationGji,
                    x.NormativeDocItem,
                    NormativeDocItemName = x.NormativeDocItem.Number,
                    x.NormativeDocItem.NormativeDoc,
                    NormativeDocName = x.NormativeDocItem.NormativeDoc.Name,
                    x.ViolationStructure
                })
                .Filter(loadParam, Container)
                .ToList()
                .AsQueryable();

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}