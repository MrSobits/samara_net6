namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    /// <summary>
    /// Вьюмодель для ExecutoryProcessDocument
    /// </summary>
    public class ExecutoryProcessDocumentViewModel : BaseViewModel<ExecutoryProcessDocument>
    {
        public override IDataResult List(IDomainService<ExecutoryProcessDocument> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var documentId = loadParam.Filter.GetAs("docId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ExecutoryProcess.Id == documentId)
                .Filter(loadParam, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}