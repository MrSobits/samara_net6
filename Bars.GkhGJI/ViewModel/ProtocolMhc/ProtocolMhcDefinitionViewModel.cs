namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    // Пустышка если в регионах наследовались от этого класс
    public class ProtocolMhcDefinitionViewModel : ProtocolMhcDefinitionViewModel<ProtocolMhcDefinition>
    {
        // Методы переопределять в Generic 
    }
    
    public class ProtocolMhcDefinitionViewModel<T> : BaseViewModel<T>
        where T : ProtocolMhcDefinition
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs("documentId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ProtocolMhc.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition
                })
                .FirstOrDefault());
        }
    }
}