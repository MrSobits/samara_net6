namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;

    public class ControlListQuestionViewModel : BaseViewModel<ControlListQuestion>
    {
        public override IDataResult List(IDomainService<ControlListQuestion> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var clistId = loadParam.Filter.GetAs("clistId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ControlList.Id == clistId)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Description,
                    x.NPDName,
                    x.ERKNMGuid
                })
                .Filter(loadParam, Container)
                .ToList()
                .AsQueryable();

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}