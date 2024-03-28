namespace Bars.GkhGji.Regions.Stavropol.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.GkhGji.Entities;

    public class ActCheckDefinitionViewModel : BaseViewModel<ActCheckDefinition>
    {
        public override IDataResult List(IDomainService<ActCheckDefinition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActCheck.Id == documentId)
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