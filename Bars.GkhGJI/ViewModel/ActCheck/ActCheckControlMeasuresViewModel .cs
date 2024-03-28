namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckControlMeasuresViewModel : BaseViewModel<ActCheckControlMeasures>
    {
        public override IDataResult List(IDomainService<ActCheckControlMeasures> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ControlActivity = x.ControlActivity.Name,
                    x.DateStart,
                    x.DateEnd,
                    x.Description
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}