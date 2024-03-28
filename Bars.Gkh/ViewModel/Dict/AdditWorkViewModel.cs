namespace Bars.Gkh.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    public class AdditWorkViewModel : BaseViewModel<AdditWork>
    {
        public override IDataResult List(IDomainService<AdditWork> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var workId = baseParams.Params.ContainsKey("workId")
                                   ? baseParams.Params["workId"].ToLong()
                                   : 0;

            var data = domain.GetAll()
                .Where(x => x.Work.Id == workId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Description,
                    x.Queue,
                    x.Percentage,
                    x.Work
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}