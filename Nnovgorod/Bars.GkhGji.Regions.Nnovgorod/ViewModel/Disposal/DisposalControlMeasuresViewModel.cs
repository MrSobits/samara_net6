namespace Bars.GkhGji.Regions.Nnovgorod.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

    public class DisposalControlMeasuresViewModel : BaseViewModel<DisposalControlMeasures>
    {
        public override IDataResult List(IDomainService<DisposalControlMeasures> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToInt()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.Disposal.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.ControlMeasuresName
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}
