namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;

    public class ResettlementProgramViewModel : BaseViewModel<ResettlementProgram>
    {
        public override IDataResult List(IDomainService<ResettlementProgram> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    PeriodName = x.Period.Name,
                    x.MatchFederalLaw,
                    x.StateProgram,
                    x.TypeProgram,
                    x.Visibility,
                    x.UseInExport
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}