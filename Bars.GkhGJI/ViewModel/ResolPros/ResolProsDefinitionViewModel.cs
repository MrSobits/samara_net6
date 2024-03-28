namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ResolProsDefinitionViewModel : BaseViewModel<ResolProsDefinition>
    {
        public override IDataResult List(IDomainService<ResolProsDefinition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.ResolPros.Id == documentId)
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

        public override IDataResult Get(IDomainService<ResolProsDefinition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.ResolPros,
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