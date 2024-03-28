namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol197
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197DefinitionViewModel : BaseViewModel<Protocol197Definition>
    {
        public override IDataResult List(IDomainService<Protocol197Definition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Protocol197.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    x.FileInfo,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition,
                    x.TimeDefinition,
                    x.DateOfProceedings
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<Protocol197Definition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Protocol197,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.DocumentNumber,
                    x.ExecutionDate,
                    x.IssuedDefinition,
                    x.Description,
                    x.FileInfo,
                    x.SignedBy,
                    x.TypeDefinition,
                    TimeDefinition = x.TimeDefinition.HasValue ? x.TimeDefinition.Value.ToShortTimeString() : "",
                    x.DateOfProceedings
                })
                .FirstOrDefault());
        }
    }
}