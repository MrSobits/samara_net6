namespace Bars.GkhGji.Regions.Tomsk.ViewModel.ResolPros
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.ResolPros;

    public class ResolProsDefinitionViewModel : BaseViewModel<ResolProsDefinition>
    {
        public override IDataResult List(IDomainService<ResolProsDefinition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.ResolPros.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeResolProsDefinition
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}