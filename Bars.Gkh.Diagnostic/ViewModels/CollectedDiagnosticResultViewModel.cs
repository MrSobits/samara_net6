namespace Bars.Gkh.Diagnostic.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Diagnostic.Entities;

    public class CollectedDiagnosticResultViewModel : BaseViewModel<CollectedDiagnosticResult>
    {
        public override IDataResult List(IDomainService<CollectedDiagnosticResult> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .Select(
                    x =>
                    new
                        {
                            x.Id,
                            x.ObjectCreateDate,
                            x.State,
                        }).Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
