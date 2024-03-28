namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class HeatSeasonDocViewModel : BaseViewModel<HeatSeasonDoc>
    {
        public override IDataResult List(IDomainService<HeatSeasonDoc> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var seasonId = baseParams.Params.ContainsKey("seasonId")
                                   ? baseParams.Params["seasonId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.HeatingSeason.Id == seasonId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.Description,
                    x.TypeDocument,
                    x.State,
                    x.File
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}