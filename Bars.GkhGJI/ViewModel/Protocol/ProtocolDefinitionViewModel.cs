namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    // Пустышка если в регионах наследовались от этого класс
    public class ProtocolDefinitionViewModel : ProtocolDefinitionViewModel<ProtocolDefinition>
    {
        // Методы переопределять в Generic 
    }
    
    public class ProtocolDefinitionViewModel<T> : BaseViewModel<T>
        where T : ProtocolDefinition
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Protocol.Id == documentId)
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

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Protocol,
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