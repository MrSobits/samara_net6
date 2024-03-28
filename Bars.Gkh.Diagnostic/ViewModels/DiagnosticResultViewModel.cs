namespace Bars.Gkh.Diagnostic.ViewModels
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Diagnostic.Entities;

    public class DiagnosticResultViewModel : BaseViewModel<DiagnosticResult>
    {
        public override IDataResult List(IDomainService<DiagnosticResult> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var collectedDiagnosticResultId = loadParam.Filter.GetAs<long>("collectedDiagnosticResultId");

            var data =
                domainService.GetAll()
                    .WhereIf(
                        collectedDiagnosticResultId > 0,
                        x => x.CollectedDiagnostic.Id == collectedDiagnosticResultId)
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                x.Name,
                                x.State,
                                x.Message 
                            })
                    .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
