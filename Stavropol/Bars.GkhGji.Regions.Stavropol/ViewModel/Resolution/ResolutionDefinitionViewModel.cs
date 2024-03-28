namespace Bars.GkhGji.Regions.Stavropol.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.GkhGji.Entities;

    public class ResolutionDefinitionViewModel : BaseViewModel<ResolutionDefinition>
    {
        public override IDataResult List(IDomainService<ResolutionDefinition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    DocumentNum = x.DocumentNumber != null ? x.DocumentNumber.ToString() : x.DocumentNum,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}