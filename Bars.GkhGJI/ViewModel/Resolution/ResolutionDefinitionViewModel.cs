namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ResolutionDefinitionViewModel : ResolutionDefinitionViewModel<ResolutionDefinition>
    {
        // Внимание методы добавлять и переопределять 
    }

    public class ResolutionDefinitionViewModel<T> : BaseViewModel<T>
        where T : ResolutionDefinition
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    x.FileInfo,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}