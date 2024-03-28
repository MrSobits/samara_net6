namespace Bars.GkhGji.Regions.Smolensk.ViewModel.Resolution
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;

    public class ResolutionDefinitionSmolViewModel : ResolutionDefinitionViewModel<ResolutionDefinitionSmol>
    {
        public override IDataResult List(IDomainService<ResolutionDefinitionSmol> domainService, BaseParams baseParams)
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
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition,
                    x.DefinitionResult
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

    }
}
