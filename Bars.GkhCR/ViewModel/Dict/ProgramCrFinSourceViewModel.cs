namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ProgramCrFinSourceViewModel : BaseViewModel<ProgramCrFinSource>
    {
        public override IDataResult List(IDomainService<ProgramCrFinSource> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var programCrId = baseParams.Params.ContainsKey("programCrId")
                       ? baseParams.Params["programCrId"].ToLong()
                       : 0L;

            var data = domain
                .GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Select(x => new
                {
                    x.Id,
                    FinanceSourceName = x.FinanceSource.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
